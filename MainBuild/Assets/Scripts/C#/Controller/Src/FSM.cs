using System.Collections.Generic;
using System.Linq;
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

    private IFSMState<T, E> _globalState;
    public E GlobalState
    {
        get { return _globalState.State; }
    }

    private IFSMState<T, E> _currentState;
    public E CurrentState
    {
        get { return _currentState.State; }
    }

    private IFSMState<T, E> _previousState;
    public E PreviousState
    {
        get { return _previousState.State; }
    }

    #endregion

    public FSM(T owner)
    {
        StateHistory = new Stack<IFSMState<T, E>>();
        StateRegistry = new Dictionary<E, IFSMState<T, E>>();
        Owner = owner;
    }

    public void Update()
    {
        if (_globalState != null) _globalState.Execute(Owner);
        if (_currentState != null) _currentState.Execute(Owner);
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
        _previousState = _currentState;
        _currentState = state;
        StateHistory.Push(state);

        if (_previousState != null) _previousState.Exit(Owner);
        if (_currentState != null) _currentState.Enter(Owner);
    }

    public void ChangeState(E state)
    {
        ChangeState(StateRegistry[state]);
    }

	public void UndoState() {
        _previousState = _currentState;
        _currentState = StateHistory.Pop();

        if (_previousState != null) _previousState.Exit(Owner);
        if (_currentState != null) _currentState.Enter(Owner);
	}
}