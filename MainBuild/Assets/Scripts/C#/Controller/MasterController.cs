using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ControllerState
{
    None,
    Door,
    PickUp,
    Climbing,
    Possession
}

/*[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    #region Fields

    private FSM<MasterController, ControllerState> _controllerFSM;
    [SerializeField] private MonoBehaviour _controller;
    public IController Controller
    {
        get { return _controller as IController; }
        set { _controller = value as MonoBehaviour; }
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
        _controllerFSM.RegisterState(GetComponent<DoorController>());
    }

    private void FixedUpdate()
    {
        Controller = Controllers
            .Where(x => x.GetTarget() != null)
            .OrderBy(x => Vector3.Distance(x.GetTarget().Transform.position, Transform.position))
            .FirstOrDefault();

        _controllerFSM.Update(Controller as IFSMState<MasterController, ControllerState>);
    }
    #endregion
}*/
