using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Possessed/DNU/RemoteGUI")]
[RequireComponent(typeof(Animation))]
public class RemoteGUI : MonoBehaviour
{
    private static RemoteGUI _Instance;
    private static RemoteGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(RemoteGUI)) as RemoteGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(RemoteGUI).ToString());
                _Instance = obj.AddComponent<RemoteGUI>();
            }
            return _Instance;
        }
    }

    private Animation _Animation;
    private Animation Animation
    {
        get
        {
            if (_Animation == null)
                _Animation = gameObject.RequireComponentAdd<Animation>();
            return _Animation;
        }
    }

    [SerializeField]
    private InputAction _Action;
    public static InputAction Action
    {
        get { return Instance._Action; }
        set {
            Instance._Action = value; 
            if (Instance.quickSlide == false)
                Instance.slideIn();
            
        }
    }
    [SerializeField]
    private string _Label = "LABEL";
    public static  string Label
    {
        get { return Instance._Label; }
        set { Instance._Label = value; }
    }

    private KeyCode _PC;
    public KeyCode PC
    {
        get {
            if(_Action != null)
                _PC = _Action.KeyCode;
            return _PC; 
        }
    }

    private Texture2D _KeyImage;
    public Texture2D KeyImage
    {
        get
        {
            _KeyImage = ResourceManager.TextureResources[PC] as Texture2D;
            return _KeyImage;
        }
    }
    public float GuiAlpha;
    public void DrawInput(Rect argRect, string argLabel)
    {
        
        if (argLabel != "" && Action != null)
        {
            //GUI.DrawTexture(argRect,(1.0, 1.0, 1.0, 0.25))
            GUI.color = new Color(1.0f, 1.0f, 1.0f, GuiAlpha);
            GUI.Label(argRect, KeyImage);
            
            //GUI.Label(new Rect(argRect.x,argRect.y -20,100,50), argLabel);
        }
        else
        {
            GUI.color = new Color(1.0f, 1.0f, 1.0f, 0);
        }
    }

    private List<Keyframe> Frames = new List<Keyframe>();
    public float posX = 0;
    private bool quickSlide = false;
    private void slideIn()
    {
        quickSlide = true;
        GuiAlpha = 0;
        //posX = KeyImage.width + (Screen.width * .05f);
        List<Keyframe> shrinkFrames = new List<Keyframe>();
        shrinkFrames.Add(new Keyframe(0, GuiAlpha));
        shrinkFrames.Add(new Keyframe(.8f, 1));
        SlideAnim(shrinkFrames);
    }
    private void TransitionEnd()
    {
        quickSlide = false;
        posX = 0;
    }
    public void SlideAnim(List<Keyframe> frameList)
    {

        Frames = frameList;
        AnimationCurve curve = new AnimationCurve(Frames.ToArray());
        AnimationClip clip = new AnimationClip();

        clip.SetCurve("", typeof(RemoteGUI), "GuiAlpha", curve);
        clip.SetCurve("", typeof(RemoteGUI), "GuiAlpha", curve);
        Animation.AddClip(clip, "test");
        Animation.Play("test");
        AnimationEvent endEvent = new AnimationEvent();
        endEvent.time = clip.length;
        endEvent.functionName = "TransitionEnd";
        Animation["test"].clip.AddEvent(endEvent);
    }
    public void OnGUI()
    {
        /*if (_Action != null)
            _Action.DrawInputLayout(_Label);*/
        if (_Action != null)
        {
            DrawInput(new Rect((Screen.width * .95f - KeyImage.width) + posX, Screen.height * .1f, KeyImage.width, KeyImage.height), _Label);

        }

    }
}