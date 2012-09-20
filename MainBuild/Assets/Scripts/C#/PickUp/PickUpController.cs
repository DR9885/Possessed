using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour, IController
{

    #region Fields

    [SerializeField]
    private int _Distance = 10;
    public float Distance { get { return _Distance; } }

    [SerializeField]
    private float _Angle = 45;
    public float Angle { get { return _Angle; } }

    public bool ShowFOV
    {
        get { throw new System.NotImplementedException(); }
    }

    [SerializeField]
    private PickUp _Door;
    public IEnumerable<ITargetable> Targets
    {
        get
        {
            return Object.FindObjectsOfType(typeof(PickUp)).Select(x => x as PickUp)
                .Where(x => x.ActionState == DoorState.Idle).Select(x => x as ITargetable);
        }
    }

    public ITargetable Target
    {
        get { return _Door; }
        set { _Door = value as PickUp; }
    }

    private Transform _transform;
    public Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }

    public FSM<IController, TargetState> TargetFSM { get; set; }
    [SerializeField]
    private TargetState _targetState;
    public TargetState TargetState
    {
        get { return _targetState; }
        set
        {
            if (_targetState != value)
                TargetFSM.ChangeState(value);
            _targetState = value;
        }
    }

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    #endregion
}
