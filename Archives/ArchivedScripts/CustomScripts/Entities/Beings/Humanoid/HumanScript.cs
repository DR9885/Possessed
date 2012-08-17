using UnityEngine;
using System;
using System.Linq;

public enum Mood
{
    Happy,
    Sad,
    Angry
}

[AddComponentMenu("Possessed/Being/Humanoid/HumanScript")]
public class HumanScript : AbstractLivingBeing
{
    //TODO: REPLACE BANDAID!!!
    private string _SadMood = "P_sad";
    private string _HappyMood = "P_happy";
    private string _AngryMood = "P_anger";

    Func<Mood, Mood, bool> OnMoodChange = null;

    #region Properties

    private DoorRemote _DoorRemote;
    public DoorRemote DoorRemote
    {
        get
        {
            if (_DoorRemote == null)
                _DoorRemote = GameObject.RequireComponentAdd<DoorRemote>();
            return _DoorRemote;
        }
    }

    private Speaker _Speaker;
    public Speaker Speaker
    {
        get
        {
            if (_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }

    public Mood Mood
    {
        get 
        {
            if (Speaker != null && Speaker.Tags != null)
            {

                var t = Speaker.Tags.Values("Mood").FirstOrDefault();
                if (t != null)
                    switch (t.ToLower())
                    {
                        case "sad": return global::Mood.Sad;
                        case "angry": return global::Mood.Angry;
                        case "happy": return global::Mood.Happy;
                    }
            }
            return Mood.Sad;
        }
    }

    private ParticleEmitter _SadParticles;
    private ParticleEmitter SadParticles
    {
        get
        {
            if (_SadParticles == null)
            {
                GameObject obj = Instantiate(ResourceManager.ParticleResources[_SadMood].gameObject) as GameObject;
                obj.transform.position = Transform.position + (Vector3.up / 2);
                obj.transform.parent = Transform;
                _SadParticles = obj.GetComponent<ParticleEmitter>();
            }
            return _SadParticles;
        }
    }

    private ParticleEmitter _HappyParticles;
    private ParticleEmitter HappyParticles
    {
        get
        {
            if (_HappyParticles == null)
            {
                GameObject obj = Instantiate(ResourceManager.ParticleResources[_HappyMood].gameObject) as GameObject;
                obj.transform.position = Transform.position + (Vector3.up / 2);
                obj.transform.parent = Transform;
                _HappyParticles = obj.GetComponent<ParticleEmitter>();
            }
            return _HappyParticles;
        }
    }


    #endregion


    #region Unity Methods

    new public void Reset()
    {
        base.Reset();
        Setup();
    }

    new public void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Setup()
    {
        _DoorRemote = DoorRemote;
    }

    //public void Update()
    //{
    //    /// Update MOOD From ID
    //    Speaker s = GetComponent<Speaker>();
    //    if (s != null && s.Tags != null)
    //    {
    //    //    Tag t = s.Tags.Find(x => x != null && x._Key.ToLower() == "mood");
    //    //    if (t != null)
    //    //    {
    //    //        if (t._Value.ToLower() == "happy")
    //    //            MoodChange(Mood, Mood.Happy);
    //    //        else
    //    //            MoodChange(Mood, Mood.Sad);
    //    //    }
    //    }
    //}

    private void MoodChange(Mood argOldMood, Mood argNewMood)
    {
//		//debug.log("MOOD CHANGE" + name);
        HideMood();
        WorldGUI.Mode = WorldGUI.Mode;

        if (OnMoodChange != null)
            OnMoodChange(argOldMood, argNewMood);
    }

    public void ShowMood()
    {
        switch(Mood)
        {
            case Mood.Happy:
                if (HappyParticles != null)
                {
                    HappyParticles.emit = true;
                    HappyParticles.gameObject.active = true;
                    //Animation.CrossFade(AnimationResources.HappyEmote);
                }
                break;
            case Mood.Sad:
                if (SadParticles != null)
                {
                    SadParticles.emit = true;
                    SadParticles.gameObject.active = true;
                    //Animation.CrossFade(AnimationResources.AngryEmote);
                }
                break;
        }
    }

    public void HideMood()
    {
        if (SadParticles != null)
        {
            SadParticles.emit = false;
            SadParticles.gameObject.active = false;
        }
        if (HappyParticles != null)
		{
//			//debug.log("HIDE HAPPY" + name);

            HappyParticles.emit = false;
		    HappyParticles.gameObject.active = false;
		}
    }

    #endregion
}