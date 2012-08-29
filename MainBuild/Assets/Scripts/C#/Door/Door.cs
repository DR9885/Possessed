using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Animation _animation;
    public Animation Animation
    {
        get
        {
            if(_animation == null)
                _animation = animation;
            return _animation;
        }
    }

    public FiniteStateMachine<Door> FSM = new FiniteStateMachine<Door>();

    public void Awake()
    {
        FSM.Init(this, null);
    }

}
