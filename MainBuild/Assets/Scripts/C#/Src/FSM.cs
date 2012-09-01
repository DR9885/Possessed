using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 
/// 
/// Ref: http://www.playmedusa.com/blog/2010/12/10/a-finite-state-machine-in-c-for-unity3d/
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class FSM<T, E> where T : class
{
    #region Fields

    private T Owner { get; set; }
    private Stack<FSMState<T, E>> StateHistory { get; set; }
    private Dictionary<E, FSMState<T, E>> StateRegistry { get; set; }

    public FSMState<T, E> GlobalState { get; set; }
    public FSMState<T, E> CurrentState { get; set; }
    public FSMState<T, E> PreviousState { get; set; }
	
    #endregion

    public FSM(T owner)
    {
        StateHistory = new Stack<FSMState<T, E>>();
        StateRegistry = new Dictionary<E, FSMState<T, E>>();
        Owner = owner;
    }

    public void Update(FSMState<T, E> state)
    {
        if (state != CurrentState) ChangeState(state);
        if (GlobalState != null) GlobalState.Execute(Owner);
        if (CurrentState != null) CurrentState.Execute(Owner);
    }

    public void Update(E state)
    {
        Update(StateRegistry[state]);
    }

    public void RegisterState(FSMState<T, E> state)
    {
        StateRegistry.Add(state.State, state);
    }

    public void UnregisterState(FSMState<T, E> state)
    {
        StateRegistry.Remove(state.State);
    }

    public void ChangeState(FSMState<T, E> state)
    {
        PreviousState = CurrentState;
        CurrentState = state;
        StateHistory.Push(state);

        if (PreviousState != null) PreviousState.Exit(Owner);
        if (CurrentState != null) CurrentState.Enter(Owner);
    }

    public void ChangeState(E state)
    {
        ChangeState(StateRegistry[state]);
    }

	public void UndoState() {
        PreviousState = CurrentState;
        CurrentState = StateHistory.Pop();

        if (PreviousState != null) PreviousState.Exit(Owner);
        if (CurrentState != null) CurrentState.Enter(Owner);
	}
}