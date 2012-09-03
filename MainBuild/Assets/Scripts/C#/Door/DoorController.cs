using System.Linq;
using UnityEngine;
using System.Collections;

public interface ITarget
{
    Transform Transform { get; }
    Vector3 Position { get; }
}

[AddComponentMenu("Possessed/Controlls/Door")]
public class DoorController : MonoBehaviour
{
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
        get
        {
            return Transform.position + _offsetPosition;
        }
    }


    public Door Target;
    private void Update()
    {
        Target = GetTatget();
    }

    private void OnGUI()
    {
       
    }

    private void OnDrawGizmos()
    {
        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Position, Target.Position);
        }

        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(Position, Transform.rotation, Transform.lossyScale);
        Gizmos.DrawFrustum(Vector3.zero, Degrees * 2, Distance, 0, 1);
    }

    public int Distance = 10;
    public float Degrees = 45;

    public Door GetTatget()
    {
        Door doorTargeted = null;
        var doors = FindObjectsOfType(typeof (Door)).Select(x => x as Door);
        foreach (var door in doors)
        {
            var distance = Vector3.Distance(Position, door.Position);
            //Debug.Log(Vector3.Distance(transform.position, door.Transform.position));
            if (Distance > distance)
            {
                if (IsInFOV(door))
                {
                    if(doorTargeted == null ||
                        distance < Vector3.Distance(Position, doorTargeted.Position))
                    doorTargeted = door;
                }
            }
        }

        return doorTargeted;
    }

    public bool IsInFOV(Door Target)
    {
        var heading = Vector3.Normalize(Target.Position - Position);
        var dot = Vector3.Dot(transform.forward, heading);
        var degrees = Mathf.Rad2Deg * Mathf.Acos(dot);
        Debug.Log(degrees);
        return degrees < Degrees;
    }
}
