using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[AddComponentMenu("Possessed/Being/Humanoid/GhostScript")]
public class GhostScript : AbstractBeing
{
    public Func<float, float, bool> OnEnergyChanged;

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

    [SerializeField]
    private float _Energy = 100;
    public float Energy
    {
        get { return _Energy; }
        set 
        {
            if (_Energy != value)
            {
                ScreenGUI.FlashColor = value > _Energy ? global::Energy.GoodColor : global::Energy.BadColor;

                var val = Mathf.Clamp(value, 0, 100);
                EnergyChanged(_Energy, val);
                _Energy = val;
            }
        }
    }

    #endregion

    #region Unity Methods

    new public void Reset()
    {
        base.Reset();

        CharacterController.height = 1;
        CharacterController.radius = 0.25f;

        DoorRemote.enabled = false;
    }

    new public void Awake()
    {
        ////debug.log(transform.GetInterfaceComponents<IBeingRemote<Component>>().Length);
        ////debug.log(transform.GetInterfaceComponents<Component>().Count());
        base.Awake();
    }

    #endregion

    #region Public Methods

    public void Show()
    {
        StartCoroutine(LerpAlpha(1, 1));
        ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();
        foreach (ParticleEmitter emitter in emitters)
            emitter.emit = true;


        //foreach (Transform trans in Transform)
        //    trans.gameObject.active = true;

        //if (GetComponent<MainPlayerGhost>() == null)
        //    gameObject.active = true;
    }

    public void Hide()
    {
        StartCoroutine(LerpAlpha(0, 1));
        ParticleEmitter[] emitters = GetComponentsInChildren<ParticleEmitter>();
        foreach (ParticleEmitter emitter in emitters)
            emitter.emit = false;

        //foreach (Transform trans in Transform)
        //    trans.gameObject.active = false;

        //if (GetComponent<MainPlayerGhost>() == null)
        //    gameObject.active = false;
        //else
        //    Transform.position -= Vector3.down * 10;

    }

    #endregion

    private void EnergyChanged(float argOld, float argNew)
    {
        EnergyGUI.Energy = argNew;

        if (OnEnergyChanged != null)
            OnEnergyChanged(argOld, argNew);
    }

    

	public void Possess(AbstractLivingBeing argBeing)
	{
        /*if (argBeing.name == "Squirrel")
        {
            SquirrelClimb climbScript = argBeing.GetComponent<SquirrelClimb>();
            climbScript.setControlled(argBeing.name); 
        }*/

        DisablePlayerControl();
        argBeing.EnablePlayerControl();

        Transform.position -= Vector3.up * 60;
	}

	public void Unpossess(AbstractLivingBeing argBeing)
	{
		EnablePlayerControl();  
		argBeing.DisablePlayerControl();
		Transform.position = argBeing.Transform.position + (Vector3.up * 0.6f);
		Transform.LookAt(argBeing.Transform);
    }
}