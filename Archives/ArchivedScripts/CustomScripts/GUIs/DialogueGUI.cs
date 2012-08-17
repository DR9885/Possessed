using UnityEngine;
using System.Collections.Generic;


[AddComponentMenu("Possessed/DNU/DialogueGUI")]
[RequireComponent(typeof(Animation))]
public class DialogueGUI : MonoBehaviour
{
    private static DialogueGUI _Instance;
    private static DialogueGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(DialogueGUI)) as DialogueGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(DialogueGUI).ToString());
                _Instance = obj.AddComponent<DialogueGUI>();
            }
            return _Instance;
        }
    }

    private Line _CurrentLine;
    public static Line CurrentLine
    {
        get { return Instance._CurrentLine; }
        set 
        {
                Instance._CurrentLine = value;
        }
    }
    private bool _TransOut;
    public static bool TransOut
    {
        set
        {
            if (value == true)
                Instance.TransitionOut(Instance._CurrentLine);
            Instance._TransOut = value;
        }
    }
    private bool _TransIn;
    public static bool TransIn
    {
        set
        {
            if (value == true)
                Instance.TransitionIn();
            Instance._TransIn = value;
        }
    }

    private string[] _Choices;
    public static string[] Choices
    {
        get { return Instance._Choices; }
        set { Instance._Choices = value; }
    }

    

    //TODO: MOVE EXTERNAL
    private Texture2D _Textbox;
    private Texture2D Textbox
    {
        get
        {
            if (_Textbox == null)
                _Textbox = ResourceManager.TextureResources["dialogue_textbox"];
            return _Textbox;
        }
    }


    private Texture2D _Selected;
    private Texture2D Selected
    {
        get
        {
            if (_Selected == null)
                _Selected = ResourceManager.TextureResources["dialogue_option_selected"];
            return _Selected;
        }
    }
    private Texture2D _Unselected;
    private Texture2D Unselected
    {
        get
        {
            if (_Unselected == null)
                _Unselected = ResourceManager.TextureResources["dialogue_option_unselected"];
            return _Unselected;
        }
    }
    private Texture2D _CharName;
    private Texture2D CharName
    {
        get
        {
            if (_CharName == null)
                _CharName = ResourceManager.TextureResources["dialogue_namebox"];
            return _CharName;
        }
    }
    private Texture2D _SelectArrow;
    private Texture2D SelectArrow
    {
        get
        {
            if (_SelectArrow == null)
                _SelectArrow = ResourceManager.TextureResources["dialogue_nextbutton"];
            return _SelectArrow;
        }
    }

    

    public float dialogueWidth;
    public float dialogueHeight;

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
    private Font _DialogueFontSmall;
    private Font DialogueFontSmall
    {
        get
        {
            if (_DialogueFontSmall == null)
                _DialogueFontSmall = ResourceManager.FontResources["wid_small"];
            return _DialogueFontSmall;
        }
    }

    
    private Font _guiFontSize;
    private Font guiFontSize
    {
        get
        {
            if (Screen.width < 700)
                _guiFontSize = DialogueFontSmall;
            else
                _guiFontSize = DialogueFont;
            return _guiFontSize;
        }
    }

    public float scaleX = 1;
    public float scaleY = 1;
    [SerializeField]
    private AnimationCurve _Curve;
    private List<Keyframe> Frames = new List<Keyframe>();

    public void OnGUI()
    {
        if (_CurrentLine != null)
        {
                DrawLine(CurrentLine);
        }
    }
    
    private void TransitionIn()
    {
        //scaleX = 0;
        scaleY = 0;
        //debug.log("transitioning in");
        List<Keyframe> growFrames = new List<Keyframe>();
        growFrames.Add(new Keyframe(0, 0));
        growFrames.Add(new Keyframe(.8f, 1));
        ResizeAnim(growFrames, 0);
    }
    public void TransitionOut(Line _val)
    {
        scaleY = 1;
        //debug.log("transition out");
        List<Keyframe> shrinkFrames = new List<Keyframe>();
        shrinkFrames.Add(new Keyframe(0, 1));
        shrinkFrames.Add(new Keyframe(.8f, 0));
        ResizeAnim(shrinkFrames, 1);
    }
    private void TransitionEnd(int isOut)
    {
        if (isOut == 1)
        {
            _CurrentLine = null;
            TransOut = false;
        }

    }
    public void ResizeAnim(List<Keyframe> frameList, int isOut)
    {

        Frames = frameList;
        AnimationCurve curve = new AnimationCurve(Frames.ToArray());
        AnimationClip clip = new AnimationClip();

        clip.SetCurve("", typeof(DialogueGUI), "scaleY", curve);
        clip.SetCurve("", typeof(DialogueGUI), "scaleY", curve);
        animation.AddClip(clip, "test");
        animation.Play("test");
        AnimationEvent endEvent = new AnimationEvent();
        endEvent.time = clip.length;
        endEvent.functionName = "TransitionEnd";
        endEvent.intParameter = isOut;
        animation["test"].clip.AddEvent(endEvent);
    }

    public void DrawLine(Line argLine)
    {
        GUI.skin = ResourceManager.GUISkinResources["dialogue"];

        dialogueWidth = (Screen.width * .95f) * scaleX;
        dialogueHeight = (Screen.height * .35f) * scaleY;
        if(GUI.skin != null)
            GUI.skin.font = DialogueFont;
        //var box = new GUIStyle {normal = {background = Textbox}, onActive = {background = Textbox}};
//        print(((Screen.width / 2f) - (dialogueWidth / 2f)) + "and scaleX" + scaleX + "and together" + ((Screen.width / 2f) - (dialogueWidth / 2f)) * scaleX);
        GUI.BeginGroup(new Rect(((Screen.width / 2f) - (dialogueWidth / 2f)), ((Screen.height - dialogueHeight)-120)-25, dialogueWidth, (dialogueHeight + 100)));
        if(Textbox)
            if(argLine.Name != null){
                GUIStyle nameBox = new GUIStyle();
                nameBox.normal.background = CharName;
                nameBox.normal.textColor = Color.white;
                nameBox.font = guiFontSize;
                GUI.Label(new Rect(10 * scaleX,70 * scaleY,250 * scaleX,40 * scaleY),argLine.Name + ": ", nameBox);
            }
        if (!string.IsNullOrEmpty(argLine.Text))
        {
            GUIStyle textBox = new GUIStyle
                {
                    wordWrap = true,
                    normal = {background = Textbox},
                    padding = new RectOffset(20, 60, 30, 30),
                    font = guiFontSize
                };
            textBox.normal.textColor = Color.white;
            
            GUI.Label(new Rect(0 * scaleX, 100 * scaleY, dialogueWidth * scaleX, dialogueHeight * scaleY), argLine.Text, textBox);
        }
        if(Textbox)
            if (argLine.HasChoices)
            {
                float choiceIncrement = 0;
                foreach (Line choice in argLine.Choices)
                {
                    GUIStyle customOption = new GUIStyle
                        {
                            wordWrap = true,
                            normal = {textColor = Color.white},
                            padding = new RectOffset(20, 10, 10, 10),
                            font = guiFontSize
                        };

                    if (choice == argLine.Responce)
                        customOption.normal.background = Selected;
                    else
                        customOption.normal.background = Unselected;
                    
                    
                    GUI.Label(new Rect(0 * scaleX,((dialogueHeight * scaleY) + choiceIncrement)*scaleY,dialogueWidth * scaleX,50 * scaleY),choice.Label, customOption);
                    if(choice == argLine.Responce)
                        GUI.Label(new Rect(0 * scaleX, ((dialogueHeight * scaleY) + choiceIncrement) * scaleY, dialogueWidth * scaleX, 50 * scaleY), SelectArrow);
                    choiceIncrement += (dialogueHeight * .1f);
                }
                choiceIncrement = 0;
            }


        GUI.EndGroup();

        
        /*
        GUILayout.BeginVertical();
        if (Textbox)
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace(); 
                
                // Draw Name
                if (argLine.Name != null)
                    GUILayout.Label(argLine.Name + ": ");

                // Draw Text
                GUI.skin.box.normal.background = Textbox;
                if (!string.IsNullOrEmpty(argLine.Text))
                    GUILayout.Label(argLine.Text, "box", GUILayout.Width(Screen.width * 0.6f), GUILayout.Height(Screen.height * 0.4f));

                GUILayout.FlexibleSpace();
        if (Textbox)
            GUILayout.EndHorizontal();

            // Draw Choices
            if (argLine.HasChoices)
            {
                GUILayout.BeginHorizontal();
                foreach (Line choice in argLine.Choices)
                    GUILayout.Label(choice.Label, choice == argLine.Responce ? GUI.skin.box : GUI.skin.button); // Box for selected, button for other.
                GUILayout.EndHorizontal();
            }

       GUILayout.EndVertical();
       */
    }

}