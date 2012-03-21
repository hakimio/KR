using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GridManager: MonoBehaviour
{
    public GameObject Ground;
    public GameObject Hex;
    public GameObject Line;
    public GameObject Monster;

    public Tile selectedTile = null;
    public TileBehaviour originTileTB = null;
    public TileBehaviour destTileTB = null;
    public Dictionary<Point, TileBehaviour> Board;

    float hexSizeX, hexSizeY, hexSizeZ, groundSizeX, groundSizeY, groundSizeZ;
    public static GridManager instance = null;
    public GameObject CameraTarget;

    List<GameObject> path;

    void Awake()
    {
        instance = this;
        MyCamera.instance.TargetLookAt = CameraTarget.transform;
        setSizes();
        createGrid();
        generateAndShowPath();
        Messenger.AddListener("characterMoved", switchOriginAndDestTiles);
    }

    void setSizes()
    {
        hexSizeX = Hex.renderer.bounds.size.x;
        hexSizeY = Hex.renderer.bounds.size.y;
        hexSizeZ = Hex.renderer.bounds.size.z;
        groundSizeX = Ground.renderer.bounds.size.x;
        groundSizeY = Ground.renderer.bounds.size.y;
        groundSizeZ = Ground.renderer.bounds.size.z;
    }

    Vector2 calcGridSize()
    {
        float sideLength = hexSizeZ / 2;
        int nrOfSides = (int)(groundSizeZ / sideLength + 0.00005f);
        int nrOfZHexes = (int)( nrOfSides * 2 / 3);
        if (nrOfZHexes % 2 == 0 
            && (nrOfSides + 0.5f) * sideLength > groundSizeZ)
            nrOfZHexes--;

        return new Vector2((int)(groundSizeX / hexSizeX), nrOfZHexes);
    }

    Vector3 calcInitPos()
    {
        Vector3 initPos;
        initPos = new Vector3(hexSizeX / 2 - groundSizeX / 2, 
            groundSizeY / 2 + hexSizeY / 2, groundSizeZ / 2 - hexSizeZ / 2);

        return initPos;
    }

    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
        Vector3 initPos = calcInitPos();
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexSizeX / 2;
        float x = gridPos.x * hexSizeX + initPos.x + offset;
        float z = initPos.z - gridPos.y * hexSizeZ * 0.75f;
        float y = groundSizeY / 2 + 0.137f;
        return new Vector3(x, y, z);
    }

    public Vector2 calcGridPos(Vector3 coord)
    {
        Vector3 initPos = calcInitPos();
        Vector2 gridPos = new Vector2();
        float offset = 0;
        gridPos.y = Mathf.RoundToInt((initPos.z - coord.z) / (hexSizeZ * 0.75f));
        if (gridPos.y % 2 != 0)
            offset = hexSizeX / 2;
        gridPos.x = Mathf.RoundToInt((coord.x - initPos.x - offset) / hexSizeX);
        return gridPos;
    }

    void createGrid()
    {
        Vector2 gridSize = calcGridSize();
        GameObject hexGridGO = new GameObject("HexGrid");
        Board = new Dictionary<Point, TileBehaviour>();

        for (float y = 0; y < gridSize.y; y++)
        {
            float sizeX = gridSize.x;
            if (y % 2 != 0 && (gridSize.x + 0.5) * hexSizeX > groundSizeX)
                sizeX--;
            for (float x = 0; x < sizeX; x++)
            {
                GameObject hex = (GameObject)Instantiate(Hex);
                Vector2 gridPos = new Vector2(x, y);
                hex.transform.position = calcWorldCoord(gridPos);
                hex.transform.parent = hexGridGO.transform;
                var tb = (TileBehaviour)hex.GetComponent("TileBehaviour");
                tb.tile = new Tile((int)x - (int)(y / 2), (int)y);
                Board.Add(tb.tile.Location, tb);
            }
        }
        bool equalLineLengths = (gridSize.x + 0.5) * hexSizeX <= groundSizeX;
        foreach(TileBehaviour tb in Board.Values)
            tb.tile.FindNeighbours(Board, gridSize, equalLineLengths);
    }

    private void DrawPath(IEnumerable<Tile> path)
    {
        if (this.path == null)
            this.path = new List<GameObject>();

        this.path.ForEach(Destroy);
        this.path.Clear();

        GameObject lines = GameObject.Find("Lines");
        if (lines == null)
            lines = new GameObject("Lines");
        foreach (Tile tile in path)
        {
            var line = (GameObject)Instantiate(Line);
            Vector2 gridPos = new Vector2(tile.X + tile.Y / 2, tile.Y);
            line.transform.position = calcWorldCoord(gridPos);
            this.path.Add(line);
            line.transform.parent = lines.transform;
        }
    }

    public void generateAndShowPath()
    {
        if (originTileTB == null || destTileTB == null)
        {
            DrawPath(new List<Tile>());
            return;
        }

        var path = PathFinder.FindPath(originTileTB.tile, destTileTB.tile);
        DrawPath(path);
        BaseChar selectedChar = GameMaster.instance.selectedChar;
        CharacterMovement cm = selectedChar.gameObject.
            GetComponent<CharacterMovement>();
        CombatManager.instance.stepsLeft -= path.ToList().Count - 1;
        HUD.instance.clickable = false;
        cm.StartMoving(path.ToList());
    }

    public static float calcDistance(Tile tile, Tile destTile)
    {
        float dx = Mathf.Abs(destTile.X - tile.X);
        float dy = Mathf.Abs(destTile.Y - tile.Y);
        int z1 = -(tile.X + tile.Y);
        int z2 = -(destTile.X + destTile.Y);
        float dz = Mathf.Abs(z2 - z1);

        return Mathf.Max(dx, dy, dz);
    }

    void switchOriginAndDestTiles()
    {
        if (!CombatManager.instance.playersTurn)
            return;
        HUD.instance.clickable = true;
        Material originMaterial = originTileTB.renderer.material;
        originTileTB.renderer.material = destTileTB.defaultMaterial;
        originTileTB.tile.Passable = true;
        originTileTB = destTileTB;
        originTileTB.renderer.material = originMaterial;
        destTileTB = null;
        generateAndShowPath();
        CombatManager.instance.charMoved();
    }

    public void setGroundSize(float width, float height)
    {
        destTileTB = null;
        originTileTB = null;
        Destroy(GameObject.Find("Lines"));
        Destroy(GameObject.Find("HexGrid"));
        Vector3 groundScale = Ground.transform.localScale;
        Vector3 groundSize = new Vector3(width, groundScale.y, height);
        Ground.transform.localScale = groundSize;
        Vector2 textureTiling = new Vector2(width * 2, height * 2);
        Ground.renderer.material.mainTextureScale = textureTiling;
        setSizes();
        createGrid();
    }
}
