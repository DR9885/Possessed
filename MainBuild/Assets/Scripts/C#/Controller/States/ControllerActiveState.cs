public class ControllerActiveState : FSMState<MasterController, TargetState>
{

    public override TargetState State
    {
        get { return TargetState.Idle; }
    }

    public override void Enter(MasterController entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(MasterController entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit(MasterController entity)
    {
        throw new System.NotImplementedException();
    }
}
