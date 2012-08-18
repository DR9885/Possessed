public var targetTransform : Transform;
public var faceForward : boolean = false;

private var thisTransform : Transform;

function Start()
{
	thisTransform = transform;
}

function Update()
{
	thisTransform.position = targetTransform.position;
	if(faceForward) thisTransform.forward = targetTransform.forward;
}