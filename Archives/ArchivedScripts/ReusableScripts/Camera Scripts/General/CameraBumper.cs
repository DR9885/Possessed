using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
class CameraBumper
{
    #region Fields

    private RaycastHit hit;
    private bool isColliderHit;

    #endregion

    #region Properties

    #region Private Properties

    private GameObject ourBumper;
    private GameObject Bumper
    {
        get
        {
            if (ourBumper == null)
                ourBumper = new GameObject("Bumper");
            return ourBumper;
        }
        set { ourBumper = value; }
    }




    #endregion

    #region Public Properties

    public enum CollisionType { None, Raycast, Collider }
    [SerializeField] private CollisionType collisionType = CollisionType.Raycast; // length of bumper ray
    public CollisionType Collision
    {
        get { return collisionType; }
        set { collisionType = value; }
    }

    [SerializeField] private float distanceCheck = 2.5f; // length of bumper
    public float DistanceCheck
    {
        get { return distanceCheck; }
        set { distanceCheck = value; }
    }

    [SerializeField] private float newCameraHeight = 1.0f; // adjust camera height while bumping
    public float NewCameraHeight
    {
        get { return newCameraHeight; }
        set { newCameraHeight = value; }
    }

    [SerializeField] private Vector3 offset = Vector3.zero; // allows offset of the bumper ray from target origin
    public Vector3 Offset
    {
        get { return offset; }
        set { offset = value; }
    }

    private List<Transform> ourIgnores = new List<Transform>();
    public List<Transform> Ignores
    {
        get { return ourIgnores; }
        set { ourIgnores = value; }
    }

    private List<Type> ourIgnoreTypes = new List<Type>();
    public List<Type> IgnoreTypes
    {
        get { return ourIgnoreTypes; }
        set { ourIgnoreTypes = value; }
    }
    #endregion

    #endregion

    #region Methods

    public bool IsBumperHit(Transform argTarget, Transform argCamera, Vector3 argWantedPosition)
    {
        switch (collisionType)
        {
            case CollisionType.Collider:
                //TODO:
            case CollisionType.Raycast:

                //Store Layer and Set to Ignore our Raycast
                int targetStartLayer = argTarget.gameObject.layer;
                int cameraStartLayer = argCamera.gameObject.layer;
                argTarget.gameObject.layer = 2; // 2 = ignore Raycast
                argCamera.gameObject.layer = 2; // 2 = ignore Raycast


                Vector3 delta = argWantedPosition - argTarget.position;
                float dist = Vector3.Distance(argWantedPosition, argTarget.position);

                // Restore Layer
                argTarget.gameObject.layer = targetStartLayer;
                argCamera.gameObject.layer = cameraStartLayer;
			
			
                bool results = Physics.Raycast(argTarget.TransformPoint(offset), delta.normalized, out hit, dist);
			
				if(results)
					if(argTarget.gameObject.name == hit.transform.name || argCamera.gameObject.name == hit.transform.name)
						results = false;
				
                //if(results)
                //    if(hit.transform.GetComponent(typeof(BaseGameEntity)) != null)
                //        results = false;
			
				return results;

                ////TODO: Remove, OLD
                //// check to see if there is anything behind the target
                //Vector3 back = argTarget.transform.TransformDirection(-1 * Vector3.forward);
                //// cast the bumper ray out from rear and check to see if there is anything behind
                //return Physics.Raycast(argTarget.TransformPoint(offset), back, out hit, distanceCheck);


            default: return false;
        }
    }

    public Vector3 UpdatePosition(Transform argTarget, Transform argCamera, Vector3 argWantedPosition, float argT)
    {
        Vector3 cameraPos = argCamera.position;

        cameraPos.x = hit.point.x;
        cameraPos.z = hit.point.z;
        cameraPos.y = Mathf.Lerp(hit.point.y + newCameraHeight, cameraPos.y, argT);
        return cameraPos;
    }

    #endregion
}
