using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Possessed/Quicktime")]
public class Quicktime : MonoBehaviour
{
    [System.Serializable]
    public class QuicktimeEvent
    {
        public float timeStart, timeEnd, alpha, keyNum, lifeTime;
        public int won = 0;
        public KeyCode whichKey;
        public Texture2D keyImg;
		public GameObject qteObject;
        public int marked = 0;

        public QuicktimeEvent(float ts, float te, KeyCode ke, Texture2D ki, GameObject quicktimeObject)
        {
            timeStart = ts;
            timeEnd = te;
            whichKey = ke;
            keyImg = ki;
            alpha = 1;
			qteObject = quicktimeObject;
        }
    }

    public class QuicktimeSequence
    {
        public float timeStart, timeEnd;
        public int won = 0;
        public QuicktimeEvent[] theEvents;
    }

    #region Fields

	public GameObject quicktimePrefab;
	
    private int buttonWins = 0, buttonFailure = 0;

    private int finished = 0;

    private Texture2D[] keyTextures;

    private int guiSet = 0;
    private int a = 0;

    [SerializeField]
    private float buttonDelay = 3, buttonLife = 3;
	
    [SerializeField]
    private int buttonsPerSequence;

    [SerializeField]
    private int possessionStep = 0, moveSpeed = 50, fadeDistance = 500;

    [SerializeField]
    private Size ButtonSize = new Size(128, 64), SpaceBetween = new Size(20, 20);

    [SerializeField]
    private float energyDrain = 10;

    //[SerializeField]
    private QuicktimeEvent[] eventQueue;
    private QuicktimeSequence eventSequence;


    #endregion

    #region Properties

    private Texture2D _SuccessTexture;
    private Texture2D SuccessTexture
    {
        get
        {
            if(_SuccessTexture == null)
                _SuccessTexture = ResourceManager.TextureResources["quicktime_good"];
            return _SuccessTexture;
        }
    }
    private Texture2D _FailureTexture;
    private Texture2D FailureTexture
    {
        get
        {
            if (_FailureTexture == null)
                _FailureTexture = ResourceManager.TextureResources["quicktime_bad"];
            return _FailureTexture;
        }
    }

    #endregion

    #region Public Methods

    public IEnumerator Fire()
    {
        finished = 1;
        eventSequence = GenerateNewSequence(buttonsPerSequence);
        
        while (finished != 2 && finished != 3)
            yield return new WaitForFixedUpdate();
    }

    public int Status
    {
        get { return finished; }
    }

    private bool _Complete = false;
    public IEnumerator FireCombo()
    {
        while (!_Complete)
        {

            yield return new WaitForFixedUpdate();
        }
    }

    //public  IEnumerator FireButton()
    //{
        
    //}

    #endregion

    QuicktimeSequence GenerateNewSequence(int size)
    {
        QuicktimeSequence theSequence = new QuicktimeSequence();
        QuicktimeEvent[] theQueue = new QuicktimeEvent[size];
        int buttonCount = 0;
        a = 0;
        float sequenceStart = Time.time;
        float sequenceEnd = Time.time;
        while (buttonCount < buttonsPerSequence)
        {
            float newTimeStart = Time.time + (buttonDelay * (buttonCount + 1));
            float newTimeEnd = newTimeStart + buttonLife;
            sequenceEnd = newTimeEnd;
        
            int rand = UnityEngine.Random.Range(0, ContextActions.QuickActions.Count - 1);

            KeyCode newKey;
            newKey = ContextActions.QuickActions[rand].KeyCode;
            Texture2D newTexture = ResourceManager.TextureResources[newKey];

            GameObject newQte = new GameObject();
            GUIMove guiMove = newQte.AddComponent<GUIMove>();
            guiMove.FireAway(newTimeStart, newTimeEnd, newTexture, buttonLife);

            theQueue[buttonCount] = new QuicktimeEvent(newTimeStart, newTimeEnd, newKey, newTexture, newQte);
//            //debug.log("Genearted: " + Time.time + " " + newTimeStart + " " + newTimeEnd + " " + buttonCount + " " + buttonsPerSequence + " gen");
            buttonCount++;
        }

        theSequence.theEvents = theQueue;
        theSequence.timeStart = sequenceStart;
        theSequence.timeEnd = sequenceEnd;

        return theSequence;
    }

    bool CheckQuicktime(int a, QuicktimeSequence sequence, KeyCode keyPressed)
    {
        return sequence.theEvents[a].whichKey == keyPressed;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished == 1)
        {
            KeyCode f;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                f = KeyCode.LeftArrow;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                f = KeyCode.RightArrow;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                f = KeyCode.DownArrow;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                f = KeyCode.UpArrow;
            else
                f = KeyCode.None;

            if (f != KeyCode.None)
            {
                if (CheckQuicktime(a, eventSequence, f) && f != KeyCode.None && eventSequence.theEvents[a].won == 0)
                {
                    eventSequence.theEvents[a].keyImg = SuccessTexture;
                    eventSequence.theEvents[a].won = 1;
                    GUIMove eqGUI = eventSequence.theEvents[a].qteObject.GetComponent<GUIMove>();
                    eqGUI.keyImg = SuccessTexture;
                    a++;
                }
                else if (!CheckQuicktime(a, eventSequence, f) && f != KeyCode.None && eventSequence.theEvents[a].won == 0)
                {
                    eventSequence.theEvents[a].keyImg = FailureTexture;
                    eventSequence.theEvents[a].won = -1;
                    GUIMove eqGUI = eventSequence.theEvents[a].qteObject.GetComponent<GUIMove>();
                    eqGUI.keyImg = FailureTexture;
                    MainPlayerGhost.Instance.Energy -= energyDrain;
                    a++;
                }
            }

            foreach (QuicktimeEvent qevent in eventSequence.theEvents)
            {
                if (Time.time > qevent.timeEnd && qevent.won == 0 && qevent.marked == 0)
                {
                    qevent.won = -1;
                    qevent.marked = 1;
                    buttonFailure++;
                    f = KeyCode.None;
                    MainPlayerGhost.Instance.Energy -= energyDrain;
                }

                if (qevent.won == 1 && qevent.timeEnd < Time.time && qevent.marked == 0)
                {
                    buttonWins++;
                    qevent.marked = 1;
                    f = KeyCode.None;
                }
                else if (qevent.won == -1 && qevent.timeEnd < Time.time && qevent.marked == 0)
                {
                    buttonFailure++;
                    qevent.marked = 1;
                    f = KeyCode.None;
                }
            }

            if (Time.time > eventSequence.timeEnd)
            {
                if (buttonFailure > 0)
                {
                    finished = 3;
                }
                else
                {
                    finished = 2;
                }
                a = 0;
                buttonWins = 0;
                buttonFailure = 0;
            }
        }
    }

    public void Stop()
    {
        finished = 0;
        ////debug.log("stopping quicktime");
    }
}