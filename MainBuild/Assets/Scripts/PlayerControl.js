@script RequireComponent( CharacterController ) 

var moveJoystick : Joystick; 
var rotateJoystick : Joystick;

var cameraPivot : Transform;						// The transform used for camera rotation
var cameraTransform : Transform;					// The actual transform of the camera
var speed : float = 6;
var rotationSpeed : Vector2 = Vector2( 50, 25 );	// Camera rotation speed for each axis

private var thisTransform : Transform; 
private var character : CharacterController;
private var animationController : AnimationController;

function Start()
{ 
	thisTransform = GetComponent( Transform ); 
	character = GetComponent( CharacterController );
	
	animationController = GetComponent( AnimationController );
	animationController.maxForwardSpeed = speed;
	
	/*// Move the character to the correct start position in the level, if one exists
	var spawn = GameObject.Find( "PlayerSpawn" );
	if ( spawn )
		thisTransform.position = spawn.transform.position;*/
}

function FaceMovementDirection()
{ 
	var horizontalVelocity : Vector3 = character.velocity; 
	horizontalVelocity.y = 0; 
	if ( horizontalVelocity.magnitude > 0.1 )
		thisTransform.forward = horizontalVelocity.normalized;
}

/*function OnEndGame()
{
	// Disable joystick when the game ends	
	moveJoystick.Disable();
	rotateJoystick.Disable();
	
	// Don't allow any more control changes when the game ends
	this.enabled = false;
}*/

function Update()
{	
	var movement = cameraTransform.TransformDirection( Vector3( moveJoystick.position.x, 0, moveJoystick.position.y ) );
	// We only want the camera-space horizontal direction
	movement.y = 0;
	movement.Normalize(); // Adjust magnitude after ignoring vertical movement
	
	// Let's use the largest component of the joystick position for the speed.
	var absJoyPos = Vector2( Mathf.Abs( moveJoystick.position.x ), Mathf.Abs( moveJoystick.position.y ) );
	movement *= speed * ( ( absJoyPos.x > absJoyPos.y ) ? absJoyPos.x : absJoyPos.y );
	
	movement += Physics.gravity;
	movement *= Time.deltaTime;
	character.Move(movement);
	
	// Face the character to match with where she is moving
	FaceMovementDirection();
	
	// Scale joystick input with rotation speed
	var camRotation = rotateJoystick.position;
	camRotation.x *= rotationSpeed.x;
	camRotation.y *= rotationSpeed.y;
	camRotation *= Time.deltaTime;
	
	// Rotate around the character horizontally in world, but use local space
	// for vertical rotation
	cameraPivot.Rotate( 0, camRotation.x, 0, Space.World );
	cameraPivot.Rotate( camRotation.y, 0, 0 );
}