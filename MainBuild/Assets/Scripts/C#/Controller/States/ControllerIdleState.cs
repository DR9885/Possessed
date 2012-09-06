public class ControllerIdleState : FSMState<ITargetable, TargetState>
{

    public override TargetState State
    {
        get { return TargetState.Idle; }
    }

    public override void Enter(ITargetable entity)
    {

    }

    public override void Execute(ITargetable entity)
    {

    }

    public override void Exit(ITargetable entity)
    {

    }
}
