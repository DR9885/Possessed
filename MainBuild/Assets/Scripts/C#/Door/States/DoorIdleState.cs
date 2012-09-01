using UnityEngine;
using System.Collections;

public class DoorIdleState : FSMState<Door, DoorState>
{
    public override DoorState State
    {
        get { return DoorState.Idle; }
    }

    public override void Enter(Door entity)
    {

    }

    public override void Execute(Door entity)
    {
    
    }

    public override void Exit(Door entity)
    {
    
    }
}
