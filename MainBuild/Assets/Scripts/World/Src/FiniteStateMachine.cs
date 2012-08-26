using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// 
/// Ref: http://www.playmedusa.com/blog/2010/12/10/a-finite-state-machine-in-c-for-unity3d/
/// </summary>
/// <typeparam name="T"></typeparam>
public class FiniteStateMachine<T> where T : class
{
    private T _owner = default(T);

    #region Fields

    private IFiniteState<T> _global = null;
    public  IFiniteState<T> Global
    {
        get { return _global; }
    }

    
    private IFiniteState<T> _current = null;
    public IFiniteState<T> Current
    {
        get { return _current; }
    }

    private Stack<IFiniteState<T>> _history = new Stack<IFiniteState<T>>();
    public IFiniteState<T> Previous
    {
        get { return _history.Peek(); }
    }

    #endregion

    public void Init(T owner, IFiniteState<T> initial, IFiniteState<T> global = null)
    {
        _owner = owner;
        _global = global;
        ChangeState(initial);
    }

    public void ChangeState(IFiniteState<T> state)
    {
        if (state != null)
        {
            _history.Push(_current);
            _current.Exit(_owner, state);
            state.Enter(_owner, _current);
            _current = state;
        }
    }

    public void UndoState()
    {
        if (_history.Count != 0)
        {
            var prev = _history.Pop() as IFiniteState<T>;
            _current.Exit(_owner, prev);
            prev.Enter(_owner, _current);
            _current = prev;
        }
    }

    public void Running()
    {
        if (_global != null) _global.Running(_owner);
        if (_current != null) _current.Running(_owner);
    }
}