using System.Collections;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Reusable/Test/DialogueTest")]
public class DialogueTest : MonoBehaviour
{
    [SerializeField] private Dialogue _Dialogue = null;

    private bool _HasInput = false;

    private Line _CurrentLine = null;

    public void Awake()
    {
        //var speaker = FindObjectOfType(typeof (Speaker)) as Speaker;
        //if (speaker != null) _Dialogue = speaker.GetDialogue(null);
        StartCoroutine(Loop());
    }

    public void Update()
    {
        if(Input.anyKeyDown)
        {
            if (ContextActions.Dialogue.Pressed)
                _HasInput = true;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                _CurrentLine.CurrentIndex++;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                _CurrentLine.CurrentIndex--;
        }
    }


    public IEnumerator Loop()
    {
        if (_Dialogue != null)
        {
            foreach (Line line in _Dialogue)
            {
                // Set Current Dialouge Line
                DialogueGUI.CurrentLine = line;
                _CurrentLine = line;

                // Update Control Label
                RemoteGUI.Action = ContextActions.Dialogue;
                switch (line.Choices.Count())
                {
                    case 0: RemoteGUI.Label = "End"; break;
                    case 1: RemoteGUI.Label = "Next"; break;
                    default: RemoteGUI.Label = "Choose"; break;
                }

                // Yield for Input
                while (!_HasInput)
                    yield return new WaitForFixedUpdate();
                _HasInput = false;
            }

            // Disable Both GUI's
            RemoteGUI.Label = "";
            RemoteGUI.Action = null;
            DialogueGUI.CurrentLine = null;
        }
    }

    public IEnumerator WaitForInput()
    {
        while (!_HasInput)
            yield return new WaitForFixedUpdate();
        _HasInput = false;
    }
}

