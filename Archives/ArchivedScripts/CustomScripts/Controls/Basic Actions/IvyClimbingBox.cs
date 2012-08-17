using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Objects/IvyClimbingBox")]
[RequireComponent(typeof(BoxCollider))]
public class IvyClimbingBox : AbstractEntity 
{


    new public void Awake()
    {
        Debug.Log("ivyBoxAwaken"); 
        base.Awake();
        active = true; 
        OnActivate += StartClimbing;
        OnDeactivate += StopClimbing;
    }

    protected IEnumerator StartClimbing(AbstractEntity argEntity)
    {
        Debug.Log("START CLIMBING DEBUG LOG");
        yield return new WaitForFixedUpdate();
    }

    protected IEnumerator StopClimbing(AbstractEntity argEntity)
    {

        Debug.Log("STop CLIMBING DEBUG LOG");
        yield return new WaitForFixedUpdate();
    }
    
}

