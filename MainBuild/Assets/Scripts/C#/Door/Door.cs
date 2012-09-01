using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public enum DoorState
{
    Idle,
    Open,
    Close,
}

public class Door : MonoBehaviour
{
    private Animation _animation;
    public Animation Animation
    {
        get
        {
            if(_animation == null)
                _animation = GetComponent<Animation>();
            return _animation;
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
