using UnityEngine;
using System.Collections;


public class AIFollow : MonoBehaviour
{
    private Clicker clicker;
    private GameObject[] NPCs;
    private float speakingDistance = 10.0f;

    bool pathComplete = true;
    NavMeshAgent navAgent;
    bool leader = false;
    Transform leaderTr;
    public float followDistance = 6;
    Transform myTransform;
    float leadersRadius;
    float myRadius;
    Vector3 myTargetPos;
    static bool leaderMoving = false;
    bool targetChanged = false;

    static bool[] takenPos = new bool[4];

    public void Start()
    {
        myTransform = transform;
        leaderTr = GameMaster.instance.selectedChar.gameObject.transform;
        navAgent = GetComponent<NavMeshAgent>();
        leaderChanged();
        myRadius = GetComponent<CapsuleCollider>().radius;
        myRadius *= myTransform.localScale.x;
        clicker = Clicker.instance;
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        Messenger<bool>.AddListener("targetPosChanged", targetPosChanged);
        Messenger.AddListener("selectedCharChanged", leaderChanged);
    }

    private void leaderChanged()
    {
        GameObject leaderGO = GameMaster.instance.selectedChar.gameObject;
        leadersRadius = leaderGO.GetComponent<CapsuleCollider>().radius;
        leadersRadius *= leaderGO.transform.localScale.x;
        Transform torus = myTransform.Find("Torus");
        leaderTr = leaderGO.transform;
        if (this.gameObject != leaderGO)
        {
            leader = false;
            torus.gameObject.active = false;
        }
        else
        {
            leader = true;
            torus.gameObject.active = true;
        }
    }

    private void checkForNPCs()
    {
        if (!clicker.hitNPC)
            return;
        foreach (GameObject npcGO in NPCs)
        {
            Vector3 npcPos = npcGO.transform.position;
            npcPos.y = myTransform.position.y;
            float distance = Vector3.Distance(npcPos, myTransform.position);
            string npcName = npcGO.GetComponent<NPC>().npcName;
            if (clicker.npcName.Equals(npcName) && distance < speakingDistance)
            {
                Messenger<string>.Broadcast("dialog starting", npcName);
                clicker.hitNPC = false;
                Vector3 dir = npcPos - myTransform.position;
                navAgent.Stop(true);
                StartCoroutine(Helper.rotate(myTransform,
                    Quaternion.LookRotation(dir), 0.5f));
            }
        }
    }

    private void lookAtBox()
    {
        if (!clicker.hitBox)
            return;
        Vector3 target = clicker.hitGO.transform.position;
        target.y = myTransform.position.y;
        Vector3 dir = target - myTransform.position;
        navAgent.Stop(true);
        StartCoroutine(Helper.rotate(myTransform,
            Quaternion.LookRotation(dir), 0.5f));
        Messenger<string>.Broadcast("toggleBoxVisibility", clicker.boxName);
    }

    void Update()
    {
        if (!pathComplete && !leader && leaderMoving && 
            targetChanged && isLeaderInFront())
        {
            targetChanged = false;
            navAgent.Resume();
            navAgent.SetDestination(myTargetPos);
        }

        if (!leader && leaderMoving && targetChanged)
            return;

        if (!pathComplete && !navAgent.pathPending 
            && navAgent.remainingDistance == 0)
        {
            navAgent.velocity = Vector3.zero;
            pathComplete = true;
            if (leader)
            {
                leaderMoving = false;
                checkForNPCs();
                lookAtBox();
            }
            else
            {
                navAgent.Stop(true);
                StartCoroutine(Helper.rotateLikeTarget(leaderTr, myTransform, 
                    0.3f));
            }
        }
    }

    int myPosNr = -1;

    Vector3 getFollowersPos()
    {
        GameMaster GM = GameMaster.instance;
        BaseChar myChar = GM.getCharByGO(gameObject);
        int myNr = GM.characters.IndexOf(myChar);
        int leadersNr = GM.characters.IndexOf(GM.selectedChar);
        int partialPosNr = -1;
        if (myNr > leadersNr)
            myNr--;
        for (int i = myNr; i < myNr + 4; i++)
        {
            int posNr = i % 4;
            if (takenPos[posNr])
                continue;
            Vector3 pos = posNrToCoord(posNr);
            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath(pos, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                takenPos[posNr] = true;
                clearTakenPosIfAllFollowersGotPos();
                myPosNr = posNr;
                return pos;
            }
            if (path.status == NavMeshPathStatus.PathPartial)
                partialPosNr = posNr;
        }

        if (partialPosNr != -1)
        {
            takenPos[partialPosNr] = true;
            clearTakenPosIfAllFollowersGotPos();
            myPosNr = partialPosNr;
            return posNrToCoord(partialPosNr);
        }

        for (int i = myNr; i < myNr + 4; i++)
        {
            int posNr = i % 4;
            if (!takenPos[posNr])
            {
                takenPos[posNr] = true;
                clearTakenPosIfAllFollowersGotPos();
                myPosNr = posNr;
                return posNrToCoord(posNr);
            }
        }

        return Vector3.zero;
    }

    void clearTakenPosIfAllFollowersGotPos()
    {
        int nrOfTakenPos = 0;
        for (int i = 0; i < takenPos.Length; i++)
            if (takenPos[i])
                nrOfTakenPos++;
        int followerCount = GameMaster.instance.characters.Count - 1;
        if (followerCount == nrOfTakenPos)
        {
            for (int i = 0; i < takenPos.Length; i++)
                takenPos[i] = false;
        }
    }

    Vector3 posNrToCoord(int posNr)
    {
        Vector3 targetPos = clicker.target.position;
        Vector3 fwdDir = (targetPos - leaderTr.position).normalized;
        Vector3 right = Quaternion.AngleAxis(-90, Vector3.up) * fwdDir;
        Vector3 left = Quaternion.AngleAxis(90, Vector3.up) * fwdDir;
        Vector3 pos = clicker.target.position;

        switch(posNr)
        {
            case 0:
                pos += followDistance * -fwdDir; 
                return pos + (myRadius + leadersRadius + 0.5f) * left;
            case 1:
                pos += followDistance * -fwdDir;
                return pos + (myRadius + leadersRadius + 0.5f) * right;
            case 2:
                pos += (myRadius + leadersRadius + 0.5f) * fwdDir; 
                return pos + (myRadius + leadersRadius + 0.5f) * left;
            case 3:
                pos += (myRadius + leadersRadius + 0.5f) * fwdDir; 
                return pos + (myRadius + leadersRadius + 0.5f) * right;
        }
        return Vector3.zero;
    }


    bool isLeaderInFront()
    {
        Vector3 leadersPos = leaderTr.position;
        Vector3 myPos = myTransform.position;
        Vector3 leadersTargetPos = clicker.target.position;
        leadersPos.y = clicker.target.position.y;
        myPos.y = myTargetPos.y;

        float leadersDistance = Vector3.Distance(leadersPos, leadersTargetPos);
        float myDistance = Vector3.Distance(myPos, myTargetPos);

        if (myPosNr > 1)
            return (leadersDistance + followDistance + leadersRadius < myDistance);
        else
            return (leadersDistance + leadersRadius < myDistance);
    }

    private void targetPosChanged(bool moveFollowers)
    {
        pathComplete = false;
        if (leader)
        {
            navAgent.Resume();
            navAgent.SetDestination(clicker.target.position);
            leaderMoving = true;
        }
        else if (moveFollowers)
        {
            myTargetPos = getFollowersPos();
            targetChanged = true;
        }
    }
}