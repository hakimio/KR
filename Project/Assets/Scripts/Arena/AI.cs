using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AI: MonoBehaviour
{
    Dictionary<GameObject, TileBehaviour> monsterDict;
    List<GameObject> monsters;
    public static AI instance = null;
    GameObject curMonster;
    BaseChar charToAttack;
    bool attack = false;
    public List<Tile> monsterTiles = new List<Tile>();

    void Start()
    {
        instance = this;
        monsterDict = new Dictionary<GameObject, TileBehaviour>();
        monsters = new List<GameObject>();
        foreach (Transform monsterTr in GameObject.Find("Monsters").transform)
        {
            TileBehaviour tb = findTileBehavior(monsterTr.gameObject);
            monsterDict.Add(monsterTr.gameObject, tb);
            monsters.Add(monsterTr.gameObject);
        }
        foreach (var monster in monsterDict.Values)
            monster.setAsImpassable();
        Messenger.AddListener("characterMoved", finishedMoving);
    }

    public void StartMove()
    {
        curMonster = monsters[0];
        moveCurMonster();
    }

    void moveCurMonster()
    {
        Tile tileOfTheCharToAttack;
        Tile monstersTile = monsterDict[curMonster].tile;
        monsterDict[curMonster].setAsNormal();
        CombatManager.instance.findClosestPlayerChar(monstersTile,
            out charToAttack, out tileOfTheCharToAttack);
        var path = PathFinder.FindPath(monstersTile, tileOfTheCharToAttack);
        tileOfTheCharToAttack.Passable = false;
        int nrOfSteps = curMonster.GetComponent<Monster>().nrOfSteps;
        if (path.ToList().Count - 2 < nrOfSteps)
            nrOfSteps = path.ToList().Count - 2;
        if (path.ToList().Count - 2 <= nrOfSteps)
            attack = true;
        else
            attack = false;
        CharacterMovement CC = curMonster.GetComponent<CharacterMovement>();
        CC.StartMoving(path.ToList(), nrOfSteps);
    }

    TileBehaviour findTileBehavior(GameObject monster)
    {
        CharacterController CC = monster.GetComponent<CharacterController>();
        Vector3 monsterPos = CC.collider.bounds.center;
        Vector2 gridPos = GridManager.instance.calcGridPos(monsterPos);
        Point point = new Point((int)(gridPos.x - (int)gridPos.y / 2),
            (int)gridPos.y);
        return GridManager.instance.Board[point];
    }

    void finishedMoving()
    {
        if (CombatManager.instance.playersTurn)
            return;
        if (attack)
            StartCoroutine(attackPlayer());
        else
            changeMovingMonster();
    }

    IEnumerator attackPlayer()
    {
        Animation monsterAnim = curMonster.animation;
        monsterAnim["ChewFem"].wrapMode = WrapMode.Once;
        monsterAnim.CrossFade("ChewFem");
        monsterAnim.CrossFadeQueued("IdleFeM");
        yield return StartCoroutine(monsterAnim.WhilePlaying("ChewFem"));
        int minHP = curMonster.GetComponent<Monster>().minDamage;
        int hp = minHP + Random.Range(0, 6);
        if (charToAttack.CurrentHP - hp < 0)
        {
            HUD.instance.addMessage(charToAttack.charName + " died!");
            OSD.instance.showGameLostScreen();
        }
        else
        {
            charToAttack.LostHP += hp;
            HUD.instance.addMessage(charToAttack.charName + " lost " + hp +
                " hit points.");
            changeMovingMonster();
        }
    }

    void changeMovingMonster()
    {
        monsterDict[curMonster] = findTileBehavior(curMonster);
        monsterDict[curMonster].setAsImpassable();
        if (monsters.IndexOf(curMonster) == monsters.Count - 1)
        {
            CombatManager.instance.StartPlayersMove();
            return;
        }
        curMonster = monsters[monsters.IndexOf(curMonster) + 1];
        moveCurMonster();
    }

    public Tile getMonstersTile(GameObject monster)
    {
        return monsterDict[monster].tile;
    }

    public void removeMonster(GameObject monster)
    {
        monsterDict[monster].setAsNormal();
        monsters.Remove(monster);
        monsterDict.Remove(monster);
        OSD.instance.hideTooltip(monster);
        Destroy(monster);
        if (OSD.instance.getTooltipMonster() == monster)
            monsterTiles.Clear();
        Messenger.Broadcast("refreshWalkableTiles");
        if (monsters.Count == 0)
        {
            Messenger<bool>.Broadcast("enable movement", false);
            HUD.instance.clickable = false;
            OSD.instance.showGameWonScreen();
        }
    }

    public void findMonstersMovementArea(Monster monster)
    {
        Tile originTile = monsterDict[monster.gameObject].tile;
        int steps = monster.nrOfSteps;
        monsterTiles = GridHelper.getTilesWithDistance(originTile, steps);
        Messenger.Broadcast("monsterTilesChanged");
    }

    public void clearMonstersMovementArea()
    {
        monsterTiles.Clear();
        Messenger.Broadcast("monsterTilesChanged");
    }
}
