using UnityEngine;

public interface ITargetable
{
    Transform Transform { get; }
    Renderer TargetRenderer { get; }
    FSM<Door, DoorState> ActionFSM { get; set; }
}