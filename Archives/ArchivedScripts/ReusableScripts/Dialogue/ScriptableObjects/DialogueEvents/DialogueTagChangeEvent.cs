using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTagChangeEvent : DialogueEvent
{
    [SerializeField]
    private List<Tag> _Tags = new List<Tag>();

    public override void Execute(Speaker[] speakers)
    {
        foreach (Speaker speaker in speakers)
            foreach (Tag tag in _Tags)
                speaker.Tags.RemoveAll(x => x.Key == tag.Key);

        foreach (Speaker speaker in speakers)
            foreach (Tag tag in _Tags)
                speaker.Tags.Add(tag);
    }
}