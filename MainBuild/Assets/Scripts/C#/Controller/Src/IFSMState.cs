using UnityEngine;
using System.Collections;

public interface IFSMState<T, E>
{
	E State { get; }
    void Enter(T entity);
    void Execute(T entity);
    void Exit(T entity);
}

