using UnityEngine;
using System.Collections;

/// <summary>
/// This Class's Main Purpose is to Store All Unity Properties
/// 
/// Normal Unity Properties call the "GetComponent" function each use.
/// </summary>
public abstract class UnityObject : MonoBehaviour 
{
    #region Feilds

    // General
    private GameObject ourGameObject = null;
    private Transform ourTransform = null;

    // Physics
    private Rigidbody ourRigidbody = null;
    private Collider ourCollider = null;

    // Visual
    private Animation ourAnimation = null;
    private Renderer ourRenderer = null;

    // Bool
    private bool isMoving = false;

    #endregion

    #region Properties

    #region General Properties

    public GameObject GameObject
    {
        get
        {
            /// if not stored, find object
            if (ourGameObject == null)
                ourGameObject = gameObject;
            return ourGameObject;
        }
    }

    public Transform Transform
    {
        get
        {
            /// if not stored, find object
            if (ourTransform == null)
                ourTransform = transform;
            return ourTransform;
        }
    }

    public Vector3 Position
    {
        set { Transform.position = value; }
        get { return Transform.position; }
    }
	
	public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    #endregion

    #region Physics Properties

    public Rigidbody Rigidbody
    {
        get
        {
            /// if not stored, find object
            if (ourRigidbody == null)
                ourRigidbody = rigidbody;
            return ourRigidbody;
        }
    }

    public Collider Collider
    {
        get
        {
            /// if not stored, find object
            if (ourCollider == null)
                ourCollider = collider;
            return ourCollider;
        }
    }

    #endregion

    #region Visual Properties

    public Animation Animation
    {
        get
        {
            /// if not stored, find object
            if (ourAnimation == null)
                ourAnimation = animation;
            return ourAnimation;
        }
    }

    public Renderer Renderer
    {
        get
        {
//            /// if not stored, find object
//            if (ourRenderer == null)
//                ourRenderer = renderer;

            /// if still not stored, use childs renderer
            if (ourRenderer == null)
            {
                Transform myRootChild = Transform.FindChild("JNT_Root");
                if(myRootChild != null)
                    ourRenderer = myRootChild.renderer;
            }
            if (ourRenderer == null)
                ourRenderer = GetComponentInChildren<Renderer>();

            return ourRenderer;
        }
    }
	
	public Vector3 CenterPoint
	{
		get 
		{
            if (Renderer != null)
            {
                return Renderer.bounds.center;
            }
            else
                return Position;
		}
	}

    private Transform ourRightHand;
    public Transform RightHand
    {
        get
        {
            if (ourRightHand == null)
                ourRightHand = FindChild(Transform, "JNT_R_Hand");

            if (ourRightHand == null)
                return Transform;

            return ourRightHand;
        }
    }

    private Transform ourLeftHand;
    public Transform LeftHand
    {
        get
        {
            if (ourLeftHand == null)
                ourLeftHand = FindChild(Transform, "JNT_L_Hand");

            if (ourLeftHand == null)
                return Transform;
            return ourLeftHand;
        }
    }

    private Transform ourHead;
    public Transform Head
    {
        get
        {
            if (ourHead == null)
                ourHead = FindChild(Transform, "JNT_HeadTop");
            if (ourHead == null)
                ourHead = FindChild(Transform, "JNT_Head");

            if (ourHead == null)
                return Transform;
            return ourHead;
        }
    }

    private Transform ourMouth;
    public Transform Mouth
    {
        get
        {
            if (ourMouth == null)
                ourMouth = FindChild(Transform, "JNT_Mouth");

            if (ourMouth == null)
                return Transform;
            return ourMouth;
        }
    }

    #endregion

    #endregion
	
	#region Methods

    private Transform FindChild(Transform currentTransform, string argName)
    {
        ////debug.log(currentTransform.name + " " + currentTransform.childCount);
        ////debug.log(currentTransform.name + " " + argName);
        if (currentTransform.name == argName)
            return currentTransform;
        else if (currentTransform.childCount != 0)
            foreach (Transform t in currentTransform)
            {
                Transform result = FindChild(t, argName);
                if(result != null)
                    return result;
            }
        return null;
    }

	#endregion
}
