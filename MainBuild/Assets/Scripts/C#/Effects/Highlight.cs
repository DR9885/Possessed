using System.Linq;
using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour
{
    #region Fields

    private Material _originalMaterial;
    private Material OriginalMaterial
    {
        get
        {
            if(_originalMaterial == null)
                _originalMaterial = Renderer.material;
            return _originalMaterial;
        }
    }

    [SerializeField] private Material _hoverMaterial;
    private Material HoverMaterial 
    { 
        get
        {
            if (_hoverMaterial == null)
            {
                _hoverMaterial = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
                _hoverMaterial.SetColor("_Color", Color.white);
                _hoverMaterial.mainTexture = OriginalMaterial.mainTexture;
            }
            return _hoverMaterial;
        } 
    }

    private Renderer _renderer;
    private Renderer Renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<Renderer>();
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            return _renderer;
        }
    }

    [SerializeField] private float _minSize = 0.5f;
    public float MinSize
    {
        get { return _minSize / 1000.0f; }
    }

    [SerializeField] private float _maxSize = 10;
    public float MaxSize
    {
        get { return _maxSize / 1000.0f; }
    }

    [SerializeField] private float _speed = 2;
    public float Speed
    {
        get { return _speed; }
    }

    [SerializeField] private Color _color = Color.blue;
    public Color Shade
    {
        get { return _color; }
    }

    #endregion

    private float duration = 0;
    void OnEnable()
    {
        if (Renderer == null) return;
        Renderer.material = HoverMaterial;
        duration = 0;
    }

    void FixedUpdate()
    {
        if (Renderer == null) return;
        HoverMaterial.SetColor("_OutlineColor", Shade);

        duration += Time.deltaTime * Speed;
        // Update Outine
        HoverMaterial.SetFloat("_Outline",
                   Mathf.Floor(duration % 2) == 0
                       ? Mathf.Lerp(MinSize, MaxSize, duration % 1)
                       : Mathf.Lerp(MaxSize, MinSize, duration % 1));
    }

    void OnDisable()
    {
        if (Renderer == null) return;
        Renderer.material = OriginalMaterial;
    }
}
