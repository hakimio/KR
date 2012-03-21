using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CombatManager: MonoBehaviour
{
    struct charInfo
    {
        public int stepsLeft;
        public TileBehaviour tileBeh;
        public bool attacked;
        public charInfo(TileBehaviour tile, int stepsLeft)
        {
            this.tileBeh = tile;
            this.stepsLeft = stepsLeft;
            attacked = false;
        }
    }

    Dictionary<BaseChar, charInfo> characters;
    public static CombatManager instance = null;
    public bool playersTurn = true;
    public List<Tile> walkableTiles;
    public int nrOfObstacles = 0;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characters = new Dictionary<BaseChar, charInfo>();
        for (int i = 0; i < GameMaster.instance.characters.Count; i++)
        {
            BaseChar character = GameMaster.instance.characters[i];
            GameObject charGO = character.gameObject;
            Vector3 charPos = charGO.transform.position;
            Vector2 gridPos = GridManager.instance.calcGridPos(charPos);
            Point point = new Point((int)(gridPos.x - (int)gridPos.y / 2), 
                (int)gridPos.y);
            TileBehaviour tb = GridManager.instance.Board[point];
            if (character == GameMaster.instance.selectedChar)
                tb.setAsOrigin();
            int steps = character.
                getSecondaryAttr((int)SecondaryAttrNames.Movement_Speed).Value;
            characters.Add(character, new charInfo(tb, steps));
        }
        foreach (var character in characters)
            if (character.Key != GameMaster.instance.selectedChar)
                character.Value.tileBeh.setAsImpassable();

        Messenger.AddListener("selectedCharChanged", selectedCharChanged);
        Messenger.AddListener("obstaclePositioned", obstaclePositioned);
        Messenger.AddListener("refreshWalkableTiles", findWalkable);
    }

    int nrOfObstaclesPositioned = 0;
    void obstaclePositioned()
    {
        nrOfObstaclesPositioned++;
        if (nrOfObstacles != nrOfObstaclesPositioned)
            return;
        findWalkable();
    }

    void findWalkable()
    {
        Tile originTile = GridManager.instance.originTileTB.tile;
        walkableTiles = GridHelper.getTilesWithDistance(originTile, stepsLeft);
        Messenger.Broadcast("walkableTilesChanged");
    }

    public void charMoved()
    {
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        charInfo selectedCharInfo = characters[selectedChar];
        selectedCharInfo.tileBeh = GridManager.instance.originTileTB;
        characters[selectedChar] = selectedCharInfo;
        findWalkable();
    }

    void selectedCharChanged()
    {
        GridManager.instance.originTileTB.setAsNormal();
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        TileBehaviour tb = characters[selectedChar].tileBeh;
        tb.setAsOrigin();
        
        foreach(var character in characters)
            if (character.Key != selectedChar)
                character.Value.tileBeh.setAsImpassable();
        findWalkable();
    }

    public int stepsLeft
    {
        get
        {
            return characters[GameMaster.instance.selectedChar].stepsLeft;
        }
        set
        {
            charInfo CI = characters[GameMaster.instance.selectedChar];
            CI.stepsLeft = value;
            characters[GameMaster.instance.selectedChar] = CI;
        }
    }

    public void findClosestPlayerChar(Tile tile, out BaseChar character,
        out Tile closestCharTile)
    {
        int distance, curDistance;
        Tile curTile;
        character = GameMaster.instance.selectedChar;
        closestCharTile = characters[character].tileBeh.tile;
        distance = int.MaxValue;

        foreach (var c in characters)
            c.Value.tileBeh.tile.Passable = true;

        foreach(var entry in characters)
        {
            curTile = entry.Value.tileBeh.tile;
            curDistance = PathFinder.FindPath(tile, curTile).ToList().Count;
            if (curDistance < distance)
            {
                distance = curDistance;
                closestCharTile = curTile;
                character = entry.Key;
            }
        }

        foreach (var c in characters)
            if (c.Key != character)
                c.Value.tileBeh.setAsImpassable();
    }

    public void StartPlayersMove()
    {
        playersTurn = true;
        resetPlayerChars();
        selectedCharChanged();
        HUD.instance.clickable = true;
    }

    void resetPlayerChars()
    {
        Dictionary<BaseChar, charInfo> temp;
        temp = new Dictionary<BaseChar, charInfo>();
        foreach(var entry in characters)
        {
            charInfo ci = entry.Value;
            int steps = entry.Key.
                getSecondaryAttr((int)SecondaryAttrNames.Movement_Speed).Value;
            ci.stepsLeft = steps;
            ci.attacked = false;
            temp.Add(entry.Key, ci);
        }
        characters = temp;
    }

    public void StartAIMove()
    {
        if (!playersTurn)
            return;
        walkableTiles.Clear();
        Messenger.Broadcast("walkableTilesChanged");
        HUD.instance.clickable = false;
        playersTurn = false;
        AI.instance.StartMove();
    }

    public Tile getCharactersTile(BaseChar character)
    {
        return characters[character].tileBeh.tile;
    }

    public bool didCharacterAttack(BaseChar character)
    {
        return characters[character].attacked;
    }

    public void characterAttacked(BaseChar character)
    {
        charInfo ci = characters[character];
        ci.attacked = true;
        characters[character] = ci;
    }
}
