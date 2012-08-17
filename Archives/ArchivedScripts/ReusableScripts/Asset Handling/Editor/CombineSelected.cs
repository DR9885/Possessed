// C# Example
// Creates a new parent for the selected transforms

using UnityEngine;
using UnityEditor;

public class CombineSelected : ScriptableObject
{
    static GameObject CombineGroup()
    {
        Transform[] selection = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
        
        GameObject newParent = new GameObject(selection[0].name + "_Combine_" + selection.Length);
        newParent.transform.parent = selection[0].parent;
        newParent.transform.localPosition = selection[0].transform.position;

        foreach (Transform t in selection)
            t.parent = newParent.transform;

        return newParent;
    }
    // Disable the menu if there is nothing selected
    [MenuItem("Tools/Group/Auto-Combine Mesh %J", true)]
    static bool ValidateSelectionJ() { return Selection.activeGameObject != null; }

    [MenuItem("Tools/Group/Auto-Combine Mesh %J")]
    static void AutoCombineGroupMeshColJ()
    {
        GameObject parent = CombineGroup();
        CombineChildren combine = parent.AddComponent<CombineChildren>();
        combine._DestroyAfterOptimized = true;
        //combine._AddMeshCollider = true;
        Selection.activeGameObject = parent;
    }

    [MenuItem("Tools/Group/Auto-Combine Box %K", true)]
    static bool ValidateSelectionK() { return Selection.activeGameObject != null; }

    [MenuItem("Tools/Group/Auto-Combine Box %K")]
    static void AutoCombineGroupBoxColK()
    {
        GameObject parent = CombineGroup();
        CombineChildren combine = parent.AddComponent<CombineChildren>();
        combine._DestroyAfterOptimized = true;
        //combine._AddBoxCollider = true;
        Selection.activeGameObject = parent;
    }


    [MenuItem("Tools/Remove/MeshCollider %M", true)]
    static bool ValidateRemoveMeshColliders()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("Tools/Remove/MeshCollider %M")]
    static void RemoveMeshColliders()
    {
        foreach (Transform trans in Selection.transforms)
        {
            DestroyImmediate(trans.GetComponent<MeshCollider>());
            foreach (Transform t in trans)
                DestroyImmediate(t.GetComponent<MeshCollider>());
        }
    }


}