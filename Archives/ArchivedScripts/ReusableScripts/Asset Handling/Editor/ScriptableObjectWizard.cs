using UnityEditor;
using UnityEngine;
using System.IO;

abstract class ScriptableObjectWizard : ScriptableWizard
{
    #region Fields

    protected static ScriptableObjectWizard _WizardInstance;

    protected ScriptableObject _ScriptableObject;

    [SerializeField]
    protected string _Dir = "Assets/";

    [SerializeField]
    protected string _Name = "Default Name";

    #endregion

    /// <summary>
    /// Create a Wizard
    /// </summary>
    /// <typeparam name="T"> The Deriving Wizard Type </typeparam>
    /// <typeparam name="K"> The Scriptable Object in Question </typeparam>
    /// <returns> Instance of Wizard </returns>
    protected static T CreateWizard<T, K>(string argDefaultDir) 
        where T : ScriptableObjectWizard
        where K : ScriptableObject
    {
        _WizardInstance = DisplayWizard<T>("Create Conversation", "Create");//, "Apply");
        _WizardInstance._Dir += argDefaultDir + typeof(K).ToString();
        if (_WizardInstance._Dir[_WizardInstance._Dir.Length - 1] == 'y')
        {
            _WizardInstance._Dir = _WizardInstance._Dir.Remove(_WizardInstance._Dir.Length - 1);
            _WizardInstance._Dir += "ies/";
        }
        else
            _WizardInstance._Dir += "s/";
        

        _WizardInstance._Name = typeof(K).ToString();
        //_WizardInstance.ChooseDir();

        _WizardInstance._ScriptableObject = CreateInstance<K>();
        return _WizardInstance as T;
    }

    //void OnGUI()
    //{
    //    EditorGUILayout.BeginHorizontal();
    //    _Dir = EditorGUILayout.TextField("Directory: ", _Dir);
    //    if (GUILayout.Button("...", GUILayout.Width(30)))
    //        ChooseDir();
    //    EditorGUILayout.EndHorizontal();

    //    EditorGUILayout.BeginHorizontal();
    //    _Name = EditorGUILayout.TextField("Line: ", _Name);
    //    if (GUILayout.Button("...", GUILayout.Width(30)))
    //        ChooseFileName();
    //    EditorGUILayout.EndHorizontal();

    //    EditorGUILayout.BeginHorizontal();
    //    GUILayout.FlexibleSpace();
    //    bool create = false;
    //    if (GUILayout.Button("Create", GUILayout.Width(100)))
    //        create = true;
    //    EditorGUILayout.EndHorizontal();

    //    if (create)
    //    {
    //        OnWizardCreate();
    //        Close();
    //    }

    //    if(_WizardInstance != null)
    //        EditorUtility.SetDirty(this);
    //}

    /// <summary>
    /// Opens a Window to Allow Directory Choice
    /// </summary>
    protected void ChooseDir()
    {
        // Open Folder Panel
        string location = EditorUtility.OpenFolderPanel("Choose Asset Directory", Application.dataPath, "");
        Focus();

        // Remove Application.dataPath
        location = location.Replace(Application.dataPath, "Assets");

        // Update Location
        if (location != "")
            _Dir = location;
    }

    /// <summary>
    /// Opens a Window to Allow Directory Choice
    /// </summary>
    protected void ChooseFileName()
    {
        // Open Folder Panel
        var location = EditorUtility.OpenFilePanel("Save Asset", Application.dataPath, "");
        Focus();

        // Remove Application.dataPath
        location = location.Replace(Application.dataPath, "Assets");

        // Update Location
        if (location != "")
            _Dir = location;
    }

    /// <summary>
    /// Create the Asset
    /// </summary>
    public virtual void OnWizardCreate()
    {
        AssetHandler.CreateAsset(_ScriptableObject, _Dir + _Name + ".asset");
    }

    /// <summary>
    /// Update The Changes
    /// </summary>
    public virtual void OnWizardUpdate()
    {
        helpString = "Dir: Location of File.     Name: Name of the File.";

        if (!this) return;
        errorString = _Dir[_Dir.Length - 1] != '/' ? "dir must be a valid directory" : "";
    }
}