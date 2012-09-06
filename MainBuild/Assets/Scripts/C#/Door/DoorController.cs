using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController
{
    #region Fields

    [SerializeField] private int _Distance = 10;
    public float Distance { get { return _Distance; } }

    [SerializeField] private float _Angle = 45;
    public float Angle { get { return _Angle; } }

    [SerializeField] private DebugControllerSettings _debug = new DebugControllerSettings();
    public DebugControllerSettings Debug { get { return _debug; } }

    [SerializeField] private  ITargetable _Target;
    public ITargetable Target { get { return _Target; } }

    private Transform _transform;
    private Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }

    public Vector3 Position
    {
        get { return Transform.position + new Vector3(0.0f, 0.6f, 0.0f); }
    }

    #endregion

    #region Unity Methods

    private void FixedUpdate()
    {
        _Target = GetTarget();
    }

    private void OnGUI()
    {
        if(Target != null)
        {
            float width = 100.0f, height = 100.0f;

            if(GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f, 
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Open Door"))
                (Target as Door).ActionState = DoorState.Open;
        }
    }

    #endregion

    private bool IsInFOV(Door target)
    {
        var heading = Vector3.Normalize(target.Position - Position);
        var dot = Vector3.Dot(transform.forward, heading);
        var degrees = Mathf.Rad2Deg * Mathf.Acos(dot);
        return degrees < _Angle;
    }

    private ITargetable GetTarget()
    {
        ITargetable doorTargeted = null;
        var doors = FindObjectsOfType(typeof(Door)).Select(x => x as Door);
        foreach (var door in doors)
        {
            var distance = Vector3.Distance(Position, door.Position);
            //Debug.Log(Vector3.Distance(transform.position, door.Transform.position));
            if (Distance > distance)
            {
                if (IsInFOV(door))
                {
                    if (doorTargeted == null ||
                        distance < Vector3.Distance(Position, doorTargeted.Position))
                        doorTargeted = door;
                }
            }
        }

        return doorTargeted;
    }
}
