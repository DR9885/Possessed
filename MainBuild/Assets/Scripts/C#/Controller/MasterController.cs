using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum MasterControlerState
{
    None,
    Door,
    PickUp,
    Possession
}

public enum ControllerState
{
    Idle,
    Target,
    Active
}

public class MasterController : MonoBehaviour 
{

}
