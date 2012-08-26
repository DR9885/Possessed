using UnityEngine;
using System.Collections;

public class DoorClosedState : IFiniteState<Door> 
{
    public void Enter(Door entity, IFiniteState<Door> prevState)
    {

    }

    public void Running(Door entity)
    {
    
    }

    public void Exit(Door entity, IFiniteState<Door> nextState)
    {

    }
}
