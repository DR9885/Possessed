using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController, IFSMState<MasterController, ControllerState>
{
    #region Fields
    
    [SerializeField] private bool _isGhost;
    public bool IsGhost { get { return _isGhost; } }

    [SerializeField] private float _distance = 4;
    public float Distance { get { return _distance; } }

    [SerializeField] private float _angle = 45;
    public float Angle { get { return _angle; } }

    [SerializeField] private DebugControllerSettings _debugSettings = new DebugControllerSettings();
    public DebugControllerSettings DebugSettings { get { return _debugSettings; } }

    private IEnumerable<Door> _doors;
    public IEnumerable<Door> Doors
    {
        get
        {
            if (_doors == null)
                _doors = FindObjectsOfType(typeof(Door)).Select(x => x as Door);
            return _doors;
        }
    }
    
    public IEnumerable<ITargetable> Targets
    {
        get { return Doors.Where(x => x.ActionState == DoorState.Idle).Select(x => x as ITargetable); }
    }
    
    [SerializeField] private  Door _door;
    public ITargetable Target
    {
        get { return _door; }
        set { _door = value as Door; }
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

    public FSM<IController, TargetState> TargetFSM { get; set; }
    [SerializeField] private TargetState _targetState;
    public TargetState TargetState
    {
        get { return _targetState; }
        set
        {
            if (_targetState != value)
                TargetFSM.ChangeState(value);
            _targetState = value;
        }
    }

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        TargetFSM = new FSM<IController, TargetState>(this);
        TargetFSM.RegisterState(new ControllerIdleState());
        TargetFSM.RegisterState(new ControllerTargetState());
        TargetFSM.RegisterState(new ControllerActiveState());
        TargetFSM.ChangeState(TargetState.Idle);
    }

    private void FixedUpdate()
    {
        if (TargetFSM.CurrentState != _targetState)
            TargetFSM.ChangeState(_targetState);
        TargetFSM.Update();
    }

    private void OnGUI()
    {
        if (Target != null && TargetState == TargetState.Target && !Target.Locked)
        {
            float width = 100.0f, height = 100.0f;
            if (GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f,
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Open Door"))
                Open(_door);

            if (GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f + width,
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Ghost Door"))
                Walkthrough(_door);
        }
    }

    private void OnDrawGizmos()
    {
        this.GizmosFOV();
        this.GizmosTarget();
    }

    #endregion

    #region IFSMState

    public ControllerState State
    {
        get { return ControllerState.Door; }
    }

    public void Enter(MasterController entity)
    {
        enabled = true;

        if(IsGhost)
        {
            foreach (Door door in Doors)
                door.WalkThrough(this);
        }
//        TargetState = TargetState.;
    }

    public void Execute(MasterController entity)
    {
        
    }

    public void Exit(MasterController entity)
    {
        TargetState = TargetState.Idle;
        enabled = false;
    }

    #endregion

    public void Open(Door door)
    {
        if (door != null)
        {
            TargetState = TargetState.Idle;
            door.Open(this);
        }
    }

    public void Walkthrough(Door door)
    {
        if (door != null)
        {
            TargetState = TargetState.Idle;
            door.WalkThrough(this);
        }
    }

    public void CloseDoor(Door door)
    {
        if (_door != null)
        {
            door.Close();
        }
    }
}
