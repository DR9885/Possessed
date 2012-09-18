/*using UnityEngine;


public class ControllerTargetState : IFSMState<IController, TargetState>
{
    private Material OriginalMaterial { get; set; }
    private Material HoverMaterial { get; set; }

    public ControllerTargetState()
    {
        HoverMaterial = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
        HoverMaterial.SetColor("_Color", Color.white);
        HoverMaterial.SetColor("_OutlineColor", Color.blue);
    }

    public TargetState State
    {
        get { return TargetState.Target; }
    }

    /// <summary>
    /// Start Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Enter(IController entity)
    {
        OriginalMaterial = entity.Target.TargetRenderer.material;
        HoverMaterial.mainTexture = OriginalMaterial.mainTexture;
        entity.Target.TargetRenderer.material = HoverMaterial;
    }

    /// <summary>
    /// Animate Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Execute(IController entity)
    {
        // TODO:  Object
        // TODO: Varible Thickness
        // TODO: Varible Speed - 
        // TODO: Varible Color -

        // Update Outine
        HoverMaterial.SetFloat("_Outline",
                   Mathf.Floor(Time.time % 2) == 0
                       ? Mathf.Lerp(0, 0.01f, Time.time % 1)
                       : Mathf.Lerp(0.01f, 0, Time.time % 1));

        var target = entity.GetTarget();
        if (target == null)
            entity.TargetState = TargetState.Idle;
    }

    /// <summary>
    /// Reset Targeting
    /// </summary>
    /// <param name="entity"></param>
    public void Exit(IController entity)
    {
        entity.Target.TargetRenderer.material = OriginalMaterial;
    }
}
*/