using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Objects/Item")]
public class Item : AbstractEntity
{
    [SerializeField] private DialogueEvent[] _DialoguePickupEvents;
    [SerializeField] private DialogueEvent[] _DialogueDropEvents;

    private Rigidbody _Rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (_Rigidbody == null)
                _Rigidbody = GameObject.RequireComponentAdd<Rigidbody>();
            return _Rigidbody;
        }
    }

    private Collider _Collider;
    public Collider Collider
    {
        get
        {
            if (_Collider == null)
                _Collider = GameObject.RequireComponentAdd<BoxCollider>();
            return _Collider;
        }
    }

    new public void Awake()
    {
        base.Awake();

        Collider.isTrigger = false;
        Rigidbody.useGravity = true;
        Rigidbody.isKinematic = false;

        OnActivate += Pickup;
        OnDeactivate += Drop;
    }

    public IEnumerator Pickup(AbstractEntity argEntity)
    {
        foreach (DialogueEvent dialogueEvent in _DialoguePickupEvents)
        {
            //debug.log(argEntity.name);
            if (dialogueEvent) dialogueEvent.Execute(argEntity as Speaker);
        }

        ////debug.log("PICKUP");
        Collider.isTrigger = true;
        Rigidbody.useGravity = false;
        Rigidbody.isKinematic = true;
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator Drop(AbstractEntity argEntity)
    {
        foreach (DialogueEvent dialogueEvent in _DialogueDropEvents)
            if(dialogueEvent) dialogueEvent.Execute();

        ////debug.log("DROP");
        Collider.isTrigger = false;
        Rigidbody.useGravity = true;
        Rigidbody.isKinematic = false;
        yield return new WaitForFixedUpdate();
    }
}