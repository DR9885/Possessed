using UnityEngine;

[AddComponentMenu("Possessed/DNU/CursorGUI")]
public class CursorGUI : MonoBehaviour
{
    private static CursorGUI _Instance;
    private static CursorGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(CursorGUI)) as CursorGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(CursorGUI).ToString());
                _Instance = obj.AddComponent<CursorGUI>();
            }
            return _Instance;
        }
    }


    private GameObject _Cursor;
    static GameObject Cursor
    {
        get
        {
            if (Instance._Cursor == null)
                Instance._Cursor = CreateCursor();
            return Instance._Cursor;
        }
    }


    private Transform _CursorTransform;
    static Transform CursorTransform
    {
        get
        {
            if (Instance._CursorTransform == null)
                Instance._CursorTransform = Cursor.transform;
            return Instance._CursorTransform;
        }
    }

    private AbstractEntity _Target;
    public static AbstractEntity Target
    {
        get { return Instance._Target; }
        set
        {
            // HideTarget Old Cursor
            if (Instance._Target != null)
                HideCursor();

            // Store Value
            Instance._Target = value;
            if (Instance._Target != null)
                ShowCursor();
        }
    }

    public static Color Color
    {
        set
        {
            if (Cursor != null && Cursor.renderer != null && Cursor.renderer.material != null)
                Cursor.renderer.material.color = value;
        }
    }

    /// <summary>
    /// Generate the Cursor Object
    /// 
    /// TODO: REPLACE WITH PREFAB
    /// </summary>
    private static GameObject CreateCursor()
    {
        // ShowTarget Target
        Instance._Cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Cursor.transform.localScale = Vector3.one / 10f;
        return Cursor;
    }


    /// <summary>
    /// Display the Cursor at a set color
    /// </summary>
    public static void ShowCursor()
    {
        if (Target != null && Cursor != null && Cursor.renderer != null)
        {
            Cursor.renderer.enabled = true;
            CursorTransform.parent = Target.Transform;
            CursorTransform.localScale = Vector3.one * Target.Renderer.bounds.extents.y;

            CursorTransform.position = Target.Renderer.bounds.center
                + (Vector3.up*Target.Renderer.bounds.extents.y)
                + (Vector3.up * CursorTransform.localScale.y)/2f;
        }
    }


    /// <summary>
    /// HideTarget the Cursor
    /// </summary>
    public static void HideCursor()
    {
        if (Target != null && Cursor != null)
            Cursor.renderer.enabled = false;
    }
}