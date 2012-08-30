using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ControllerStates
{
    None,
    Door,
    PickUp,
    Possession
}

public class MasterController : MonoBehaviour 
{
    public FiniteStateMachine<MasterController> FSM = new FiniteStateMachine<MasterController>();

    public Dictionary<ControllerStates, IFiniteState<MasterController>> _finiteStates;
    public Dictionary<ControllerStates, IFiniteState<MasterController>> FiniteStates
    {
        get
        {
            if (_finiteStates == null)
                _finiteStates = new Dictionary<ControllerStates, IFiniteState<MasterController>>()
                                        {
                                            { global::ControllerStates.Door, GetComponent<DoorController>() }
                                        };
            return _finiteStates;
        }
    }

    private void Awake()
    {
        FSM.Init(this);
        FSM.ChangeState(new DoorController());
    }

    private void FixedUpdate()
    {
        FSM.OnDecision();
    }
}
