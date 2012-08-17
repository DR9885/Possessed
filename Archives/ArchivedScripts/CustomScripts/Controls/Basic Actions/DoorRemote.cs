
using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("Possessed/Remotes/DoorRemote")]
public class DoorRemote : AbstractRemote
{
    #region Field(s)

    //[SerializeField] private bool _IsTimedDoor = true;
    //[SerializeField] private float _DoorCloseDelay = 3f;
    private Door _MarkedDoor = null;

    #endregion

    #region Unity Methods

    new public void Reset()
    {
        base.Reset();
        _ContextAction = ContextActions.DoorOpen;
    }

    new public void Awake()
    {
        base.Awake();

        _StartLabel = "Open";
        _EndLabel = "";

        // Set Input
        _ContextAction = ContextActions.DoorOpen;

        // Set Events
        TargetCheck += FindClosestDoor;
        PowerOn += OpenDoor;
        PowerOff += CloseDoor;
    }

    new public void OnDisable()
    {
        base.OnDisable();
        HighlightGUI.Target = null;
        HighlightGUI.Color = Color.clear;
    }

    new public void OnEnable()
    {
        base.OnEnable();
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

    private bool FindClosestDoor(AbstractEntity argEntity)
    {
        Door door = argEntity as Door;
        return door != null && !door.Active && door.CanOpen(GetComponent<Speaker>());
    }

    protected IEnumerator OpenDoor(AbstractEntity argEntity)
    {
        // Activate Door
        StartCoroutine(argEntity.Activate(Speaker));

        // Target No Longet Needed
        Target = null;

        //// Wait Till Out of View
        //while (FindTarget() == argEntity)
        //    yield return new WaitForFixedUpdate();

        StartCoroutine(PowerOff(argEntity));
        yield return new WaitForFixedUpdate();
    }

    protected IEnumerator CloseDoor(AbstractEntity argEntity)
    {
        //// Wait on Delay
        //if (_IsTimedDoor)
        //    yield return new WaitForSeconds(_DoorCloseDelay);

        StartCoroutine(argEntity.Deactivate(Speaker));
        yield return new WaitForFixedUpdate();
    }

    #endregion
}