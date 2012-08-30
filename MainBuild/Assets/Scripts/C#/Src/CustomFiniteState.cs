using UnityEngine;
using System.Collections;

public abstract class CustomFiniteState : ScriptableObject, IFiniteState<MonoBehaviour>
{
    public abstract void OnEnter(MonoBehaviour target);
    public abstract void OnExit(MonoBehaviour target);
    public abstract void OnDecision(MonoBehaviour target);
}
