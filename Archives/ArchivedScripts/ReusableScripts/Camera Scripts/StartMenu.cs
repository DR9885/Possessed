using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
[AddComponentMenu("Possessed/DNU/StartMenu")]
[RequireComponent(typeof(Animation))]
public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("MainCamera").active = false;
        GameObject.Find("StartCam").active = true;
	}

    private static StartMenu _Instance;
    public static StartMenu Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(StartMenu)) as StartMenu;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(StartMenu).ToString());
                _Instance = obj.AddComponent<StartMenu>();

            }
            return _Instance;
        }
    }

    private bool SplashScreen = false;

    private Texture2D _Title;
    public Texture2D Title
    {
        get
        {
            if(_Title == null)
                _Title = ResourceManager.TextureResources["titlescreen"];
            return _Title;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI ()
    {
        GUI.skin = ResourceManager.GUISkinResources["dialogue"];
        if (GUI.skin != null)
            GUI.skin.font = DialogueFont;
        if (!SplashScreen)
        {
            GUI.Label(
                new Rect((Screen.width/2) - (Title.width/2), (Screen.height/2) - (Title.height/2), Title.width,
                         Title.height), Title);
            GUIStyle StartButton = new GUIStyle();
            StartButton.onHover.background = HoverBtn;
            StartButton.hover.background = HoverBtn;
            StartButton.normal.background = StartBtn;
            if (
                GUI.Button(
                    new Rect((Screen.width/2) - (StartBtn.width/2), (Screen.height) - 150, StartBtn.width,
                             StartBtn.height), "", StartButton))
            {
                SplashScreen = true;
                GrowAnim();
            }
        }

        if(SplashScreen)
        {
             GUIStyle GroupStyle = new GUIStyle();
             //GreyBg.width = Screen.width/2;
             //GreyBg.height = Screen.height/2;
             //GroupStyle.font = DialogueFont;
             //GroupStyle.normal.background = Cham;
            GUI.color = new Color(1, 1, 1, ChampAlpha);
            //Debug.Log(ChampAlpha);
             GUI.Label(new Rect((Screen.width / 2) - (ChamplainSplash.width / 2), (Screen.height / 2) - (ChamplainSplash.height / 2), ChamplainSplash.width, ChamplainSplash.height), ChamplainSplash);

             //
        }

    }
    public static void GrowAnim()
    {
        List<Keyframe> growFrames = new List<Keyframe>();
        growFrames.Add(new Keyframe(0, 0));
        growFrames.Add(new Keyframe(1.5f, 1));
        growFrames.Add(new Keyframe(3.0f, 0));
        ResizeAnim(growFrames);
        //Debug.Log(growFrames);
    }

    public float ChampAlpha = 0;

    private Animation _animation;
    private Animation animation
    {
        get
        {
            if (_animation == null)
                _animation = gameObject.RequireComponentAdd<Animation>();
            return _animation;
        }
    }

    public static void ResizeAnim(List<Keyframe> frameList)
    {
        List<Keyframe> Frames = new List<Keyframe>();
        Frames = frameList;
        /*for (int i = 0; i < 300; i++)
            Frames.Add(new Keyframe(i, i * i));*/
        //Frames.Add(new Keyframe(0,startSize));
        //Frames.Add(new Keyframe(growSpeed, val));
        AnimationCurve curve = new AnimationCurve(Frames.ToArray());
        AnimationClip clip = new AnimationClip();

        clip.SetCurve("", typeof(StartMenu), "ChampAlpha", curve);
        
        Instance.animation.AddClip(clip, "test");
        AnimationEvent endEvent = new AnimationEvent();
        endEvent.time = clip.length;
        endEvent.functionName = "PlayGame";
        Debug.Log(endEvent.time);
        Instance.animation["test"].clip.AddEvent(endEvent);
        Instance.animation.Play("test");
    }

    private void PlayGame()
    {
        Application.LoadLevel(1);
    }

    private Texture2D _ChamplainSplash;
    private Texture2D ChamplainSplash
    {
        get
        {
            if(_ChamplainSplash == null)
                _ChamplainSplash = ResourceManager.TextureResources["ChampSplash"];
            return _ChamplainSplash;
        }
    }

    private Font _DialogueFont;
    private Font DialogueFont
    {
        get
        {
            if (_DialogueFont == null)
                _DialogueFont = ResourceManager.FontResources["wid"];
            return _DialogueFont;
        }
    }
    private Texture2D _GreyBg;
    public Texture2D GreyBg
    {
        get
        {
            if(_GreyBg == null)
                _GreyBg = ResourceManager.TextureResources["GreyBg"];
            return _GreyBg;
        }
    }

    private Texture2D _HoverBtn;
    public Texture2D HoverBtn
    {
        get
        {
            if (_HoverBtn == null)
                _HoverBtn = ResourceManager.TextureResources["Startbtn_hover"];
            return _HoverBtn;
        }
    }
    private Texture2D _StartBtn;
    public Texture2D StartBtn
    {
        get
        {
            if (_StartBtn == null)
                _StartBtn = ResourceManager.TextureResources["Startbtn"];
            return _StartBtn;
        }
    }
}
