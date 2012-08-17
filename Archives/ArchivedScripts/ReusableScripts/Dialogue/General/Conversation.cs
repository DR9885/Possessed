using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Conversation
{
    #region Fields

    #pragma warning disable 0414 
    [SerializeField] private string _Label = "Label";
    [SerializeField] private List<TagFilter> _NPCFilters = new List<TagFilter>();
    [SerializeField] private List<TagFilter> _PlayerFilters = new List<TagFilter>();
    [SerializeField] private Dialogue _Dialogue = null;
    #pragma warning restore 0414

    #endregion

    #region Properties

    public Dialogue Dialogue
    {
        get { return _Dialogue; }
    }

    #endregion

    #region Methods

    public bool Acknowledges(Speaker npc, Speaker player)
    {
        return ReadyToSpeak(player) && ReadyToListen(npc);
    }

    /// <summary>
    /// Returns true if Listener Filters agree with responce
    /// </summary>
    private bool ReadyToListen(Speaker argListener)
    {
        if (argListener != null)
            if (_PlayerFilters.Any(x => x == null || x.Accepted(argListener.Tags)))
                return true;
        return _PlayerFilters.Count == 0;
    }

    /// <summary>
    /// Returns true if  Self Filters agree with responce
    /// </summary>
    private bool ReadyToSpeak(Speaker argSpeaker)
    {
        if (argSpeaker != null)
            if (_NPCFilters.Any(x => x == null || x.Accepted(argSpeaker.Tags)))
                return true;
        return _NPCFilters.Count == 0;
    }

    #endregion
}