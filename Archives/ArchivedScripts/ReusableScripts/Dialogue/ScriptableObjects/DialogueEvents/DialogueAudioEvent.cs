using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudioEvent : DialogueEvent
{
    [SerializeField] private AudioClip _Clip = null;

    public override void Execute(params Speaker[] speakers)
    {
        if(_Clip != null)
            foreach (Speaker speaker in speakers)
            {
                var source = speaker.GameObject.RequireComponentAdd<AudioSource>();
                source.clip = _Clip;
                source.Play();
            }
    }
}