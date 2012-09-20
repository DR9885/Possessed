
using UnityEngine;

/// <summary>
/// Searching For Targets
/// </summary>
public class ControllerIdleState : IFSMState<MasterController, TargetState>
{

    public TargetState State
    {
        get { return TargetState.Idle; }
    }

    public void Enter(MasterController entity)
    {

    }

    public void Execute(MasterController entity)
    {
        var target = entity.Controller.GetTarget();
        if (target != null)
        {
            entity.Controller.Target = target;
            entity.TargetState = TargetState.Target;
        }
    }


    public void Exit(MasterController entity)
    {

    }
}
