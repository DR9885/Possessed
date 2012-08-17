using UnityEngine;

[AddComponentMenu("Possessed/Being/Humanoid/MainPlayer-Ghost")]
public class MainPlayerGhost : GhostScript
{
    #region Properties

    private static bool _FindMainPlayerGhost = true;
    private static MainPlayerGhost _Instance;
    public static MainPlayerGhost Instance
    {
        get
        {
            if (_Instance == null && _FindMainPlayerGhost)
            {
                _Instance = FindObjectOfType(typeof (MainPlayerGhost)) as MainPlayerGhost;
                _FindMainPlayerGhost = false;
            }
            return _Instance;
        }
    }

    private PossessionRemote _PossessionRemote;
    public PossessionRemote PossessionRemote
    {
        get
        {
            if (_PossessionRemote == null)
                _PossessionRemote = GameObject.RequireComponentAdd<PossessionRemote>();
            return _PossessionRemote;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called when Object is Reset
    /// </summary>
    new public void Reset()
    {
        base.Reset(); // call base before control
        EnablePlayerControl();
        PossessionRemote.enabled = false;
    }

    /// <summary>
    /// Called When Level is Loaded
    /// </summary>
    new public void Awake()
    {
        base.Awake(); // call base before control

        if(MainPlayerHuman.Instance == null) EnablePlayerControl();
        PossessionRemote.enabled = false;
    }

    /// <summary>
    /// Called Once Awake is Complete
    /// </summary>
    new public void Start()
    {
        base.Start();

        if (MainPlayerHuman.Instance == null)
        {
            EnergyGUI.Energy = Energy;
            WorldGUI.Mode = Mode.Spirit;
        }
    }

    #endregion
}