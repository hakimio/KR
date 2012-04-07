using UnityEngine;
using System.Collections;

public class AIAnimation : MonoBehaviour
{
    public float minimumRunSpeed = 1.0F;
    public string runAnimName = "run";
    NavMeshAgent navAgent;

    void Start()
    {
        // Set all animations to loop
        animation.wrapMode = WrapMode.Loop;

        // Put idle and run in a lower layer. 
        //They will only animate if our action animations are not playing
        animation["idle"].layer = -1;
        animation[runAnimName].layer = -1;

        animation.Stop();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float speed = navAgent.velocity.magnitude;
        if (speed > minimumRunSpeed)
            animation.CrossFade(runAnimName);
        else
            animation.CrossFade("idle");
    }
}