//A* Pathfinding - Example Script
//This is an example script for starting paths from a unit to a position

using UnityEngine;
using System.Collections;
using AstarProcess;
using AstarClasses;
using AstarMath;

public class Clicker : MonoBehaviour
{

    //An object which will be used as a marker of where the pathfinding target is currently
    public Transform target;

    //Or use an array of units
    public static Clicker instance = null;

    public bool hitNPC = false;
    public bool hitBox = false;
    public string npcName = "";
    public string boxName = "";
    public bool movementEnabled = true;
    public GameObject hitGO = null;
    RaycastHit hit;

    public LayerMask mask;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Messenger<string>.AddListener("dialog starting", dialogStarting);
        Messenger<bool>.AddListener("enable movement", enableMovement);
    }

    void dialogStarting(string name)
    {
        enableMovement(false);
    }

    void enableMovement(bool enable)
    {
        if (enable)
            hitNPC = false;
        movementEnabled = enable;
    }

    // Update is called once per frame
    void Update()
    {

        if (!movementEnabled)
            return;

        if (Input.GetKeyDown("mouse 0"))
        {
            if (mouseOnGUI())
                return;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000F, mask))
            {
                target.position = hit.point;
                correctTargetPos(hit.collider);
            }
            if (!Application.loadedLevelName.Equals("Arena"))
                Messenger.Broadcast("targetPosChanged");
        }
    }

    bool mouseOnGUI()
    {
        Rect HUDrect;
        if (HUD.instance.Minimized)
            HUDrect = new Rect(Screen.width / 2 - 208, 0, 445, 34);
        else
            HUDrect = new Rect(Screen.width / 2 - 400, 0, 800, 90);

        int charCount = GameMaster.instance.characters.Count;
        float bottom = (Screen.height - charCount * 90) / 2;
        Rect charSelectorRect = new Rect(0, bottom, 67, charCount * 90);

        if (HUDrect.Contains(Input.mousePosition)
            || charSelectorRect.Contains(Input.mousePosition))
            return true;
        else
            return false;
    }

    void correctTargetPos(Collider collider)
    {
        string tag = collider.tag;
        if (tag.Equals("NPC") || tag.Equals("Box"))
        {
            hitGO = collider.gameObject;

            GameObject pcGO = GameObject.Find("Player Character");
            Vector3 pcPosition = pcGO.transform.position;
            Vector3 hitPosition = hitGO.transform.position;
            if (tag.Equals("NPC"))
            {
                hitNPC = true;
                NPC npc = hitGO.GetComponent<NPC>();
                npcName = npc.npcName;
            }
            else
            {
                hitBox = true;
                boxName = hitGO.name;
            }

            Transform hitTransform = hitGO.transform;
            if (Vector3.Distance(pcPosition, hitPosition) > 6.5f
                || tag.Equals("Box"))
                target.position = hitPosition + 3 * hitTransform.forward;
        }
        else
        {
            hitNPC = false;
            hitBox = false;
        }
    }
}
