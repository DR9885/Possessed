public class ControllerTargetState : FSMState<MasterController, TargetState>
{

    public override TargetState State
    {
        get { return TargetState.Target; }
    }

    public override void Enter(MasterController entity)
    {
        
    }

    public override void Execute(MasterController entity)
    {
        
    }

    public override void Exit(MasterController entity)
    {
        
    }
}
