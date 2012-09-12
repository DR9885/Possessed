
using UnityEngine;

/// <summary>
/// Searching For Targets
/// </summary>
public class ControllerIdleState : IFSMState<IController, TargetState>
{

    public TargetState State
    {
        get { return TargetState.Idle; }
    }

    public void Enter(IController entity)
    {
        entity.Target = null;
    }

    public void Execute(IController entity)
    {
        var target = entity.GetTarget();
        if (target != null)
        {
            entity.Target = target;
            entity.TargetState = TargetState.Target;
        }
    }


    public void Exit(IController entity)
    {

    }
}
