using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[AddComponentMenu("Possessed/Objects/Door")]
public class Door : MonoBehaviour, ITargetable
{
    #region Fields

    public float CloseDistance = 4;

    private GameObject _gameObject;
    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = gameObject;
            return _gameObject;
        }
    }

    private Animation _animation;
    public Animation Animation
    {
        get
        {
            if (_animation == null)
                _animation = GetComponent<Animation>();
            return _animation;
        }
    }

    private Transform _transform;
    public Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }

    private BoxCollider _boxCollider;
    public Collider TargetCollider
    {
        get
        {
            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider>();
            if (_boxCollider == null)
            {
                _boxCollider = GameObject.AddComponent<BoxCollider>();
                _boxCollider.isTrigger = true;
                _boxCollider.center = new Vector3(0, 0.84f, 0.58f);
                _boxCollider.size = new Vector3(1.46f, 1.7f, 1.35f);
            }
            return _boxCollider;
        }
    }


    private SkinnedMeshRenderer _targetRenderer;
    public Renderer TargetRenderer
    {
        get
        {
            if (_targetRenderer == null)
                _targetRenderer = Transform.Find("joint1").GetComponent<SkinnedMeshRenderer>();
            return _targetRenderer;
        }
    }

    private Highlight _highlight;
    public Highlight Highlight
    {
        get
        {
            if (_highlight == null)
                _highlight = GetComponent<Highlight>();
            if (_highlight == null)
                _highlight = GameObject.AddComponent<Highlight>();
            return _highlight;
        }
    }

    public FSM<Door, DoorState> ActionFSM { get; set; }
    [SerializeField] private DoorState _actionState;
    public DoorState ActionState
    {
        get { return _actionState; }
        set
        {
            if (_actionState != value)
                ActionFSM.ChangeState(value);
            _actionState = value;
        }
    }

    private DoorController _opener;
    public DoorController Opener
    {
        get { return _opener; }
        set { _opener = value; }
    }

    [SerializeField] private bool _locked;
    public bool Locked
    {
        get { return _locked; }
        set { _locked = value; }
    }

    #endregion

    #region Unity Methods

    private void Reset()
    {
        TargetCollider.enabled = false;
    }

    private void Awake()
    {
        ActionFSM = new FSM<Door, DoorState>(this);
        ActionFSM.RegisterState(new DoorIdleState());
        ActionFSM.RegisterState(new DoorOpenState());
        ActionFSM.RegisterState(new DoorGhostState());
        ActionFSM.ChangeState(DoorState.Idle);
        TargetCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (ActionFSM != null)
            ActionFSM.Update();
    }

    #endregion

    public void Open(DoorController opener)
    {
        _opener = opener;
        ActionState = DoorState.Open;
    }

    public void WalkThrough(DoorController opener)
    {
        _opener = opener;
        ActionState = DoorState.Ghost;
    }

    public void Close()
    {
        _opener = null;
        ActionState = DoorState.Idle;
    }
}