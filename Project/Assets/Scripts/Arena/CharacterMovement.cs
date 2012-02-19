using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement: MonoBehaviour
{
    public float speed = 0.0025F;
    public float rotationSpeed = 0.004F;

    private CharacterController controller;
    public static CharacterMovement instance = null;
    public static float MinNextTileDist = 0.07f;

    Vector3 curTilePos;
    Tile curTile;
    List<Tile> path;
    public bool IsMoving { get; private set; }
    Transform myTransform;

    void Awake()
    {
        instance = this;
        IsMoving = false;
    }

    // Use this for initialization
    void Start()
    {
        controller = this.GetComponent<CharacterController>();

        animation.wrapMode = WrapMode.Loop;
        myTransform = transform;
    }

    void Update()
    {
        if (!IsMoving)
            return;

        if ((curTilePos - myTransform.position).magnitude < MinNextTileDist)
        {
            if (path.IndexOf(curTile) == 0)
            {
                IsMoving = false;
                animation.CrossFade("idle");
                switchOriginAndDestinationTiles();
                return;
            }
            curTile = path[path.IndexOf(curTile) - 1];
            curTilePos = calcTilePos(curTile);
        }

        MoveTowards(curTilePos);
    }

    void switchOriginAndDestinationTiles()
    {
        GridManager GM = GridManager.instance;
        Material originMaterial = GM.originTileTB.renderer.material;
        GM.originTileTB.renderer.material = GM.destTileTB.defaultMaterial;
        GM.originTileTB = GM.destTileTB;
        GM.originTileTB.renderer.material = originMaterial;
        GM.destTileTB = null;
        GM.generateAndShowPath();
    }

    public void StartMoving(List<Tile> path)
    {
        if (path.Count == 0)
            return;
        curTile = path[path.Count - 2];
        curTilePos = calcTilePos(curTile);
        IsMoving = true;
        this.path = path;
    }

    Vector3 calcTilePos(Tile tile)
    {
        Vector2 tileGridPos = new Vector2(tile.X + tile.Y / 2, tile.Y);
        Vector3 tilePos = GridManager.instance.calcWorldCoord(tileGridPos);
        tilePos.y = myTransform.position.y;
        return tilePos;
    }
    
    void MoveTowards(Vector3 position)
    {
        Vector3 dir = position - myTransform.position;

        // Rotate towards the target
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

        // Modify speed so we slow down when we are not facing the target
        Vector3 forwardDir = myTransform.forward;
        //Move Forwards - forwardDir is already normalized
        forwardDir = forwardDir * speed;
        float speedModifier = Vector3.Dot(dir.normalized, myTransform.forward);
        forwardDir *= Mathf.Clamp01(speedModifier);
        if (speedModifier > 0.95f)
        {
            controller.SimpleMove(forwardDir);
            if (!animation["walk"].enabled)
                animation.CrossFade("walk");
        }
        else if (!animation["idle"].enabled)
            animation.CrossFade("idle");
    }
}
