using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Reusable/Dialogue/Speaker")]
public class Speaker : AbstractEntity
{
    #region Fields

    private static Line _CurrenLine = null;
    [SerializeField] private List<Conversation> _Conversations = new List<Conversation>();
    [SerializeField] private List<Tag> _Tags = new List<Tag>();

    #endregion

    #region Properties

    public List<Tag> Tags
    {
        get { return _Tags; }
    }

    public List<Conversation> Conversations
    {
        get { return _Conversations; }
    }

    #endregion

    void AddTag(string key, string value)
    {
        Tag tag = ScriptableObject.CreateInstance<Tag>();
        tag.Key = key;
        tag.Value = value;
        _Tags.Add(tag);
    }

    void RemoveTag(string key, string value)
    {
        _Tags.RemoveAll(x => x.Key == key && x.Value == value);
    }

    void Update()
    {



        // Update Tags
        //_CurrentTags = _Tags;
    }

    #region Methods

    private Conversation GetConversation(Speaker player)
    {
        return _Conversations.Find(x => x != null && x.Acknowledges(this, player));
    }

    public bool CanSpeakTo(Speaker player)
    {
        return GetConversation(player) != null;
    }

    /// <summary>
    /// Checks to See if Target Will Respond
    /// </summary>
    public Dialogue GetDialogue(Speaker player)
    {
        Conversation convo = GetConversation(player);
        return convo != null ? GetConversation(player).Dialogue : null;
    }

    #endregion
}