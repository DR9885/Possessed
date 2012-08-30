using UnityEngine;
using System.Collections;

public class CustomFiniteStateMachine : MonoBehaviour {

    FiniteStateMachine<MonoBehaviour> FSM = new FiniteStateMachine<MonoBehaviour>();

    public void OnChangeState(IFiniteState<MonoBehaviour> state)
    {
        FSM.ChangeState(state);
    }

    public void OnPreviousState() {
        FSM.UndoState();
    }
}
