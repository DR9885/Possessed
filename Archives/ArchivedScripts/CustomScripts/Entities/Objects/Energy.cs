using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed/Objects/Energy")]
public class Energy : AbstractEntity
{
    #region Fields

    [SerializeField] private float _Value = 10;
    [SerializeField] private float _GrowDuration = 1;

    private float _Time;
    private AnimationCurve _CurrentCurve;

    #endregion

    #region Properties

    private static Color _GoodColor = new Color(100f / 256f, 145f / 256f, 237f / 256f, 0.5f);
    public static Color GoodColor { get { return _GoodColor; } }

    private static Color _BadColor = new Color(256f / 256f, 0f / 256f, 256f / 256f, 0.5f);
    public static Color BadColor { get { return _BadColor; } }

    private Color Color { get { return _Value > 0 ? GoodColor : BadColor; } }

    #region Components

    private SphereCollider _SphereCollider;
    private SphereCollider SphereCollider
    {
        get
        {
            if (_SphereCollider == null)
                _SphereCollider = GameObject.RequireComponentAdd<SphereCollider>();
            return _SphereCollider;
        }
    }

    private Animation _Animation;
    private Animation Animation
    {
        get
        {
            if (_Animation == null)
                _Animation = GameObject.RequireComponentAdd<Animation>();
            return _Animation;
        }
    }

    #endregion

    #region Clips

    private AnimationClip _GrowClip = null;
    private AnimationClip GrowClip
    {
        get
        {
            if (_GrowClip == null)
            {
                _GrowClip = new AnimationClip();
                _GrowClip.name = "Grow";
                _GrowClip.SetCurve("", typeof(Transform), "localScale.x", GrowCurve);
                _GrowClip.SetCurve("", typeof(Transform), "localScale.y", GrowCurve);
                _GrowClip.SetCurve("", typeof(Transform), "localScale.z", GrowCurve);
            }
            return _GrowClip;
        }
    }

    private AnimationClip _ShrinkClip = null;
    private AnimationClip ShrinkClip
    {
        get
        {
            if (_ShrinkClip == null)
            {
                _ShrinkClip = new AnimationClip();
                _ShrinkClip.name = "Shrink";
                _ShrinkClip.SetCurve("", typeof(Transform), "localScale.x", ShrinkCurve);
                _ShrinkClip.SetCurve("", typeof(Transform), "localScale.y", ShrinkCurve);
                _ShrinkClip.SetCurve("", typeof(Transform), "localScale.z", ShrinkCurve);
            }
            return _ShrinkClip;
        }
    }

    private AnimationClip _FadeInClip = null;
    private AnimationClip FadeInClip
    {
        get
        {
            if (_FadeInClip == null)
            {
                _FadeInClip = new AnimationClip();
                _FadeInClip.name = "FadeIn";
                _FadeInClip.SetCurve("", typeof(Material), "_Color.a", GrowCurve);
            }
            return _FadeInClip;
        }
    }

    private AnimationClip _FadeOutClip = null;
    private AnimationClip FadeOutClip
    {
        get
        {
            if (_FadeOutClip == null)
            {
                _FadeOutClip = new AnimationClip();
                _FadeOutClip.name = "FadeOut";
                _FadeOutClip.SetCurve("", typeof(Material), "_Color.a", ShrinkCurve);
            }
            return _FadeOutClip;
        }
    }

    #endregion

    #region Curves

    private AnimationCurve _GrowCurve;
    private AnimationCurve GrowCurve
    {
        get
        {
            if (_GrowCurve == null)
                _GrowCurve = AnimationCurve.EaseInOut(0, 0, _GrowDuration, 1);
            return _GrowCurve;
        }
    }

    private AnimationCurve _ShrinkCurve;
    private AnimationCurve ShrinkCurve
    {
        get
        {
            if (_ShrinkCurve == null)
                _ShrinkCurve = AnimationCurve.EaseInOut(0, 1, _GrowDuration, 0);
            return _ShrinkCurve;
        }
    }

    #endregion

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        Animation.CrossFade(FadeInClip.name);
        Animation.Blend(GrowClip.name);
    }

    void OnDisable()
    {
        Animation.CrossFade(FadeOutClip.name);
        Animation.Blend(ShrinkClip.name);
    }

    new public void Awake()
    {
        base.Awake();

        SphereCollider.isTrigger = true;

        if(renderer)
		{		
			renderer.material.shader = Shader.Find("Transparent/Diffuse");
            renderer.material.color = _Value > 0 ? GoodColor : BadColor;
		}

        Animation.AddClip(GrowClip, GrowClip.name);
        Animation.AddClip(ShrinkClip, ShrinkClip.name);
        Animation.AddClip(FadeInClip, FadeInClip.name);
        Animation.AddClip(FadeOutClip, FadeOutClip.name);
    }

    public void OnTriggerEnter(Collider argCollider)
    {
        // If Collider is Solid
        if (!argCollider.isTrigger)
        {
            // If Being is Ghost
            GhostScript ghost = argCollider.GetComponent<MainPlayerGhost>();
            if (ghost != null)
                StartCoroutine(Execute(ghost));
        }
    }

    #endregion

    public IEnumerator Execute(GhostScript ghost)
    {
        enabled = false;
		
		StartCoroutine(ghost.Absorb(this));

        yield return StartCoroutine(NotifyUser());

        ghost.Energy += _Value;
		
        Destroy(gameObject);
    }


    private IEnumerator NotifyUser()
    {
        Color flashColor = Color;

        // Flash Screen
        //yield return StartCoroutine(ScreenGUI.Flash(flashColor, 2));
        yield return new WaitForEndOfFrame();
    }

}