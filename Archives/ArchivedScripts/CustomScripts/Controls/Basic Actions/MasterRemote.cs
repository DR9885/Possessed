using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Vision))]
[AddComponentMenu("Possessed/Remotes/MasterRemote")]
public class MasterRemote : MonoBehaviour
{
    #region Field(s)

    private AbstractRemote _CurrentRemote = null;

    #endregion

    #region Properties

    private Transform _Transform;
    public Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            return _Transform;
        }
    }

    private AbstractRemote[] _Remotes;
    public AbstractRemote[] Remotes
    {
        get 
        {
            if(_Remotes == null)
                _Remotes = GetComponents<AbstractRemote>();
            return _Remotes;
        }
    }

    #endregion

    #region Methods

    public void OnDisable()
    {
        foreach (AbstractRemote remote in Remotes)
            if (remote != null)
            {
                remote.enabled = false;
            }
        
    }

    public void Update()
    {
        
        AbstractRemote closestRemote = null;
        
        if (Remotes.Any(x => x.FindTarget() != null) && !Remotes.Any(x => x.FindTarget() != null && x.FindTarget().Locked)
            || MainPlayerGhost.Instance.PossessionRemote.Target && MainPlayerGhost.Instance.PossessionRemote.Target.Active)
        {
            /// Find Closest Remote
            Profiler.BeginSample("inactiveRemotesInRange");
            IEnumerable<AbstractRemote> inactiveRemotesInRange = Remotes.Where(x => x.FindTarget() != null && !x.FindTarget().Active);
            if (inactiveRemotesInRange.Count() != 0)
                closestRemote = inactiveRemotesInRange.Aggregate((rem1, rem2) =>
                    (rem1 == null || Distance(rem1.FindTarget()) < Distance(rem2.FindTarget())) ? rem1 : rem2); // .Min(x => Vector3.Distance(Position, x.Target.Position));
            Profiler.EndSample();

            /// If No Close Remote, Find Active
            Profiler.BeginSample("activeRemotes");
            if (closestRemote == null)
            {
                IEnumerable<AbstractRemote> activeRemotes = Remotes.Where(x => x.FindTarget() != null && x.FindTarget().Active);
                if (activeRemotes.Count() != 0)
                    closestRemote = activeRemotes.Aggregate((x, y) => x.ContextAction.Priority < y.ContextAction.Priority ? x : y);
            }
            Profiler.EndSample();

            /// If Still no Close Remote, Find Check Main Player Possession Remote
            Profiler.BeginSample("MainPlayerGhost");
            if (closestRemote == null)
                if (MainPlayerGhost.Instance != null)
                    if (MainPlayerGhost.Instance.PossessionRemote != null)
                        if (MainPlayerGhost.Instance.PossessionRemote.Target != null)
                            if (MainPlayerGhost.Instance.PossessionRemote.Target.Active)
                                closestRemote = MainPlayerGhost.Instance.PossessionRemote;


            Profiler.EndSample();

            /// Change Remote if New
            if (closestRemote != _CurrentRemote)
            {
                if (_CurrentRemote != null)
                    _CurrentRemote.enabled = false;

                if (closestRemote != null)
                    closestRemote.enabled = true;

                _CurrentRemote = closestRemote;
            }
        }

    }

    public float Distance(AbstractEntity argObject)
    {
        return Vector3.Distance(argObject.transform.position, Transform.position);
    }


    #endregion
}