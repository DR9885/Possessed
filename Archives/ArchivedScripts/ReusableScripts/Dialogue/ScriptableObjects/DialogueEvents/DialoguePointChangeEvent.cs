
using UnityEngine;

public class DialoguePointChangeEvent : DialogueEvent
{
    [SerializeField] Vector3 _Point;
    [SerializeField] Vector3 _Rotation;

    public override void Execute(params Speaker[] speakers)
    {
        foreach (Speaker speaker in speakers)
        {
            speaker.Transform.position = _Point;
            speaker.Transform.rotation = Quaternion.Euler(_Rotation);
        }
    }
}