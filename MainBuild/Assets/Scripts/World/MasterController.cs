using UnityEngine;

public class MasterController : MonoBehaviour
{
    public FiniteStateMachine<MasterController> FSM = new FiniteStateMachine<MasterController>();
 
    public void Awake()
    {
        FSM.Init(this, new DoorController());
    }

    public void FixedUpdate()
    {
        FSM.Running();
    }
}
