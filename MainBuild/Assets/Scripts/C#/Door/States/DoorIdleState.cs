using UnityEngine;
using System.Collections;

public class DoorIdleState : IFSMState<Door, DoorState>
{
    public DoorState State
    {
        get { return DoorState.Idle; }
    }

    public void Enter(Door entity)
    {

    }

    public void Execute(Door entity)
    {

    }

    public void Exit(Door entity)
    {
    
    }
}
