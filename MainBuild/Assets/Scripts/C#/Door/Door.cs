using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[AddComponentMenu("Possessed/Objects/Door")]
public class Door : MonoBehaviour, ITargetable
{
    #region Fields

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

    public FSM<ITargetable, TargetState> TargetFSM { get; set; }
    public FSM<Door, DoorState> ActionFSM { get; set; }

    [SerializeField] private TargetState _targetState;
    public TargetState TargetState
    {
        set { _targetState = value; }
        get { return _targetState; }
    }

    //[SerializeField] private bool _Debug;
    //public bool Debug
    //{
    //    get { return _Debug; }
    //}

    public DoorState ActionState;
    public bool CanClose;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        TargetFSM = new FSM<ITargetable, TargetState>(this);
        TargetFSM.RegisterState(new ControllerIdleState());
        TargetFSM.RegisterState(new ControllerTargetState());
        TargetFSM.RegisterState(new ControllerActiveState());


        ActionFSM = new FSM<Door, DoorState>(this);
        ActionFSM.RegisterState(new DoorIdleState());
        ActionFSM.RegisterState(new DoorOpenState());
    }

    private void FixedUpdate()
    {
        TargetFSM.Update(TargetState);

        ActionFSM.Update(ActionState);
    }

    #endregion
}
