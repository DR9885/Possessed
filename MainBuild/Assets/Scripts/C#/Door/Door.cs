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
public class Door : MonoBehaviour, ITargetable
{
    #region Fields

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

    private SkinnedMeshRenderer _targetRenderer;
    public Renderer TargetRenderer
    {
        get
        {
            if (_targetRenderer == null)
                _targetRenderer = GameObject.Find("joint1").GetComponent<SkinnedMeshRenderer>();
            return _targetRenderer;
        }
    }

    private Vector3 _offsetPosition = new Vector3(0f, 0.6f, 0f);
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

    #endregion

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
