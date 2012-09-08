using UnityEngine;

public interface IController
{
    Transform Transform { get; }
    ITargetable Target { get; set; }
    float Distance { get; }
    float Angle { get; }
    DebugControllerSettings DebugSettings { get; }
}