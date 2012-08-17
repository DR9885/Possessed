using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Reusable/Vision")]
public class Vision : MonoBehaviour
{

    #region Fields

    [SerializeField]
    public List<GameObject> _ObjectsInRadius = new List<GameObject>();
    public List<GameObject> ObjectsInRadius
    {
        get { return _ObjectsInRadius; }
    }

    [SerializeField]
    private float _Distance = 3f;

    #endregion

    #region Properties

    private GameObject _GameObject;
    public GameObject GameObject
    {
        get
        {
            if (_GameObject == null)
                _GameObject = gameObject; // "gameObject" calls method GetComponent<GameObject>() each time
            return _GameObject;
        }
    }


    private Transform _Transform;
    public Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform; // "transform" calls method GetComponent<Transform>() each time
            return _Transform;
        }
    }

    private Rigidbody _Rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            if (_Rigidbody == null)
                _Rigidbody = rigidbody; // "rigidbody" calls method GetComponent<Transform>() each time
            if (_Rigidbody == null)
                _Rigidbody = GameObject.AddComponent<Rigidbody>();
            return _Rigidbody;
        }
    }

    #endregion

    #region Unity Methods 

    void Reset()
    {
        if (!HasVision())
            BuildVision();

        if (Rigidbody)
            Rigidbody.useGravity = false;
    }

    /// <summary>
    /// Something has entered the field of view, add it to the list
    /// </summary>
    /// <param name='other'>
    /// The thing that has entered the view
    /// </param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GameObject)
        {
            if(!_ObjectsInRadius.Contains(other.gameObject))
                _ObjectsInRadius.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Something has left the field of view, remove it from the list
    /// </summary>
    /// <param name='other'>
    /// The thing that has entered the view
    /// </param>
    public void OnTriggerExit(Collider other)
    {
        _ObjectsInRadius.Remove(other.gameObject);
    }

    #endregion

    #region Internal Methods

    private bool HasVision()
    {
        foreach (Collider c in GetComponents<BoxCollider>())
            if (c.isTrigger)
                return true;
        return false;
    }

    private void BuildVision()
    {
        BoxCollider c = GameObject.AddComponent<BoxCollider>();
        c.size = Vector3.one * _Distance;
        c.center += Vector3.forward * ( (_Distance / 4f));// + Camera.main.nearClipPlane);
        c.isTrigger = true;
    }

    #endregion

    /// <summary>
    /// Get a list of all of X component
    /// </summary>
    /// <returns>
    /// A list of all of X components
    /// </returns>
    public List<T> GrabComponent<T>() where T : Component
    {
        List<T> componentList = new List<T>();

        foreach (GameObject theObject in _ObjectsInRadius)
        {
            T theComponent = theObject.GetComponent<T>();
            componentList.Add(theComponent);
        }

        return componentList;
    }

    /// <summary>
    /// Get the closest of X type of component
    /// </summary>
    /// <returns>
    /// The closest instance of X component
    /// </returns>
    public T GrabClosestComponent<T>() where T : Component
    {
        T closestComponent = new Component() as T;
        float closestDistance = float.MaxValue;

        _ObjectsInRadius = _ObjectsInRadius.FindAll(x => x != null);

        foreach (GameObject theObject in _ObjectsInRadius)
        {
            if (theObject != null)
            {
                T theComponent = theObject.GetComponent<T>();

                if (theComponent != null)
                {
                    float distance = Vector3.Distance(Transform.position, theObject.transform.position);
                    if (distance < closestDistance)
                    {
                        closestComponent = theComponent;
                        closestDistance = distance;
                    }
                }
            }

        }

        return closestComponent;
    }

    /// <summary>
    /// Get the closest of X type of component
    /// </summary>
    /// <returns>
    /// The closest instance of X component
    /// </returns>
     public T GrabClosestComponent<T>(Func<T,bool> argFunc) where T : Component
    {
        T closestComponent = new Component() as T;
        float closestDistance = float.MaxValue;

        _ObjectsInRadius = _ObjectsInRadius.FindAll(x => x != null);

        foreach (GameObject theObject in _ObjectsInRadius)
        {
            if (theObject != null)
            {
                T[] components = theObject.GetComponents<T>();

                foreach (T component in components)
                {
                    if (components != null)
                    {
                        float distance = Vector3.Distance(Transform.position, theObject.transform.position);
                        if (distance < closestDistance)
                        {
                            if (argFunc(component))
                            {
                                closestComponent = component;
                                closestDistance = distance;
                            }
                        }
                    }
                }
            }

        }

        return closestComponent;
    }
}
