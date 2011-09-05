//A* Pathfinding - Example Script
//This is an example script for starting paths from a unit to a position

using UnityEngine;
using System.Collections;
using AstarProcess;
using AstarClasses;
using AstarMath;

public class Clicker : MonoBehaviour {
	
	//An object which will be used as a marker of where the pathfinding target is currently
	public Transform target;
	
	//The Seeker to start pathfinding from when clicking
	public Seeker controler = null;
	
	//Or use an array of units
	public Seeker[] units;

    public bool hitNPC = false;
    public string npcName = "";
    public bool movementEnabled = true;
	
	RaycastHit hit;//, otherHits;
	
	public LayerMask mask;
	
	//The object to use when placing stuff on the ground
	public GameObject building;
	
	//If true, the target will be moved every frame instead of on every click, paths will not be calculated on click
	public bool continuous = false;

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
	void Update () {
		if (controler == null)
			controler = GameObject.Find("Player Character").GetComponent<Seeker>();
        
        if (!movementEnabled)
            return;

		if (Input.GetKeyDown ("mouse 0") || continuous) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast(ray, out hit,1000F, mask))
            {	
				target.position = hit.point;

                if (hit.collider.tag.Equals("NPC"))
                {
                    hitNPC = true;
                    GameObject npcGO = hit.collider.gameObject;
                    NPC npc = npcGO.GetComponent<NPC>();
                    GameObject pcGO = GameObject.Find("Player Character");
                    Vector3 pcPosition = pcGO.transform.position;
                    Vector3 npcPosition = npcGO.transform.position;
                    npcName = npc.npcName;
                    //Vector3 npcPosition = npcGO.transform.position;
                    Transform npcTransform = npcGO.transform;
                    if (Vector3.Distance(pcPosition, npcPosition) > 6.5f)
                        target.position = npcPosition + npcTransform.forward * 3;
                    else
                        return;
                }
                else
                    hitNPC = false;

				if (!continuous) {
					//Start a path from the unit(s) to the target position
					
					if (units.Length > 0) {
						for (int i=0;i<units.Length;i++) {
							//Call the seeker script to start a path to the target
							units[i].StartPath (units[i].transform.position,target.position);
						}
					} else {
						controler.StartPath (controler.transform.position,target.position);
					}
				}
			}
		}
		
		if (Input.GetKeyDown ("t") && building != null) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray,out hit,1000F,mask)) {
				target.position = hit.point;
				GameObject go = Instantiate (building,hit.point,Quaternion.identity) as GameObject;
				AstarPath.active.SetNodes (false,go.collider.bounds,true);
			}
		}
		
		if (Input.GetKeyDown ("r")) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray,out hit,1000F,mask)) {
				if (hit.transform.tag == "DynamicObstacle") {
					//Add a small delay (0.1 seconds) so the object has time to be destroyed first (it doesn't get destroyed until next frame
					StartCoroutine (AstarPath.active.SetNodes (true,hit.collider.bounds,true,0.1F));
					Destroy (hit.transform.gameObject);
				} else {
					Debug.Log (hit.transform.gameObject.tag);
				}
			}
		}
	}
}
