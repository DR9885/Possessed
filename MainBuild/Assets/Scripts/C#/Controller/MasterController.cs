using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    #region Fields

    private FSM<MasterController, ControllerState> _controllerFSM;
    [SerializeField] private ControllerState _controllerState;
    private ControllerState State
    {
        set
        {
            if (_controllerState != value)
                _controllerFSM.ChangeState(value);
            _controllerState = value;
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

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        _controllerFSM = new FSM<MasterController, ControllerState>(this);
        _controllerFSM.RegisterState(null);
        _controllerFSM.RegisterState(GetComponent<DoorController>());
        _controllerFSM.ChangeState(ControllerState.None);
    }

    private void FixedUpdate()
    {
        var controller = Controllers
            .Where(x => x.GetTarget() != null)
            .OrderBy(x => Vector3.Distance(x.GetTarget().Transform.position, Transform.position))
            .FirstOrDefault();

        State = controller is IFSMState<MasterController, ControllerState>
                    ? (controller as IFSMState<MasterController, ControllerState>).State
                    : ControllerState.None;
        _controllerFSM.Update();
    }
    #endregion
}