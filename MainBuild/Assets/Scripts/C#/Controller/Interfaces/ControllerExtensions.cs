using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ControllerExtensions
{
    public static ITargetable GetTarget(this IController entity)
    {
        ITargetable targeted = null;
        foreach (ITargetable target in entity.Targets)
        {
            var distance = Vector3.Distance(entity.Transform.position, target.Transform.position);
            if (entity.Distance <= distance || !IsInFOV(entity, target)) continue;
            if (targeted == null || distance < Vector3.Distance(entity.Transform.position, target.Transform.position))
                targeted = target;
        }
        return targeted;
    }

    private static bool IsInFOV(this IController controller, ITargetable target)
    {
        var heading = Vector3.Normalize(target.Transform.position - controller.Transform.position);
        var dot = Vector3.Dot(controller.Transform.forward, heading);
        var degrees = Mathf.Rad2Deg * Mathf.Acos(dot);
        return degrees < controller.Angle;
    }

    private static readonly Vector3 offset = Vector3.up*0.6f;
    public static void GizmosFOV(this IController controller)
    {
        if (controller.DebugSettings.Active)
        {
            // Draw View Frustum
            Gizmos.color = controller.DebugSettings.ViewColor;
            Gizmos.matrix = Matrix4x4.TRS(controller.Transform.position + offset, controller.Transform.rotation,
                                          controller.Transform.lossyScale);
            Gizmos.DrawFrustum(Vector3.zero, controller.Angle*2, controller.Distance, 0, 1);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    public static void GizmosTarget(this IController controller)
    {
        // Draw Target Line
        if (controller.Target != null && controller.DebugSettings.Active)
        {
            Gizmos.color = controller.DebugSettings.TargetColor;
            Gizmos.DrawLine(controller.Target.Transform.position + offset,
                            controller.Transform.position + offset);
        }

    }
}