using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    // Settings
    float Distance { get; }
    float Angle { get; }
    bool ShowFOV { get; }

    // Objects
    Transform Transform { get; }
    IEnumerable<ITargetable> Targets { get; }
    ITargetable Target { get; set; }
    bool Enabled { get; set; }
}