using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour, ITargetable
{
    #region Fields

    private GameObject _gameObject;
    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = gameObject;
            return _gameObject;
        }
    }

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
                _targetRenderer = Transform.GetComponentInChildren<SkinnedMeshRenderer>();
            return _targetRenderer;
        }
    }

    public FSM<Door, DoorState> ActionFSM { get; set; }
    [SerializeField] private DoorState _actionState;
    public DoorState ActionState
    {
        get { return _actionState; }
        set
        {
            if (_actionState != value)
                ActionFSM.ChangeState(value);
            _actionState = value;
        }
    }
    
    [SerializeField] private bool _locked;
    public bool Locked
    {
        get { return _locked; }
        set { _locked = value; }
    }

    #endregion
}