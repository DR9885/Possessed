using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Possessed/EndGameScene3")]
public class EndGameScene3 : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> _RequiredObjects = new List<GameObject>();

    private Vision _Vision;
    private Vision Vision
    {
        get 
        {
            if (_Vision == null)
                _Vision = gameObject.GetComponent<Vision>();
            if (_Vision == null)
                _Vision = gameObject.AddComponent<Vision>();
            return _Vision; 
        }
    }


    void OnTriggerEnter(Collider other)
    {
        bool results = true;
        foreach (GameObject obj in _RequiredObjects)
        {
            if (!Vision.ObjectsInRadius.Contains(obj))
                results = false;
        }

        if (results)
        {
            Application.LoadLevel("Ch4_Alpha");

        }
    }
}
