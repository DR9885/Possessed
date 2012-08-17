using UnityEngine;

[System.Serializable]
public class InputAction
{
    #region Fields

    //[SerializeField]
    
    //private XboxCode _Xbox = XboxCode.Cross;

    #endregion

    #region Properties

    [SerializeField]
    private KeyCode _PC = KeyCode.Space;
    public KeyCode KeyCode
    {
        get { return _PC; }
    }

    private string attached_controller;

    public string attached_Controller
    {
        get{
            if (attached_controller == "")
            {
                attached_controller = (Input.GetJoystickNames()[0]); 
            }
            return attached_controller; }
    }
    //[SerializeField]
    //private XboxCode _Xbox = XboxCode.X;
    //public XboxCode XboxCode
    //{
    //    get { return _Xbox; }
    //}

    private Texture2D _KeyImage;
    public Texture2D KeyImage
    {
        get
        {
            if (_KeyImage == null)
                _KeyImage = ResourceManager.TextureResources[_PC] as Texture2D;
            return _KeyImage;
        }
    }

    //private Texture2D _XboxButtonImage;
    //private Texture2D XboxButtonImage
    //{
    //    get
    //    {
    //        if (_KeyImage == null)
    //            _KeyImage = ResourceManager.TextureResources[_Xbox] as Texture2D;
    //        return _KeyImage;
    //    }
    //}

    #region Booleans

    public bool Pressed
    {
        get { return Input.GetKeyDown(_PC); }
    }

    public bool Released
    {
        get { return Input.GetKeyUp(_PC); }
    }

    public bool Held
    {
        get { return Input.GetKey(_PC); }
    }

    #endregion

    #endregion

    #region Constructors

    public InputAction() 
        : this(KeyCode.Space) { }

    public InputAction(KeyCode argKey) { _PC = argKey; }

    public InputAction(KeyCode argKey, Texture2D argKeyImage )
        : this(argKey) { _KeyImage = argKeyImage; }

    #endregion

    /*public void DrawInputLayout(string argLabel)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(KeyImage, GUILayout.Width(256), GUILayout.Height(128));
        GUILayout.EndHorizontal();

        GUILayout.Space(-30);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(argLabel, GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.EndHorizontal();
    }*/

    public void DrawInput(Rect argRect)
    {
        GUI.Label(argRect, KeyImage);
    }
}