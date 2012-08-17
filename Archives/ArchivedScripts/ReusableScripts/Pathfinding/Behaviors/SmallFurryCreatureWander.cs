using UnityEngine;
using System.Collections;

public class SmallFurryCreatureWander : WanderAI {
    
    private GameObject _GameObject; 
    public GameObject GameObject
    {
        get
        {
            if (_GameObject == null)
            {
                _GameObject = gameObject;
            }
            return _GameObject;
        }
    }

    bool CheckToSeeIfControlled()
    {

        if (GameObject.camera == null)
        {
            return true;
        }
        else 
            return false;

    }

	// Update is called once per frame
	void Update () 
    {
	
	}
}
