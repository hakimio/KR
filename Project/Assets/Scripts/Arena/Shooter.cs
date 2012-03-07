using UnityEngine;
using System.Collections;

public class Shooter: MonoBehaviour
{
    public GameObject Bullet;
    Transform shotgunEnd;
    GameObject target;
    const float shotgunOffset = 0.096191f;
    float rotationSpeed = 4;
    Quaternion rotation, centerRotation, maxRotation, minRotation;
    bool isRotating = false;
    bool shoot = false;
    public static Shooter instance = null;

    Transform gunTr;

    void Start()
    {
        shotgunEnd = ((GameObject)GameObject.Find("shotgunEnd")).transform;
        gunTr = ((GameObject)GameObject.Find("gun")).transform;
        instance = this;
    }

    public bool shootAt(GameObject target, bool miss)
    {
        this.target = target;
        calcRotations();
        if (isTargetBlocked())
            return false;
        rotation = getRandomRotation(miss);
        isRotating = true;
        return true;
    }

    void calcRotations()
    {
        CharacterController CC = target.GetComponent<CharacterController>();
        Vector3 targetPos = CC.collider.bounds.center;
        targetPos.y = transform.position.y;

        Vector3 toTarget = targetPos - transform.position;
        float distToTarget = toTarget.magnitude;
        float targetRadius = calcTargetRadius();

        float offsetAngle;
        offsetAngle = -Vector3.Angle(transform.forward, shotgunEnd.forward);
        float offsetAngle1 = offsetAngle;
        float offsetAngle2 = offsetAngle;

        offsetAngle += Mathf.Atan(shotgunOffset / distToTarget)
            * Mathf.Rad2Deg;
        offsetAngle1 += Mathf.Atan((shotgunOffset - targetRadius) / distToTarget)
            * Mathf.Rad2Deg;
        offsetAngle2 += Mathf.Atan((shotgunOffset + targetRadius) / distToTarget)
            * Mathf.Rad2Deg;
        float maxAngle, minAngle;
        if (offsetAngle1 > offsetAngle2)
        {
            maxAngle = offsetAngle1;
            minAngle = offsetAngle2;
        }
        else
        {
            maxAngle = offsetAngle2;
            minAngle = offsetAngle1;
        }
        centerRotation = Quaternion.LookRotation(toTarget)
            * Quaternion.AngleAxis(-offsetAngle, Vector3.up);
        maxRotation = Quaternion.LookRotation(toTarget)
            * Quaternion.AngleAxis(-maxAngle, Vector3.up);
        minRotation = Quaternion.LookRotation(toTarget)
            * Quaternion.AngleAxis(-minAngle, Vector3.up);
    }

    bool isTargetBlocked()
    {
        CharacterController CC = target.GetComponent<CharacterController>();
        Vector3 targetPos = CC.collider.bounds.center;
        targetPos.y = transform.position.y;

        Vector3 minRotPoint, maxRotPoint, centerRotPoint, targetLeft, 
            targetRight;

        calcRaycastPoints(out minRotPoint, out centerRotPoint, out maxRotPoint,
            out targetLeft, out targetRight);

        Vector3 toTarget = targetPos - centerRotPoint;
        RaycastHit hitInfo;
        float distance = Vector3.Distance(centerRotPoint, targetPos);

        //Debug.DrawRay(centerRotPoint, toTarget.normalized * distance, Color.blue, 30);
        Physics.Raycast(centerRotPoint, toTarget, out hitInfo, distance);
        if (hitInfo.collider.gameObject != target)
            return true;

        toTarget = targetRight - maxRotPoint;
        distance = Vector3.Distance(maxRotPoint, targetRight);

        //Debug.DrawRay(maxRotPoint, toTarget.normalized * distance, Color.red, 30);
        Physics.Raycast(maxRotPoint, toTarget, out hitInfo, distance);
        if (hitInfo.collider.gameObject != target)
            return true;

        toTarget = targetLeft - minRotPoint;
        distance = Vector3.Distance(minRotPoint, targetLeft);

        //Debug.DrawRay(minRotPoint, toTarget.normalized * distance, Color.black, 30);
        Physics.Raycast(minRotPoint, toTarget, out hitInfo, distance);
        if (hitInfo.collider.gameObject != target)
            return true;

        return false;
    }

    Quaternion getRandomRotation(bool miss)
    {
        float randAngle, maxAngle;

        maxAngle = Quaternion.Angle(minRotation, maxRotation);

        randAngle = Random.Range(0.01f, maxAngle);
        if (miss)
        {
            int missRightOrLeft = Random.Range(0, 2);
            Debug.Log("Miss: " + missRightOrLeft);
            if (missRightOrLeft == 0)
                return minRotation * Quaternion.AngleAxis(randAngle,
                    Vector3.up);
            else
                return maxRotation * Quaternion.AngleAxis(-randAngle, 
                    Vector3.up);
        }

        return minRotation * Quaternion.AngleAxis(-randAngle, Vector3.up);
    }
    
    void calcRaycastPoints(out Vector3 minRotPoint, 
        out Vector3 centerRotPoint, out Vector3 maxRotPoint, 
        out Vector3 targetLeft, out Vector3 targetRight)
    {
        CharacterController CC = target.GetComponent<CharacterController>();
        Vector3 targetPos = CC.collider.bounds.center;
        targetPos.y = transform.position.y;

        Vector3 charPos = transform.position;
        maxRotPoint = shotgunEnd.localPosition;
        maxRotPoint.y = 0;
        minRotPoint = maxRotPoint;
        centerRotPoint = maxRotPoint;

        centerRotPoint = centerRotation * centerRotPoint;
        centerRotPoint *= transform.localScale.x;
        centerRotPoint += charPos;

        maxRotPoint = maxRotation * maxRotPoint;
        maxRotPoint *= transform.localScale.x;
        maxRotPoint += charPos;

        float targetRadius = calcTargetRadius();
        Vector3 toMe = transform.position - targetPos;
        Quaternion rotToMe = Quaternion.LookRotation(toMe);
        targetRight = rotToMe * new Vector3(-targetRadius, 0, 0);
        targetRight = targetPos - targetRight;
        targetRight.y = shotgunEnd.transform.position.y;

        minRotPoint = minRotation * minRotPoint;
        minRotPoint *= transform.localScale.x;
        minRotPoint += charPos;

        targetLeft = rotToMe * new Vector3(-targetRadius, 0, 0);
        targetLeft = targetPos + targetLeft;
        targetLeft.y = shotgunEnd.transform.position.y;
    }

    void Update()
    {
        //Debug.DrawRay(shotgunEnd.position, shotgunEnd.forward * 100, Color.red);

        if (shoot && Vector3.Angle(transform.forward, gunTr.forward) > 88.3f)
        {
            GameObject bullet = (GameObject)Instantiate(Bullet);
            Vector3 bulletPos = shotgunEnd.position;
            Transform bulletModel = bullet.GetComponentInChildren<Transform>();
            bulletPos += shotgunEnd.forward * bulletModel.collider.bounds.size.y;
            bullet.transform.position = bulletPos;
            bullet.transform.rotation = shotgunEnd.rotation;
            bullet.GetComponent<BulletMovement>().move = true;
            shoot = false;
        }

        if (!isRotating)
            return;

        if (Quaternion.Angle(transform.rotation, rotation) < 1f)
        {
            transform.rotation = rotation;
            isRotating = false;
            animation["shoot"].wrapMode = WrapMode.Once;
            animation.CrossFade("shoot");
            animation.CrossFadeQueued("idle");
            shoot = true;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,
            rotation, rotationSpeed * Time.deltaTime);
    }

    float calcTargetRadius()
    {
        CharacterController CC = target.GetComponent<CharacterController>();
        Vector3 targetPos = CC.collider.bounds.center;
        float bulletPosY = shotgunEnd.position.y;
        float offsetY = bulletPosY - targetPos.y;
        float CCRadius = CC.collider.bounds.extents.x;

        return Mathf.Sqrt(Mathf.Pow(CCRadius, 2) - Mathf.Pow(offsetY, 2));
    }
}
