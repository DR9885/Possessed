using UnityEngine;

[System.Serializable]
public class ContextAction : InputAction
{
    [SerializeField]
    private int _Priority;
    public int Priority
    {
        get { return _Priority; }
        set { _Priority = value; }
    }

    [SerializeField]
    private Color _CursorColor;
    public Color CursorColor
    {
        get { return _CursorColor; }
        set { _CursorColor = value; }
    }

    public ContextAction()
        : this(Color.black) { } 

    public ContextAction(Color argColor)
        : this(KeyCode.Space, argColor) { }

    public ContextAction(KeyCode argKey, Color argColor)
        : this(argKey, argColor, null) { }

    public ContextAction(KeyCode argKey, Color argColor, Texture2D argKeyImage)
        : base(argKey, argKeyImage) { _CursorColor = argColor; }

}