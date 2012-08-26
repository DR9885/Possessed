using UnityEngine;
using System.Collections;

public class FSMController : MonoBehaviour
{
    private readonly FiniteStateMachine<FSMController> FSM = new FiniteStateMachine<FSMController>();

    public IFiniteState<FSMController> State
    {
        set { FSM.ChangeState(value); }
    }

    public void Undo()
    {
        FSM.UndoState();
    }

    #region Unity Methods

    private void Awake()
    {
        FSM.Init(this , null);
    }

    private void FixedUpdate()
    {
        FSM.Running();
    }

    #endregion
}
