using UnityEngine;
using System.Collections;

public class DialogueAnimationEvent : DialogueEvent
{
    [SerializeField] private string _ClipName = "";

    public override void Execute(params Speaker[] speakers)
    {
        if (_ClipName != "")
            foreach (Speaker speaker in speakers)
            {
                var source = speaker.GameObject.GetComponent<Animation>();
                if (source && source[_ClipName] != null)
                    source.CrossFade(_ClipName);

                if(_ClipName != AnimationResources.Death)
                    speaker.StartCoroutine(QueueIdle(source));
            }
    }

    private IEnumerator QueueIdle(Animation source)
    {
        while (source.IsPlaying(_ClipName))
            yield return new WaitForFixedUpdate();
        source.CrossFade(AnimationResources.Idle);
    }
}