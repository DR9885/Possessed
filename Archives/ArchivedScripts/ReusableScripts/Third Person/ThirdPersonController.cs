using UnityEngine;

// Require a character controller to be attached to the same game object
[AddComponentMenu("Possessed/ThirdPersonController")]
[RequireComponent(typeof(CharacterController), typeof(ThirdPersonCamera))]
public class ThirdPersonController : MonoBehaviour
{
    public enum CharacterState {
        Idle = 0,
        Walking = 1,
        Trotting = 2,
        Running = 3,
        Jumping = 4,
        Climbing = 5, 
    }

    public float _WalkMaxAnimationSpeed = 0.75f;
    public float _TrotMaxAnimationSpeed = 1.0f;
    public float _RunMaxAnimationSpeed = 1.0f;
    public float _JumpAnimationSpeed = 1.15f;
    public float _LandAnimationSpeed = 1.0f;
    public float _ClimbAnimationSpeed = 1.0f; 

    private Animation _Animation;


    private CharacterState _CharacterState;

    // The speed when walking
    public float _WalkSpeed = 2.0f;
    // after trotAfterSeconds of walking we trot with trotSpeed
    public float _TrotSpeed = 4.0f;
    // when pressing "Fire3" button (cmd) we start running
    public float _RunSpeed = 6.0f;

    public float _InAirControlAcceleration = 3.0f;

    // How high do we jump when pressing jump and letting go immediately
    public float _JumpHeight = 0.5f;

    // The gravity for the character
    public float _Gravity = 20.0f;
    // The gravity in controlled descent mode
    public float _SpeedSmoothing = 10.0f;
    public float _RotateSpeed = 500.0f;
    public float _TrotAfterSeconds = 3.0f;

    public bool _CanJump = true;

    private const float _JumpRepeatTime = 0.05f;
    private const float _JumpTimeout = 0.15f;
    private const float _GroundedTimeout = 0.25f;

    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    private float _LockCameraTimer;

    // The current move direction in x-z
    private Vector3 _MoveDirection = Vector3.zero;
    // The current vertical speed
    private float _VerticalSpeed;
    // The current x-z move speed
    private float _MoveSpeed;

    public float MoveSpeed
    {
        set { _MoveSpeed = value; }
    }

    // The last collision flags returned from controller.Move
    private CollisionFlags _CollisionFlags; 

    // Are we jumping? (Initiated with jump button and not grounded yet)
    private bool _Jumping = false;
    private bool _JumpingReachedApex = false;

    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    private bool _MovingBack = false;
    // Is the user pressing any keys?
    private bool _IsMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    private float _WalkTimeStart = 0.0f;
    // Last time the jump button was clicked down
    private float _LastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    private float _LastJumpTime = -1.0f;

    // Is the squirrel in the climb slot?
    //private bool _IsClimbing = false; 

    private Vector3 _InAirVelocity = Vector3.zero;

    private float _LastGroundedTime = 0.0f;


    private bool isControllable = true;

    void OnDisable()
    {
        if (_Animation && _Animation[AnimationResources.Idle] != null
            && !_Animation.IsPlaying(AnimationResources.Idle))
                _Animation.CrossFade(AnimationResources.Idle);
    }

    void OnEnable ()
    {
        _Jumping = false;

        _MoveDirection = transform.TransformDirection(Vector3.forward);
    	
        _Animation = GetComponent(typeof(Animation)) as Animation;
        /*if(!_Animation)
            //debug.log("The character you would like to control doesn't have animations. Moving her might look weird.");
    	

        if(!animation[AnimationResources.Idle]) {
            _Animation = null;
            ////debug.log("No idle animation found. Turning off animations.");
        }
        if(!animation[AnimationResources.Walk]) {
            _Animation = null;
            ////debug.log("No walk animation found. Turning off animations.");
        }
        if(!animation[AnimationResources.Run]) {
            _Animation = null;
            ////debug.log("No run animation found. Turning off animations.");
        }
        if(!animation[AnimationResources.Idle] && _CanJump) {
            _Animation = null;
            ////debug.log("No jump animation found and the character has canJump enabled. Turning off animations.");
        }*/
    			
    }


    void UpdateSmoothedMovementDirection ()
    {

        Transform cameraTransform = Camera.main.transform;
        bool grounded = IsGrounded();
    	
        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");

        // Are we moving backwards or looking backwards
        if (v < -0.2)
            _MovingBack = true;
        else
            _MovingBack = false;
    	
        var wasMoving = _IsMoving;
        _IsMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
    		
        // Target direction relative to the camera
        var targetDirection = h * right + v * forward;
    	
        // Grounded controls
        if (grounded)
        {
            // Lock camera for short period when transitioning moving & standing still
            _LockCameraTimer += Time.deltaTime;
            if (_IsMoving != wasMoving)
                _LockCameraTimer = 0.0f;

            // We store speed and direction seperately,
            // so that when the character stands still we still have a valid forward direction
            // moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
                // If we are really slow, just snap to the target direction
                if (_MoveSpeed < _WalkSpeed * 0.9 && grounded)
                {
                    _MoveDirection = targetDirection.normalized;
                }
                // Otherwise smoothly turn towards it
                else
                {
                    _MoveDirection = Vector3.RotateTowards(_MoveDirection, targetDirection, _RotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
    				
                    _MoveDirection = _MoveDirection.normalized;
                }
            }
    		
            // Smooth the speed based on the current target direction
            var curSmooth = _SpeedSmoothing * Time.deltaTime;
    		
            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
    	
            _CharacterState = CharacterState.Idle;
    		
            // Pick speed modifier
            if (Input.GetKey (KeyCode.LeftShift) | Input.GetKey (KeyCode.RightShift))
            {
                targetSpeed *= _RunSpeed;
                _CharacterState = CharacterState.Running;
            }
            else if (Time.time - _TrotAfterSeconds > _WalkTimeStart)
            {
                targetSpeed *= _TrotSpeed;
                _CharacterState = CharacterState.Trotting;
            }
            else
            {
                targetSpeed *= _WalkSpeed;
                _CharacterState = CharacterState.Walking;
            }
    		
            _MoveSpeed = Mathf.Lerp(_MoveSpeed, targetSpeed, curSmooth);
    		
            // Reset walk time start when we slow down
            if (_MoveSpeed < _WalkSpeed * 0.3)
                _WalkTimeStart = Time.time;
        }
        // In air controls
        else
        {
            // Lock camera while in air
            if (_Jumping)
                _LockCameraTimer = 0.0f;

            if (_IsMoving)
                _InAirVelocity += targetDirection.normalized * Time.deltaTime * _InAirControlAcceleration;
        }
    	

    		
    }


    void ApplyJumping ()
    {
        // Prevent jumping too fast after each other
        if (_LastJumpTime + _JumpRepeatTime > Time.time)
            return;

        if (IsGrounded()) {
            // Jump
            // - Only when pressing the button down
            // - With a timeout so you can press the button slightly before landing		
            if (_CanJump && Time.time < _LastJumpButtonTime + _JumpTimeout) {
                _VerticalSpeed = CalculateJumpVerticalSpeed (_JumpHeight);
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    void ApplyGravity ()
    {
        if (isControllable)	// don't move player at all if not controllable.
        {
            // Apply gravity
            var jumpButton = Input.GetButton("Jump");
    		
    		
            // When we reach the apex of the jump we send out a message
            if (_Jumping && !_JumpingReachedApex && _VerticalSpeed <= 0.0)
            {
                _JumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }
    	
            if (IsGrounded ())
                _VerticalSpeed = 0.0f;
            else
                _VerticalSpeed -= _Gravity * Time.deltaTime;
        }
    }

    float CalculateJumpVerticalSpeed (float targetJumpHeight)
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * targetJumpHeight * _Gravity);
    }
    void Climb()
    {
        rigidbody.transform.position += Vector3.up * Time.deltaTime;
    }
    void DidJump ()
    {
        _Jumping = true;
        _JumpingReachedApex = false;
        _LastJumpTime = Time.time;
        _LastJumpButtonTime = -10;
    	
        _CharacterState = CharacterState.Jumping;
    }

    void Update() {
    	
        if (!isControllable)
        {
            // kill all inputs if not controllable.
            Input.ResetInputAxes();
        }

        if (Input.GetButtonDown ("Jump"))
        {
            _LastJumpButtonTime = Time.time;
        }

        UpdateSmoothedMovementDirection();
    	
        // Apply gravity
        // - extra power jump modifies gravity
        // - controlledDescent mode modifies gravity
        ApplyGravity ();

        // Apply jumping logic
        ApplyJumping ();
    	
        // Calculate actual motion
        Vector3 movement = _MoveDirection * _MoveSpeed + new Vector3(0, _VerticalSpeed, 0) + _InAirVelocity;
        movement *= Time.deltaTime;
    	
        // Move the controller
        CharacterController controller = GetComponent(typeof(CharacterController)) as CharacterController;
        _CollisionFlags = controller.Move(movement);
    	
        // ANIMATION sector
        if(_Animation) {
            if(_CharacterState == CharacterState.Jumping) 
            {
                if (_Animation[AnimationResources.Jump] != null)
                {
                    if (!_JumpingReachedApex)
                    {
                        _Animation[AnimationResources.Jump].speed = _JumpAnimationSpeed;
                        _Animation[AnimationResources.Jump].wrapMode = WrapMode.ClampForever;
                        _Animation.CrossFade(AnimationResources.Jump);
                    }
                    else
                    {
                        _Animation[AnimationResources.Jump].speed = -_LandAnimationSpeed;
                        _Animation[AnimationResources.Jump].wrapMode = WrapMode.ClampForever;
                        _Animation.CrossFade(AnimationResources.Jump);
                    }
                }
            }
            else if (_CharacterState == CharacterState.Climbing)
            {
                if (_Animation[AnimationResources.Climb] != null)
                {
                    _Animation[AnimationResources.Climb].speed = _ClimbAnimationSpeed;
                    _Animation[AnimationResources.Climb].wrapMode = WrapMode.ClampForever;
                    _Animation.CrossFade(AnimationResources.Climb); 
                }
            }
            else
            {
                if (controller.velocity.sqrMagnitude < 0.1)
                {
                    _Animation.CrossFade(AnimationResources.Idle);
                }
                else
                {
                    if (_CharacterState == CharacterState.Running && _Animation[AnimationResources.Run] != null)
                    {
                        _Animation[AnimationResources.Run].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, _RunMaxAnimationSpeed);
                        _Animation.CrossFade(AnimationResources.Run);
                    }
                    else if (_CharacterState == CharacterState.Trotting && _Animation[AnimationResources.Walk] != null)
                    {
                        _Animation[AnimationResources.Walk].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, _TrotMaxAnimationSpeed);
                        _Animation.CrossFade(AnimationResources.Walk);
                    }
                    else if (_CharacterState == CharacterState.Walking && _Animation[AnimationResources.Walk] != null)
                    {
                        _Animation[AnimationResources.Walk].speed = Mathf.Clamp(controller.velocity.magnitude, 0.0f, _WalkMaxAnimationSpeed);
                        _Animation.CrossFade(AnimationResources.Walk);
                    }

                }
            }
        }
        // ANIMATION sector
    	
        // Set rotation to the move direction
        if (IsGrounded())
        {
    		
            transform.rotation = Quaternion.LookRotation(_MoveDirection);
    			
        }	
        else
        {
            var xzMove = movement;
            xzMove.y = 0;
            if (xzMove.sqrMagnitude > 0.001)
            {
                transform.rotation = Quaternion.LookRotation(xzMove);
            }
        }	
    	
        // We are in jump mode but just became grounded
        if (IsGrounded())
        {
            _LastGroundedTime = Time.time;
            _InAirVelocity = Vector3.zero;
            if (_Jumping)
            {
                _Jumping = false;
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnControllerColliderHit (ControllerColliderHit hit)
    {
    //	Debug.DrawRay(hit.point, hit.normal);
        if (hit.moveDirection.y > 0.01) 
            return;
    }

    public float GetSpeed()
    {
        return _MoveSpeed;
    }

    public bool IsJumping()
    {
        return _Jumping;
    }

    public bool IsGrounded()
    {
        return (_CollisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    public Vector3 GetDirection()
    {
        return _MoveDirection;
    }

    public bool IsMovingBackwards()
    {
        return _MovingBack;
    }

    public float GetLockCameraTimer () 
    {
        return _LockCameraTimer;
    }

    public bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
    }

    public bool HasJumpReachedApex()
    {
        return _JumpingReachedApex;
    }

    public bool IsGroundedWithTimeout()
    {
        return _LastGroundedTime + _GroundedTimeout > Time.time;
    }

    void Reset ()
    {
        gameObject.tag = "Player";
    }

}