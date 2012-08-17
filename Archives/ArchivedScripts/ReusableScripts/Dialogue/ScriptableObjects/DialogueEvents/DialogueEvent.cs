using System;
using UnityEngine;
using System.Linq;

public abstract class DialogueEvent : ScriptableObject
{
    [SerializeField]
    private TagFilter _Target;
    public TagFilter Target
    {
        get { return _Target; }
    }

    public void Execute()
    {
        if (Target != null)
        {
            Execute(FindObjectsOfType(typeof(Speaker)).Cast<Speaker>().Where(speaker => 
                Target.Accepted(speaker.Tags)).ToArray());
        }
    }

    public abstract void Execute(params Speaker[] speakers);
}

