using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Possessed/Controlls/Master")]
public class MasterController : MonoBehaviour
{
    #region Fields

    private MonoBehaviour _target;
    private ITargetable Target
    {
        get { return _target as ITargetable; }
        set { _target = value as MonoBehaviour; }
    }

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

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Input.multiTouchEnabled = true;

    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            Debug.Log("ENTER");

        // Update Controller, with closest target
        var controller = Controllers
            .OrderBy(x => Vector3.Distance(x.Target.Transform.position, Transform.position))
            .FirstOrDefault();

        if(controller != null)
        {
            // Update Target
            if (Target != controller.Target)
            {
                if (Target == null)
                {
                    Target = controller.Target;
                    OnTarget(controller.Target);
                }
                else
                {
                    OnUntarget(Target);
                    _target = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (IController controller in Controllers)
            controller.OnDrawGizmos();
    }

    #endregion

    public void OnTarget(ITargetable target)
    {
        target.TargetState = TargetState.Target;
    }

    public void OnUntarget(ITargetable target)
    {
        target.TargetState = TargetState.Idle;
    }
}