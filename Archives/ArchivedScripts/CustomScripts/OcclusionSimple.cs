using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OcclusionSimple : MonoBehaviour
{
    private List<Transform> _Transforms = null;

    private List<Camera> _Cameras = new List<Camera>();
    private List<Renderer> _Renderers = new List<Renderer>();
    private List<Cloth> _Cloths = new List<Cloth>();
    private List<Terrain> _Terrains = new List<Terrain>();
    private List<Light> _Lights = new List<Light>();

    #region Properties

    private static List<OcclusionSimple> _Volumes;
    private static IEnumerable<OcclusionSimple> Volumes
    {
        get
        {
            if (_Volumes == null)
                _Volumes = FindObjectsOfType(typeof(OcclusionSimple)).Cast<OcclusionSimple>().ToList();
            return _Volumes;
        }
    }

    private Collider _Collider;
    private Collider Collider
    {
        get
        {
            if (_Collider == null)
                _Collider = collider;
            return _Collider;
        }
    }

    private Collider[] _Colliders;
    private Collider[] Colliders
    {
        get
        {
            if (_Colliders == null)
                _Colliders = GetComponentsInChildren<Collider>();
            return _Colliders;
        }
    }

    #endregion

    #region Unity Methods

    void Reset()
    {
        Collider.isTrigger = true;
    }

    void Awake()
    {
        StartCoroutine(Hide());
    }

    void Update()
    {
        ////debug.log(Colliders.Count());
        foreach (Camera cam in Camera.allCameras)
        {
            if (Collider != null)
            {
                bool inBounds = Colliders.Any(c => c.bounds.Contains(cam.transform.position));
                bool inList = _Cameras.Contains(cam);

                if (inBounds && !inList)
                    StartCoroutine(OnCamEnter(cam));
                else if (inList && !inBounds)
                    StartCoroutine(OnCamExit(cam));
            }
        }
    }

    #endregion

    #region Private Methods

    IEnumerator OnCamEnter(Camera cam)
    {
        StartCoroutine(Show());
        _Cameras.Add(cam);
        yield return new WaitForFixedUpdate();
    }

    IEnumerator OnCamExit(Camera cam)
    {
        yield return StartCoroutine(Hide());
        _Cameras.Remove(cam);
        foreach (OcclusionSimple volume in Volumes.Where(x => x._Cameras.Count != 0))
            StartCoroutine(volume.Show());
        yield return new WaitForFixedUpdate();
    }

    IEnumerator Show()
    {
        ////debug.log(Colliders.Count());
        //foreach(Transform t in  FindObjectsOfType(typeof(Transform)).Cast<Transform>())
        //    foreach (Collider c in Colliders)
        //        //debug.log(c.name);

        _Transforms = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(x => Colliders.Any(c => c.bounds.Contains(x.position)) || x.position.y < 0).ToList();


        foreach (Transform trans in _Transforms)
        {
            if (trans != null)
            {
                Renderer rend = trans.GetComponent<Renderer>();
                //Terrain terrain = trans.GetComponent<Terrain>();
                Light light = trans.GetComponent<Light>();
                Cloth cloth = trans.GetComponent<Cloth>();
                //ParticleAnimator partAnimator = trans.GetComponent<ParticleAnimator>();
                //ParticleEmitter partEmitter = trans.GetComponent<ParticleEmitter>();

                if (trans.gameObject.isStatic && rend) rend.enabled = true;
                //if (terrain) terrain.enabled = true;
                if (light) light.enabled = true;
                if (cloth) cloth.enabled = true;
                //if (partEmitter) partEmitter.enabled = true;
                //if (partAnimator) partAnimator.active = true;
            }
        }
        yield return new WaitForFixedUpdate();

        //_Renderers.ForEach(x => x.enabled = true);
        //_Cloths.ForEach(x => x.enabled = true);
        //_Terrains.ForEach(x => x.enabled = true);
        //_Lights.ForEach(x => x.enabled = true);
    }

    IEnumerator Hide()
    {
        _Transforms = FindObjectsOfType(typeof(Transform)).Cast<Transform>().Where(x => Colliders.Any(c => c.bounds.Contains(x.position)) || x.position.y < 0).ToList();

        foreach (Transform trans in _Transforms)
        {
            if (trans != null)
            {
                Renderer rend = trans.GetComponent<Renderer>();
                //Terrain terrain = trans.GetComponent<Terrain>();
                Light light = trans.GetComponent<Light>();
                Cloth cloth = trans.GetComponent<Cloth>();
                //ParticleAnimator partAnimator = trans.GetComponent<ParticleAnimator>();
                //ParticleEmitter partEmitter = trans.GetComponent<ParticleEmitter>();

                if (trans.gameObject.isStatic && rend) rend.enabled = false;
                //if (terrain) terrain.enabled = false;
                if (light) light.enabled = false;
                if (cloth) cloth.enabled = false;
                //if (partEmitter) partEmitter.enabled = false;
                //if (partAnimator) partAnimator.active = false;

            }
        }
        yield return new WaitForFixedUpdate();

        //_Renderers.ForEach(x => x.enabled = false);
        //_Cloths.ForEach(x => x.enabled = false);
        //_Terrains.ForEach(x => x.enabled = false);
        //_Lights.ForEach(x => x.enabled = false);
    }

    #endregion
}