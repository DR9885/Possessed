using System.Collections;
using UnityEngine;
using System.Linq;

[AddComponentMenu("Possessed/Remotes/DialogueRemote")]
public class DialogueRemote : AbstractRemote
{
    private bool _HasInput = false;
    private Line _CurrentLine = null;

    #region Properties

    private Speaker _Speaker;
    public Speaker Speaker
    {
        get
        {
            if(_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }

    private ThirdPersonController _ThirdPersonController;
    public ThirdPersonController ThirdPersonController
    {
        get
        {
            if (_ThirdPersonController == null)
                _ThirdPersonController = GetComponent<ThirdPersonController>();
            return _ThirdPersonController;
        }
    }

    private ThirdPersonCamera _ThirdPersonCamera;
    public ThirdPersonCamera ThirdPersonCamera
    {
        get
        {
            if (_ThirdPersonCamera == null)
                _ThirdPersonCamera = GetComponent<ThirdPersonCamera>();
            return _ThirdPersonCamera;
        }
    }

    #endregion

    #region Methods

    #region Unity Methods

    new public void Reset()
    {
        base.Reset();
        _ContextAction = ContextActions.Dialogue;
    }

    new public void Awake()
    {
        base.Awake();

        // Set Input
        _ContextAction = ContextActions.Dialogue;

        // Set Events
        TargetCheck += FindClosestSpeaker;
        PowerOn += StartConversation;
        PowerOff += EndConversation;

        _StartLabel = "Speak";
        _EndLabel = "End";
    }

    new public void OnDisable()
    {
        base.OnDisable();
        DialogueGUI.CurrentLine = null;
        _CurrentLine = null;
        ThirdPersonController.enabled = true;
    }

    new public void OnEnable()
    {
        base.OnEnable();
        DialogueGUI.CurrentLine = _CurrentLine;
        ThirdPersonController.enabled = true;
    }

    new public void Update()
    {
        base.Update();
        if (_CurrentLine != null && Input.anyKeyDown)
        {
            if (ContextActions.Dialogue.Pressed)
                _HasInput = true;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                _CurrentLine.CurrentIndex++;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                _CurrentLine.CurrentIndex--;
        }
    }

    #endregion

    #region External Methods

    public bool FindClosestSpeaker(AbstractEntity argEntity)
    {
        Speaker speaker = argEntity as Speaker;
        AbstractBeing being = argEntity.GetComponent<AbstractBeing>();
        return speaker != null && being != null ? speaker.CanSpeakTo(Speaker) : false;
    }

    public IEnumerator StartConversation(AbstractEntity argEntity)
    {
        if (argEntity is Speaker)
        {
            yield return StartCoroutine(argEntity.Activate(Speaker));
            
            // Disable Controllers
            if(ThirdPersonController != null)
                ThirdPersonController.enabled = false;

            ThirdPersonCamera.offset += new Vector3(1, 1, 0) * 0.25f;
            ThirdPersonCamera.distance -= 0.5f;
            ThirdPersonCamera.height += 0.5f;

            StartCoroutine(argEntity.RotateTowards(Speaker.Transform.position - argEntity.Transform.position, 2));
            StartCoroutine(Speaker.RotateTowards(argEntity.Transform.position - Speaker.Transform.position, 2));

            // Execute Commands
            Dialogue dialogue = (argEntity as Speaker).GetDialogue(Speaker);
            yield return StartCoroutine(Read(dialogue));
        }
    }

    public IEnumerator Read(Dialogue dialogue)
    {
        _HasInput = false;
        if (dialogue != null)
        {
            DialogueGUI.TransIn = true;
            foreach (Line line in dialogue)
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
                while (!_HasInput && line.Choices.Count() != 0)
                {
                    ////debug.log(line.Choices.ToList()[0].Text);
                    ////debug.log("RUNNING");
                    yield return new WaitForFixedUpdate();
                }
                _HasInput = false;
            }
        }
    }

    public IEnumerator EndConversation(AbstractEntity argEntity)
    {
        if (argEntity is Speaker)
        {
            //Go to idle pose

            ThirdPersonCamera.offset -= new Vector3(1, 1, 0) * 0.25f;
            ThirdPersonCamera.distance += 0.5f;
            ThirdPersonCamera.height -= 0.5f;

            AbstractBeing being = argEntity.GetComponent<AbstractBeing>(); 
            if(being != null)
                being.Animation.CrossFade(AnimationResources.Idle); 
            
            yield return StartCoroutine(argEntity.Deactivate(Speaker));

            // Re-Enable Controllers
            if (ThirdPersonController != null)
                ThirdPersonController.enabled = true;

            // Disable GUIs
            RemoteGUI.Label = "";
            RemoteGUI.Action = null;
            //DialogueGUI.CurrentLine = null;
            DialogueGUI.TransOut = true;
        }
    }

    #endregion

    #endregion
}