using UnityEngine;
using System.Collections;

public class ControllerTargetState : FSMState<MonoBehaviour, ControllerState> {

    public override ControllerState State
    {
        get { return ControllerState.Target; }
    }

    public override void Enter(MonoBehaviour entity)
    {
        
    }

    public override void Execute(MonoBehaviour entity)
    {
        
    }

    public override void Exit(MonoBehaviour entity)
    {
        
    }
}
