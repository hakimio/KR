using UnityEngine;
using System.Collections;

public class RandomPathExecuter : MonoBehaviour {

	public Vector3 worldOrigin;
	public float worldWidth = 100;
	public GameObject projectorPrefab;
	
	public Transform targetGO;
	
	private Seeker[] seekers;
	
	public void OnGUI () {
		if (GUILayout.Button ("Send All At Once")) {
			SendAllAtOnce ();
		}
		
		GUILayout.Label ("Log - 5 out of 20 entries");
		if (AstarPath.log != null) {
			for (int i=0;i<5;i++) {
				GUI.color = new Color (1,1,1,1-(i/5F));
				GUILayout.Label (AstarPath.log[i]);
				GUI.color = Color.white;
			}
		}
	}
	
	public void SendAllAtOnce () {
		foreach (Seeker seeker in seekers) {
			Vector2 rnd = Random.insideUnitCircle;
			Vector3 target = new Vector3 (rnd.x,0,rnd.y)*worldWidth;
			target += worldOrigin;
			
			seeker.StartPath (seeker.transform.position,target);
			//targetGO.position = target;
		}
		
	}
	
	// Use this for initialization
	IEnumerator Start () {
		seekers = FindObjectsOfType(typeof(Seeker)) as Seeker[];
		
		while (true) {
			foreach (Seeker seeker in seekers) {
				Vector2 rnd = Random.insideUnitCircle;
				Vector3 target = new Vector3 (rnd.x,0,rnd.y)*worldWidth;
				target += worldOrigin;
				seeker.StartPath (seeker.transform.position,target);
				targetGO.position = target;
				yield return new WaitForSeconds (0.5F);
			}
			yield return new WaitForSeconds (0.5F);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
