using UnityEngine;
using System.Collections;

public class Clicker : MonoBehaviour
{
    //An object which will be used as a marker of where the pathfinding target is currently
    public Transform target;
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

                GameObject pcGO;
                pcGO = GameMaster.instance.selectedChar.gameObject;
                Vector3 pcPos = pcGO.transform.position;
                pcPos.y = -0.6284826f;
                if (!correctTargetPos(hit.collider))
                    target.position = pcPos;

                if (Vector3.Distance(pcPos, target.position) > 3)
                    Messenger<bool>.Broadcast("targetPosChanged", true);
                else
                    Messenger<bool>.Broadcast("targetPosChanged", false);
            }
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

    bool correctTargetPos(Collider collider)
    {
        string tag = collider.tag;
        if (tag.Equals("NPC") || tag.Equals("Box"))
        {
            hitGO = collider.gameObject;

            GameObject pcGO = GameMaster.instance.selectedChar.gameObject;
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
            Vector3 targetPos = target.position;
            targetPos.y = -0.6284826f;
            target.position = targetPos;
            if (Vector3.Distance(pcPosition, hitPosition) > 6.5f
                || tag.Equals("Box"))
                target.position += 3 * hitTransform.forward;
            else
                return false;
        }
        else
        {
            hitNPC = false;
            hitBox = false;
        }
        return true;
    }
}
