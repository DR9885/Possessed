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
public class FiniteStateMachine<T> where T : class
{
    #region Fields

    private T _owner = default(T);

    private IFiniteState<T> _global = null;
    public IFiniteState<T> Global { get { return _global; } }
    
    private IFiniteState<T> _current = null;
    public IFiniteState<T> Current { get { return _current; } } 

    private Stack<IFiniteState<T>> _history = new Stack<IFiniteState<T>>();
    public IFiniteState<T> Previous { get { return _history.Peek(); } }

    #endregion

    public void Init(T owner, IFiniteState<T> global = null)
    {
        _owner = owner;
        _global = global;
    }

    public void ChangeState(IFiniteState<T> state)
    {
        if (state != null)
        {
            if (_current != null)
            {
                _history.Push(_current);
                _current.OnExit(_owner);
            }

            state.OnEnter(_owner);
            _current = state;
        }
    }

    public void UndoState()
    {
        if (_history.Count != 0)
        {
            var prev = _history.Pop();
            _current.OnExit(_owner);
            prev.OnEnter(_owner);
            _current = prev;
        }
    }

    public void OnDecision()
    {
        if (_global != null) _global.OnDecision(_owner);
        if (_current != null) _current.OnDecision(_owner);
    }
}