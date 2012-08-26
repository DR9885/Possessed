using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour, IFiniteState<MasterController>
{
    public void Enter(MasterController entity, IFiniteState<MasterController> prevState)
    {
        Debug.Log("Show Controls");
        enabled = true;
    }

    public void Running(MasterController entity)
    {
        Debug.Log("Controls Loop");
    }

    public void Exit(MasterController entity, IFiniteState<MasterController> nextState)
    {
        Debug.Log("Hide Controls");

        enabled = false;
    }
}
