using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("Possessed/Remotes/PossessionRemote")]
public class PossessionRemote : AbstractRemote
{
    #region Properties

    private GhostScript _Ghost;
    public GhostScript Ghost
    {
        get
        {
            if (_Ghost == null)
                _Ghost = GetComponent<GhostScript>();
            return _Ghost;
        }
    }

    private Speaker _Speaker;
    private Speaker Speaker
    {
        get
        {
            if (_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }

    private ThirdPersonController _ThirdPersonController;
    public ThirdPersonController ThirdPersonController
    {
        get
        {
            if (_ThirdPersonController == null)
                _ThirdPersonController = GetComponent<ThirdPersonController>();
            return _ThirdPersonController;
        }
    }

    private ThirdPersonCamera _ThirdPersonCamera;
    public ThirdPersonCamera ThirdPersonCamera
    {
        get
        {
            if (_ThirdPersonCamera == null)
                _ThirdPersonCamera = GetComponent<ThirdPersonCamera>();
            return _ThirdPersonCamera;
        }
    }

    #endregion

    #region Unity Methods

    new public void Reset()
    {
        base.Reset();

        // Set Input
        _ContextAction = ContextActions.Possession;
    }

    new public void Awake()
    {
        base.Awake();

        // Set Input
        _ContextAction = ContextActions.Possession;

        // Set Labels
        _StartLabel = "Possess";
        _EndLabel = "UnPossess";

        /// Set Events
        TargetCheck += FindClosestHappyLiving;
        PowerOn += PossessLiving;
        PowerOff += UnpossessLiving;
    }

    public void Start()
    {
        AddCrossFadeTargetEvent(1.5f,   AnimationResources.PossBegin, AnimationResources.PossBegin);
        AddCrossFadeTargetEvent(0.3f, AnimationResources.PossInput1, AnimationResources.PossInput1);
        AddCrossFadeTargetEvent(0.366f, AnimationResources.PossInput2, AnimationResources.PossInput2);
        AddCrossFadeTargetEvent(0.466f, AnimationResources.PossInput3, AnimationResources.PossInput3);
        AddCrossFadeTargetEvent(0.2f, AnimationResources.PossInput4, AnimationResources.PossInput4);
       
        AddCrossFadeTargetEvent(0, AnimationResources.PossInput1B, AnimationResources.PossInput1);
        AddCrossFadeTargetEvent(0, AnimationResources.PossInput2B, AnimationResources.PossInput2);
        AddCrossFadeTargetEvent(0, AnimationResources.PossInput3B, AnimationResources.PossInput3);
        AddCrossFadeTargetEvent(0, AnimationResources.PossInput4B, AnimationResources.PossInput4);

        Ghost.Animation[AnimationResources.PossComplete].clip.AddEvent(new AnimationEvent { time = .8f, functionName = "PossessionComplete" });
        Ghost.Animation[AnimationResources.PossFail].clip.AddEvent(new AnimationEvent { time = 1.26f, functionName = "PossessionFailed" });
        Ghost.Animation[AnimationResources.PossExit].clip.AddEvent(new AnimationEvent { time = 1.26f, functionName = "UnPossess" });
        Ghost.Animation[AnimationResources.PossWrongInput].clip.AddEvent(new AnimationEvent { time = 1.26f, functionName = "WrongInput" });
    }

    #endregion

    #region Event Methods

    private bool FindClosestHappyLiving(AbstractEntity argEntity)
    {
        if (Ghost != null)
            if (Ghost.Energy <= 0)
            {
                EnergyGUI.Pulse(0, 10); 
                return false;
            }
        return argEntity is HumanScript ? (argEntity as HumanScript).Mood == Mood.Sad : argEntity is AbstractAnimal;
    }

    private IEnumerator PossessLiving(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Activate(Speaker));
      
        /// Disable Control
//        Ghost.DisablePlayerControl();
        Ghost.ThirdPersonController.enabled = false;


        if (argEntity is HumanScript)
            (argEntity as HumanScript).HideMood();

        /// Animate Rotation
        StartCoroutine(argEntity.RotateTowards(Ghost.Transform.position - argEntity.Transform.position, 2));
        StartCoroutine(Ghost.RotateTowards(argEntity.Transform.position - Ghost.Transform.position, 2));

        //TODO: REMOVE, SHORTCUT
        if (Input.GetKey(KeyCode.Tab))
        {
            // Animate Pull
            yield return new WaitForSeconds(0.3f); // Wait for Rotation Complete
            Ghost.animation.CrossFade(AnimationResources.PossWrongInput);

            //TODO: DROP ROPES HERE


            // Rotate Target
            yield return new WaitForSeconds(0.3f);
            argEntity.animation.CrossFade(AnimationResources.PossComplete);
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(Target.RotateTowards(Ghost.Transform.position - Target.Transform.position, 2));

            // Move Back
//            yield return StartCoroutine(Ghost.MoveTo(Ghost.Transform.position - (Ghost.Transform.forward * 0.3f) + (Ghost.Transform.up * 0.3f), 1));


            // Jump Into Target
            Ghost.Animation.CrossFade(AnimationResources.PossComplete);
            //argEntity.animation.CrossFade(AnimationResources.PossComplete);
        }
        else
            yield return StartCoroutine(PossessionStruggle(argEntity as AbstractLivingBeing));
    }

    protected IEnumerator UnpossessLiving(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Deactivate(Speaker));

        WorldGUI.Mode = Mode.Spirit;
        Ghost.Animation[AnimationResources.PossExit].speed = 1; ;
        Ghost.Animation.CrossFade(AnimationResources.PossExit);
        //argEntity.animation.CrossFade(AnimationResources.PossExit);

        Ghost.Unpossess(argEntity as AbstractLivingBeing);

        while (Ghost.Animation.IsPlaying(AnimationResources.PossExit))
            yield return new WaitForFixedUpdate();

        Soundtrack.PlayClip("GhostLayer", -1);
    }

    #endregion

    #region Helper Methods

    private IEnumerator PossessionStruggle(AbstractLivingBeing argEntity)
    {
        bool complete = false;
        int status = 0;
        int step = 0;
        Quicktime quicktime = argEntity.GameObject.RequireComponentAdd<Quicktime>();
        ParticleAnimator prefab = ResourceManager.ParticleResources["SequenceEmitter_Glow"];
        ParticleAnimator particle = Instantiate(prefab) as ParticleAnimator;
        particle.transform.position = argEntity.Transform.position;
        particle.transform.rotation = argEntity.Transform.rotation;
        particle.transform.parent = argEntity.Transform;

        AbstractAnimal argAnimal = argEntity as AbstractAnimal;
        if (argAnimal != null)
            argAnimal.AnimalSoundtrack.playPossession();

        Ghost.Animation[AnimationResources.PossBegin].speed = 1.5f;
        argEntity.animation[AnimationResources.PossBegin].speed = 1.5f;
        Ghost.Animation.CrossFade(AnimationResources.PossBegin);
        yield return new WaitForSeconds(2);

        while (complete == false)
        {
            // If Not Complete Run Quicktime Event
            if (!complete)
            {
                ////debug.log("FIRE: " + Time.time);
                yield return StartCoroutine(quicktime.Fire());
                while (Ghost.Animation.IsPlaying(AnimationResources.PossBegin))
                    yield return new WaitForFixedUpdate();
                status = quicktime.Status;
                step++;
                ////debug.log("ANIMATE: " + Time.time);
            }

            string animationName = "";
            switch (step)
            {
                case 1: animationName = AnimationResources.PossInput1;  break;
                case 2: animationName = AnimationResources.PossInput2; break;
                case 3: animationName = AnimationResources.PossInput3; break;
                case 4: animationName = AnimationResources.PossInput4; break;
                case 5: complete = true; break;
            }

            // Fail Entirely only if No EnergyLeft);
            if (Ghost.Energy <= 0)
            {
                complete = true;
                Ghost.Animation.CrossFade(AnimationResources.PossFail);
                Target.animation.CrossFade(AnimationResources.PossFail);
            }

            // Animate Action
            if (animationName != "")
            {
                Ghost.Animation.CrossFade(animationName + "A");
            }

            // Shoot Beams
            if (animationName != "")
            {
                if (step != 0) StartCoroutine(FireRope(status != 3, argEntity as AbstractLivingBeing, animationName));
            }

            // Wait For Animation Complete

        }

        // Animate Pull
        yield return new WaitForSeconds(0.3f); // Wait for Rotation Complete
        Ghost.animation.CrossFade(AnimationResources.PossWrongInput);
        yield return new WaitForSeconds(0.3f);
        argEntity.animation[AnimationResources.PossComplete].speed = 2;
        argEntity.animation.CrossFade(AnimationResources.PossComplete);
        yield return new WaitForSeconds(0.3f);
        _NeedRopes = false;

        // Rotate Target
        Ghost.Animation.CrossFade(AnimationResources.PossInput4Idle);
        yield return StartCoroutine(Target.RotateTowards(Target.Transform.position + Ghost.Transform.forward, 3));


        // Jump Into Target
        Ghost.Animation.CrossFade(AnimationResources.PossComplete);

        Destroy(particle.gameObject);

        yield return new WaitForFixedUpdate();
    }
    #endregion


    bool _NeedRopes = false;
    List<GameObject> _Ropes = new List<GameObject>();

    public IEnumerator FireRope(bool correct, AbstractLivingBeing argBeing, string animationName)
    {
        
        while (Ghost.Animation.IsPlaying(animationName + "A"))
            yield return new WaitForFixedUpdate();
        Ghost.Animation.CrossFade(animationName + "B");

        _NeedRopes = true;
        GameObject _Rope = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(_Rope.collider);
        _Rope.renderer.material.shader = Shader.Find("Transparent/Shield");
        _Rope.renderer.material.color = new Color(0.6f, 0.8f, 1f, 0.1f);

        float time = 0;
        float speed = 3f;

        Transform start = null, end = null;
        switch (_Ropes.Count)
        {
            case 0: start = Ghost.RightHand; end = argBeing.LeftHand; break;
            case 1: start = Ghost.LeftHand; end = argBeing.RightHand; break;
            case 2: start = Ghost.LeftHand; end = argBeing.RightFoot; break;
            case 3: start = Ghost.RightHand; end = argBeing.LeftFoot; break;
        }

        ParticleAnimator prefab = correct ? ResourceManager.ParticleResources["SequenceEmitter_Correct"] : ResourceManager.ParticleResources["SequenceEmitter_Wrong"];
        ParticleAnimator instance = Instantiate(prefab) as ParticleAnimator;
        instance.transform.position = start.position;
        instance.transform.rotation = start.rotation;

        _Ropes.Add(_Rope);

        while (_NeedRopes)
        {
            time += Time.deltaTime;

            /// Scale Size to Distance
            float size = 0.03f;
            float dist = Mathf.Lerp(0, Vector3.Distance(start.position, end.position), time * speed);
            _Rope.transform.localScale = new Vector3(size, dist, size);

            /// Find Center
            _Rope.transform.position = Vector3.Lerp(start.position, (start.position + end.position) / 2, time * speed);

            /// Look At Target
            _Rope.transform.LookAt(end.position);
            _Rope.transform.Rotate(Vector3.right * 90); // Offset

            yield return new WaitForFixedUpdate();
        }

        // Break Ropes
        foreach(GameObject obj in _Ropes)
            GameObject.Destroy(obj);
    }


    /// <summary>
    /// Adds a cross fade event on target once start animation is called
    /// </summary>
    private void AddCrossFadeTargetEvent(float t, string startAnimation, string responceAnimation)
    {
        var animationEvent = new AnimationEvent
        {
            functionName = "CrossFadeTargetEvent",
            stringParameter = responceAnimation,
            time = t
        };

        animation[startAnimation].clip.AddEvent(animationEvent);
    }

    /// <summary>
    /// Adds a cross fade event on self once start animation is called
    /// </summary>
    private void AddCrossFadeSelfEvent(float t, string startAnimation, string responceAnimation)
    {
        var animationEvent = new AnimationEvent
        {
            functionName = "CrossFadeSelfEvent",
            stringParameter = responceAnimation,
            time = t
        };

        animation[startAnimation].clip.AddEvent(animationEvent);
    }

    #region Animation Events

    private void WrongInput(AnimationEvent animationEvent)
    {
        //Ghost.Energy -= 10;
    }

    private void ChangeSoundtrack(AbstractEntity argTarget)
    {
        if (argTarget is HumanScript)
        {
            Soundtrack.FadeIn("HumanLayer", 8, -1);
        }
        else if (argTarget is AbstractAnimal)
        {
            Soundtrack.PlayClip("AnimalLayer", -1);
        }
    }

    private IEnumerator PossessionComplete(AnimationEvent animationEvent)
    {
        Target.active = false;

        StartCoroutine(Ghost.MoveTo(Target.Transform.position, 1));
        yield return new WaitForSeconds(1);
        //Target.animation.CrossFade(AnimationResources.PossComplete);

        WorldGUI.Mode = Mode.Living;

        //Target.animation[AnimationResources.Jump].speed = 1;

        ScreenGUI.FlashSpeed = 0.5f;
        ScreenGUI.FlashColor = new Color(0.6f, 0.8f, 1, 0.65f);
        ScreenGUI.FlashSpeed = 1f;

        ChangeSoundtrack(Target);


        //while (Target.animation.IsPlaying(AnimationResources.PossComplete))
        //    yield return new WaitForFixedUpdate();

        Target.active = true;
        Ghost.Possess(Target as AbstractLivingBeing);
}

    private void PossessionFailed(AnimationEvent animationEvent)
    {
        if (PowerOff != null)
            PowerOff(Target);
        //Active = false;

        Ghost.Unpossess(Target as AbstractLivingBeing);
        WorldGUI.Mode = Mode.Spirit;
    }

    private void UnPossess(AnimationEvent animationEvent)
    {
        ScreenGUI.FlashSpeed = 0.5f;
        ScreenGUI.FlashColor = new Color(0.6f, 0.8f, 1, 0.65f);
        ScreenGUI.FlashSpeed = 1f;
    }

    public void CrossFadeTargetEvent(AnimationEvent animationEvent)
    {
        if (animationEvent == null) return;
        Animation anim = Target.animation;
        if (anim != null) anim.CrossFade(animationEvent.stringParameter);
    }

    #endregion
}

//[System.Serializable]
//public class PossessionStep
//{
//    [SerializeField]
//    private string _AnimStart;
//    private string _AnimFinish;

//    [SerializeField]
//    private string _Idle;

//    [SerializeField]
//    private PossessionRemote.Limb _GhostLimb;

//    [SerializeField]
//    private PossessionRemote.Limb _BeingLimb;


//    private GameObject _Rope;

//    public PossessionStep(PossessionRemote.Limb ghostLimb, PossessionRemote.Limb beingLimb, string animStart, string animFinish, string idle)
//    {
//        _GhostLimb = ghostLimb;
//        _BeingLimb = beingLimb;
//        _AnimStart = animStart;
//        _AnimFinish = animFinish;
//        _Idle = idle;
//    }

//    public Transform GetLimb(PossessionRemote.Limb limb, AbstractBeing being)
//    {
//        switch (limb)
//        {
//            case PossessionRemote.Limb.LeftArm: return being.LeftHand;
//            case PossessionRemote.Limb.RightArm: return being.RightHand;
//            case PossessionRemote.Limb.LeftLeg: return being is GhostScript ? being.Tail : being.LeftFoot;
//            case PossessionRemote.Limb.RightLeg: return being is GhostScript ? being.Tail : being.RightFoot;
//        }
//        return null;
//    }

//    public IEnumerator Animate(PossessionRemote remote, GhostScript ghost, AbstractLivingBeing being)
//    {
//        // Animate Animation
//        if (_AnimStart != null)
//        {
//            if (_BeingLimb == PossessionRemote.Limb.None)
//                if (being.Animation[_AnimStart] != null)
//                    being.animation.CrossFade(_AnimStart);
//            if (ghost.Animation[_AnimStart] != null)
//            {
//                ghost.animation.CrossFade(_AnimStart);
//                while (ghost.animation.IsPlaying(_AnimStart))
//                    yield return new WaitForFixedUpdate();
//            }
//        }

//        // Throw Rope
//        if (_GhostLimb != PossessionRemote.Limb.None)
//            ghost.StartCoroutine(RopeCoroutine(remote, ghost, being));


//        //yield return new WaitForSeconds(0.5f);

//        // Animate Animation
//        if (_AnimFinish != "")
//        {
//            if (_BeingLimb == PossessionRemote.Limb.None)
//            {
//                if (being.Animation[_AnimFinish] != null)
//                    being.animation.CrossFade(_AnimFinish);
//                if (ghost.Animation[_AnimFinish] != null)
//                {
//                    ghost.animation.CrossFade(_AnimFinish);
//                    while (ghost.animation.IsPlaying(_AnimFinish))
//                        yield return new WaitForFixedUpdate();
//                }
//            }
//            else
//                yield return being.StartCoroutine(AnimateResponce(ghost, being));
//        }

//        // Run Idle
//        if (_Idle != "")
//        {
//            if (being.Animation[_Idle] != null)
//                being.animation.CrossFade(_Idle);
//            if (ghost.Animation[_Idle] != null)
//                ghost.animation.CrossFade(_Idle);
//            yield return new WaitForFixedUpdate();
//        }
//    }

//    public IEnumerator AnimateResponce(GhostScript ghost, AbstractBeing being)
//    {
//        if (_AnimFinish != "" && ghost.Animation[_AnimFinish] != null)
//            ghost.animation.CrossFade(_AnimFinish);

//        if (_AnimStart != "" && being.Animation[_AnimStart] != null)
//        {
//            being.animation.CrossFade(_AnimStart);
//            while (being.animation.IsPlaying(_AnimStart))
//                yield return new WaitForFixedUpdate();
//        }


//        if (_AnimFinish != "" && being.animation[_AnimFinish] != null)
//        {
//            being.animation.CrossFade(_AnimFinish);
//            while (being.animation.IsPlaying(_AnimFinish))
//                yield return new WaitForFixedUpdate();
//        }

//    }


//    /// <summary>
//    /// Animates the idle.
//    /// </summary>
//    public IEnumerator AnimateIdle(AbstractBeing being)
//    {
//        if (_Idle != "")
//            being.animation.CrossFade(_Idle);
//        yield return new WaitForFixedUpdate();
//    }

//    /// <summary>
//    /// Runes the Ropes coroutine.
//    /// </summary>
//    public IEnumerator RopeCoroutine(PossessionRemote remote, GhostScript ghost, AbstractLivingBeing being)
//    {
//        _Rope = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//        GameObject.Destroy(_Rope.collider);
//        _Rope.renderer.material.shader = Shader.Find("Transparent/Shield");
//        _Rope.renderer.material.color = new Color(1f, 1f, 1f, 0.1f);

//        float time = 0;
//        float speed = 3f;
//        Transform start = GetLimb(_GhostLimb, ghost);
//        Transform end = GetLimb(_BeingLimb, being);

//        if (start && end)
//        {
//            while (remote.Locked)
//            {
//                time += Time.deltaTime;

//                /// Scale Size to Distance
//                float size = 0.03f;
//                float dist = Mathf.Lerp(0, Vector3.Distance(start.position, end.position), time * speed);
//                _Rope.transform.localScale = new Vector3(size, dist, size);

//                /// Find Center
//                _Rope.transform.position = Vector3.Lerp(start.position, (start.position + end.position) / 2, time * speed);

//                /// Look At Target
//                _Rope.transform.LookAt(end.position);
//                _Rope.transform.Rotate(Vector3.right * 90); // Offset

//                yield return new WaitForFixedUpdate();
//            }
//        }
//        GameObject.Destroy(_Rope);

//    }
//}