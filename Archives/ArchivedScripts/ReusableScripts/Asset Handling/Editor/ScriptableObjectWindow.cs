using UnityEngine;
using UnityEditor;


public class ScriptableObjectWindow : EditorWindow
{  
    #region Fields

    private static ScriptableObjectWindow _WindowInstance;

    protected ScriptableObject _ScriptableObject;

    [SerializeField]
    protected string _Dir = "Assets/";

    [SerializeField]
    protected string _Name = "Default Name";

    #endregion

    public static T CreateWindow<T, K>(string argDefaultDir) 
        where T : ScriptableObjectWindow
        where K : ScriptableObject
    {
        // Get existing open window or if none, make a new one:
        _WindowInstance = EditorWindow.GetWindow<T>();
        _WindowInstance._Dir += argDefaultDir + typeof(K).ToString() + "s/";
        _WindowInstance._Name = typeof(K).ToString();
        //_WizardInstance.ChooseDir();

        _WindowInstance._ScriptableObject = CreateInstance<K>();
        return _WindowInstance as T;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        _Dir = EditorGUILayout.TextField("Directory: ", _Dir);
        if (GUILayout.Button("...", GUILayout.Width(30)))
            ChooseDir();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _Name = EditorGUILayout.TextField("Line: ", _Name);
        if (GUILayout.Button("...", GUILayout.Width(30)))
            ChooseFileName();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool create = false;
        if (GUILayout.Button("Create", GUILayout.Width(100)))
            create = true;
        EditorGUILayout.EndHorizontal();

        if (create)
            OnCreate();
    }

    /// <summary>
    /// Opens a Window to Allow Directory Choice
    /// </summary>
    protected void ChooseDir()
    {
        /// Open Folder Panel
        string location = EditorUtility.OpenFolderPanel("Choose Asset Directory", Application.dataPath, "");
        Focus();

        /// Remove Application.dataPath
        location = location.Replace(Application.dataPath, "Assets");

        /// Update Location
        if (location != null && location != "")
            _Dir = location;
    }

    /// <summary>
    /// Opens a Window to Allow Directory Choice
    /// </summary>
    protected void ChooseFileName()
    {
        /// Open Folder Panel
        string location = EditorUtility.OpenFilePanel("Save Asset", Application.dataPath, "");
        Focus();

        /// Remove Application.dataPath
        location = location.Replace(Application.dataPath, "Assets");

        /// Update Location
        if (location != null && location != "")
            _Dir = location;
    }

    /// <summary>
    /// Create the Asset
    /// </summary>
    public virtual void OnCreate()
    {
        AssetHandler.CreateAsset(_ScriptableObject, _Dir + _Name + ".asset");
    }

}