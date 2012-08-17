using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Objects/Door")]
public class Door : AbstractEntity
{
    [SerializeField] private TagFilter _Filter = null;
    [SerializeField] private float _DoorCloseDelay = 3f;

    new public void Awake()
    {
        base.Awake();
        OnActivate += Open;
        OnDeactivate += Close;
    }

    public bool CanOpen(Speaker argSpeaker)
    {
        if(_Filter == null) return true;
        if(argSpeaker == null) return true;
        bool canDo = _Filter.Accepted(argSpeaker.Tags);
        if (!canDo)
        {
            HighlightGUI.Color = Color.red;
            if (HighlightGUI.Target == null)
                HighlightGUI.Target = this;
        }
        return (canDo);
    }

    public IEnumerator Open(AbstractEntity argEntity)
    {
        //debug.log(argEntity.name);
        ////debug.log("OPEN: " + name);
        Transform.parent.animation.Play("Open");

        while (Transform.parent.animation.IsPlaying("Open"))
            yield return new WaitForFixedUpdate();
    }

    public IEnumerator Close(AbstractEntity argEntity)
    {
        yield return new WaitForSeconds(_DoorCloseDelay);

        while (Transform.parent.animation.IsPlaying("Open"))
            yield return new WaitForFixedUpdate();

        ////debug.log("CLOSE: " + name);
        Transform.parent.animation.Play("Close");

        while (Transform.parent.animation.IsPlaying("Close"))
            yield return new WaitForFixedUpdate();
    }

    public void PhaseOut()
    {
        collider.isTrigger = true;
    }

    public void PhaseIn()
    {
        collider.isTrigger = false;
    }
}