using UnityEngine;
using System.Collections;


public class AIFollow : MonoBehaviour
{
    private Clicker clicker;
    private Transform PC;
    private GameObject[] NPCs;
    private float speakingDistance = 10.0f;
    bool pathComplete = true;
    NavMeshAgent navAgent;
    Transform myTransform;
    public float pickNextWaypointDistance = 3;

    public void Start()
    {
        myTransform = transform;
        navAgent = GetComponent<NavMeshAgent>();
        PC = GameObject.Find("Player Character").transform;
        GameObject mainCamera = GameObject.Find("Main Camera");
        clicker = mainCamera.GetComponent<Clicker>();
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        Messenger.AddListener("targetPosChanged", targetPosChanged);
    }

    private void checkForNPCs()
    {
        if (!clicker.hitNPC)
            return;
        foreach (GameObject npcGO in NPCs)
        {
            Vector3 npcPos = npcGO.transform.position;
            float distance = Vector3.Distance(npcPos, PC.position);
            string npcName = npcGO.GetComponent<NPC>().npcName;
            if (clicker.npcName.Equals(npcName) && distance < speakingDistance)
            {
                Messenger<string>.Broadcast("dialog starting", npcName);
                clicker.hitNPC = false;
            }
        }
    }

    private void lookAtBox()
    {
        if (!clicker.hitBox)
            return;
        GameObject pcGO = GameObject.Find("Player Character");
        Vector3 target = clicker.hitGO.transform.position;
        target.y = pcGO.transform.position.y;
        pcGO.transform.LookAt(target);
        Messenger<string>.Broadcast("toggleBoxVisibility", clicker.boxName);
    }

    void Update()
    {
        if (!pathComplete && !navAgent.pathPending
            && navAgent.remainingDistance == 0)
        {
            checkForNPCs();
            lookAtBox();
            pathComplete = true;
            navAgent.velocity = Vector3.zero;
            animation.CrossFade("idle");
        }
    }

    private void targetPosChanged()
    {
        pathComplete = false;
        animation.CrossFade("run");
        navAgent.SetDestination(Clicker.instance.target.position);
    }
}