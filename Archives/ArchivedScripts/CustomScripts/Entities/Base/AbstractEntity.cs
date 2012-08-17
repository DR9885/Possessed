using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public abstract class AbstractEntity : MonoBehaviour
{
	#region Events

    protected Func<AbstractEntity, IEnumerator> OnActivate;
    protected Func<AbstractEntity, IEnumerator> OnDeactivate;

	#endregion
	
    #region Properties

    private bool _Active;
    public bool Active
    {
        get { return _Active; }
        set { _Active = value; }
    }

    private bool _Locked;
    public bool Locked
    {
        get { return _Locked; }
        set { _Locked = value; }
    }

    #region Basic Components 
 
    private GameObject _GameObject;
    public GameObject GameObject
    {
        get
        {
            if(_GameObject == null)
                _GameObject = gameObject;
            return _GameObject;
        }
    }

    private Transform _Transform;
    public Transform Transform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            return _Transform;
        }
    }

    #endregion

    #region Renderer

    private Renderer _Renderer;
    public Renderer Renderer
    {
        get
        {
            if (_Renderer == null)
                _Renderer = renderer;
            if (_Renderer == null)
            {
                Transform t = Transform.GetChildren().FirstOrDefault(x => x.renderer != null);
                if(t != null) _Renderer = t.renderer;
            }
            return _Renderer;
        }
    }

    private readonly List<Renderer> _Renderers = new List<Renderer>();
    public List<Renderer> Renderers
    {
        get
        {
            if (_Renderers == null)
            {
                foreach (var trans in Transform.GetChildren().Where(x => x.renderer != null))
                    if (_Renderers != null) 
                        _Renderers.Add(trans.renderer);
            }
            return _Renderers;
        }
    }

    #endregion

    #endregion

    #region Unity Methods 

    public void Reset()
    {
        Setup();
    }

    public void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (Renderer != null) { }
        if (Renderers.Count != 0) { }
        if (GameObject != null) { }
        if (Transform != null) { }
    }

    #endregion

    #region Public Methods

    #region Cursor Methods


    #endregion

    #region Activation

    /// <summary>
    /// Activate the Object
    /// </summary>
    public IEnumerator Activate(AbstractEntity argEntity)
    {
        Active = true;

        if (OnActivate != null)
            yield return StartCoroutine(OnActivate(argEntity));
    }

    /// <summary>
    /// Deactivate the Object
    /// </summary>
    public IEnumerator Deactivate(AbstractEntity argEntity)
    {
        Active = false;

        if (OnDeactivate != null)
            yield return StartCoroutine(OnDeactivate(argEntity));
    }

    #endregion

    #region Coroutines

    public IEnumerator LerpColor(Color argNewColor, float argSpeed)
    {
        Renderer r = Renderer;

        if (r != null)
        {
            float startTime = 0;
            Color startColor = r.material.color;
            Color endColor = r.material.color;

            while (endColor != argNewColor)
            {
				// Color Step
                startTime += Time.deltaTime;
                Color currentColor = Color.Lerp(r.material.color, argNewColor, startTime * argSpeed);
                endColor = currentColor;
				
				// Update Material
                if (r.material != null)
                    r.material.color = endColor;

				yield return new WaitForFixedUpdate();
            }
        }
    }
	
	public IEnumerator Absorb(AbstractEntity argEntity)
    {
        StartCoroutine(argEntity.LerpAlpha(0, 0.3f));
        StartCoroutine(argEntity.ShrinkTo(Vector3.zero, 1));
		yield return StartCoroutine(argEntity.MoveTo(Transform.position, 1));
        
        if (renderer != null) 
            renderer.enabled = false;
    }

    public IEnumerator LerpAlpha(float argValue, float argSpeed)
    {
        if (Renderer != null)
        {
            var m = Renderer.material;
            if (m != null)
            {
                var newColor = m.color;
                newColor.a = argValue;
                yield return StartCoroutine(LerpColor(newColor, argSpeed));
            }
        }
    }

    public IEnumerator RotateTowards(Vector3 argPos, float argSpeed)
    {
        if (argPos != Vector3.zero)
        {
            float time = 0;
            Transform trans = Transform;
            Quaternion startRot = Transform.rotation;
            Quaternion goalRot = Quaternion.LookRotation(argPos);

            while (trans != null && time * argSpeed <= 1)
            {
                time += Time.deltaTime;
                trans.rotation = Quaternion.Lerp(startRot, goalRot, time * argSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator MoveTo(Vector3 argPos, float argSpeed)
    {
        float time = 0;
        Transform trans = Transform;
        Vector3 startPos = Transform.position;

        while (trans != null && time * argSpeed <= 1)
        {
            time += Time.deltaTime;
            trans.position = Vector3.Lerp(startPos, argPos, time * argSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator ShrinkTo(Vector3 argSize, float argSpeed)
    {
        float startTime = 0;
        Transform trans = Transform;
        Vector3 startSize = Transform.localScale;

        while (trans != null && Transform.localScale != argSize)
        {
            startTime += Time.deltaTime;
            trans.localScale = Vector3.Lerp(startSize, argSize, startTime * argSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #endregion

}