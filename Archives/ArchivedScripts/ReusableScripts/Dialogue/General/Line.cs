using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line
{
    #region Fields

    #pragma warning disable 0414
    [SerializeField] private static readonly Rect _ScreenRect = new Rect(0, 0, Screen.width, Screen.height);

    [SerializeField] private string _Label = "Label";
    [SerializeField] private string _Name = "Name";
    [SerializeField] private string _Text = "Text";
    //[SerializeField] private AudioClip _Clip = null;
    //[SerializeField] private Tag _ID = null;
    //[SerializeField] private Texture _Portrate = null;
    [SerializeField] private List<DialogueEvent> _DialogueEvents = null;
    [SerializeField] private List<Line> _TextResponces = new List<Line>();

    private int _CurrentIndex = 0;
    
    #pragma warning restore 0414

    #endregion

    #region Properties

    //public Texture Portrate
    //{
    //    get { return _Portrate; }
    //}

    //public AudioClip Clip
    //{
    //    get { return _Clip; }
    //}

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    public string Text
    {
        get { return _Text; }
    }

    //public Tag Id
    //{
    //    get { return _ID; }
    //}

    public List<Line> TextResponces
    {
        get { return _TextResponces; }
    }


    public bool HasResponce
    {
        get { return TextResponces != null && TextResponces.Count != 0; }
    }

    public Line Responce
    {
        get { return HasResponce ? TextResponces[CurrentIndex] : null; }
    }

    public bool HasChoices
    {
        get {  return _TextResponces != null && _TextResponces.Count > 1; }
    }

    public int CurrentIndex
    {
        get { return _CurrentIndex; }
        set
        {
            if (_TextResponces != null)
            {
                int max = _TextResponces.Count - 1;
                if (value < 0)
                    _CurrentIndex = max;
                else if (value > max)
                    _CurrentIndex = 0;
                else
                    _CurrentIndex = value;
            }
        }
    }

    public string Label
    {
        get { return _Label; }
    }

    public IEnumerable<Line> Choices
    {
        get { return TextResponces; }
    }

    public List<DialogueEvent> DialogueEvents
    {
        get { return _DialogueEvents; }
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _CurrentIndex = 0;
        foreach (Line responce in TextResponces)
            responce.Reset();
    }

    public override string ToString()
    {
        return Name + ": \"" + _Text + "\"";
    }

    #endregion
}