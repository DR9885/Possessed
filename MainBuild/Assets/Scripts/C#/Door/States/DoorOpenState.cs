using UnityEngine;
using System.Collections;

public class DoorOpenState : IFSMState<Door, DoorState>
{
    public DoorState State
    {
        get { return DoorState.Open; }
    }

    public void Enter(Door entity)
    {
        entity.Animation.Play("Open");
    }

    public void Execute(Door entity)
    {
        if (entity.CanClose && !entity.Animation.IsPlaying("Open"))
            entity.ActionState = DoorState.Idle;
    }

    public void Exit(Door entity)
    {
        entity.Animation.Play("Close");
    }
}
