using UnityEngine;
using System.Collections;

public class WanderAI: MonoBehaviour 
{
    private Vector2 startingLocation;
    private float distX;
    private float distY;

    Random randSeed;
    Random randDistMove; 
    
    private bool wander = false;

    [SerializeField]
    private float maxWander = 25; 

    private GameObject _GameObject;
    public GameObject GameObject
    {
        get
        {
            if (_GameObject == null)
            {
                _GameObject = gameObject;
                //print("GO assigned");
            }
            return _GameObject;
        }
    }
	// Use this for initialization
	void Start () 
    {
        startingLocation = GameObject.rigidbody.position;

        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (checkAnim())
        {
            if (CheckDistance())
            {
                wander = true;
            }
        }
        else
            wander = false; 
	}

    private bool checkAnim()
    {
        if (GameObject.animation.active)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckDistance()
    {
        //randSeed = new Random.seed(randSeed); 
        //randDistMove = 

        distX = gameObject.rigidbody.position.x - startingLocation.x;
        distY = gameObject.rigidbody.position.y - startingLocation.y;

        if ((distY <= maxWander) && (distX <= maxWander))
        {
            return true;
        }

        else
            return false; 
    }
}
