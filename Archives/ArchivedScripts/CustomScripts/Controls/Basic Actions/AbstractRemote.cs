 using UnityEngine;
using System.Collections;
 using System;

[RequireComponent(typeof(Vision))]
public abstract class AbstractRemote : MonoBehaviour
{
    #region Events

    public Func<AbstractEntity, bool> TargetCheck { get; set; }
    protected Func<AbstractEntity, IEnumerator> PowerOn { get; set; }
    protected Func<AbstractEntity, IEnumerator> PowerOff { get; set; }
    protected Func<AbstractEntity, IEnumerator> ShowTarget { get; set; }
    protected Func<AbstractEntity, IEnumerator> HideTarget { get; set; }

    #endregion

    #region Fields

    protected string _StartLabel = "Start Label";
    protected string _EndLabel = "End Label";

    protected ContextAction _ContextAction = new ContextAction();
    public ContextAction ContextAction
    {
        get { return _ContextAction; }
    }

    #endregion

    #region Properties

    private AbstractEntity _Target;
    public AbstractEntity Target 
    {
        get { return _Target; }
        protected set { _Target = value; }
    }

    private AbstractBeing _Actor;
    public virtual AbstractBeing Actor
    {
        get
        {
            if (_Actor == null)
                _Actor = GetComponent<AbstractBeing>();
            return _Actor;
        }
        protected set { _Actor = value; }
    }

    private Vision _Vision;
    public Vision Vision
    {
        get
        {
            if (_Vision == null)
                _Vision = GetComponent<Vision>();
            return _Vision;
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Called When Object is Reset
    /// 
    /// Disable this
    /// </summary>
    public void Reset()
    {
        enabled = false;
    }

    /// <summary>
    /// Called When Level Loads
    /// 
    /// Disable this
    /// </summary>
    public void Awake()
    {
        enabled = false;
        ShowTarget += DefaultShow;
        HideTarget += DefaultHide;
    }

    /// <summary>
    /// Called When Enabled
    /// 
    /// Show Target
    /// </summary>
    public void OnEnable()
    {
        UpdateTarget();
        ////debug.log("ENABLE: " + GetType());
        //if(_Target == null)
        //    _Target = FindTarget();
        //ShowAction(_StartLabel);
        //DefaultShow();

        if (_Target != null && _Target.enabled && ShowTarget != null)
            _Target.StartCoroutine( ShowTarget(_Target) );
    }

    /// <summary>
    /// Called When Disabled
    /// 
    /// Hide Target
    /// </summary>
    public void OnDisable()
    {
        ////debug.log("DISABLE: " + GetType());
        //DefaultHide();

        if (_Target != null && _Target.enabled && HideTarget != null)
                _Target.StartCoroutine(HideTarget(_Target));
    }

    /// <summary>
    /// Called on Update
    /// 
    /// Update Target
    /// Update the Controlls
    /// </summary>
    public void Update()
    {
        UpdateTarget();
        UpdateControls();
    }

    /// <summary>
    /// Updates the Current Target if Not Locked
    /// </summary>
    void UpdateTarget()
    {
        if (_Target == null || !_Target.Active && !_Target.Locked)
        {
            ////debug.log("UPDATING...");

            // Check for New Target
            AbstractEntity entity = FindTarget();
            if (entity != _Target)
            {
                // Hide Old Target
                if (_Target != null && _Target.enabled && HideTarget != null)
                    _Target.StartCoroutine(HideTarget(_Target));

                // Update Target
                _Target = entity;

                // Show New Target
                if (_Target != null && _Target.enabled && ShowTarget != null)
                    _Target.StartCoroutine(ShowTarget(_Target));
            }

        }
    }

    /// <summary>
    /// Checks the Control Input, and Activates any choice
    /// </summary>
    void UpdateControls()
    {
        if (_Target && !_Target.Locked && (_ContextAction.Pressed))
            StartCoroutine(!_Target.Active ? TurnOn(_Target) : TurnOff(_Target));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Default Show
    /// </summary>
    IEnumerator DefaultShow(AbstractEntity abstractEntity)
    {
        // Display Target
        if (!abstractEntity.Active)
        {
            //CursorGUI.Target = _Target;
            //CursorGUI.Color = ContextAction.CursorColor;
            HighlightGUI.Color = WorldGUI.Mode == Mode.Living? new Color(.3f, .8f, 1, 0.5f) : new Color(0.2f, 0.5f, 1, 0.6f);
            HighlightGUI.Target = _Target;
            RemoteGUI.Label = _StartLabel;
            RemoteGUI.Action = _ContextAction;
        }
        else
        {
            RemoteGUI.Label = _EndLabel;
            RemoteGUI.Action = _ContextAction;
        }

        // Display Action Options
        yield return new WaitForFixedUpdate();
    }

    /// <summary>
    /// Default Hide
    /// </summary>
    static IEnumerator DefaultHide(AbstractEntity abstractEntity)
    {
        HighlightGUI.Color = Color.clear;
        HighlightGUI.Target = null;
        //CursorGUI.Target = null;
        RemoteGUI.Action = null;
        RemoteGUI.Label = "";
        yield return new WaitForFixedUpdate();
    }

    public AbstractEntity FindTarget()
    {
        return Vision.GrabClosestComponent<AbstractEntity>(x => (TargetCheck != null ? TargetCheck(x) : true));
    }

    #endregion

    #region Coroutines

    public IEnumerator TurnOn(AbstractEntity argTarget)
    {
        if (argTarget != null)
        {
            if (argTarget.enabled && HideTarget != null)
                argTarget.StartCoroutine(HideTarget(_Target));

            /// Call Delegate
            if (PowerOn != null)
            {
                argTarget.Locked = true;
                yield return argTarget.StartCoroutine(PowerOn(argTarget));
                argTarget.Locked = false;

            }

            // If No Associated Methods, Unchoose Immediately
            if (PowerOff == null)
                yield return StartCoroutine(TurnOff(argTarget));
        }
    }

    public IEnumerator TurnOff(AbstractEntity argTarget)
    {
        if (argTarget != null)
        {
            if (PowerOff != null)
            {
                argTarget.Locked = true;
                yield return StartCoroutine(PowerOff(argTarget));
                argTarget.Locked = false;
            }
        }
        argTarget.Active = false;
        argTarget.Locked = false;
        _Target = null;
    }

    #endregion
}
