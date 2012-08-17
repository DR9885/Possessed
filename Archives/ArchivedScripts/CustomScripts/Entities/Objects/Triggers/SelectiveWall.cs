using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SelectiveWall : MonoBehaviour
{
    [ SerializeField ] private TagFilter _Filter = null;


    private BoxCollider _Collider;
    private BoxCollider Collider
    {
        get
        {
            if (_Collider == null)
                _Collider = collider as BoxCollider;
            return _Collider;
        }
    }


    private bool CanWalkThrough(Speaker speaker)
    {
        if(speaker == null || _Filter == null) return true;
        return !_Filter.Accepted(speaker.Tags);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CanWalkThrough(collision.gameObject.GetComponent<Speaker>()))
            PhaseOut();
        else
            PhaseIn();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (CanWalkThrough(collision.gameObject.GetComponent<Speaker>()))
            PhaseOut();
        else
            PhaseIn();
    }

    private void OnCollisionExit(Collision collision)
    {
        PhaseIn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CanWalkThrough(other.gameObject.GetComponent<Speaker>()))
            PhaseOut();
        else
            PhaseIn();
    }

    private void OnTriggerStay(Collider other)
    {
        if (CanWalkThrough(other.gameObject.GetComponent<Speaker>()))
            PhaseOut();
        else
            PhaseIn();
    }

    private void OnTriggerExit(Collider other)
    {
        PhaseIn();
    }


    private void PhaseOut()
    {

        Collider.isTrigger = true;
    }

    private void PhaseIn()
    {
        Collider.isTrigger = false;
    }
}