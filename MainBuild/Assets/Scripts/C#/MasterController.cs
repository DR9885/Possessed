using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MasterController : MonoBehaviour
{
    public FiniteStateMachine<MasterController> FSM = new FiniteStateMachine<MasterController>();

    public IEnumerable<IFiniteState<MasterController>> Controllers
    {
        get
        {
            return GetComponents<MonoBehaviour>()
                .Where(x => x as IFiniteState<MasterController> != null)
                .Select(x => x as IFiniteState<MasterController>);
        }
    }
        

    private void Awake()
    {


        Debug.Log(Controllers.Count());
        FSM.Init(this, Controllers.FirstOrDefault());
    }

    private void FixedUpdate()
    {
        FSM.OnDecision();
    }
}
