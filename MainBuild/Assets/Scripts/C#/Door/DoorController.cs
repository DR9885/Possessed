using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController
{
    #region Fields
    
    [SerializeField] private bool _isGhost;
    public bool IsGhost { get { return _isGhost; } }

    [SerializeField] private float _distance = 4;
    public float Distance { get { return _distance; } }

    [SerializeField] private float _angle = 45;
    public float Angle { get { return _angle; } }

    [SerializeField] private bool _showFOV;
    public bool ShowFOV
    {
        get { return _showFOV; }
    }

    private IEnumerable<Door> _doors;
    public IEnumerable<Door> Doors
    {
        get
        {
            if (_doors == null)
                _doors = FindObjectsOfType(typeof(Door)).Select(x => x as Door);
            return _doors;
        }
    }

    public IEnumerable<ITargetable> Targets
    {
        get { return Doors.Where(x => x.ActionState == DoorState.Idle).Select(x => x as ITargetable); }
    }
    
    [SerializeField] private  Door _door;
    public ITargetable Target
    {
        get { return _door; }
        set { _door = value as Door; }
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

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
    }

    private void OnGUI()
    {
        
    }

    private void OnDrawGizmos()
    {
        this.GizmosFOV();
        this.GizmosTarget();
    }

    #endregion

    public void Open(Door door)
    {

    }

    public void Walkthrough(Door door)
    {
        if (door != null)
            door.WalkThrough(this);
    }

    public void CloseDoor(Door door)
    {
        if (_door != null)
            door.Close();
    }


    #region MasterController Messages

    private void OnEnter()
    {
        if (IsGhost)
            foreach (Door door in Doors)
                door.WalkThrough(this);
        else if (_door != null)
            _door.Open(this);
    }

    private void OnExit()
    {

    }

    #endregion
}
