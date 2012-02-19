using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCamera : MonoBehaviour 
{
    public bool controllingEnabled = true;
	public static MyCamera instance;
	public Transform TargetLookAt = null;
	public static bool DEBUG = false;
	
	public float Distance = 50f;
	public float MinDistance = 12f;
	public float MaxDistance = 80f;
	
	public float DistanceSmooth = 0.05f;
	
	public float mouseSensitivityX = 5f;
	public float mouseSensitivityY = 5f;
	public float mouseWheelSensitivity = 40f;
	
	public float minLimitY = 0f;
	public float maxLimitY = 80f;
	
	public float smoothX = 0.1f;
	public float smoothY = 0.1f;
	
	public float MouseXRot = 0f;
	public float MouseYRot = 0f;
	private float startDistance = 0f;
	private float desiredDistance = 0f;
	private float velDistance = 0f;
	private Vector3 desiredPosition = Vector3.zero;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private Vector3 position = Vector3.zero;
	private HashSet<Renderer> hits;

	void Awake()
	{
		instance = this;
		hits = new HashSet<Renderer>();
	}
	
	void Start () 
	{
		Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
		startDistance = Vector3.Distance(this.transform.position, 
            TargetLookAt.position);
        desiredPosition = transform.position;
        position = transform.position;
        reset();
	}
	
	void LateUpdate () 
	{
		if (TargetLookAt == null)
			return;
		handlePlayerInput();
		calcDesiredPosition();
        if (TargetLookAt.collider != null)
            checkIfOccluded();
		updatePosition();
	}

    float timeToRotate = 0;

	void handlePlayerInput()
	{
        if (!controllingEnabled)
            return;
		if (Input.GetKey(KeyCode.LeftControl))
		{
			MouseXRot += Input.GetAxis("Mouse X") * mouseSensitivityX;
			MouseYRot -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
		}

        if (Input.GetKey(KeyCode.UpArrow) && Time.time > timeToRotate)
        {
            MouseYRot -= 1.25f;
            timeToRotate = Time.time + 0.125f;
        }

        if (Input.GetKey(KeyCode.DownArrow) && Time.time > timeToRotate)
        {
            MouseYRot += 1.25f;
            timeToRotate = Time.time + 0.125f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Time.time > timeToRotate)
        {
            MouseXRot -= 2.5f;
            timeToRotate = Time.time + 0.125f;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Time.time > timeToRotate)
        {
            MouseXRot += 2.5f;
            timeToRotate = Time.time + 0.125f;
        }
		MouseYRot = clampAngle(MouseYRot, minLimitY, maxLimitY);

        if (HUD.instance != null && !HUD.instance.Minimized)
        {
            Rect HUDRect = new Rect(Screen.width / 2 - 392, 5, 194, 78);
            if (HUDRect.Contains(Input.mousePosition))
                return;
        }

		float wheelAxis = Input.GetAxis("Mouse ScrollWheel");
		
		if (wheelAxis < -0.01 || wheelAxis > 0.01)
		{	
			desiredDistance = Mathf.Clamp(Distance - wheelAxis * 
				mouseWheelSensitivity, MinDistance, MaxDistance);
		}
	}
	
	public void reset()
	{
		Distance = startDistance;
		desiredDistance = Distance;
	}
	
	void calcDesiredPosition()
	{
        if (TargetLookAt.collider != null)
		    resetTransparency();
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, 
		                            ref velDistance, DistanceSmooth);
		desiredPosition = calcPosition(MouseYRot, MouseXRot, Distance);
	}
	
	Vector3 calcPosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
		Vector3 targetPosition = new Vector3(TargetLookAt.position.x,
		                                     TargetLookAt.position.y,
		                                     TargetLookAt.position.z);
		
		return targetPosition + rotation * direction;
	}

	void checkIfOccluded()
	{
		List<Renderer> localHits = checkCameraPoints();
		
		foreach (Renderer hit in localHits)
		{
			if (hit.material.color.a == 1)
			{
				hit.material.shader = Shader.Find("Transparent/Diffuse");
				Color c = hit.renderer.material.color;
				c.a = 0.3f;
				hit.material.color = c;
				hits.Add(hit);
			}
		}
	}
	
	List<Renderer> checkCameraPoints ()
	{
		HashSet<RaycastHit> raycastHits = new HashSet<RaycastHit>();
		RaycastHit[] tempHits;
		Vector3 direction;
		float distance;
		List<Renderer> returnedHits = new List<Renderer>();
		ClipPlanePoints clipPlanePoints = ClipPlaneAtNear(TargetLookAt.position);
		
		if (DEBUG)
		{
			Debug.DrawLine (transform.position, TargetLookAt.position, 
			                Color.red);
			Debug.DrawLine(transform.position, clipPlanePoints.LowerLeft);
			Debug.DrawLine(transform.position, clipPlanePoints.LowerRight);
			Debug.DrawLine(transform.position, clipPlanePoints.UpperLeft);
			Debug.DrawLine(transform.position, clipPlanePoints.UpperRight);
			
			Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
			Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.LowerRight);
			Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.LowerLeft);
			Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
		}
		direction = (TargetLookAt.position - transform.position).normalized;
		distance = Vector3.Distance(transform.position, TargetLookAt.position);
        LayerMask mask = 1 << LayerMask.NameToLayer("NPC");
        mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
        mask = ~mask;

		tempHits = Physics.RaycastAll(transform.position, direction, distance, 
            mask);
		foreach (RaycastHit hit in tempHits)
			raycastHits.Add(hit);
		
		direction = (clipPlanePoints.UpperLeft - transform.position).normalized;
		distance = Vector3.Distance(transform.position, clipPlanePoints.UpperLeft);

        tempHits = Physics.RaycastAll(transform.position, direction, distance,
            mask);
		foreach (RaycastHit hit in tempHits)
			raycastHits.Add(hit);
		
		direction = (clipPlanePoints.UpperRight - transform.position).normalized;
		distance = Vector3.Distance(transform.position, clipPlanePoints.UpperRight);

        tempHits = Physics.RaycastAll(transform.position, direction, distance,
            mask);
		foreach (RaycastHit hit in tempHits)
			raycastHits.Add(hit);
		
		direction = (clipPlanePoints.LowerLeft - transform.position).normalized;
		distance = Vector3.Distance(transform.position, clipPlanePoints.LowerLeft);

        tempHits = Physics.RaycastAll(transform.position, direction, distance,
            mask);
		foreach (RaycastHit hit in tempHits)
			raycastHits.Add(hit);
		
		direction = (clipPlanePoints.LowerRight - transform.position).normalized;
		distance = Vector3.Distance(transform.position, clipPlanePoints.LowerRight);

        tempHits = Physics.RaycastAll(transform.position, direction, distance,
            mask);
		foreach (RaycastHit hit in tempHits)
			raycastHits.Add(hit);
		
		foreach (RaycastHit raycastHit in raycastHits)
		{
			Renderer renderer = raycastHit.collider.renderer;
			returnedHits.Add(renderer);
		}
		
		return returnedHits;
	}
	
	void resetTransparency()
	{
		List<Renderer> curHits = checkCameraPoints();
		HashSet<Renderer> toRemove = new HashSet<Renderer>();
		
		if (curHits.Count == 0)
		{
			foreach (Renderer hit in hits)
			{
				hit.material.shader = Shader.Find("Diffuse");
				Color c = hit.material.color;
				c.a = 1f;
				hit.material.color = c;
			}
			hits.Clear();
		}
		else
		{
			foreach (Renderer hit in hits)
			{
				if (!curHits.Contains(hit))
				{
					hit.material.shader = Shader.Find("Diffuse");
					Color c = hit.material.color;
					c.a = 1f;
					hit.material.color = c;
					toRemove.Add(hit);
				}
			}
			
			foreach (Renderer hit in toRemove)
				hits.Remove(hit);
		}
	}
	
	void updatePosition()
	{
		float posX = Mathf.SmoothDamp(position.x, 
		                              desiredPosition.x, ref velX, smoothX);
		float posY = Mathf.SmoothDamp(position.y, 
		                              desiredPosition.y, ref velY, smoothY);
		float posZ = Mathf.SmoothDamp(position.z, 
		                              desiredPosition.z, ref velZ, smoothX);
		position = new Vector3(posX, posY, posZ);

		transform.position = position;
		Vector3 targetPosition = new Vector3(TargetLookAt.position.x,
		                                     TargetLookAt.position.y + 4f,
		                                     TargetLookAt.position.z);
		transform.LookAt(targetPosition);
	}
	
	public static void acquireCamera()
	{
		GameObject tempCamera;

		if (Camera.mainCamera != null)
			tempCamera = Camera.mainCamera.gameObject;
		else
		{
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent("Camera");
			tempCamera.tag = "MainCamera";
		}
		
		tempCamera.AddComponent("MyCamera");
	}
		
	public static float clampAngle(float angle, float min, float max)
	{
		float myAngle = angle % 360;
		
		return Mathf.Clamp(myAngle, min, max);
	}

	struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 LowerLeft;
		public Vector3 UpperRight;
		public Vector3 LowerRight;
	}
	
	private ClipPlanePoints ClipPlaneAtNear(Vector3 position)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints();
		Transform cameraTransform = Camera.mainCamera.transform;
		
		float distance = Camera.mainCamera.nearClipPlane;
		Bounds bounds = TargetLookAt.GetComponent<Collider>().bounds;
		float height = bounds.size.x/2f - 0.2f;
		float width = bounds.size.x/2f - 0.3f;
		
		clipPlanePoints.LowerRight = position + cameraTransform.right * width;
		clipPlanePoints.LowerRight += cameraTransform.forward * distance;
		
		clipPlanePoints.LowerLeft = position - cameraTransform.right * width;
		clipPlanePoints.LowerLeft += cameraTransform.forward * distance;
		
		clipPlanePoints.UpperRight = position + cameraTransform.right * width;
		clipPlanePoints.UpperRight += cameraTransform.up * height;
		clipPlanePoints.UpperRight += cameraTransform.forward * distance;
		
		clipPlanePoints.UpperLeft = position - cameraTransform.right * width;
		clipPlanePoints.UpperLeft += cameraTransform.up * height;
		clipPlanePoints.UpperLeft += cameraTransform.forward * distance;
		
		return clipPlanePoints;
	}
}
