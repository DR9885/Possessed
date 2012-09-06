using UnityEngine;

public interface ITargetable
{
    FSM<ITargetable, TargetState> TargetFSM { get; }
    TargetState TargetState { get; set; } 
    Renderer TargetRenderer { get; }
    Vector3 Position { get; }
}