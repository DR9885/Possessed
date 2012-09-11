using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    // Settings
    float Distance { get; }
    float Angle { get; }
    DebugControllerSettings DebugSettings { get; }

    // Objects
    Transform Transform { get; }
    FSM<IController, TargetState> TargetFSM { get; }
    TargetState TargetState { get; set; }
    IEnumerable<ITargetable> Targets { get; }
    ITargetable Target { get; set; }
    bool Enabled { get; set; }
}