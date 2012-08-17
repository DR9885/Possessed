using UnityEngine;
using System.Collections;

public class Xbox360Controller : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	    
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            //if Input.
            print("left stick horizontal axis");
        }

        if (Input.GetButtonDown("Vertical"))
        {
            print("left stick vertical axis");
        }

        if (Input.GetButtonDown("JumpXbox"))
        {
            print("Xbox jump button");
        }
        print("TESTTESTTESTTEST"); 
	}
}
