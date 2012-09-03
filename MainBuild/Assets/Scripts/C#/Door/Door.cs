using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public enum DoorState
{
    Idle,
    Open,
    Close,
}

[AddComponentMenu("Possessed/Objects/Door")]
public class Door : MonoBehaviour
{
    private Animation _animation;
    public Animation Animation
    {
        get
        {
            if (_animation == null)
                _animation = GetComponent<Animation>();
            return _animation;
        }
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

    private Vector3 _offsetPosition = new Vector3(.6f, 0.6f, 0);
    public Vector3 Position
    {
        get
        {
            return Transform.position + _offsetPosition;
        }
    }

    public FSM<Door, DoorState> FSM;


    public bool CanClose;
    public DoorState State;


    #region Unity Methods

    private void Awake()
    {
        FSM = new FSM<Door, DoorState>(this);
        FSM.RegisterState(new DoorIdleState());
        FSM.RegisterState(new DoorOpenState());
        FSM.RegisterState(new DoorCloseState());
    }

    private void FixedUpdate()
    {
        FSM.Update(State);
    }

    #endregion
}
