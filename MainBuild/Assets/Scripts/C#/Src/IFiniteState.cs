using UnityEngine;
using System.Collections;

public interface IFiniteState<T>
{
    void OnEnter(T entity);
    void OnDecision(T entity);
    void OnExit(T entity);
}

