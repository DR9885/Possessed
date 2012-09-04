
using UnityEngine;
public interface ITargetable
{
    Renderer TargetRenderer { get; }
    Vector3 Position { get; }
}