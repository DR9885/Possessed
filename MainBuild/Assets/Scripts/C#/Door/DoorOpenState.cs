using UnityEngine;
using System.Collections;

public class DoorOpenState : IFiniteState<Door> {

    public void OnEnter(Door entity)
    {
        Debug.Log("Open Door");
    }

    public void OnDecision(Door entity)
    {
        Debug.Log("Wait For Controller To Be Out of Range");
    }

    public void OnExit(Door entity)
    {

    }
}
