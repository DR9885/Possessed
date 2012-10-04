using UnityEngine;

public class ControllerActiveState : IFSMState<MasterController, TargetState>
{

    public TargetState State
    {
        get { return TargetState.Active; }
    }


    public void Enter(MasterController entity)
    {
        (entity.Controller as MonoBehaviour).SendMessage("OnEnter");

//        entity.ControllerState = (entity.Controller as IFSMState<MasterController, ControllerState>).State;
    }

    public void Execute(MasterController entity)
    {

    }

    public void Exit(MasterController entity)
    {
        entity.Controller.Target = null;
        entity.ControllerState = ControllerState.None;
    }
}
