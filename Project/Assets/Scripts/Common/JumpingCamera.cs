using UnityEngine;
using System.Collections;

public class JumpingCamera : MonoBehaviour {

	public static JumpingCamera instance;
	public Transform TargetLookAt = null;
	
	public float Distance = 50f;
	public float MinDistance = 10f;
	public float MaxDistance = 80f;
	
	public float DistanceSmooth = 0.05f;
	//sekanti
	public float DistanceResumeSmooth = 1f;
	
	public float mouseSensitivityX = 5f;
	public float mouseSensitivityY = 5f;
	public float mouseWheelSensitivity = 40f;
	
	public float minLimitY = 0f;
	public float maxLimitY = 80f;
	
	public float smoothX = 0.1f;
	public float smoothY = 0.1f;
	//dvi sekančios
	public float occlusionDistanceStep = 0.5f;//2f; //0.5
	public int maxOcclusionChecks = 40;//40;
	
	private float mouseX = 0f;
	private float mouseY = 0f;
	private float startDistance = 0f;
	private float desiredDistance = 0f;
	private float velDistance = 0f;
	private Vector3 desiredPosition = Vector3.zero;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private Vector3 position = Vector3.zero;
	//dvi sekančios
	private float distanceSmooth = 0f;
	private float preOcclusionDistance = 0f;
	
	void Awake()
	{
		instance = this;
	}
	
	void Start () 
	{
		Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
		startDistance = Distance;
		reset();
	}
	
	void LateUpdate () 
	{
		if (TargetLookAt == null)
			return;
		
		handlePlayerInput();
		calcDesiredPosition();
		
		//checkCameraPoints(TargetLookAt.position, desiredPosition);
		//nuo čia
		int count = 0;
		do
		{
			calcDesiredPosition();
			count++;
			
		}
		while (checkIfOccluded(count));
		//iki čia
		updatePosition();
	}
	
	void handlePlayerInput()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			mouseX += Input.GetAxis("Mouse X") * mouseSensitivityX;
			mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
		}
		
		mouseY = clampAngle(mouseY, minLimitY, maxLimitY);
		float wheelAxis = Input.GetAxis("Mouse ScrollWheel");
		
		if (wheelAxis < -0.01 || wheelAxis > 0.01)
		{
			desiredDistance = Mathf.Clamp(Distance - wheelAxis * 
				mouseWheelSensitivity, MinDistance, MaxDistance);
			//dvi sekančios
			preOcclusionDistance = desiredDistance;
			distanceSmooth = DistanceSmooth;
		}
	}
	
	public void reset()
	{
		mouseX = 0;
		mouseY = 30;
		Distance = startDistance;
		//dvi sekančios
		desiredDistance = Distance;
		preOcclusionDistance = Distance;
	}
	
	void calcDesiredPosition()
	{
		//sekanti eilutė
		resetDesiredDistance();
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, 
		                            ref velDistance, distanceSmooth);
		desiredPosition = calcPosition(mouseY, mouseX, Distance);
	}
	
	Vector3 calcPosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
		Vector3 targetPosition = new Vector3(TargetLookAt.position.x,
		                                     TargetLookAt.position.y + 4f,
		                                     TargetLookAt.position.z - 2.5f);
		
		return targetPosition + rotation * direction;
	}
	//nuo cia
	bool checkIfOccluded(int count)
	{
		bool isOccluded = false;
		
		float nearestDistance = checkCameraPoints(TargetLookAt.position,
		                                          desiredPosition);
		
		if (nearestDistance != -1)
		{
			if (count < maxOcclusionChecks)
			{
				isOccluded = true;
				Distance -= occlusionDistanceStep;
				if (Distance < 0.25f)
					Distance = 0.25f;
			}
			else
				Distance = nearestDistance - Camera.mainCamera.nearClipPlane;
		
			desiredDistance = Distance;
			distanceSmooth = DistanceResumeSmooth;
		}
		
		return isOccluded;
	}
	
	float checkCameraPoints (Vector3 fromV, Vector3 toV)
	{
		float nearestDistance = -1f;
		
		RaycastHit hitInfo;
		
		ClipPlanePoints clipPlanePoints = ClipPlaneAtNear(toV);
		Debug.DrawLine (fromV, toV + transform.forward * -camera.nearClipPlane, 
		                Color.red);
		Debug.DrawLine(fromV, clipPlanePoints.LowerLeft);
		Debug.DrawLine(fromV, clipPlanePoints.LowerRight);
		Debug.DrawLine(fromV, clipPlanePoints.UpperLeft);
		Debug.DrawLine(fromV, clipPlanePoints.UpperRight);
		
		Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
		Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.LowerRight);
		Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.LowerLeft);
		Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
		
		if (Physics.Linecast(fromV, clipPlanePoints.UpperLeft, out hitInfo) &&
		                     hitInfo.collider.tag != "Player")
			nearestDistance = hitInfo.distance;

		if (Physics.Linecast(fromV, clipPlanePoints.UpperRight, out hitInfo) &&
		    hitInfo.collider.tag != "Player" && hitInfo.distance < nearestDistance)
				nearestDistance = hitInfo.distance;
		
		if (Physics.Linecast(fromV, clipPlanePoints.LowerLeft, out hitInfo) &&
		    hitInfo.collider.tag != "Player" && hitInfo.distance < nearestDistance)
				nearestDistance = hitInfo.distance;
		
		if (Physics.Linecast(fromV, clipPlanePoints.LowerRight, out hitInfo) &&
		    hitInfo.collider.tag != "Player" && hitInfo.distance < nearestDistance)
				nearestDistance = hitInfo.distance;
		
		if (Physics.Linecast(fromV, toV + transform.forward * 
		                     -camera.nearClipPlane, out hitInfo) &&
		    hitInfo.collider.tag != "Player" && hitInfo.distance < nearestDistance)
				nearestDistance = hitInfo.distance;
		
		return nearestDistance;
	}
	
	void resetDesiredDistance()
	{
		if (desiredDistance < preOcclusionDistance)
		{
			Vector3 pos = calcPosition(mouseY, mouseX, preOcclusionDistance);
			float nearestDistance = checkCameraPoints(TargetLookAt.position,
			                                          pos);
			if (nearestDistance == -1 || nearestDistance > preOcclusionDistance)
			{
				desiredDistance = preOcclusionDistance;
			}
		}
			
	}
	//iki cia
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
		transform.LookAt(TargetLookAt);
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
		
		tempCamera.AddComponent("jumpingCamera");
	}
		
	public static float clampAngle(float angle, float min, float max)
	{
		float myAngle = angle % 360;
		
		return Mathf.Clamp(myAngle, min, max);
	}
	//toliau viskas nauja
	struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 LowerLeft;
		public Vector3 UpperRight;
		public Vector3 LowerRight;
	}
	
	private static ClipPlanePoints ClipPlaneAtNear(Vector3 position)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints();
		Transform cameraTransform = Camera.mainCamera.transform;
		
		float FOVhalf = Camera.mainCamera.fieldOfView / 2 * Mathf.Deg2Rad;
		float aspect = Camera.mainCamera.aspect;
		float distance = Camera.mainCamera.nearClipPlane;
		float height = distance * Mathf.Tan(FOVhalf);
		float width = height * aspect;
		
		clipPlanePoints.LowerRight = position + cameraTransform.right * width;
		clipPlanePoints.LowerRight -= cameraTransform.up * height;
		clipPlanePoints.LowerRight += cameraTransform.forward * distance;
		
		clipPlanePoints.LowerLeft = position - cameraTransform.right * width;
		clipPlanePoints.LowerLeft -= cameraTransform.up * height;
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
