using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;

public abstract class AssetHandler
{
    public static readonly char[] _Seperators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

    #region Helper Methods

    /// <summary>
    /// Creates an Asset of type T, to file address.
    /// </summary>
    public static T CreateAsset<T>(string file) where T : ScriptableObject
    {
        return CreateAsset<T>(ScriptableObject.CreateInstance<T>(), file);
    }

    /// <summary>
    /// Creates an Asset of type T, to file address.
    /// </summary>
    public static T CreateAsset<T>(T argObject, string file) where T : ScriptableObject
    {
        /// Create Asset
        string newFile = GetUniqueFilename<T>(file);
        //debug.log(newFile);
        AssetDatabase.CreateAsset(argObject, newFile);
        AssetDatabase.SaveAssets();

        /// Set Focus
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = argObject;
        AssetDatabase.OpenAsset(argObject);

        /// Import Asset to Project
        AssetDatabase.ImportAsset(newFile);
        AssetDatabase.Refresh();

        return argObject;
    }

    /// <summary>
    /// Creates a Unique name of object T at the Directory of the Filename
    /// </summary>
    public static string GetUniqueFilename<T>(string argFile)
    {
        /// Build Filename, Extension, and Directory
        string dir = GetFileDirectory(argFile);
        string name = GetFileName(argFile);
        string ext = GetFileExt(argFile);

        //debug.log("Dir: " + dir);
        //debug.log("Name: " + name);
        //debug.log("Ext: " + ext);

        /// Ensure Directory Exists
        RequireDirectory(dir);

        /// Increment Till no File is Found
        int count = 0;
        while (FileExists<T>(dir + name + (count == 0 ? "" : count.ToString()) + ext))
            count++;

        /// Return Results of Unique Name
        return dir + name + (count == 0 ? "" : count.ToString()) + ext;
    }

    /// <summary>
    /// Check to see if file already exists
    /// </summary>
    public static bool FileExists<T>(string argFile)
    {
        return AssetDatabase.LoadAssetAtPath(argFile, typeof(T)) != null;
    }

    /// <summary>
    /// Retrieves the Extension of a File
    /// </summary>
    public static string GetFileExt(string argFile)
    {
        string[] parts = GetFullFileName(argFile).Split('.');
        if(parts.Length == 2)
            return "." + GetFullFileName(argFile).Split('.')[1];
        return ".asset";
    }

    /// <summary>
    /// Retrieves the Filename
    /// </summary>
    public static string GetFileName(string argFile)
    {
        return GetFullFileName(argFile).Split('.')[0];
    }

    /// <summary>
    /// Retrieves the Filename
    /// </summary>
    public static string GetFullFileName(string argFile)
    {
        return argFile.Replace(GetFileDirectory(argFile), "");
    }

    /// <summary>
    /// Retrieves File Directory
    /// </summary>
    public static string GetFileDirectory(string argFile)
    {
        return Directory.GetParent(argFile).ToString() + _Seperators[1];
    }

    /// <summary>
    /// Requires that a Directory Exists
    /// </summary>
    /// <param name="argDir"></param>
    public static void RequireDirectory(string argDir)
    {
        if (!Directory.Exists(argDir))
            Directory.CreateDirectory(argDir);
    }

    #endregion
}
