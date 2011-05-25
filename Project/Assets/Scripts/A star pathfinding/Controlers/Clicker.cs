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
	
	RaycastHit hit;
	
	public LayerMask mask;
	
	//The object to use when placing stuff on the ground
	public GameObject building;
	
	//If true, the target will be moved every frame instead of on every click, paths will not be calculated on click
	public bool continuous = false;
		
	// Update is called once per frame
	void Update () {
		if (controler == null)
			controler = GameObject.Find("Player Character").GetComponent<Seeker>();
		if (Input.GetKeyDown ("mouse 0") || continuous) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray,out hit,Mathf.Infinity,mask)) {
				/*if (hit.collider.renderer.material.color.a < 1.0f)
				{
					RaycastHit tempHit = hit;
					LayerMask layer = tempHit.collider.gameObject.layer;
					tempHit.collider.gameObject.layer = LayerMask.
						NameToLayer("Ignore Raycast");
					Physics.Raycast (ray,out hit,1000F,mask);
					tempHit.collider.gameObject.layer = layer;
				}*/
				
				target.position = hit.point;
				
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
