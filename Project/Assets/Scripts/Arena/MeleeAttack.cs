using UnityEngine;
using System.Collections;

public class MeleeAttack: MonoBehaviour
{
    bool isRotating = false;
    Quaternion rotation;
    float rotationSpeed = 4;
    GameObject monster;

    public bool attack(GameObject monster)
    {
        this.monster = monster;
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        Tile charTile = CombatManager.instance.getCharactersTile(selectedChar);
        Tile monsterTile = AI.instance.getMonstersTile(monster);
        if (GridManager.calcDistance(charTile, monsterTile) > 1)
            return false;
        CombatManager.instance.characterAttacked(selectedChar);
        Vector3 monsterPos = monster.transform.position;
        monsterPos.y = transform.position.y;
        Vector3 dir = monsterPos - transform.position;
        rotation = Quaternion.LookRotation(dir);
        isRotating = true;
        return true;
    }

    void Update()
    {
        if (!isRotating)
            return;
        if (Quaternion.Angle(transform.rotation, rotation) < 1f)
        {
            transform.rotation = rotation;
            isRotating = false;
            StartCoroutine(animateAndRemoveHP());
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,
            rotation, rotationSpeed * Time.deltaTime);
    }

    IEnumerator animateAndRemoveHP()
    {
        if (animation["punch"] != null)
        {
            animation["punch"].wrapMode = WrapMode.Once;
            animation["punch"].speed = 0.5f;
            animation.CrossFade("punch");
            animation.CrossFadeQueued("idle");
            yield return StartCoroutine(animation.WhilePlaying("punch"));
        }

        int hp = 5 + Random.Range(0, 6);
        Monster monsterCl = monster.GetComponent<Monster>();
        HUD.instance.addMessage(monsterCl.monsterName + " lost " + hp +
            " hit points.");
        if (monsterCl.HP - hp > 0)
        {
            monsterCl.HP -= hp;
            OSD.instance.updateTooltip(monsterCl);
        }
        else
        {
            AI.instance.removeMonster(monster);
            HUD.instance.addMessage(monsterCl.monsterName + " died.");
        }
    }
}
