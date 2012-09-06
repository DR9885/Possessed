using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ControllerState
{
    None,
    Door,
    PickUp,
    Possession
}

public enum TargetState
{
    Idle,
    Target,
    Active
}

[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    #region Fields

    private Material _hoverMaterial;
    private Material _originalMaterial;

    private ITargetable _target;
    private IEnumerable<IController> _controllers;
    private IEnumerable<IController> Controllers
    {
        get
        {
            if (_controllers == null)
                _controllers = GetComponents<MonoBehaviour>().OfType<IController>();
            return _controllers;
        }
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

    public Vector3 Position
    {
        get { return Transform.position + new Vector3(0.0f, 0.6f, 0.0f); }
    }

    #endregion

    #region Unity Methods

    private void FixedUpdate()
    {
        // Update Controller, with closest target
        var controller = Controllers
            .OrderBy(x => Vector3.Distance(x.Target.Position, Position))
            .FirstOrDefault();


        // Update Target
        if (controller != null && _target != controller.Target)
        {
            if (_target == null)
            {
                _target = controller.Target;
                //OnTarget(controller.Target);
            }
            else
            {
                //OnUntarget(_target);
                _target = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var controller in Controllers)
        {
            if (controller.Debug.Active == false) continue;

            // Draw Target Line
            if (controller.Target != null)
            {
                Gizmos.color = controller.Debug.TargetColor;
                Gizmos.DrawLine(Position, controller.Target.Position);
            }

            // Draw View Frustum
            Gizmos.color = controller.Debug.ViewColor;
            Gizmos.matrix = Matrix4x4.TRS(Position, Transform.rotation, Transform.lossyScale);
            Gizmos.DrawFrustum(Vector3.zero, controller.Angle * 2, controller.Distance, 0, 1);
        }
    }

    #endregion

    //public void OnTarget(ITargetable target)
    //{
    //    target.TargetState = TargetState.Target;
    //}

    //public void OnUntarget(ITargetable target)
    //{
    //    target.TargetState = TargetState.Idle;
    //}

}