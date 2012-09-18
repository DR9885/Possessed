/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController, IFSMState<MasterController, ControllerState>
{

    #region Fields

    [SerializeField] private int _Distance = 10;
    public float Distance { get { return _Distance; } }

    [SerializeField] private float _Angle = 45;
    public float Angle { get { return _Angle; } }

    [SerializeField] private DebugControllerSettings _debugSettings = new DebugControllerSettings();
    public DebugControllerSettings DebugSettings { get { return _debugSettings; } }

    [SerializeField] private  Door _Target;
    public IEnumerable<ITargetable> Targets
    {
        get { return Object.FindObjectsOfType(typeof(Door)).Select(x => x as Door)
            .Where(x => x.ActionState == DoorState.Idle).Select(x => x as ITargetable); }
    }

    public ITargetable Target
    {
        get { return _Target; }
        set { _Target = value as Door; }
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
    [SerializeField] private TargetState _state;
    public TargetState TargetState
    {
        get { return _state; }
        set { _state = value; }
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
    }

    private void FixedUpdate()
    {
        TargetFSM.Update(_state);
    }

    private void OnGUI()
    {
        if(Target != null)
        {
            float width = 100.0f, height = 100.0f;
            if (GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f,
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Open Door"))
                (Target as Door).Open(this);

            if (GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f + width,
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Ghost Door"))
                (Target as Door).WalkThrough(this);
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
        //TargetState = TargetState.Target;
    }

    public void Execute(MasterController entity)
    {
        
    }

    public void Exit(MasterController entity)
    {
        TargetState = TargetState.Idle;
        TargetFSM.Update(_state);
        enabled = false;
    }

    #endregion
}
*/