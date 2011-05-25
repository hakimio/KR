var speed = 10;
var rotspeed = 40;
var maxdist = 400;
var mask : LayerMask;
private var horiz : float = 0;
private var vert : float = 0;
private var zoom : float = 0;
private var roty : float = 0;
private var rotx : float = 0;
private var hit : RaycastHit;
private var prehit : RaycastHit;
function LateUpdate () {
	vert = Input.GetAxis ("Vertical");
	horiz = Input.GetAxis ("Horizontal");
	zoom = Input.GetAxis ("Zoom");
	rotx = Input.GetAxis ("Rotatex");
	roty = Input.GetAxis ("Rotatey");
	if (horiz != 0 || vert != 0 || zoom != 0) {
		dir = transform.TransformDirection(Vector3.up);
		dir.y = 0;
		dir = dir.normalized;
		
		dir2 = transform.TransformDirection(Vector3.right);
		dir2.y = 0;
		dir2 = dir2.normalized;
		
		dir3 = transform.TransformDirection(Vector3.forward);
		//dir3.y = 0;
		dir3 = dir3.normalized;
		/*var dir = transform.TransformDirection (Vector3.right);
		var dir2 = transform.TransformDirection (Vector3.up);
		dir2.y = 0;
		pos += dir * horiz;
		pos += dir2 * vert;
		transform.position = pos;*/
		rigidbody.AddForce (dir2 * Time.deltaTime * horiz * speed);
		rigidbody.AddForce (dir * Time.deltaTime * vert * speed);
		if (transform.position.y <= maxdist || zoom > 0) {
			rigidbody.AddForce (dir3 * Time.deltaTime * zoom * speed);
		}
		//transform.position = Vector3.Lerp (transform.position, pos, Time.deltaTime * smooth);
	}
	if (rotx != 0 || roty != 0) {
		Physics.Raycast (transform.position,transform.TransformDirection (Vector3.forward),hit,1500, mask);
		if (hit.point != Vector3.zero) {
			transform.RotateAround (hit.point, Vector3.up, rotspeed * Time.deltaTime * rotx);
			transform.RotateAround (hit.point, transform.TransformDirection (Vector3.right), rotspeed * Time.deltaTime * roty);
			prehit = hit;
		} else {
			transform.RotateAround (prehit.point, Vector3.up, rotspeed * Time.deltaTime * rotx);
			transform.RotateAround (prehit.point, transform.TransformDirection (Vector3.right), rotspeed * Time.deltaTime * roty);
		}
	}
}