using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMovement: MonoBehaviour
{
    public float speed = 0.0025F;
    public float rotationSpeed = 0.004F;

    private CharacterController controller;
    public float MinNextTileDist = 0.07f;
    public bool monster = false;

    Vector3 curTilePos;
    Tile curTile;
    List<Tile> path;
    public bool IsMoving { get; private set; }
    Transform myTransform;
    int nrOfSteps;
    bool rotateAndStop = false;

    void Awake()
    {
        IsMoving = false;
    }

    // Use this for initialization
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        animation.wrapMode = WrapMode.Loop;
        if (monster)
        {
            animation["IdleFeM"].layer = -1;
            animation["CreepFem"].layer = -1;
            animation["ChewFem"].layer = -1;
            animation["ChewFem"].wrapMode = WrapMode.Once;
            animation["ChewFem"].speed = 0.5f;
            animation["PoisionFem"].layer = -1;
            animation["PoisionFem"].wrapMode = WrapMode.Once;
        }
        else
        {
            animation["idle"].layer = -1;
            animation["walk"].layer = -1;
            animation["run"].layer = -1;
            if (animation["shoot"] != null)
                animation["shoot"].layer = -1;
        }
        
        myTransform = transform;
    }

    void Update()
    {
        if (!IsMoving)
            return;

        Vector3 myPos;
        if (monster)
        {
            CharacterController CC = GetComponent<CharacterController>();
            myPos = CC.collider.bounds.center;
        }
        else
            myPos = myTransform.position;

        if ((curTilePos - myPos).magnitude < MinNextTileDist)
        {
            if (path.Count - 1 - path.IndexOf(curTile) == nrOfSteps || 
                path.IndexOf(curTile) == 0)
            {
                if (path.IndexOf(curTile) - 1 == 0)
                    rotateAndStop = true;
                else
                {
                    endMovement();
                    return;
                }
            }
            curTile = path[path.IndexOf(curTile) - 1];
            curTilePos = calcTilePos(curTile);
        }

        MoveTowards(curTilePos);
    }

    void endMovement()
    {
        IsMoving = false;
        if (monster)
            animation.CrossFade("IdleFeM");
        else
            animation.CrossFade("idle");
        Messenger.Broadcast("characterMoved");
    }

    public void StartMoving(List<Tile> path)
    {
        if (path.Count == 0)
            return;
        curTile = path[path.Count - 2];
        curTilePos = calcTilePos(curTile);
        IsMoving = true;
        this.path = path;
        nrOfSteps = int.MaxValue;
    }

    public void StartMoving(List<Tile> path, int nrOfSteps)
    {
        StartMoving(path);
        this.nrOfSteps = nrOfSteps;
        rotateAndStop = false;
        if (nrOfSteps == 0)
            rotateAndStop = true;
    }

    Vector3 calcTilePos(Tile tile)
    {
        Vector2 tileGridPos = new Vector2(tile.X + tile.Y / 2, tile.Y);
        Vector3 tilePos = GridManager.instance.calcWorldCoord(tileGridPos);
        if (monster)
        {
            CharacterController CC = GetComponent<CharacterController>();
            Vector3 myPos = CC.collider.bounds.center;
            tilePos.y = myPos.y;
        }
        else
            tilePos.y = myTransform.position.y;
        return tilePos;
    }
    
    void MoveTowards(Vector3 position)
    {
        Vector3 dir;
        if (monster)
        {
            CharacterController CC = GetComponent<CharacterController>();
            Vector3 myPos = CC.collider.bounds.center;
            dir = position - myPos;
        }
        else
            dir = position - myTransform.position;

        // Rotate towards the target
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

        // Modify speed so we slow down when we are not facing the target
        Vector3 forwardDir = myTransform.forward;
        //Move Forwards - forwardDir is already normalized
        forwardDir = forwardDir * speed;
        float speedModifier = Vector3.Dot(dir.normalized, myTransform.forward);
        forwardDir *= Mathf.Clamp01(speedModifier);
        float speedModMin = 0.97f;
        if (monster)
            speedModMin = 0.99f;

        if (speedModifier > speedModMin)
        {
            if (rotateAndStop)
            {
                endMovement();
                return;
            }

            controller.SimpleMove(forwardDir);
            if (!monster && !animation["walk"].enabled)
                animation.CrossFade("walk");
            else if (monster && !animation["CreepFem"].enabled)
                animation.CrossFade("CreepFem");
        }
        else if (!monster && !animation["idle"].enabled)
            animation.CrossFade("idle");
        else if (monster && !animation["IdleFeM"].enabled)
            animation.CrossFade("IdleFeM");
    }
}
