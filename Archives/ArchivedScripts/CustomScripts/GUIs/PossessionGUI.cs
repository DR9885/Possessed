using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Possessed/DNU/PossessionGUI")]
[RequireComponent(typeof(Animation))]
public class PossessionGUI : MonoBehaviour 
{
    //[System.Serializable]
    [RequireComponent(typeof(Animation))]
    public class QuicktimeEvent : MonoBehaviour
    {
        public float timeStart, timeEnd, alpha, keyNum, lifeTime;
        public int won = 0;
        public KeyCode whichKey;
        public Texture2D keyImg;
		public GameObject qteObject;

        public QuicktimeEvent(float ts, float te, KeyCode ke, Texture2D ki, GameObject quicktimeObject)
        {
            timeStart = ts;
            timeEnd = te;
            whichKey = ke;
            keyImg = ki;
            alpha = 1;
			qteObject = quicktimeObject;

            List<Keyframe> RotateFrames = new List<Keyframe>();
            RotateFrames.Add(new Keyframe(timeStart, 0));
            RotateFrames.Add(new Keyframe(timeEnd, 5));
            RotateAnim(RotateFrames);

        }
        private Animation _Animation;
        private static Animation Animation
        {
            get
            {
                if (Instance._Animation == null)
                    Instance._Animation = Instance.gameObject.RequireComponentAdd<Animation>();
                return Instance._Animation;
            }
        }

        [SerializeField]
        private float quickAlpha = 1;

        private float rotAngle = 0;
        [SerializeField]
        private float quickAnimLength = 5;
        [SerializeField]
        private float quickRad = 200;
        [SerializeField]
        private AnimationCurve Curve;
        List<Keyframe> Frames = new List<Keyframe>();



        public AnimationClip clip;

        public void RotateAnim(List<Keyframe> frameList)
        {
            //gt the starting size of the texture
            float startAngle = Instance.quickRot;

            Frames = frameList;

            /*for (int i = 0; i < 300; i++)
                Frames.Add(new Keyframe(i, i * i));*/
            //Frames.Add(new Keyframe(0,startSize));
            //Frames.Add(new Keyframe(growSpeed, val));
            AnimationCurve curve = new AnimationCurve(Frames.ToArray());

            List<Keyframe> AlphaFrames = new List<Keyframe>();
            AlphaFrames.Add(new Keyframe(timeEnd - 2, 1));
            AlphaFrames.Add(new Keyframe(timeEnd, 0));

            AnimationCurve curve2 = new AnimationCurve(AlphaFrames.ToArray());


            clip = new AnimationClip();
            //debug.log("we added" + Animation);
            clip.SetCurve("", typeof(QuicktimeEvent), "quickRot", curve);
            clip.SetCurve("", typeof(QuicktimeEvent), "quickAlpha", curve2);
            //clip.wrapMode = WrapMode.Loop;
            Animation.AddClip(clip, "test");

            AnimationEvent endEvent = new AnimationEvent();
            endEvent.time = clip.length;
            endEvent.functionName = "AlphaOut";
            Animation["test"].clip.AddEvent(endEvent);

            print("the clip length" + clip.length);
            Animation.Play("test");


        }

    }


    private static PossessionGUI _Instance;
    private static PossessionGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(PossessionGUI)) as PossessionGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(PossessionGUI).ToString());
                _Instance = obj.AddComponent<PossessionGUI>();
                _Instance.tag = "possessGUI";
            }
            return _Instance;
        }
    }

    

    private Animation _Animation;
    private static Animation Animation
    {
        get
        {
            if (Instance._Animation == null)
                Instance._Animation = Instance.gameObject.RequireComponentAdd<Animation>();
            return Instance._Animation;
        }
    }
    [SerializeField]
    private QuicktimeEvent[] evQueue;
	// Use this for initialization
	void Start () {
       

        int keyNum = 0;
        keyTextures = new Texture2D[ContextActions.QuickActions.Count];
        while (keyNum < ContextActions.QuickActions.Count)
        {
            keyTextures[keyNum] = ContextActions.QuickActions[keyNum].KeyImage;
            keyNum++;
        }

        evQueue = new QuicktimeEvent[buttonsPerSequence];
        //GenerateNewSequence(evQueue);
        //RunPossession(ref Quicktime.evQueue);
	}

    private Texture2D[] keyTextures;
    [SerializeField]
    private int buttonsPerSequence = 3;
    [SerializeField]
    private float buttonDelay = 3, buttonLife = 3;
	
	private int qteRunning = 0;
	
	private Quicktime engagedQuicktime;

    void GenerateNewSequence(QuicktimeEvent[] theQueue)
    {
        int z = 0;
        while (z < buttonsPerSequence)
        {
            float newTimeStart = Time.time + (buttonDelay * (z + 1));
            float newTimeEnd = newTimeStart + buttonLife;

            int rand = UnityEngine.Random.Range(0, ContextActions.QuickActions.Count - 1);

            KeyCode newKey;
            newKey = ContextActions.QuickActions[rand].KeyCode;
            Texture2D newTexture = WhichTexture(rand);

            ////debug.log(z + " " + theQueue.Length + " " + buttonsPerSequence);
            //theQueue[z] = new QuicktimeEvent(newTimeStart, newTimeEnd, newKey, newTexture);
            z++;
        }
        //PossessionGUI.RunPossession();
    }

    Texture2D WhichTexture(int key)
    {
        return keyTextures[key];
    }

    public void RunPossession(Quicktime theQuicktime)
    {
		engagedQuicktime = theQuicktime;
		qteRunning = 1;
		/*
        for (int i = 0; i < eventQueue.Length; i++)
        {
            //evQueue[i] = eventQueue[i];
        }

        foreach (Quicktime.QuicktimeEvent qEvent in eventQueue)
        {
            print("got a qEvent for ya start: "+qEvent.timeStart+"and end: "+qEvent.timeEnd);
        }
        print(evQueue.Length);
        List<Keyframe> RotateFrames = new List<Keyframe>();
        RotateFrames.Add(new Keyframe(0, 0));
        RotateFrames.Add(new Keyframe(quickAnimLength, 5));
        RotateAnim(RotateFrames);
		*/
    }

	

    private Texture2D _buttonLeft;
    private static Texture2D buttonLeft
    {
        get
        {
            if (Instance._buttonLeft == null)
                Instance._buttonLeft = ResourceManager.TextureResources["GUI_icon_LeftArrow"];
            return Instance._buttonLeft;
        }
    }

    private Texture2D _buttonRight;
    private static Texture2D buttonRight
    {
        get
        {
            if (Instance._buttonRight == null)
                Instance._buttonRight = ResourceManager.TextureResources["GUI_icon_RightArrow"];
            return Instance._buttonRight;
        }
    }

    private float quickX = 100;
    private float quickY = 100;
    private float quickWidth = 170;
    private float quickHeight = 120;
    [SerializeField]
    private float quickAlpha = 1;

    //private float rotAngle = 0;
    private Vector2 pivotPoint;

    [SerializeField]
    private float quickAnimLength = 5;
    [SerializeField]
    private float quickRad = 200;
    [SerializeField]
    private float quickRot = 0;

    [SerializeField]
    private AnimationCurve Curve;
    List<Keyframe> Frames = new List<Keyframe>();

    

    public AnimationClip clip;

    public void RotateAnim(List<Keyframe> frameList)
    {
        //gt the starting size of the texture
        float startAngle = Instance.quickRot;

        Frames = frameList;
        
        /*for (int i = 0; i < 300; i++)
            Frames.Add(new Keyframe(i, i * i));*/
        //Frames.Add(new Keyframe(0,startSize));
        //Frames.Add(new Keyframe(growSpeed, val));
        AnimationCurve curve = new AnimationCurve(Frames.ToArray());

        List<Keyframe> AlphaFrames = new List<Keyframe>();
        AlphaFrames.Add(new Keyframe(quickAnimLength -2, 1));
        AlphaFrames.Add(new Keyframe(quickAnimLength, 0));

        AnimationCurve curve2 = new AnimationCurve(AlphaFrames.ToArray());


        clip = new AnimationClip();
        //debug.log("we added"+Animation);
        clip.SetCurve("", typeof(PossessionGUI), "quickRot", curve);
        clip.SetCurve("", typeof(PossessionGUI), "quickAlpha", curve2);
        //clip.wrapMode = WrapMode.Loop;
        Animation.AddClip(clip, "test");

        AnimationEvent endEvent = new AnimationEvent();
        endEvent.time = clip.length;
        endEvent.functionName = "AlphaOut";
        Animation["test"].clip.AddEvent(endEvent);
        
        print("the clip length"+clip.length);
        Animation.Play("test");
        
        
    }

    public void AlphaOut()
    {
        
        List<Keyframe> AlphaFrames = new List<Keyframe>();
        AlphaFrames.Add(new Keyframe(clip.length, 1));
        AlphaFrames.Add(new Keyframe(clip.length + 2, 0));

        AnimationCurve curve2 = new AnimationCurve(AlphaFrames.ToArray());

        //AnimationClip clip2 = new AnimationClip();
        clip.SetCurve("", typeof(PossessionGUI), "quickAlpha", curve2);


        print("alphaout"+clip.length);
        //Animation.AddClip(clip2, "test2");
        //Animation.Play("test2");
        //Animation.CrossFade("test2");
    }

    private Rect _quickRect;
    private static Rect quickRect
    {
        get
        {
            if (Instance._quickRect == new Rect())
            {
               Instance._quickRect = new Rect(Instance.quickX, Instance.quickY, 70, 20);
            }
            return Instance._quickRect;
        }
    }

    

    public void OnGUI()
    {
        /*
		if(qteRunning == 1)
		{
			foreach(Quicktime.QuicktimeEvent qevent in engagedQuicktime.eventQueue)
			{
				float quickPosX = Screen.width / 2;
				float quickPosY = Screen.height / 2;
				
				//quickRot += .01f;
				quickX = quickRad * Mathf.Cos(qevent.quickRot);
				quickY = quickRad * Mathf.Sin(qevent.quickRot);

				//quickY = 100;
				GUI.color = new Color(1, 1, 1, qevent.quickAlpha);
				//buttonLeft.
				GUI.Label(new Rect((quickPosX - (quickWidth/2)) + quickX, (quickPosY - (quickHeight/2)) + quickY, quickWidth, quickHeight), buttonLeft);
				//print("print it");	
			}
		}
         */
    }
}
