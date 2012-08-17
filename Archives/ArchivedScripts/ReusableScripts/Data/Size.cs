using UnityEngine;

[System.Serializable]
public class Size
{
    [SerializeField]
    private float _Width;
    public float Width
    {
        get { return _Width; }
        set { _Width = value; }
    }

    [SerializeField]
    private float _Height;
    public float Height
    {
        get { return _Height; }
        set { _Height = value; }
    }

    public Size() : this(0, 0) { }

    public Size(float width, float height)
    {
        _Width = width;
        _Height = height;
    }
}