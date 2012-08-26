var animationTarget : Animation;
var maxForwardSpeed : float = 6;

private var character : CharacterController; 
private var thisTransform : Transform; 

function Start()
{
	character = GetComponent( CharacterController ); 
	thisTransform = transform; 
	
	animationTarget.wrapMode = WrapMode.Loop;
	/*animationTarget["jump"].wrapMode = WrapMode.ClampForever;
	animationTarget["jump-land"].wrapMode = WrapMode.ClampForever; 
	animationTarget["run-land"].wrapMode = WrapMode.ClampForever;
	animationTarget["LOSE"].wrapMode = WrapMode.ClampForever;*/
}

function OnEndGame()
{
	// Don't update animations when the game has ended
	this.enabled = false;
}

function Update() 
{ 
	var characterVelocity = character.velocity; 
	var horizontalVelocity : Vector3 = characterVelocity; 

	horizontalVelocity.y = 0; 

	var speed = horizontalVelocity.magnitude; 

	if ( speed > 0 )
	{
		var forwardMotion = Vector3.Dot( thisTransform.forward, horizontalVelocity );
		var sidewaysMotion = Vector3.Dot( thisTransform.right, horizontalVelocity );
		var t = 0.0;
		
		if ( forwardMotion > 0 )
		{
			if ( forwardMotion < 1.51 )
			{
				// Adjust the animation speed to match with how fast the
				// character is moving forward
				t = Mathf.Clamp( Mathf.Abs( speed / maxForwardSpeed ), 0, maxForwardSpeed );
				animationTarget[ "walk" ].speed = Mathf.Lerp( 0.25, 1, t );
										
				if ( animationTarget.IsPlaying( "idle" ) )
					// Don't blend coming from a land, just play
					animationTarget.Play( "walk" ); 
				else
					animationTarget.CrossFade( "walk" );
			}
			else
			{
				// Adjust the animation speed to match with how fast the
				// character is moving forward
				t = Mathf.Clamp( Mathf.Abs( speed / maxForwardSpeed ), 0, maxForwardSpeed );
				animationTarget[ "run" ].speed = Mathf.Lerp( 0.25, 1, t );
										
				if ( animationTarget.IsPlaying( "idle" ) )
					// Don't blend coming from a land, just play
					animationTarget.Play( "run" ); 
				else
					animationTarget.CrossFade( "run" );
			}
		}
	}
	else
	// Play the idle animation by default
	animationTarget.CrossFade( "idle" );
}		
	