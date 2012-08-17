using UnityEngine;
//using UnityEditor;
using System.Collections.Generic;

public class ContextActions : ScriptableObject
{
    private static readonly string _Address = @"Data/Controlls/ContextActions";

    private static ContextActions _Instance;
    private static ContextActions Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load(_Address) as ContextActions;
            }
            //if (_Instance == null)
            //{
            //    _Instance = ContextActions.CreateInstance<ContextActions>();
            //    AssetHandler.CreateAsset<ContextActions>("Assets/Resources/" + _Address);
            //    AssetDatabase.Refresh();
            //    _Instance = Resources.Load(_Address) as ContextActions;

            //    QuickActions.Add(new InputAction(KeyCode.LeftArrow));
            //    QuickActions.Add(new InputAction(KeyCode.RightArrow));
            //    QuickActions.Add(new InputAction(KeyCode.UpArrow));
            //    QuickActions.Add(new InputAction(KeyCode.DownArrow));
            //}
            return _Instance;
        }
    }

    // TODO: OPEN/CLOSE DRAWR

    [SerializeField]
    private ContextAction _Pickup = new ContextAction(KeyCode.Return, Color.red);
    public static ContextAction Pickup
    {
        get { return Instance._Pickup; }
        set { Instance._Pickup = value; }
    }

    [SerializeField]
    private ContextAction _OpenDoor = new ContextAction(KeyCode.Return, Color.yellow);
    public static ContextAction DoorOpen
    {
        get { return Instance._OpenDoor; }
        set { Instance._OpenDoor = value; }
    }

    [SerializeField]
    private ContextAction _SquirrelClimb = new ContextAction(KeyCode.Return, Color.green);
    public static ContextAction SquirrelClimb
    {
        get { return Instance._SquirrelClimb; }
        set { Instance._SquirrelClimb = value; }
    }

    [SerializeField]
    private ContextAction _Possession = new ContextAction(KeyCode.Space, Color.blue);
    public static ContextAction Possession
    {
        get { return Instance._Possession; }
        set { Instance._Possession = value; }
    }

    [SerializeField]
    private ContextAction _Dialogue = new ContextAction(KeyCode.Return, Color.magenta);
    public static ContextAction Dialogue
    {
        get { return Instance._Dialogue; }
        set { Instance._Dialogue = value; }
    }

    [SerializeField]
    private List<InputAction> _QuickActions = new List<InputAction>();
    public static List<InputAction> QuickActions
    {
        get
        {
            if (Instance._QuickActions.Count == 0)
                Instance.Reset();
            return Instance._QuickActions;
        }
        set { Instance._QuickActions = value; }
    }

    void Reset()
    {
        _QuickActions.Add(new InputAction(KeyCode.LeftArrow));
        _QuickActions.Add(new InputAction(KeyCode.RightArrow));
        _QuickActions.Add(new InputAction(KeyCode.UpArrow));
        _QuickActions.Add(new InputAction(KeyCode.DownArrow));
    }
}