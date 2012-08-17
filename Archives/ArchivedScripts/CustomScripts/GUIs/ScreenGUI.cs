using UnityEngine;
using System.Collections;
using System.Linq;

[AddComponentMenu("Possessed/DNU/ScreenGUI")]
public class ScreenGUI : MonoBehaviour
{

    [SerializeField] private Color _Color = Color.clear;

	#region Properties
	
    private static ScreenGUI _Instance;
    private static ScreenGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(ScreenGUI)) as ScreenGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(ScreenGUI).ToString());
                _Instance = obj.AddComponent<ScreenGUI>();
            }
            return _Instance;
        }
    }

    private Rect _ScreenRect;
    public static Rect ScreenRect
    {
        get
        {
            if(Instance._ScreenRect == new Rect(0, 0, 0, 0))
                Instance._ScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            return new Rect(0, 0, Screen.width, Screen.height);
        }
    }

    private Rect _ScreenRatio;
    public static Rect ScreenRatio
    {
        get
        {
            if(Instance._ScreenRatio == new Rect(0, 0, 0, 0))
                Instance._ScreenRatio = new Rect(0, 0,
                    ScreenRect.width / 1028f, ScreenRect.height / 768f);
            return Instance._ScreenRatio;
        }
    }
   
    #region Private

    [SerializeField]
    private Texture2D _Texture;
    private Texture2D Texture
    {
        get
        {
            if (_Texture == null)
            {
                _Texture = new Texture2D(1, 1);

                var y = 0;
                while (y < _Texture.height)
                {
                    var x = 0;
                    while (x < _Texture.width)
                    {
                        _Texture.SetPixel(x, y, Color.white);
                        ++x;
                    }
                    ++y;
                }
                _Texture.Apply();
            }
            return _Texture;
        }
    }

    private Animation _Animation;
    private static Animation Animation
    {
        get
        {
            if (Instance._Animation == null)
                Instance._Animation = Instance.gameObject.RequireComponentAdd<Animation>();
            return Instance._Animation;
        }
    }

    private Color _ColorChange = Color.clear;
    private static Color ColorChange
    {
        get { return Instance._ColorChange; }
        set { Instance._ColorChange = value; }
    }

    #endregion

    #region Public

    public static Color Color
    {
        get { return Instance._Color; }
        set
        {
            ColorChange = value;
            if (Animation[ColorChangeClip.name] != null)
            {
                Animation.RemoveClip(ColorChangeClip.name);
                ColorChangeClip = null;
            }

            Animation.AddClip(ColorChangeClip, ColorChangeClip.name);
            Animation.CrossFade(ColorChangeClip.name);
        }
    }

    public static Color FlashColor
    {
        get { return Instance._Color; }
        set
        {
            ColorChange = value;
            if (Animation[ColorFlashClip.name] != null)
            {
                Animation.RemoveClip(ColorFlashClip.name);
                ColorFlashClip = null;
            }

            Animation.AddClip(ColorFlashClip, ColorFlashClip.name);
            Animation.CrossFade(ColorFlashClip.name);
        }
    }

    [SerializeField] 
    private float _FlashSpeed = 1f/3f;
    public static float FlashSpeed
    {
        get { return Instance._FlashSpeed; }
        set { Instance._FlashSpeed = value; }
    }

    #endregion

    #region Clips

    private AnimationClip _ColorFlashClip = null;
    private static AnimationClip ColorFlashClip
    {
        set { Instance._ColorChangeClip = value; }
        get
        {
            if (Instance._ColorChangeClip == null)
            {
                AnimationCurve curveAUp = AnimationCurve.EaseInOut(0, 0, FlashSpeed, ColorChange.a);
                AnimationCurve curveRUp = AnimationCurve.EaseInOut(0, ColorChange.r, FlashSpeed, ColorChange.r);
                AnimationCurve curveGUp = AnimationCurve.EaseInOut(0, ColorChange.g, FlashSpeed, ColorChange.g);
                AnimationCurve curveBUp = AnimationCurve.EaseInOut(0, ColorChange.b, FlashSpeed, ColorChange.b);

                AnimationCurve curveADown = AnimationCurve.EaseInOut(FlashSpeed, ColorChange.a, FlashSpeed * 2f, 0);
                AnimationCurve curveRDown = AnimationCurve.EaseInOut(FlashSpeed, ColorChange.r, FlashSpeed * 2f, ColorChange.r);
                AnimationCurve curveGDown = AnimationCurve.EaseInOut(FlashSpeed, ColorChange.g, FlashSpeed * 2f, ColorChange.g);
                AnimationCurve curveBDown = AnimationCurve.EaseInOut(FlashSpeed, ColorChange.b, FlashSpeed * 2f, ColorChange.b);

                AnimationCurve curveA = new AnimationCurve(curveAUp.keys.Union(curveADown.keys).ToArray());
                AnimationCurve curveR = new AnimationCurve(curveRUp.keys.Union(curveRDown.keys).ToArray());
                AnimationCurve curveG = new AnimationCurve(curveGUp.keys.Union(curveGDown.keys).ToArray());
                AnimationCurve curveB = new AnimationCurve(curveBUp.keys.Union(curveBDown.keys).ToArray());
                
                Instance._ColorChangeClip = new AnimationClip();
                Instance._ColorChangeClip.name = "ColorChange";
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.a", curveA);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.r", curveR);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.g", curveG);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.b", curveB);
            }
            return Instance._ColorChangeClip;
        }
    }

    private AnimationClip _ColorChangeClip = null;
    private static AnimationClip ColorChangeClip
    {
        set { Instance._ColorChangeClip = value; }
        get
        {
            if (Instance._ColorChangeClip == null)
            {
                AnimationCurve curveA = AnimationCurve.EaseInOut(0, Instance._Color.a, 1, Instance._ColorChange.a);
                AnimationCurve curveR = AnimationCurve.EaseInOut(0, Instance._Color.r, 1, Instance._ColorChange.r);
                AnimationCurve curveG = AnimationCurve.EaseInOut(0, Instance._Color.g, 1, Instance._ColorChange.g);
                AnimationCurve curveB = AnimationCurve.EaseInOut(0, Instance._Color.b, 1, Instance._ColorChange.b);

                Instance._ColorChangeClip = new AnimationClip();
                Instance._ColorChangeClip.name = "ColorChange";
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.a", curveA);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.r", curveR);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.g", curveG);
                Instance._ColorChangeClip.SetCurve("", typeof(ScreenGUI), "_Color.b", curveB);
            }
            return Instance._ColorChangeClip;
        }
    }


    private AnimationClip _FadeInClip = null;
    private static AnimationClip FadeInClip
    {
        get
        {
            if (Instance._FadeInClip == null)
            {
                Instance._FadeInClip = new AnimationClip();
                Instance._FadeInClip.name = "FadeIn";
                Instance._FadeInClip.SetCurve("", typeof(ScreenGUI), "_Color.a", GrowCurve);
                Animation.AddClip(Instance._FadeInClip, Instance._FadeInClip.name);
            }
            return Instance._FadeInClip;
        }
    }

    private AnimationClip _FadeOutClip = null;
    private static AnimationClip FadeOutClip
    {
        get
        {
            if (Instance._FadeOutClip == null)
            {
                Instance._FadeOutClip = new AnimationClip();
                Instance._FadeOutClip.name = "FadeOut";
                Instance._FadeOutClip.SetCurve("", typeof(ScreenGUI), "_Color.a", ShrinkCurve);
                Animation.AddClip(Instance._FadeOutClip, Instance._FadeOutClip.name);
            }
            return Instance._FadeOutClip;
        }
    }

    #endregion

    #region Curves

    private AnimationCurve _GrowCurve;
    private static AnimationCurve GrowCurve
    {
        get
        {
            if (Instance._GrowCurve == null)
                Instance._GrowCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            return Instance._GrowCurve;
        }
    }

    private AnimationCurve _ShrinkCurve;
    private static AnimationCurve ShrinkCurve
    {
        get
        {
            if (Instance._ShrinkCurve == null)
                Instance._ShrinkCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
            return Instance._ShrinkCurve;
        }
    }

    #endregion

    #endregion

    public void Awake()
    {
        FlashColor = Color.clear;
        Color = Color.clear;
    }

    public void OnGUI()
    {
        GUI.color = Color;
        GUI.DrawTexture(ScreenRect, Texture, ScaleMode.StretchToFill);
    }    
}