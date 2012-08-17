using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("Possessed/Remotes/SquirrelClimbRemote")]
public class SquirrelClimbRemote : AbstractRemote
{
    private bool climbCheck = false; 
    private GameObject _GameObject;
    private float climb1X = -14.62f;
    private float climb2X = 11.0f;

    private float topClimb1Y = 0.018f;
    private float topClimb2Y = 8.31f;

    private float climb1Z = 5.80f;
    private float climb2Z = -5.4f;

    private bool climb1 = false;
    private bool climb2 = false;

    private float climb1TopX = -15.223f; 
    private float climb2TopX = 11.81f;
    

    private Speaker _Speaker;
    private Speaker Speaker
    {
        get
        {
            if (_Speaker == null)
                _Speaker = GetComponent<Speaker>();
            return _Speaker;
        }
    }
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

    new public void Reset()
    {
        base.Reset();
        ///Set Input
        _ContextAction = ContextActions.SquirrelClimb; 
    }

    new public void Awake()
    {
        base.Awake();

        //Set Input
        _ContextAction = ContextActions.SquirrelClimb;

        ///Set Labels
        _StartLabel = "Climb";
        _EndLabel = "Stop climbing is this necessary even?";

        ///Set Events
        TargetCheck += FindClimbableSurface;
        PowerOn += StartClimbing;
        PowerOff += StopClimbing;

        //print("is turned on");
    }

    protected bool FindClimbableSurface(AbstractEntity argEntity)
    {
        return argEntity.GetType() == typeof(IvyClimbingBox); 
    }

    protected IEnumerator StartClimbing(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Activate(Speaker));

        GetComponent<ThirdPersonController>().enabled = false;


        animation[AnimationResources.Climb].speed = 1.5f; 
        GameObject.animation.CrossFade(AnimationResources.Climb); 
        

        GetComponent<ThirdPersonController>()._Gravity = 0;
        GetComponent<ThirdPersonController>().MoveSpeed = 0;
        climbCheck = true;
        

        yield return new WaitForFixedUpdate();
    }

    protected IEnumerator StopClimbing(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.Deactivate(Speaker));

        GetComponent<ThirdPersonController>().enabled = true; 

        GetComponent<ThirdPersonController>()._Gravity = 20;

        climbCheck = false;

        yield return new WaitForFixedUpdate();
    }

    public Vector3 climb1Rotate = new Vector3(0, 274, 0); 
    public Vector3 climb2Rotate = new Vector3(0, 75, 0);
    Quaternion Q_rotation ;

    public void OnDisable()
    {
        base.OnDisable();
    }
    new public void OnEnable()
    {
        base.OnEnable();
    }

	// Update is called once per frame
	void Update () 
    {        
        checkClimbArea();
        if (climbCheck == true)
        {
            if (climb1)
            {
                if (GameObject.rigidbody.position.y < topClimb1Y)
                {
                    if (GameObject.rigidbody.position.z < climb1Z)
                    {
                        transform.position += transform.right * (Time.deltaTime / 2);
                    }
                    if (GameObject.rigidbody.position.z > climb1Z)
                    {
                        transform.position -= transform.right * (Time.deltaTime / 2);
                    }
                    GameObject.rigidbody.transform.position += Vector3.up * (Time.deltaTime * 0.85f);
                    
                }
                else if (GameObject.rigidbody.position.y >= topClimb1Y)
                {
                    if (GameObject.rigidbody.position.x >= climb1TopX)
                    {
                        GameObject.animation.CrossFade(AnimationResources.Walk);
                        transform.position += transform.forward * (Time.deltaTime / 2);
                    }
                }
            }
            else if (climb2)
            {                

                if (GameObject.rigidbody.position.y < topClimb2Y)
                {
                    if (GameObject.rigidbody.position.z > climb2Z)
                    {
                        transform.position += transform.right * (Time.deltaTime / 2);
                    }
                    if (GameObject.rigidbody.position.z < climb2Z)
                    {
                        transform.position -= transform.right * (Time.deltaTime / 2);
                    }
                    GameObject.rigidbody.transform.position += Vector3.up * (Time.deltaTime * 0.85f);
                    lowerCamera();
                }
                else if (GameObject.rigidbody.position.y >= topClimb2Y)
                {
                    if (GameObject.rigidbody.position.x < climb2TopX)
                    {
                        GameObject.animation.CrossFade(AnimationResources.Walk);
                        transform.position += transform.forward * (Time.deltaTime / 2);
                    }
                    raiseCamera(); 
                }
            }
            //print("test");
            GameObject.rigidbody.MoveRotation(Q_rotation);

            

        }
        else
        {
            GetComponent<ThirdPersonCamera>().height = 0.05f; 
        }
        base.Update();       
	}

    void raiseCamera()
    {
        if (GetComponent<ThirdPersonCamera>().height < 0.05f)
        {
            GetComponent<ThirdPersonCamera>().height += 0.05f;
        }
        else
            GetComponent<ThirdPersonCamera>().height = 0.05f;


    }
    void lowerCamera()
    {
        if (GetComponent<ThirdPersonCamera>().height >= -0.5f)
        {
            GetComponent<ThirdPersonCamera>().height -= 0.002f;
        }
    }

    void checkClimbArea()
    {
        ///1 interior = -15.44, 0.017, 5.79
        ///1 interior ends, -15.94, 0.015, 5.728
        ///1 starts = -14.62, -1.24, 5.86

        /*private float climb1X = -14.62f;
        private float climb2X = 11.0f;

        private float climb1Z = 5.80f;
        private float climb2Z = -5.4f;*/

        if ((GameObject.rigidbody.position.x > (climb1X - 0.5f)) && (GameObject.rigidbody.position.x < (climb1X + 0.5f)))
        {
            if ((GameObject.rigidbody.position.z > (climb1Z - 0.5f)) && (GameObject.rigidbody.position.z < (climb1Z + 0.5f)))
            {
                climb1 = true;
                climb2 = false;
            }
        }

        if ((GameObject.rigidbody.position.x > (climb2X - 0.5f)) && (GameObject.rigidbody.position.x < (climb2X + 0.5f)))
        {
            if ((GameObject.rigidbody.position.z > (climb2Z - 0.5f)) && (GameObject.rigidbody.position.z < (climb2Z + 0.5f)))
            {
                climb1 = false;
                climb2 = true;                
            }
        }

        if (climb1)
        {
            Q_rotation = Quaternion.Euler(climb1Rotate);
        }
        else if (climb2)
        {
            Q_rotation = Quaternion.Euler(climb2Rotate);
        }
        else
        {
            Q_rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<ThirdPersonCamera>().height = 0.05f; 
        }


    }
}
