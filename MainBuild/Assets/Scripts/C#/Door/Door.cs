using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public enum DoorState
{
    Idle,
    Hover,
    Open,
    Close,
}

public class Door : MonoBehaviour
{
    public static Dictionary<DoorState, IFiniteState<Door>> FiniteStates =
        new Dictionary<DoorState, IFiniteState<Door>>()
            {
                { DoorState.Idle, new DoorIdleState() },
                { DoorState.Hover, new DoorHoverState() },
                { DoorState.Open, new DoorOpenState() },
                { DoorState.Close, new DoorCloseState() }
            };

    private Animation _animation;
    public Animation Animation
    {
        get
        {
            if(_animation == null)
            {
                _animation = GetComponent<Animation>();
                _animation.playAutomatically = false;
            }
            return _animation;
        }
    }

    public FiniteStateMachine<Door> FSM = new FiniteStateMachine<Door>();


    public bool CanClose;
    public DoorState State;


    #region Unity Methods

    private void Awake()
    {
        FSM.Init(this);
    }

    private void FixedUpdate()
    {
        if (FSM.Current != FiniteStates[State])
            FSM.ChangeState(FiniteStates[State]);

        FSM.OnDecision();
    }

    #endregion
}
