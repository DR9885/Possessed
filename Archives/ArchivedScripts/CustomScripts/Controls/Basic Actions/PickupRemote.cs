
using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("Possessed/Remotes/PickupRemote")]
public class PickupRemote : AbstractRemote
{
    #region Unity Methods

    new public void Reset()
    {
        base.Reset();

        /// Set Input
        _ContextAction = ContextActions.Pickup;
    }
    public void OnDisable()
    {
        base.OnDisable();
    }
    new public void OnEnable()
    {
        base.OnEnable();
    }
    new public void Awake()
    {
        base.Awake();

        /// Set Input
        _ContextAction = ContextActions.Pickup;

        /// Set Labels
        _StartLabel = "Pickup";
        _EndLabel = "Drop";

        /// Set Events
        TargetCheck += FindClosestItem;
        PowerOn += PickupItem;
        PowerOff += DropItem;
    }

    #endregion

    private Speaker _Speaker;
    private Speaker Speaker
    {
        get
        {
            if (_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }

    #region Helper Methods

    private bool FindClosestItem(AbstractEntity argEntity)
    {
        return argEntity.GetType() == typeof(Item);
    }

    Transform CarryTransform { get; set; }

    protected IEnumerator PickupItem(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Activate(Speaker));

        CarryTransform = Actor.GetComponent<AbstractAnimal>() != null ? Actor.Mouth : Actor.LeftHand;
        argEntity.Transform.parent = CarryTransform;
        argEntity.Transform.position = CarryTransform.position;

        argEntity.rigidbody.useGravity = false;
        argEntity.rigidbody.isKinematic = true;
        argEntity.collider.isTrigger = true;
        yield return new WaitForSeconds(1);
    }

    protected IEnumerator DropItem(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Deactivate(Speaker));

        //Vision.objectsInRadius.Remove(argEntity.GameObject);
        //_StoredTarget.Drop();
        argEntity.rigidbody.useGravity = true;
        argEntity.rigidbody.isKinematic = false;
        argEntity.collider.isTrigger = false;

        argEntity.Transform.parent = null;
        argEntity.Transform.position += Vector3.up * .25f;
        
        yield return new WaitForSeconds(1);
    }

    #endregion
}