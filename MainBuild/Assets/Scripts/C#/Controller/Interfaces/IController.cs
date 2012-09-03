using UnityEngine;

public interface IController
{
    ITargetable Target { get; }
    float Distance { get; }
    float Angle { get; }
}