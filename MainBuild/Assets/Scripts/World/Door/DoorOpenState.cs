using UnityEngine;
using System.Collections;

public class DoorOpenState : IFiniteState<Door> {

    public void Enter(Door entity, IFiniteState<Door> prevState)
    {
        Debug.Log("Open Door");
    }

    public void Running(Door entity)
    {
        Debug.Log("Wait For Controller To Be Out of Range");
    }

    public void Exit(Door entity, IFiniteState<Door> nextState)
    {

    }
}
