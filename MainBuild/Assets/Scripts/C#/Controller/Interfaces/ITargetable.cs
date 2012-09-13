using UnityEngine;

public interface ITargetable
{
    Animation Animation { get; }
    Transform Transform { get; }
    Renderer TargetRenderer { get; }
    FSM<Door, DoorState> ActionFSM { get; set; }
}