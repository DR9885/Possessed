/*using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[AddComponentMenu("Possessed/Objects/Door")]
public class Door : MonoBehaviour, ITargetable
{
    #region Fields

    public float CloseDistance = 10;

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

    public FSM<Door, DoorState> ActionFSM { get; set; }
    [SerializeField]
    private DoorState _actionState;
    public DoorState ActionState
    {
        get { return _actionState; }
        set { _actionState = value; }
    }

    private DoorController _opener;
    public DoorController Opener
    {
        get { return _opener; }
        set { _opener = value; }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        ActionFSM = new FSM<Door, DoorState>(this);
        ActionFSM.RegisterState(new DoorIdleState());
        ActionFSM.RegisterState(new DoorOpenState());
        ActionFSM.RegisterState(new DoorGhostState());
    }

    private void FixedUpdate()
    {
        if (ActionFSM != null)
            ActionFSM.Update(ActionState);
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

        //TODO: Disable Collider
    }

    public void Close()
    {
        _opener = null;
        ActionState = DoorState.Idle;

        //TODO: Enable Collider
    }
}*/