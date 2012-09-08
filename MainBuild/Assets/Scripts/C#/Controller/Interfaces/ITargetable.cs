using UnityEngine;

public interface ITargetable
{
    Transform Transform { get; }
    FSM<ITargetable, TargetState> TargetFSM { get; }
    TargetState TargetState { get; set; } 
    Renderer TargetRenderer { get; }
}