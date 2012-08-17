using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class AbstractBeing : AbstractEntity, IBeing
{
    #region Properties

    #region Components

    private MasterRemote _MasterRemote;
    public MasterRemote MasterRemote
    {
        get
        {
            if (_MasterRemote == null)
                _MasterRemote = GameObject.RequireComponentAdd<MasterRemote>();
            return _MasterRemote;
        }
    }

    private CharacterController _CharacterController;
    public CharacterController CharacterController
    {
        get
        {
            if (_CharacterController == null)
                _CharacterController = GameObject.RequireComponentAdd<CharacterController>();
            return _CharacterController;
        }
    }

    private ThirdPersonController _ThirdPersonController;
    public ThirdPersonController ThirdPersonController
    {
        get
        {
            if (_ThirdPersonController == null)
                _ThirdPersonController = GameObject.RequireComponentAdd<ThirdPersonController>();
            return _ThirdPersonController;
        }
    }

    private ThirdPersonCamera _ThirdPersonCamera;
    public ThirdPersonCamera ThirdPersonCamera
    {
        get
        {
            if (_ThirdPersonCamera == null)
                _ThirdPersonCamera = GameObject.RequireComponentAdd<ThirdPersonCamera>();
            return _ThirdPersonCamera;
        }
    }

    private Animation _Animation;
    public Animation Animation
    {
        get
        {
            if (_Animation == null)
                _Animation = animation;
            if (_Animation == null)
                _Animation = GameObject.AddComponent<Animation>();
            return _Animation;
        }
    }


    #endregion

    #region Transforms

    private Transform _Root;
    public Transform Root
    {
        get 
        {
            // OLD
            if (_Root == null)
                _Root = Transform.FindSubChild("JNT_Root");
            
            // New
            if (_Root == null)
                _Root = Transform.FindSubChild("Bip001");

            if (_Root == null)
                _Root = Transform;
            return _Root;
        }
    }

    private Transform _Mouth = null;
    public Transform Mouth
    {
        get
        {
            if (_Mouth == null)
                _Mouth = Transform.FindSubChild("JNT_Mouth");
            if (_Mouth == null)
                _Mouth = Root;
            return _Mouth;
        }
    }

    private Transform _LeftHand = null;
    public Transform LeftHand
    {
        get
        {
            //NEW
            if (_LeftHand == null)
                _LeftHand = Transform.FindSubChild("Bip001 L Hand");
            
			//OLD
            if (_LeftHand == null)
                _LeftHand = Transform.FindSubChild("JNT_L_hand");
            if (_LeftFoot == null)
                _LeftFoot = Transform.FindSubChild("CNTL_L_BackFoot");
            if (_LeftHand == null)
                _LeftHand = Tail;
            if (_LeftHand == null)
                _LeftHand = Root; 
            return _LeftHand;
        }
    }

    private Transform _RightHand;
    public Transform RightHand
    {
        get
        {
            //NEW
            if (_RightHand == null)
                _RightHand = Transform.FindSubChild("Bip001 R Hand");
			
			//OLD
            if (_RightHand == null)
                _RightHand = Transform.FindSubChild("JNT_R_hand");
            if (_RightHand == null)
                _RightHand = Transform.FindSubChild("CNTL_R_FrontFoot");
            if (_RightFoot == null)
                _RightFoot = Tail;
            if (_RightFoot == null)
                _RightFoot = Root; 
            return _RightHand;
        }
    }

    private Transform _LeftFoot;
    public Transform LeftFoot
    {
        get
        {
            if (_LeftFoot == null)
                _LeftFoot = Transform.FindSubChild("Bip001 L Foot");
            if (_LeftFoot == null)
                _LeftFoot = Transform.FindSubChild("JNT_L_foot");
            if (_LeftFoot == null)
                _LeftFoot = Transform.FindSubChild("CNTL_L_BackFoot");

            if (_LeftFoot == null)
                _LeftFoot = Tail;
            if (_LeftFoot == null)
                _LeftFoot = Root; 
            return _LeftFoot;
        }
    }
	
    private Transform _RightFoot;
    public Transform RightFoot
    {
        get
        {
            if (_RightFoot == null)
                _RightFoot = Transform.FindSubChild("JNT_R_foot");
            if (_RightFoot == null)
                _RightFoot = Transform.FindSubChild("Bip001 R Foot");
            if (_RightFoot == null)
                _RightFoot = Transform.FindSubChild("CNTL_R_BackFoot");
            if (_RightFoot == null)
                _RightFoot = Tail;
            if (_RightFoot == null)
                _RightFoot = Root;
            return _RightFoot;
        }
    }
	
	private Transform _Tail;
    public Transform Tail
    {
        get
        {
            if (_Tail == null)
                _Tail = Transform.FindSubChild("Bip001 Tail");
            if (_Tail == null)
                _Tail = Transform.FindSubChild("JNT_Tail_0");
            if (_Tail == null)
                _Tail = Root;
            return _Tail;
        }
    }

    #endregion

    #endregion

    #region Unity Methods

    new public void Reset()
    {
        base.Reset();
        Setup();
        ResourceManager.AnimationResources.InitAnimations(this, true);
    }

    new public void Awake()
    {
        base.Awake();
        Setup();


        if (this is MainPlayerGhost)
            ResourceManager.AnimationResources.InitAnimations(this, false);
    }
    
    public void Start()
    {
        if (!(this is MainPlayerGhost))
            ResourceManager.AnimationResources.InitAnimations(this, false);

        if (Animation[AnimationResources.Idle] != null)
            Animation.CrossFade(AnimationResources.Idle);
    }

    private void Setup()
    {
        DisablePlayerControl();
        ThirdPersonCamera.angularMaxSpeed = 1000.01f;
        MasterRemote.enabled = false;
    }

    void OnBecameVisible()
    {
        ////debug.log(name);
    }

    #endregion

    #region Public Methods

    public bool IsPlayerControlling
    {
        get { return ThirdPersonCamera.enabled && ThirdPersonController.enabled && MasterRemote.enabled; }
    }

    public void EnablePlayerControl()
    {
        ThirdPersonController.enabled = true;
        ThirdPersonCamera.enabled = true;
        MasterRemote.enabled = true;
    }

    public void DisablePlayerControl()
    {
        ThirdPersonController.enabled = false;
        ThirdPersonCamera.enabled = false;

        if (GetComponent<MainPlayerGhost>() == null)
            MasterRemote.enabled = false;
    }

    #endregion

    #region Animation Events

    private void IdleEvent(AnimationEvent animationEvent)
    {
        ////debug.log("IDLE EVENT-> " + name + ": " + animationEvent.stringParameter);
        AbstractBeing being = animationEvent.objectReferenceParameter as AbstractBeing;
        if (being != null) being.Animation.CrossFade(animationEvent.stringParameter);
    }

    #endregion
}