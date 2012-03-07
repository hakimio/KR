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
    public GameObject PlayerChar;
    public GameObject Monster;

    public Tile selectedTile = null;
    public TileBehaviour originTileTB = null;
    public TileBehaviour destTileTB = null;

    float hexSizeX, hexSizeY, hexSizeZ, groundSizeX, groundSizeY, groundSizeZ;
    public static GridManager instance = null;
    public GameObject CameraTarget;

    List<GameObject> path;

    void Awake()
    {
        instance = this;
        MyCamera.instance.TargetLookAt = CameraTarget.transform;
    }
    void Start()
    {
        setSizes();
        createGrid();
        instantiateCharacters();
        generateAndShowPath();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }

    void instantiateCharacters()
    {
        GameObject PC = (GameObject)Instantiate(PlayerChar);
        PC.transform.position = originTileTB.transform.position;
        PC.transform.position += new Vector3(0, 0.26f, 0);
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

    void createGrid()
    {
        Vector2 gridSize = calcGridSize();
        GameObject hexGridGO = new GameObject("HexGrid");
        Dictionary<Point, Tile> board = new Dictionary<Point, Tile>();

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
                board.Add(tb.tile.Location, tb.tile);
                if (x == 0 && y == 0)
                    setAsOrigin(tb);
                if (y == (int)gridSize.y / 2 && x == (int)sizeX / 2)
                    initializeMonster(hex.transform.position);
            }
        }
        bool equalLineLengths = (gridSize.x + 0.5) * hexSizeX <= groundSizeX;
        foreach(Tile tile in board.Values)
            tile.FindNeighbours(board, gridSize, equalLineLengths);
    }

    void initializeMonster(Vector3 pos)
    {
        GameObject monster = (GameObject)Instantiate(Monster);
        monster.transform.position = pos;
        float height = monster.collider.bounds.size.y * monster.transform.localScale.y;
        CharacterController CC = monster.GetComponent<CharacterController>();
        float offsetZ = monster.transform.localScale.y * CC.center.y;
        monster.transform.position += new Vector3(0, height / 2, -offsetZ);
        monster.animation.wrapMode = WrapMode.Loop;
    }

    void setAsOrigin(TileBehaviour TB)
    {
        TB.renderer.material = TB.OpaqueMaterial;
        Color red = Color.red;
        red.a = 158f / 255f;
        TB.renderer.material.color = red;
        originTileTB = TB;
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
        Func<Tile, Tile, double> distance = (node1, node2) => 1;

        var path = PathFinder.FindPath(originTileTB.tile, destTileTB.tile, 
            distance, calcDistance);
        DrawPath(path);
        CharacterMovement.instance.StartMoving(path.ToList());
    }

    double calcDistance(Tile tile)
    {
        Tile destTile = destTileTB.tile;
        float dx = Mathf.Abs(destTile.X - tile.X);
        float dy = Mathf.Abs(destTile.Y - tile.Y);
        int z1 = -(tile.X + tile.Y);
        int z2 = -(destTile.X + destTile.Y);
        float dz = Mathf.Abs(z2 - z1);

        return Mathf.Max(dx, dy, dz);
    }

    public void setGroundSize(float width, float height)
    {
        destTileTB = null;
        originTileTB = null;
        Destroy(CharacterMovement.instance.gameObject);
        Destroy(GameObject.Find("Lines"));
        Destroy(GameObject.Find("HexGrid"));
        Vector3 groundScale = Ground.transform.localScale;
        Vector3 groundSize = new Vector3(width, groundScale.y, height);
        Ground.transform.localScale = groundSize;
        Vector2 textureTiling = new Vector2(width * 2, height * 2);
        Ground.renderer.material.mainTextureScale = textureTiling;
        setSizes();
        createGrid();
        instantiateCharacters();
    }
}
