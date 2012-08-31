using UnityEngine;
using System.Collections;

public class DoorCloseState : FSMState<Door, DoorState> {
    public override DoorState State
    {
        get { return DoorState.Close; }
    }

    public override void Enter(Door entity)
    {
        Debug.Log("close");
//        entity.Animation.Play("Close");    
    }

    public override void Execute(Door entity)
    {
        if (!entity.Animation.IsPlaying("Close"))
            entity.State = DoorState.Idle;
    }

    public override void Exit(Door entity)
    {
    
    }
}
