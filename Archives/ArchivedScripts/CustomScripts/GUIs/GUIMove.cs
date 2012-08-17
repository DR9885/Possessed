using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animation))]
public class GUIMove : MonoBehaviour 
{
    public float timeStart, timeEnd, timeOrig;

    //public QuickTimeEvent 

    AnimationClip clip;

    [SerializeField]
    public float quickAnimLength = 5;
    [SerializeField]
    public float quickRad = 200;
    [SerializeField]
    public float quickRot;
    [SerializeField]
    public float quickAlpha = 1;

    public float buttonLife = 0;


    public Texture2D keyImg;

    List<Keyframe> Frames = new List<Keyframe>();

    private Animation _Animation;
    private Animation Animation
    {
        get
        {
            if (_Animation == null)
                _Animation = gameObject.RequireComponentAdd<Animation>();
            return _Animation;
        }
    }

    public void AlphaOut()
    {

        List<Keyframe> AlphaFrames = new List<Keyframe>();
        AlphaFrames.Add(new Keyframe(clip.length, 1));
        AlphaFrames.Add(new Keyframe(clip.length + 2, 0));

        AnimationCurve curve2 = new AnimationCurve(AlphaFrames.ToArray());

        //AnimationClip clip2 = new AnimationClip();
        clip.SetCurve("", typeof(PossessionGUI), "quickAlpha", curve2);


        print("alphaout" + clip.length);
        //Animation.AddClip(clip2, "test2");
        //Animation.Play("test2");
        //Animation.CrossFade("test2");
    }

    void Complete()
    {
        Destroy(gameObject);
    }

    public void RotateAnim(List<Keyframe> frameList)
    {
            //Animation theAnim = gameObject.GetComponent<Animation>();
            //gt the starting size of the texture
            float startAngle = quickRot;

            Frames = frameList;

            /*for (int i = 0; i < 300; i++)
                Frames.Add(new Keyframe(i, i * i));*/
            //Frames.Add(new Keyframe(0,startSize));
            //Frames.Add(new Keyframe(growSpeed, val));
            AnimationCurve curve = new AnimationCurve(Frames.ToArray());

            List<Keyframe> AlphaFrames = new List<Keyframe>();
            AlphaFrames.Add(new Keyframe(timeEnd - 1, 1));
            AlphaFrames.Add(new Keyframe(timeEnd, 0));

            AnimationCurve curve2 = new AnimationCurve(AlphaFrames.ToArray());


            clip = new AnimationClip();
            clip.SetCurve("", typeof(GUIMove), "quickRot", curve);
            clip.SetCurve("", typeof(GUIMove), "quickAlpha", curve2);

            //clip.wrapMode = WrapMode.Loop;
            Animation.animation.AddClip(clip, "test");

            AnimationEvent endEvent = new AnimationEvent();
            endEvent.time = clip.length;
            endEvent.functionName = "Complete";
            Animation["test"].clip.AddEvent(endEvent);

            //print("the clip length" + clip.length);
            Animation.animation.Play("test");
            
     }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireAway(float ts, float te, Texture2D ki, float bl)
    {

        timeOrig = ts;
        timeStart = ts - Time.time;
        timeEnd = te - Time.time;

        keyImg = ki;
        quickRot = 0;
        buttonLife = bl;
        List<Keyframe> RotateFrames = new List<Keyframe>();
        RotateFrames.Add(new Keyframe(timeStart, 0));
        RotateFrames.Add(new Keyframe(timeEnd, 5));
        RotateAnim(RotateFrames);
    }

    private float quickX = 100;
    private float quickY = 100;
    private float quickWidth = 170;
    private float quickHeight = 120;

    public void OnGUI()
    {
        
        if (Time.time > timeOrig)
        {
            float quickPosX = Screen.width / 2;
            float quickPosY = Screen.height / 2;

            //quickRot += .01f;
            quickX = quickRad * Mathf.Cos(quickRot);
            quickY = quickRad * Mathf.Sin(quickRot);


            //quickY = 100;
            GUI.color = new Color(1, 1, 1, quickAlpha);
            //buttonLeft.
            GUI.Label(new Rect((quickPosX - (quickWidth / 2)) + quickX, (quickPosY - (quickHeight / 2)) + quickY, quickWidth, quickHeight), keyImg);
            //print("print it");	
        }
          
    }
}
