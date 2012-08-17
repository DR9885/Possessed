using System.Collections;
using UnityEngine;

[AddComponentMenu("Possessed/DNU/HighlightGUI")]
public class HighlightGUI : MonoBehaviour
{
    private static HighlightGUI _Instance;
    private static HighlightGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(HighlightGUI)) as HighlightGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(HighlightGUI).ToString());
                _Instance = obj.AddComponent<HighlightGUI>();
            }
            return _Instance;
        }
    }

    private AbstractEntity _Target;
    public static AbstractEntity Target
    {
        get { return Instance._Target; }
        set
        {
            Hide(); // Hide Old
            Instance._Target = value;
            Show(); // Show New
        }
    }

    [SerializeField]
    private Color _Color = Color.blue;
    public static Color Color
    {
        get { return Instance._Color; }
        set { Instance._Color = value; }
    }


    private Shader _TempShader = null;
    private static Shader TempShader
    {
        get { return Instance._TempShader; }
        set { Instance._TempShader = value; }
    }

    static void Show()
    {
        if (Target != null)
        {
            TempShader = Target.Renderer.material.shader;
            Target.Renderer.material.shader = Shader.Find("Toon/Lighted Outline");
            Target.Renderer.material.SetColor("_OutlineColor", Color);
        }
    }

    static void Hide()
    {
        if (Target != null)
        {
            Target.Renderer.material.SetFloat("_Outline", 0);
            Target.Renderer.material.SetColor("_OutlineColor", Color.black);
            Target.Renderer.material.shader = TempShader;
        }
    }

    void Update()
    {
        if (Target != null)
        {
            float newVal = ((Mathf.Cos(Time.time * 7.07f) / 200f) + (1f / 100f));
            Target.Renderer.material.SetFloat("_Outline", newVal);
        }
    }

}