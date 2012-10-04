using System.Linq;
using UnityEngine;


public class ControllerTargetState : IFSMState<MasterController, TargetState>
{
    public TargetState State
    {
        get { return TargetState.Target; }
    }

    /// <summary>
    /// Start Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Enter(MasterController entity)
    {
        entity.Controller.Target.Highlight.enabled = true;
        entity.Controller.Target.TargetCollider.enabled = true;
    }

    /// <summary>
    /// Animate Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Execute(MasterController entity)
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            RaycastHit[] hits = Physics.RaycastAll(entity.Camera.ScreenPointToRay(Input.mousePosition),
                                                   entity.Controller.Distance +
                                                   Vector3.Distance(entity.Camera.transform.position,
                                                                    entity.Controller.Transform.position));
            bool hit = hits.Any(x => x.transform == entity.Controller.Target.Transform);
            if (hit)
            {
                entity.Controller.Enabled = true;
                entity.TargetState = TargetState.Active;
            }
        }
    }

    /// <summary>
    /// Reset Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Exit(MasterController entity)
    {
        entity.Controller.Target.Highlight.enabled = false;
//        entity.Controller.Target.TargetRenderer.material = OriginalMaterial;
        entity.Controller.Target.TargetCollider.enabled = false;
    }
}
