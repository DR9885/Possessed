using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public void OnEnter(MasterController entity)
    {
        enabled = true;
    }

    public void OnDecision(MasterController entity)
    {
        Debug.Log("Controls Loop");
    }

    public void OnExit(MasterController entity)
    {
        enabled = false;
    }



}
