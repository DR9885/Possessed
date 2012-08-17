using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Dialogue : ScriptableObject, IEnumerable, IEnumerator
{
    #region Fields

    [SerializeField]
    private Line _StartingLine = new Line();

    private Line _CurrentLine = null;

    #endregion

    #region IEnumerator Members

    /// <summary>
    /// Get Current Line
    /// </summary>
    public object Current
    {
        get { return _CurrentLine; }
    }

    /// <summary>
    /// Traverse to Next Line
    /// </summary>
    public bool MoveNext()
    {
        //_CurrentLine = null;

        if (_CurrentLine == null)
        {
            _CurrentLine = _StartingLine;
            //return true;
        }
        else if (_CurrentLine.HasResponce)
        {
            _CurrentLine = _CurrentLine.Responce;
            //return true;
        }
        else
            _CurrentLine = null;
        
        // Fire Events
        if(_CurrentLine != null)
            foreach (DialogueEvent dialogueEvent in _CurrentLine.DialogueEvents)
            {
                if (dialogueEvent != null && dialogueEvent.Target != null)
                    dialogueEvent.Execute();
            }


        return _CurrentLine != null;
    }

    /// <summary>
    /// Reset Line Data
    /// </summary>
    public void Reset()
    {
        _CurrentLine = null;
        _StartingLine.Reset();
    }

    #endregion

    #region IEnumerable Members

    public IEnumerator GetEnumerator()
    {
        Reset();
        return this;
    }

    #endregion
}
