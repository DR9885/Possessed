using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController, IFSMState<MasterController, ControllerState>
{
    public ControllerState State
    {
        get { return ControllerState.Door; }
    }

    public void Enter(MasterController entity)
    {

    }

    public void Execute(MasterController entity)
    {

    }

    public void Exit(MasterController entity)
    {

    }

    #region Fields

    [SerializeField] private int _Distance = 10;
    public float Distance { get { return _Distance; } }

    [SerializeField] private float _Angle = 45;
    public float Angle { get { return _Angle; } }

    [SerializeField] private DebugControllerSettings _debugSettings = new DebugControllerSettings();
    public DebugControllerSettings DebugSettings { get { return _debugSettings; } }

    [SerializeField] private  Door _Target;
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
    
    #endregion

    #region Unity Methods

    private void FixedUpdate()
    {
        // Note: Just For Debugging
        Target = this.GetTarget();
    }

    private void OnGUI()
    {
        if(Target != null)
        {
            float width = 100.0f, height = 100.0f;

            if(GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f, 
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Open Door"))
                (Target as Door).ActionState = DoorState.Open;
        }
    }

    #endregion

}
