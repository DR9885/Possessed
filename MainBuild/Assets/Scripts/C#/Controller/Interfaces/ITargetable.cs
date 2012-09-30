using UnityEngine;

public interface ITargetable
{
    GameObject GameObject { get; }
    Transform Transform { get; }
    Animation Animation { get; }
    Renderer TargetRenderer { get; }
    Collider TargetCollider { get;  }
    Highlight Highlight { get; }
    FSM<Door, DoorState> ActionFSM { get; set; }
    bool Locked { get; }
}