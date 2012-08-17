
using System;
using UnityEngine;

[AddComponentMenu("Possessed/Being/Humanoid/MainPlayer-Human")]
public class MainPlayerHuman : HumanScript
{

    private static bool _FindMainPlayerGhost = true;
    private static MainPlayerHuman _Instance;
    public static MainPlayerHuman Instance
    {
        get
        {
            if (_Instance == null && _FindMainPlayerGhost)
            {
                _Instance = FindObjectOfType(typeof(MainPlayerHuman)) as MainPlayerHuman;
                _FindMainPlayerGhost = false;
            }
            return _Instance;
        }
    }

    new public void Awake()
    {
        base.Awake();

        EnablePlayerControl();

    }

    /// <summary>
    /// Called Once Awake is Complete
    /// </summary>
    new public void Start()
    {
        base.Start();

        EnablePlayerControl();
        WorldGUI.Mode = Mode.Living;
    }

    public void Update()
    {
        if (MainPlayerGhost.Instance != null)
        {
            if (Vector3.Distance(Transform.position, MainPlayerGhost.Instance.Transform.position) < 10)
            {
                Animation.Blend(AnimationResources.Drunk);
            }
            if (Vector3.Distance(Transform.position, MainPlayerGhost.Instance.Transform.position) < 5)
            {
                DisablePlayerControl();
                StartCoroutine(DeathSequence());
                enabled = false;
            }
        }
    }

    private System.Collections.IEnumerator DeathSequence()
    {
        DisablePlayerControl();
        SteerForPursuit steer = GameObject.RequireComponentAdd<SteerForPursuit>();
        AutonomousVehicle humanVehicle = GameObject.RequireComponentAdd<AutonomousVehicle>();
        RadarPing radar = GameObject.RequireComponentAdd<RadarPing>();
        SteerForSphericalObstacleAvoidance steer2 = GameObject.RequireComponentAdd<SteerForSphericalObstacleAvoidance>();
        yield return new WaitForFixedUpdate(); 

        MainPlayerGhost.Instance.ThirdPersonCamera.enabled = true;
    
        yield return new WaitForSeconds(1);
        Animation.CrossFade(AnimationResources.Walk);
        Animation.Blend(AnimationResources.Drunk);
        AutonomousVehicle ghostVehicle = MainPlayerGhost.Instance.GameObject.RequireComponentAdd<AutonomousVehicle>();
        steer._quarry = ghostVehicle;
        steer.enabled = true;


        while (steer.enabled)
            yield return new WaitForFixedUpdate();

        Animation.CrossFade(AnimationResources.Death);
        while(Animation.IsPlaying(AnimationResources.Death))
            yield return new WaitForFixedUpdate();

        MainPlayerGhost.Instance.ThirdPersonCamera.enabled = false;
        MainPlayerHuman.Instance.ThirdPersonCamera.enabled = true;
        yield return new WaitForSeconds(1);
        MainPlayerGhost.Instance.Transform.position = MainPlayerHuman.Instance.Transform.position;
        MainPlayerGhost.Instance.Transform.rotation = MainPlayerHuman.Instance.Transform.rotation;
        
        WorldGUI.Mode = Mode.Spirit;

        MainPlayerHuman.Instance.DisablePlayerControl();
        MainPlayerGhost.Instance.EnablePlayerControl();
        Soundtrack.PlayClip("GhostLayer", 2);

        Destroy(this);
        Destroy(ThirdPersonController);
        Destroy(ThirdPersonCamera);
        Destroy(CharacterController);
    }
}