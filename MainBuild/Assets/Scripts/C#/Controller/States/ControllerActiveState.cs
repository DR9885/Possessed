public class ControllerActiveState : FSMState<ITargetable, TargetState>
{

    public override TargetState State
    {
        get { return TargetState.Active; }
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
