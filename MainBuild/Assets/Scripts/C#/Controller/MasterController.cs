using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ControllerState
{
    None,
    Door,
    PickUp,
    Possession
}

public enum TargetState
{
    Idle,
    Target,
    Active
}

[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    public FSM<MasterController, TargetState> FSM;
    public TargetState State;

    #region Unity Methods

    private void Awake()
    {
        FSM = new FSM<MasterController, TargetState>(this);
        FSM.RegisterState(new ControllerIdleState());
        FSM.RegisterState(new ControllerTargetState());
    }

    private void FixedUpdate()
    {
        FSM.Update(State);
    }

    #endregion
}
