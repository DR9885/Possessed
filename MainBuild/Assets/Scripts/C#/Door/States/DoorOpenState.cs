using UnityEngine;
using System.Collections;

public class DoorOpenState : FSMState<Door, DoorState>
{
    public override DoorState State
    {
        get { return DoorState.Open; }
    }

    public override void Enter(Door entity)
    {
        Debug.Log("Open");
        entity.Animation.Play("Open");
    }

    public override void Execute(Door entity)
    {
        if (entity.CanClose && !entity.Animation.IsPlaying("Close"))
            entity.State = DoorState.Close;
            entity.State = DoorState.Idle;
    }

    public override void Exit(Door entity)
    {

    }
}
