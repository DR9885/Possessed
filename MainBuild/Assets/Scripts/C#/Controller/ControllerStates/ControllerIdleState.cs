using UnityEngine;
using System.Collections;

public class ControllerIdleState : FSMState<MonoBehaviour, ControllerState>
{

    public override ControllerState State
    {
        get { return ControllerState.Idle; }
    }

    public override void Enter(MonoBehaviour entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(MonoBehaviour entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit(MonoBehaviour entity)
    {
        throw new System.NotImplementedException();
    }
}
