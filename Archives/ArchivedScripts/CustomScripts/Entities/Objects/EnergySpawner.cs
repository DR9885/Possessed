using UnityEngine;
using System.Collections;

public class EnergySpawner : MonoBehaviour
{
    [SerializeField] private float _MinLife = 5, _MaxLife = 20;
    [SerializeField] private float _MinWait = 5, _MaxWait = 20;
    [SerializeField] public int _Living = 1;
    private Energy _Energy = null;

    private Transform _Transform;
    private Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            return _Transform;
        }
    }

    public void DisableEnergy()
    {
        _Living = 0;
        if (_Energy != null)
            _Energy.enabled = false;
    }

    public void EnableEnergy()
    {
        _Living = 1;
        if (_Energy != null)
            _Energy.enabled = true;
    }

    public IEnumerator Start()
    {
        while (Application.isPlaying)
        {

            if (_Living == 1)
            {
                // Energy Create
                if (_Energy == null)
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _Energy = obj.AddComponent<Energy>();
                    _Energy.Transform.position = Transform.position;
                }

                // Energy Available
                if (_Energy != null)
                    _Energy.enabled = true;
                yield return new WaitForSeconds(Random.Range(_MinLife, _MaxLife));

                // Energy UnAvailable
                if (_Energy != null)
                    _Energy.enabled = false;
                yield return new WaitForSeconds(Random.Range(_MinWait, _MaxWait));
            }
            else
            {

                if (_Energy != null)
                    _Energy.enabled = false;
                yield return new WaitForSeconds(Random.Range(_MinWait, _MaxWait));
            }
        }
    }
}