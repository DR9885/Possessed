using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Possessed/DNU/EnergyGUI")]
[RequireComponent(typeof(Animation))]
public class EnergyGUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private float textureWidth;
    [SerializeField] private float textureHeight;
    [SerializeField] private AnimationCurve _Curve;
    private float energyMeterSize = 180;
    private List<Keyframe> Frames = new List<Keyframe>();

    #endregion

    #region Properties

    private static EnergyGUI _Instance;
    public static EnergyGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(EnergyGUI)) as EnergyGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(EnergyGUI).ToString());
                _Instance = obj.AddComponent<EnergyGUI>();
                
            }
            return _Instance;
        }
    }

    private Texture _Texture;
    public static Texture Texture
    {
        get
        {
            if (Instance._Texture == null)
                Instance._Texture = ResourceManager.TextureResources["energy sphere_blue"];
            return Instance._Texture;
        }
    }

    private Texture2D _SphereContainer;
    public static Texture2D SphereContainer
    {
        get
        {
            if (Instance._SphereContainer == null)
                Instance._SphereContainer = ResourceManager.TextureResources["energy sphere_container_2"];
            return Instance._SphereContainer;
        }
    }
    private Texture2D _SphereBG;
    public static Texture2D SphereBG
    {
        get
        {
            if (Instance._SphereBG == null)
                Instance._SphereBG = ResourceManager.TextureResources["energy sphere_container_inner"];
            return Instance._SphereBG;
        }
    }

    private float _Energy;
    public static float Energy
    {
        get { return Instance._Energy; }
        set 
        { 
            Instance._Energy = Mathf.Clamp(value, 0, 100);
            Resize(Instance._Energy);
        }
    }

    #endregion

    private Color _Color = Color.white;

    public static void StopPulse()
    {
        Instance.animation.Stop("Pulse");
    }

    float _PulseTime = 0;
    public static void Pulse(float min, float max)
    {
        Instance._PulseTime = 0;
        float duration = 1f;
        Instance._Color = Color.red;

        if (Instance.animation["Pulse"] == null)
        {
            //debug.log("BUILD");
            Instance.Frames.AddRange(AnimationCurve.EaseInOut(min, 0, duration / 2f, max).keys);
            Instance.Frames.AddRange(AnimationCurve.EaseInOut(duration / 2f, max, duration, min).keys);

            //Frames.Add(new Keyframe(growSpeed, val));
            AnimationCurve curve = new AnimationCurve(Instance.Frames.ToArray());
            AnimationClip clip = new AnimationClip();
            clip.wrapMode = WrapMode.Loop;

            clip.SetCurve("", typeof(EnergyGUI), "textureWidth", curve);
            clip.SetCurve("", typeof(EnergyGUI), "textureHeight", curve);

            Instance.animation.AddClip(clip, "Pulse");
        }

        if (!Instance.animation.IsPlaying("Pulse"))
        {
            Instance.animation.Play("Pulse");
            
        }
    }

    public static void Resize(float val)
    {
        float startSize = Instance.textureWidth;
        if (val > startSize)
            GrowAnim(val);
        else if (val < startSize)
            ShrinkAnim(val);
    }

    public static void GrowAnim(float val)
    {
        List<Keyframe> growFrames = new List<Keyframe>();
        growFrames.Add(new Keyframe(0, Instance.textureWidth));
        growFrames.Add(new Keyframe(.5f, Instance.textureWidth - 5));
        growFrames.Add(new Keyframe(1.5f, val));
        ResizeAnim(growFrames);
    }

    public static void ShrinkAnim(float val)
    {
        List<Keyframe> shrinkFrames = new List<Keyframe>();
        shrinkFrames.Add(new Keyframe(0, Instance.textureWidth));
        shrinkFrames.Add(new Keyframe(.5f, Instance.textureWidth + 5));
        shrinkFrames.Add(new Keyframe(1.5f, val));
        ResizeAnim(shrinkFrames);
    }

    public static void ResizeAnim(List<Keyframe> frameList)
    {
        //gt the starting size of the texture
        float startSize = Instance.textureWidth;

        Instance.Frames = frameList;
        /*for (int i = 0; i < 300; i++)
            Frames.Add(new Keyframe(i, i * i));*/
        //Frames.Add(new Keyframe(0,startSize));
        //Frames.Add(new Keyframe(growSpeed, val));
        AnimationCurve curve = new AnimationCurve(Instance.Frames.ToArray());
        AnimationClip clip = new AnimationClip();
        
        clip.SetCurve("", typeof(EnergyGUI), "textureWidth", curve);
        clip.SetCurve("", typeof(EnergyGUI), "textureHeight", curve);
        Instance.animation.AddClip(clip, "test");
        Instance.animation.Play("test");
    }

    public void Update()
    {
        _PulseTime += Time.deltaTime;

        if (_PulseTime > 1 && Instance.animation.IsPlaying("Pulse"))
        {
            Instance.animation.Stop("Pulse");
            Resize(0);
        }
    }

    public void OnGUI()
    {
        //the width and height of the energy meter multiplied by the screen ratio
        float scaleWidth = energyMeterSize * .88f * (textureWidth/100);
        float scaleHeight = energyMeterSize * .88f * (textureWidth / 100);
        float scaleX = ((energyMeterSize - scaleWidth) / 2);
        float scaleY = ((energyMeterSize - scaleHeight) / 2);

        //GUILayout.Label(_Energy.ToString());
        GUI.BeginGroup(new Rect(Screen.width * .05f, Screen.height * .05f, 180, 180));
        GUI.DrawTexture(new Rect(0, 0, energyMeterSize, energyMeterSize + 10), SphereBG);
        GUI.DrawTexture(new Rect(scaleX, scaleY, scaleWidth, scaleHeight), Texture);
        GUI.DrawTexture(new Rect(0, 0, energyMeterSize , energyMeterSize), SphereContainer);
        
        GUI.color = Instance._Color;
        
        GUI.EndGroup();
    }
}