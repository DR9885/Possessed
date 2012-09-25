using System.Linq;
using UnityEngine;


public class ControllerTargetState : IFSMState<MasterController, TargetState>
{
    private Material OriginalMaterial { get; set; }
    private Material HoverMaterial { get; set; }
    private BoxCollider Collider { get; set; }

    public ControllerTargetState()
    {
        HoverMaterial = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
        HoverMaterial.SetColor("_Color", Color.white);
    }

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
        OriginalMaterial = entity.Controller.Target.TargetRenderer.material;
        HoverMaterial.mainTexture = OriginalMaterial.mainTexture;
        entity.Controller.Target.TargetRenderer.material = HoverMaterial;
        Collider = entity.Controller.Target.GameObject.AddComponent<BoxCollider>();
        Collider.center = new Vector3(0, 0.84f, 0.58f);
        Collider.size = new Vector3(1.46f, 1.7f, 1.35f);
    }

    /// <summary>
    /// Animate Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Execute(MasterController entity)
    {
        HoverMaterial.SetColor("_OutlineColor", entity.Controller.Target.Locked ? entity.ColorInActive : entity.ColorActive);

        var time = Time.time*entity.Speed;
        // Update Outine
        HoverMaterial.SetFloat("_Outline",
                   Mathf.Floor(time % 2) == 0
                       ? Mathf.Lerp(entity.MinSize, entity.MaxSize, time % 1)
                       : Mathf.Lerp(entity.MaxSize, entity.MinSize, time % 1));

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
        Object.Destroy(Collider);
        entity.Controller.Target.TargetRenderer.material = OriginalMaterial;
    }
}
