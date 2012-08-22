public var followTransform : Transform;
public var offset : Vector3 = new Vector3(0f, 0.75f, -2.55f);
public var moveSpeed : float = 3;
public var turnSpeed : float = 3;

private var goalPos : Vector3;

function Start()
{
	if(!followTransform)followTransform = GameObject.FindGameObjectWithTag("PC_Player").transform;
	if(!followTransform) this.enabled = false;
}

function Update()
{
	  goalPos = followTransform.position + followTransform.TransformDirection(offset);
      transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * moveSpeed);
      transform.rotation = Quaternion.Lerp(transform.rotation, followTransform.rotation, Time.deltaTime * moveSpeed);
}