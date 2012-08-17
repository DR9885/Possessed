using UnityEngine;
using System.Collections;
using StateMachine;

[System.Serializable]
[AddComponentMenu("Possessed/Remotes/FightRemote")]
public class FightRemote : MonoBehaviour
{
    #region Field(s)

    //public enum Limb
    //{
    //    None = 0x0000,
    //    LeftArm = 0x0001,
    //    LeftLeg = 0x0002,
    //    RightArm = 0x0004,
    //    RightLeg = 0x0008
    //}
    [SerializeField]
    private int winsNeededToEscape = 3;

    [SerializeField]
    private float restTime = 5;

    private int dodgeOrHit = 0;
    private int totalWins = 0;
    private int fightIsOn = 0;

    private float timeUntilActive = 0;
    private AbstractBeing theTarget;
    private SteerForPursuit predSteer;
                        
    #endregion

    #region Helper Methods

    public void SetUpAnims()
    {
        AddCrossFadeTargetEvent(.7222f, AnimationResources.Attack, AnimationResources.Dodge);
    }

    private void AddCrossFadeTargetEvent(float t, string startAnimation, string responceAnimation)
    {
        var animationEvent = new AnimationEvent
        {
            functionName = "CrossFadeTargetEvent",
            stringParameter = responceAnimation,
            time = t
        };

        animation[startAnimation].clip.AddEvent(animationEvent);
    }

    public void CrossFadeTargetEvent(AnimationEvent animationEvent)
    {
        if (animationEvent == null) return;
        var anim = theTarget.animation;
        if (anim != null)
        {
            if (dodgeOrHit == 0)
            {
                
                anim.CrossFade(AnimationResources.Dodge);
                anim.CrossFadeQueued(AnimationResources.Freeze);
            }
            else
            {
                anim.CrossFade(AnimationResources.Flinch);
                anim.CrossFadeQueued(AnimationResources.Freeze);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (fightIsOn == 0)
        {
            AbstractBeing ab = other.gameObject.GetComponent<AbstractBeing>();
            AbstractBeing tb = gameObject.GetComponent<AbstractBeing>();

            Vector3 localPos = other.gameObject.transform.position - gameObject.transform.position;
            // Only Look At Animals
            if (ab is AbstractAnimal)
                StartCoroutine(tb.RotateTowards(localPos, .5f));

            if (tb is Wolf && Time.time > timeUntilActive)
            {
                if (ab is Squirrel || ab is Cat)
                {
                    ab.DisablePlayerControl();
                    fightIsOn = 1;
                    StartCoroutine(StartNewFight(ab, this.gameObject.GetComponent<AbstractLivingBeing>()));
                }
            }
            else if (tb is Cat && Time.time > timeUntilActive)
            {
                if (ab is Squirrel)
                {
                    ab.DisablePlayerControl();
                    fightIsOn = 1;
                    StartCoroutine(StartNewFight(ab, this.gameObject.GetComponent<AbstractLivingBeing>()));
                }
            }
        }
    }

    public IEnumerator StartNewFight(AbstractBeing argEntity, AbstractLivingBeing argFightableObject)
    {

        SetUpAnims();
        bool complete = false;
        int finished = 0;
        int step = 0;
        var quicktime = argFightableObject.GameObject.RequireComponentAdd<Quicktime>();

        theTarget = argEntity;

        gameObject.RequireComponentAdd<AutonomousVehicle>();
        predSteer = gameObject.RequireComponentAdd<SteerForPursuit>();
        predSteer.Quarry = argEntity.gameObject.RequireComponentAdd<AutonomousVehicle>();
        predSteer.enabled = true;
        ////debug.log("Enable chase");

        while (complete == false)
        {


            // Remove Energy if Failed
            //if (finished == 3)
            //Ghost.Animation.Blend(AnimationResources.PossWrongInput);

            // Fail Entirely only if No EnergyLeft);

            if (MainPlayerGhost.Instance.Energy <= 0)
            {
                complete = true;
                argEntity.Animation.Stop();
                argEntity.Animation.CrossFade(AnimationResources.Death);
                AbstractAnimal abAnimal = argEntity as AbstractAnimal;
                abAnimal._isDead = 1;
                abAnimal.AnimalSoundtrack.playDeath();
                StartCoroutine(MainPlayerGhost.Instance.PossessionRemote.TurnOff(argEntity));
            }


            // If Not Complete Run Quicktime Event
            if (!complete)
            {
                if (predSteer.enabled == false)
                {
                    switch (step)
                    {
                        case 0:
                            argFightableObject.Animation.CrossFade(AnimationResources.AttackDance);
                            argEntity.Animation.CrossFade(AnimationResources.Freeze);
                            break;
                    }
                    step++;
                    yield return StartCoroutine(quicktime.Fire());

                    finished = quicktime.Status;
                    if (finished == 2 && step != 0)
                    {
                        dodgeOrHit = 0;
                        totalWins++;
                        argFightableObject.Animation.Stop();
                        argFightableObject.Animation.Play(AnimationResources.Attack);
                        
                        AbstractAnimal abAnimal = argFightableObject as AbstractAnimal;
                        abAnimal.AnimalSoundtrack.playBattle();
                    }
                    else if (finished == 3 && step != 0)
                    {
                        dodgeOrHit = 1;
                        argFightableObject.Animation.Stop();
                        argFightableObject.Animation.Play(AnimationResources.Attack);

                        AbstractAnimal abAnimal = argFightableObject as AbstractAnimal;
                        abAnimal.AnimalSoundtrack.playBattle();
                    }

                    argFightableObject.Animation.CrossFadeQueued(AnimationResources.AttackDance);


                    if (totalWins >= winsNeededToEscape)
                    {
                        complete = true;
                        timeUntilActive = Time.time + restTime;
                        argEntity.EnablePlayerControl();
                        SteerForPursuit steer = this.gameObject.GetComponent<SteerForPursuit>();

                        quicktime.Stop();
                        steer.enabled = false;
                        totalWins = 0;
                        fightIsOn = 0;
                    }
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        yield return new WaitForFixedUpdate();
    }

    #endregion
}
