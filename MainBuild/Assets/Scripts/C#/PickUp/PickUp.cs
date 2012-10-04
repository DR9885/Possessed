using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;



[AddComponentMenu("Possessed/Objects/PickUp")]
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

    private BoxCollider _boxCollider;
    public Collider TargetCollider
    {
        get
        {
            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider>();
            if (_boxCollider == null)
            {
                _boxCollider = GameObject.AddComponent<BoxCollider>();
                _boxCollider.isTrigger = true;
                _boxCollider.center = new Vector3(0, 0.84f, 0.58f);
                _boxCollider.size = new Vector3(1.46f, 1.7f, 1.35f);
            }
            return _boxCollider;
        }
    }

    private Renderer _targetRenderer;
    public Renderer TargetRenderer
    {
        get
        {
            if (_targetRenderer == null)
                _targetRenderer = Transform.GetComponentInChildren<Renderer>();
            if (_targetRenderer == null)
                _targetRenderer = Transform.GetComponent<Renderer>();
            return _targetRenderer;
        }
    }
    
    private Highlight _highlight;
    public Highlight Highlight
    {
        get
        {
            if (_highlight == null)
                _highlight = GetComponent<Highlight>();
            if (_highlight == null)
                _highlight = GameObject.AddComponent<Highlight>();
            return _highlight;
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