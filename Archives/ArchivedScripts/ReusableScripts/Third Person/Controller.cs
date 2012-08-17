using UnityEngine;

[AddComponentMenu("Possessed-UnderConstruction/Controller")]
[RequireComponent(typeof(CharacterController), typeof(ThirdPersonCamera))]
public class Controller : MonoBehaviour
{
    #region Fields

    public float _WalkAnimationSpeed = 0.75f;
    public float _TrotAnimationSpeed = 1.0f;
    public float _RunAnimationSpeed = 1.0f;

    public float _WalkSpeed = 2.0f;
    public float _TrotSpeed = 4.0f;
    public float _RunSpeed = 6.0f;
    public float _TurnSpeed = 360.0f;

    public float _Gravity = 20.0f;

    private CollisionFlags _CollisionFlags;
    private Vector3 _MoveDirection = Vector3.zero;
    private float _MoveSpeed;


    #endregion

    #region Property

    public GameObject _GameObject;
    public GameObject GameObject
    {
        get
        {
            if (_GameObject == null)
                _GameObject = gameObject;
            return _GameObject;
        }
    }

    public Transform _Transform;
    public Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            return _Transform;
        }
    }


    public Animation _Animation;
    public Animation Animation
    {
        get
        {
            if (_Animation == null)
                _Animation = animation;
            return _Animation;
        }
    }

    public CharacterController _CharacterController;
    public CharacterController CharacterController
    {
        get
        {
            if (_CharacterController == null)
                _CharacterController = GetComponent <CharacterController>();
            return _CharacterController;
        }
    }

    #endregion

    void Awake()
    {

    }

    void OnEnable()
    {
        CrossFade(AnimationResources.Idle);
    }

    void OnDisable()
    {
        CrossFade(AnimationResources.Idle);
    }

    void Update()
    {

    }
    void CrossFade(string animationName)
    {
        if (_Animation && _Animation[animationName] != null
            && !_Animation.IsPlaying(animationName))
            _Animation.CrossFade(animationName);
    }
}
