public class ControllerActiveState : IFSMState<IController, TargetState>
{

    public TargetState State
    {
        get { return TargetState.Active; }
    }

    public void Enter(IController entity)
    {
        
    }

    public void Execute(IController entity)
    {

    }

    public void Exit(IController entity)
    {

    }
}
