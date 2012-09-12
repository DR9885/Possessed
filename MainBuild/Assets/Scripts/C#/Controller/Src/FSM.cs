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
    private Stack<IFSMState<T, E>> StateHistory { get; set; }
    private Dictionary<E, IFSMState<T, E>> StateRegistry { get; set; }

    public IFSMState<T, E> GlobalState { get; set; }
    public IFSMState<T, E> CurrentState { get; set; }
    public IFSMState<T, E> PreviousState { get; set; }
	
    #endregion

    public FSM(T owner)
    {
        StateHistory = new Stack<IFSMState<T, E>>();
        StateRegistry = new Dictionary<E, IFSMState<T, E>>();
        Owner = owner;
    }

    public void Update(IFSMState<T, E> state)
    {
        if (state != CurrentState) ChangeState(state);
        if (GlobalState != null) GlobalState.Execute(Owner);
        if (CurrentState != null) CurrentState.Execute(Owner);
    }

    public void Update(E state)
    {
        Update(StateRegistry.ContainsKey(state)? StateRegistry[state] : null);
    }

    public void RegisterState(IFSMState<T, E> state)
    {
        StateRegistry.Add(state.State, state);
    }

    public void UnregisterState(IFSMState<T, E> state)
    {
        StateRegistry.Remove(state.State);
    }

    public void ChangeState(IFSMState<T, E> state)
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