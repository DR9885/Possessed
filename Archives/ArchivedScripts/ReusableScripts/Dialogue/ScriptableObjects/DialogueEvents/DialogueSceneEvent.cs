using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneEvent : DialogueEvent
{
    [SerializeField]
    private String sceneName = null;

    public override void Execute(params Speaker[] speakers)
    {
        Application.LoadLevel(sceneName);
    }
}