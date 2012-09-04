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

    public FSM<MasterController, TargetState> FSM;
    public TargetState State;

    private Material _hoverMaterial;
    private Material _originalMaterial;

    private ITargetable _Target;
    private IController _Controller;

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

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _hoverMaterial = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
        _hoverMaterial.SetColor("_Color", Color.white);
        _hoverMaterial.SetColor("_OutlineColor", Color.blue);

        FSM = new FSM<MasterController, TargetState>(this);
        FSM.RegisterState(new ControllerIdleState());
        FSM.RegisterState(new ControllerTargetState());
    }

    private void FixedUpdate()
    {
        // Update FSM
        FSM.Update(State);

        // Update Controller, with closest target
        _Controller = GetComponents<MonoBehaviour>().OfType<IController>()
            .OrderBy(x => Vector3.Distance(x.Target.Position, Position))
            .FirstOrDefault();

        if (_Controller != null && _Target != _Controller.Target)
        {
            if (_Target == null)
                OnTarget(_Controller.Target);
            else
                OnUntarget(_Target);
        }

        // Update Outine
        _hoverMaterial.SetFloat("_Outline",
                   Mathf.Floor(Time.time % 2) == 0
                       ? Mathf.Lerp(0, 0.01f, Time.time % 1)
                       : Mathf.Lerp(0.01f, 0, Time.time % 1));
    }

    private void OnDrawGizmos()
    {
        if (_Target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Position, _Target.Position);
        }

        if (_Controller != null)
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(Position, Transform.rotation, Transform.lossyScale);
            Gizmos.DrawFrustum(Vector3.zero, _Controller.Angle*2, _Controller.Distance, 0, 1);
        }
    }

    #endregion

    public void OnTarget(ITargetable target)
    {
        _Target = target;

        _originalMaterial = target.TargetRenderer.material;
        _hoverMaterial.mainTexture = _originalMaterial.mainTexture;
        target.TargetRenderer.material = _hoverMaterial;
    }

    public void OnUntarget(ITargetable target)
    {
        _Target = null;
        target.TargetRenderer.material = _originalMaterial;
    }

}