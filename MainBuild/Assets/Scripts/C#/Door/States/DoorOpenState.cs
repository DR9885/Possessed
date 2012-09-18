/*using UnityEngine;
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
        if(entity.Opener == null || entity.CloseDistance < Vector3.Distance(entity.Opener.Transform.position, entity.Transform.position))
            if (!entity.Animation.isPlaying)
                entity.Close();
    }

    public void Exit(Door entity)
    {
        entity.Animation.Play("Close");
    }
}
*/