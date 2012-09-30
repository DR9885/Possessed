using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    #region Fields

    public FSM<MasterController, ControllerState> ControllerFSM;
    [SerializeField] private ControllerState _controllerState;
    public ControllerState ControllerState
    {
        set
        {
            if (_controllerState != value)
                ControllerFSM.ChangeState(value);
            _controllerState = value;
        }
    }

    public FSM<MasterController, TargetState> TargetFSM;
    [SerializeField] private TargetState _targetState;
    public TargetState TargetState
    {
        set
        {
            if (_targetState != value)
                TargetFSM.ChangeState(value);
            _targetState = value;
        }
    }

    private IEnumerable<IController> _controllers;
    private IEnumerable<IController> Controllers
    {
        get
        {
            if (_controllers == null)
                _controllers = GetComponents<MonoBehaviour>().OfType<IController>();
            return _controllers;
        }
    }

    private IController _controller;
    public IController Controller
    {
        get { return _controller; }
        set
        {
            if(value == null) TargetState = TargetState.Idle;
            _controller = value;
            if (value != _controller) TargetState = TargetState.Target;
        }
    }

    private Transform _transform;
    private Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }

    [SerializeField] private Color _colorActive = Color.blue;
    public Color ColorActive
    {
        get { return _colorActive; }
    }
    [SerializeField] private Color _colorInActive = Color.red;
    public Color ColorInActive
    {
        get { return _colorInActive; }
    }
    [SerializeField] private float _minSize = 0.5f;
    public float MinSize
    {
        get { return _minSize / 1000.0f; }
    }
    [SerializeField] private float _maxSize = 10;
    public float MaxSize
    {
        get { return _maxSize / 1000.0f; }
    }
    [SerializeField] private float _speed = 2;
    public float Speed
    {
        get { return _speed; }
    }

    private Camera _camera;
    public Camera Camera
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;
            if (_camera == null)
                _camera = FindObjectOfType(typeof(Camera)) as Camera;
            return _camera;

        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        ControllerFSM = new FSM<MasterController, ControllerState>(this);
        ControllerFSM.RegisterState(null);
//        ControllerFSM.RegisterState(GetComponent<DoorController>());
//        ControllerFSM.RegisterState(GetComponent<PickUpController>());
        ControllerFSM.ChangeState(ControllerState.None);

        TargetFSM = new FSM<MasterController, TargetState>(this);
        TargetFSM.RegisterState(new ControllerIdleState());
        TargetFSM.RegisterState(new ControllerTargetState());
        TargetFSM.RegisterState(new ControllerActiveState());
        TargetFSM.ChangeState(TargetState.Idle);
    }

    private void FixedUpdate()
    {
        Controller = Controllers
            .Where(x => x.GetTarget() != null)
            .OrderBy(x => Vector3.Distance(x.GetTarget().Transform.position, Transform.position))
            .FirstOrDefault();

        ControllerFSM.Update();
        TargetFSM.Update();
    }

    //private void OnGUI()
    //{
    //    if (Controller != null && _targetState == TargetState.Target && !Controller.Target.Locked)
    //    {
    //        float width = 100.0f, height = 100.0f;
    //        if (GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f,
    //            Screen.height / 2.0f - height / 2.0f, 100, 100),
    //            "Act"))
    //            TargetState = TargetState.Active;
    //    }
    //}

    #endregion
}