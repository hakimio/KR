using UnityEngine;
using System.Collections;


public class AIFollow : MonoBehaviour {
	//[RequireComponent (typeof(CharacterController))]
	public float speed = 3.0F;
	public float rotationSpeed = 5.0F;
	public float pickNextWaypointDistance = 3.0F;
	public float SearchWaypointFrequency = 0.2F;
	public float maxStop = 3;
	
	public bool continousTargetSearch = false;
	public float targetSearchFrequency = 1.0F;
	private bool canSearchAgain = true;
	public Command command = Command.Stay;
	
	public Transform target;
	private CharacterController controller;
	private AIAnimation animator;
	private Seeker seeker;
	
	// Make sure there is always a character controller
	
	public enum Command {
		Stay,
		Walk
	}

    private Clicker clicker;
    private Transform PC;
    private GameObject[] NPCs;
    private float speakingDistance = 10.0f;

	public IEnumerator Start () 
    {
        if (controller == null)
            controller = GameObject.Find("Player Character").
                GetComponent<CharacterController>();
        PC = GameObject.Find("Player Character").transform;
        GameObject mainCamera = GameObject.Find("Main Camera");
        clicker = mainCamera.GetComponent<Clicker>();
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

		waypointPosition = transform.position;
		command = Command.Stay;
		//controller = GetComponent (typeof(CharacterController)) as CharacterController;
		//controller.stepOffset = 5f;
		Object anim = GetComponent (typeof(AIAnimation));
		animator = anim != null ? anim as AIAnimation : null;
		seeker = GetComponent (typeof(Seeker)) as Seeker;
		
		StartCoroutine (Patrol());
		yield return new WaitForSeconds (Random.value*0.5F);
		
		if (continousTargetSearch) {
			StartCoroutine (SearchPlayer());
		}
		
		while (true) {
			FindPoint (curpoint);
			yield return new WaitForSeconds (SearchWaypointFrequency);
		}
	}
	
	private Vector3 waypointPosition;
	//private bool continuous = false;
	private Vector3[] points;
	private int curpoint = 0;
	
	public void Update () {
		//Debug.color = Color.blue;
		Debug.DrawLine (transform.position, waypointPosition, Color.blue);
	}
	
	public IEnumerator SearchPlayer () {
		yield return 0;
		while (true) {
			yield return 0;
			while (!canSearchAgain) {
				yield return 0;
			}
			if (continousTargetSearch) {
				canSearchAgain = false;
				seeker.StartPath (transform.position,target.position);
			}
			yield return new WaitForSeconds (targetSearchFrequency);
		}
		
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

	public void PathComplete (Vector3[] newPoints) {

		canSearchAgain = true;
		points = newPoints;
		FindPoint (0);
		command = Command.Walk;
	}
	
	public void PathError () {
		canSearchAgain = true;
	}
	
	public bool HasReachedTarget () {
		return curpoint >= points.Length;
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
	public void FindPoint (int cpoint) {
		curpoint = cpoint;
        
		if (points == null || points.Length == 0 || curpoint >= points.Length) {	
			waypointPosition = transform.position;
			Stop ();
            checkForNPCs();
            lookAtBox();
			return;
		}

		if (points.Length == 1) {
			waypointPosition = points[0];
			command = Command.Walk;
            checkForNPCs();
            lookAtBox();
			return;
		}
		
		command = Command.Walk;
		
		waypointPosition = points[curpoint];
		Vector3 p = waypointPosition;
		p.y = transform.position.y;
		
		if (curpoint < points.Length - 1) {
			if ((transform.position-p).sqrMagnitude < pickNextWaypointDistance*pickNextWaypointDistance) {
				curpoint++;
				FindPoint (curpoint);
			}
			
		} else {
			if ((transform.position-p).sqrMagnitude < maxStop*maxStop) {
				curpoint++;
				FindPoint (curpoint);
			}
		}
	}
	
	public IEnumerator Patrol () {
		while (true) {
			if (command == Command.Walk) {
				MoveTowards(waypointPosition);
			}
			
			yield return 0;
		}
	}
	
	public void Stop () {
		command = Command.Stay;
		if (animator != null) {
			animator.SetSpeed (0.0F);
		}
	}
	
	public void RotateTowards (Vector3 position) {
		if (animator != null) {
			animator.SetSpeed (0.0F);
		}
		
		Vector3 direction = position - transform.position;
		direction.y = 0;
		
		if (curpoint == points.Length - 1 && direction.sqrMagnitude < maxStop*maxStop) {
			FindPoint (curpoint);
			return;
		}
		
		if (direction.sqrMagnitude < 0.1F*0.1F) {
			return;
		}
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
	
	public void MoveTowards (Vector3 position) {
		Vector3 direction = position - transform.position;
		direction.y = 0;
		
		if (direction.sqrMagnitude < 0.2F*0.2F) {
			controller.SimpleMove(Vector3.zero);
			Stop ();
			return;
		}
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	
		// Modify speed so we slow down when we are not facing the target
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		float speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);
		
		// Move the character
		if (controller) {
			direction = forward * speed * speedModifier*speedModifier;
			controller.SimpleMove(direction);
		} else {
			direction = forward * speed * speedModifier*speedModifier;
			transform.Translate (direction*Time.deltaTime,Space.World);
		}
		
		if (animator != null) {
			animator.SetSpeed (speed * speedModifier);
		}
	}
}