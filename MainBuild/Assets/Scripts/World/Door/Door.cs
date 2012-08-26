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
}
