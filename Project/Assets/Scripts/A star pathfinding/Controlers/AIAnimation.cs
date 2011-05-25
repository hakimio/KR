using UnityEngine;
using System.Collections;

public class AIAnimation : MonoBehaviour {
	public float minimumRunSpeed = 1.0F;
	
	public void Start () {
		// Set all animations to loop
		animation.wrapMode = WrapMode.Loop;
	
		// Except our action animations, Dont loop those
		animation["jump_pose"].wrapMode = WrapMode.Once;
		
		// Put idle and run in a lower layer. They will only animate if our action animations are not playing
		animation["idle"].layer = -1;
		animation["walk"].layer = -1;
		animation["run"].layer = -1;
		
		animation.Stop();
	}
	
	public void SetSpeed (float speed) {
		if (speed > minimumRunSpeed) {
			animation.CrossFade("run");
		} else {
			animation.CrossFade("idle");
		}
	}
}