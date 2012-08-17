using UnityEngine;

[RequireComponent(typeof(PickupRemote))]
public abstract class AbstractLivingBeing : AbstractBeing, ILiving
{
    #region Properties

    private PickupRemote _PickupRemote;
    public PickupRemote PickupRemote
    {
        get
        {
            if (_PickupRemote == null)
                _PickupRemote = GameObject.RequireComponentAdd<PickupRemote>();
            return _PickupRemote;
        }
    }

    public bool IsPossessed
    {
        get { return IsPlayerControlling; }
    }

    #endregion

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
        _PickupRemote = PickupRemote;
    }

    #region Methods

    public void Possess(GhostScript argGhost)
    {
        argGhost.Hide();

        EnablePlayerControl();
    }

    public void Unpossess(GhostScript argGhost)
    {
        argGhost.Show();
        argGhost.Transform.position = Renderer.bounds.max + Vector3.up;
        print("I AM GETTING CALLED");
        DisablePlayerControl();
        print("NO REALLY I AM PLEASE BELIVE ME");
        print("I'LL DO ANYTHING"); 
        argGhost.EnablePlayerControl();
        
        argGhost.Transform.LookAt(Transform);
    }

    #endregion
}

