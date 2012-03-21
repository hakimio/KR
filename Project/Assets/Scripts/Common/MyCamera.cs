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
    private Transform myTransform;

	void Awake()
	{
		instance = this;
		hits = new HashSet<Renderer>();
	}
	
	void Start () 
	{
        myTransform = transform;
        if (TargetLookAt != null)
            calcCameraPosition();
        else
            Debug.LogError("TargetLookAt was not assigned!");
	}
	
    void calcCameraPosition()
    {
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        startDistance = Vector3.Distance(myTransform.position,
            TargetLookAt.position);
        desiredPosition = myTransform.position;
        position = myTransform.position;
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
        
        changeVarOnKeyPress(KeyCode.UpArrow, ref MouseYRot, -1.25f);
        changeVarOnKeyPress(KeyCode.DownArrow, ref MouseYRot, 1.25f);
        changeVarOnKeyPress(KeyCode.LeftArrow, ref MouseXRot, -2.5f);
        changeVarOnKeyPress(KeyCode.RightArrow, ref MouseXRot, 2.5f);

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

    void changeVarOnKeyPress(KeyCode key, ref float varToChange, float amount)
    {
        if (Input.GetKey(key) && Time.time > timeToRotate)
        {
            varToChange += amount;
            timeToRotate = Time.time + 0.125f;
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
		HashSet<Renderer> localHits = checkCameraPoints();
		
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
	
	HashSet<Renderer> checkCameraPoints ()
	{
		RaycastHit[] tempHits;
		HashSet<Renderer> returnedHits = new HashSet<Renderer>();
        Vector3 targetPos = TargetLookAt.position;
		ClipPlanePoints clipPlanePoints = ClipPlaneAtNear(targetPos);
		
		if (DEBUG)
		{
			Debug.DrawLine (myTransform.position, TargetLookAt.position, 
			                Color.red);
			Debug.DrawLine(myTransform.position, clipPlanePoints.LowerLeft);
			Debug.DrawLine(myTransform.position, clipPlanePoints.LowerRight);
			Debug.DrawLine(myTransform.position, clipPlanePoints.UpperLeft);
			Debug.DrawLine(myTransform.position, clipPlanePoints.UpperRight);
			
			Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
			Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.LowerRight);
			Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.LowerLeft);
			Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
		}

        LayerMask mask = 1 << LayerMask.NameToLayer("NPC");
        mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
        mask = ~mask;
        
        getHits(TargetLookAt.position, mask, ref returnedHits);
        getHits(clipPlanePoints.UpperLeft, mask, ref returnedHits);
        getHits(clipPlanePoints.UpperRight, mask, ref returnedHits);
        getHits(clipPlanePoints.LowerLeft, mask, ref returnedHits);
        getHits(clipPlanePoints.LowerRight, mask, ref returnedHits);
		
		return returnedHits;
	}

    void getHits(Vector3 target, LayerMask mask, ref HashSet<Renderer> hits)
    {
        Vector3 direction = (target - myTransform.position).normalized;
        float distance = Vector3.Distance(myTransform.position, target);

        RaycastHit[] tempHits = Physics.RaycastAll(myTransform.position, 
            direction, distance, mask);
        foreach (RaycastHit hit in tempHits)
        {
            Renderer renderer = hit.collider.renderer;
            hits.Add(renderer);
        }
    }
	
	void resetTransparency()
	{
        if (hits.Count == 0)
            return;

		HashSet<Renderer> curHits = checkCameraPoints();
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

		myTransform.position = position;
		Vector3 targetPosition = new Vector3(TargetLookAt.position.x,
		                                     TargetLookAt.position.y + 4f,
		                                     TargetLookAt.position.z);
		myTransform.LookAt(targetPosition);
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
        Transform cameraTransform = myTransform;
		
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
