using UnityEngine;
using System.Collections;

public class TorusScaler : MonoBehaviour
{
    Vector3 startScale = new Vector3(0.85f, 0.85f, 0.01f);
    Transform myTransform;

    void Start()
    {
        myTransform = transform;
        Messenger<bool>.AddListener("targetPosChanged", targetPosChanged);
    }

    void targetPosChanged(bool b)
    {
        StopCoroutine("scale");
        StartCoroutine("scale");
    }

    IEnumerator scale()
    {
        float t = 0;
        float speed = 0.7f;
        
        while (t < 1.0)
        {
            t += Time.deltaTime * speed;
            myTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }
    }
}
