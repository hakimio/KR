using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCamera: MonoBehaviour
{
    public bool controllingEnabled = true;
    public static MyCamera instance;
    public Transform Target = null;
    public static bool DEBUG = false;

    private float Distance = 0f;
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

    private float MouseXRot = 0f;
    private float MouseYRot = 0f;
    private float startDistance = 0f;
    private float desiredDistance = 0f;
    private float velDistance = 0f;
    private Vector3 desiredPosition = Vector3.zero;
    private HashSet<Renderer> hits;
    private Transform myTransform;

    private Transform targetsParent;
    public float panSpeed = 2;
    public float returnInitSpeed = 1;
    private Transform originalTarget;
    public float maxPanX;
    public float maxPanY;
    float curPanXLimit, curPanYLimit;
    public bool cameraReturns;
    public float YRotSpeed = 12.5f;
    public float XRotSpeed = 25f;

    void Awake()
    {
        instance = this;
        hits = new HashSet<Renderer>();
    }

    void Start()
    {
        curPanSpeed = panSpeed;
        myTransform = transform;
        targetsParent = Target.parent;
        Target.parent = null;
        duplicateTarget();

        if (Target != null)
            calcCameraPosition();
        else
            Debug.LogError("TargetLookAt was not assigned!");
        reCalcMaxPan();
    }

    void duplicateTarget()
    {
        GameObject targetClone = new GameObject("OriginalTarget");
        targetClone.transform.parent = targetsParent;
        targetClone.transform.position = Target.position;
        originalTarget = targetClone.transform;
    }

    void calcCameraPosition()
    {
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        startDistance = Vector3.Distance(myTransform.position,
            Target.position);
        desiredPosition = myTransform.position;
        MouseXRot = myTransform.rotation.eulerAngles.y;
        MouseYRot = myTransform.rotation.eulerAngles.x;
        reset();
    }

    Vector3 targetOffset = Vector3.zero;

    void LateUpdate()
    {
        Target.position = originalTarget.position + targetOffset;
        Target.rotation = Quaternion.Euler(0, MouseXRot, 0);
        if (Target == null)
            return;
        handlePlayerInput();
        calcDesiredPosition();
        if (targetsParent != null)
            checkIfOccluded();
        updatePosition();
        targetOffset = Target.position - originalTarget.position;
    }

    void handlePlayerInput()
    {
        if (!controllingEnabled)
            return;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            MouseXRot += Input.GetAxis("Mouse X") * mouseSensitivityX;
            MouseYRot -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        }

        changeVarOnKeyPress(KeyCode.PageUp, ref MouseYRot, -YRotSpeed);
        changeVarOnKeyPress(KeyCode.PageDown, ref MouseYRot, YRotSpeed);
        changeVarOnKeyPress(KeyCode.Home, ref MouseXRot, -XRotSpeed);
        changeVarOnKeyPress(KeyCode.End, ref MouseXRot, XRotSpeed);

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
            reCalcMaxPan();
            Vector3 curOffset = Target.position - originalTarget.position;
            if (curOffset.x > curPanXLimit || curOffset.z > curPanYLimit
                || curOffset.x < -curPanXLimit || curOffset.z < -curPanYLimit)
            {
                Vector3 to = curOffset;
                to.x = Mathf.Clamp(to.x, -curPanXLimit, curPanXLimit);
                to.z = Mathf.Clamp(to.z, -curPanYLimit, curPanYLimit);
                StartCoroutine(moveTo(to + originalTarget.position));
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
            return;
        Vector3 mousePos = Input.mousePosition;
        Rect noPanRect = new Rect(5, 5, Screen.width - 10, Screen.height - 10);
        if (!noPanRect.Contains(mousePos) || arrowKeyPressed())
            panning(mousePos);
        else
        {
            curPanSpeed = panSpeed;
            if (cameraReturns && Vector3.Distance(Target.position,
                originalTarget.position) > 0.00001f)
                moveCamBack();
        }
    }

    void reCalcMaxPan()
    {
        float ratio;
        ratio = (MaxDistance - desiredDistance) / (MaxDistance - MinDistance);
        curPanXLimit = maxPanX * ratio;
        curPanYLimit = maxPanY * ratio;
    }

    bool arrowKeyPressed()
    {
        return (Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow));
    }

    float smothingT = 0;
    float rate = 0;

    void moveCamBack()
    {
        if (smothingT >= 1 || smothingT == 0)
        {
            smothingT = 0;
            rate = 1 / Vector3.Distance(Target.position,
                originalTarget.position) * returnInitSpeed;
        }
        smothingT += Time.deltaTime * rate;
        Target.position = Vector3.Lerp(Target.position,
            originalTarget.position, Mathf.SmoothStep(0, 1, smothingT));
    }

    IEnumerator moveTo(Vector3 to)
    {
        float t = 0;
        Vector3 from = Target.position;
        float rate = 1 / Vector3.Distance(from, to) * returnInitSpeed;
        while (t < 1)
        {
            t += Time.deltaTime * rate;
            Target.position = Vector3.Lerp(from, to, 
                Mathf.SmoothStep(0, 1, t));
            targetOffset = Target.position - originalTarget.position;
            yield return null;
        }
    }

    float curPanSpeed;

    void panning(Vector3 mousePos)
    {
        if (mousePos.x >= Screen.width - 5 || Input.GetKey(KeyCode.RightArrow))
            Target.Translate(Target.right * Time.deltaTime * curPanSpeed, Space.World);
        if (mousePos.x <= 5 || Input.GetKey(KeyCode.LeftArrow))
            Target.Translate(-Target.right * Time.deltaTime * curPanSpeed, Space.World);
        if (mousePos.y <= 5 || Input.GetKey(KeyCode.DownArrow))
            Target.Translate(-Target.forward * Time.deltaTime * curPanSpeed, Space.World);
        if (mousePos.y >= Screen.height - 5 || Input.GetKey(KeyCode.UpArrow))
            Target.Translate(Target.forward * Time.deltaTime * curPanSpeed, Space.World);

        Vector3 toClamp = Target.position - originalTarget.position;
        float clampedX = Mathf.Clamp(toClamp.x, -curPanXLimit, curPanXLimit);
        float clampedZ = Mathf.Clamp(toClamp.z, -curPanYLimit, curPanYLimit);
        toClamp = new Vector3(clampedX, toClamp.y, clampedZ);
        Target.position = originalTarget.position + toClamp;
        smothingT = 0;
        if (curPanSpeed < panSpeed * 3)
            curPanSpeed += Time.deltaTime * panSpeed / 5;
    }

    void changeVarOnKeyPress(KeyCode key, ref float varToChange, float amount)
    {
        if (Input.GetKey(key))
            varToChange += amount * Time.deltaTime;
    }

    public void reset()
    {
        Distance = startDistance;
        desiredDistance = Distance;
    }

    void calcDesiredPosition()
    {
        if (targetsParent != null)
            resetTransparency();
        Distance = Mathf.SmoothDamp(Distance, desiredDistance,
                                    ref velDistance, DistanceSmooth);
        desiredPosition = calcPosition(MouseYRot, MouseXRot, Distance);
    }

    Vector3 calcPosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        return Target.position + rotation * direction;
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

    HashSet<Renderer> checkCameraPoints()
    {
        HashSet<Renderer> returnedHits = new HashSet<Renderer>();
        Vector3 targetPos = Target.position;
        ClipPlanePoints clipPlanePoints = ClipPlaneAtNear(targetPos);

        if (DEBUG)
        {
            Debug.DrawLine(myTransform.position, Target.position, Color.red);
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

        getHits(Target.position, mask, ref returnedHits);
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
        myTransform.position = desiredPosition;
        myTransform.LookAt(Target.position);
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
        if (tempCamera.GetComponent<MyCamera>() == null)
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
        Bounds bounds = targetsParent.GetComponent<Collider>().bounds;
        float height = bounds.size.x / 2f - 0.2f;
        float width = bounds.size.x / 2f - 0.3f;

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
