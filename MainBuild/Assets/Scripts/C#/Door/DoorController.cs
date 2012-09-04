using System.Linq;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour, IController
{
    #region Fields

    public int _Distance = 10;
    public float Distance { get { return _Distance; } }

    public float _Angle = 45;
    public float Angle { get { return _Angle; } }

    public ITargetable _Target;
    public ITargetable Target
    {
        get { return _Target; }
        set { _Target = value; }
    }

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

    private Vector3 _offsetPosition = new Vector3(0.0f, 0.6f, 0.0f);
    public Vector3 Position
    {
        get { return Transform.position + _offsetPosition; }
    }

    private Material _hoverMaterial;
    private Material _originalMaterial;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _hoverMaterial = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
        _hoverMaterial.SetColor("_Color", Color.white);
        _hoverMaterial.SetColor("_OutlineColor", Color.blue);
    }

    private void FixedUpdate()
    {
        Target = GetTarget();
    }

    private void OnGUI()
    {
        if(Target != null)
        {
            float width = 100.0f, height = 100.0f;

            GUI.Button(new Rect(Screen.width / 2.0f - width / 2.0f, 
                Screen.height / 2.0f - height / 2.0f, 100, 100),
                "Open Door");

        }
    }

    #endregion

    public bool IsInFOV(Door Target)
    {
        var heading = Vector3.Normalize(Target.Position - Position);
        var dot = Vector3.Dot(transform.forward, heading);
        var degrees = Mathf.Rad2Deg * Mathf.Acos(dot);
        return degrees < _Angle;
    }

    public ITargetable GetTarget()
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
