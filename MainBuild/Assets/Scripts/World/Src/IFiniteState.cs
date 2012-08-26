using UnityEngine;
using System.Collections;

public interface IFiniteState<T>
{
    void Enter(T entity, IFiniteState<T> prevState);
    void Running(T entity);
    void Exit(T entity, IFiniteState<T> nextState);
}

