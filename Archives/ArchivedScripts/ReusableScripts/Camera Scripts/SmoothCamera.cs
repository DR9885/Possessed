using UnityEngine;
using System.Collections;

[AddComponentMenu("Possessed-UnderConstruction/Camera-Control/SmoothCamera")]
public class SmoothCamera : MonoBehaviour
{
    enum BumperType { Raycast, Collider }

    #region Static Properties
    #region Private Properties

    private static SmoothCamera instance = null;
    public static SmoothCamera Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(SmoothCamera)) as SmoothCamera;
            if (instance == null && Camera.main != null)
                instance = Camera.main.gameObject.AddComponent(typeof(SmoothCamera)) as SmoothCamera;
            return instance;
        }
    }

    // Unity property "transform" calls GetComponent(Transform), so we store the value
    private Transform ourCameraTransform;
    private static Transform CameraTransform
    {
        get
        {
            if (Instance.ourCameraTransform == null)
                Instance.ourCameraTransform = Instance.transform;
            return Instance.ourCameraTransform;
        }
    }

    #endregion
    #region Public Properties

    [SerializeField] private Transform target = null;
    public static Transform Target
    {
        set { Instance.target = value; }
        get { return Instance.target; }
    }

    #region Public Properties Camera Position

    [SerializeField] private float distance = 3.0f;
    public static float Distance
    {
        get { return Instance.distance; }
        set { Instance.distance = value; }
    }

    [SerializeField] private float height = 1.0f;
    public static float Height
    {
        get { return Instance.height; }
        set { Instance.height = value; }
    }

//    [SerializeField] private float pan = 45.0f;
    public static float Pan
    {
        get { return Instance.distance; }
        set { Instance.distance = value; }
    }

    [SerializeField] private Vector3 lookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights
    public static Vector3 LookAtOffset
    {
        get { return Instance.lookAtOffset; }
        set { Instance.lookAtOffset = value; }
    }
    [SerializeField] private float damping = 5.0f;

    public float Damping
    {
        get { return damping; }
        set { damping = value; }
    }

    private enum CameraMovementType { Instant, LinearInterpolation, SphericalLinearInterpolation }
    [SerializeField] private CameraMovementType cameraRotation = CameraMovementType.SphericalLinearInterpolation;
    private CameraMovementType CameraRotation
    {
        get { return cameraRotation; }
        set { cameraRotation = value; }
    }

    [SerializeField] private float rotationDamping = 10.0f;
    public float RotationDamping
    {
        get { return rotationDamping; }
        set { rotationDamping = value; }
    }

    #endregion
    [SerializeField] private float bumperDistanceCheck = 2.5f; // length of bumper ray
    public float BumperDistanceCheck
    {
        get { return bumperDistanceCheck; }
        set { bumperDistanceCheck = value; }
    }

    [SerializeField] private float bumperCameraHeight = 1.0f; // adjust camera height while bumping
    public float BumperCameraHeight
    {
        get { return bumperCameraHeight; }
        set { bumperCameraHeight = value; }
    }

    [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin
    public Vector3 BumperRayOffset
    {
        get { return bumperRayOffset; }
        set { bumperRayOffset = value; }
    }

    #endregion

    private RaycastHit hit;
    private static RaycastHit Hit
    {
        get { return Instance.hit; }
    }

    private bool IsBumperHit
    {
        get {
            // check to see if there is anything behind the target
            Vector3 back = target.transform.TransformDirection(-1 * Vector3.forward);

            // cast the bumper ray out from rear and check to see if there is anything behind
            return Physics.Raycast(target.TransformPoint(bumperRayOffset), back, out hit, bumperDistanceCheck) 
                && hit.transform != target;
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        FocusTarget(target);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        ///this is the thing that needs to be altered by the axis 3 and 4 changing
        Vector3 wantedPosition = target.TransformPoint(Pan, height, -distance);

        // if Bumper Is Hit
        if (IsBumperHit)
        {
            print("BUMPER IS HIT!!!!! ALERTS!");
            // clamp wanted position to hit position
            ///these values need to be altered when hitting walls and ceilings and floors and etc
            wantedPosition.x = hit.point.x;
            wantedPosition.z = hit.point.z;
            wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
        }

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        /// Rotate Camera
        Vector3 lookPosition = target.TransformPoint(lookAtOffset);
        Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);

        switch (cameraRotation)
        {
            case CameraMovementType.Instant:
                transform.rotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
                break;
            case CameraMovementType.LinearInterpolation:
                transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                break;
            case CameraMovementType.SphericalLinearInterpolation:
                transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                break;
        }
    }

    #endregion

    #region Private Methods



    #endregion

    #region Public Methods

    public static void FocusTarget(Transform argTarget)
    {
        Target = argTarget;
        UnityObject Object = Target.GetComponent(typeof(UnityObject)) as UnityObject;
        if (Object)
        {
            float myYOffSet = Object.Renderer.bounds.size.y;
            LookAtOffset = Vector3.up * myYOffSet;
            Height = myYOffSet + .5f;
            Distance = Height + .5f;

        }

    }

    #endregion
}

