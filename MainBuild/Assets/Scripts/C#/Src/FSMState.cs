using UnityEngine;
using System.Collections;

public abstract class FSMState<T, E>
{
	protected T entity;

	public abstract E State { get; }
    public abstract void Enter(T entity);
    public abstract void Execute(T entity);
    public abstract void Exit(T entity);
}

