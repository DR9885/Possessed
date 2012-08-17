using UnityEngine;
using System;
using System.Collections.Generic;

public delegate void StatHandler(Stat m, EventArgs e);

[System.Serializable]
public class Stat : System.Object
{
    #region Feilds

    [System.Serializable]
    public class DisplayData : System.Object
    {
        public string name;
        public string shortName;
        public Color ValueColor = Color.white;
        public Color backgroundColor = Color.clear;
        public Color foregroundColor = Color.blue;
    }

    // Designer Feilds
    [SerializeField] public DisplayData displayData = new DisplayData();
    [SerializeField] protected float start = 1;
    [SerializeField] protected float current = 1;

    // Class Feilds
    private float ourPrevious;

    // Class Events
    public event StatHandler StatChanged = delegate { };

    #endregion

    #region Propeties: Current, Delta...

    public float Current
    {
        get { return current; }
        set
        {
            current = value;
            Clamp();
        }
    }

    public float Delta
    {
        get { return current - ourPrevious; }
    }

    #endregion

    #region Constructor & Copy Constructor...

    public Stat(string argName, string argShortName)
    {
        displayData.name = argName;
        displayData.shortName = argShortName;
    }

    public Stat(Stat argStat)
    {
        displayData.name = argStat.displayData.name;
        displayData.shortName = argStat.displayData.shortName;
        current = argStat.Current;
        ourPrevious = argStat.ourPrevious;
        StatChanged = argStat.StatChanged;
    }

    #endregion

    #region public Functions: Awake, Clamp

    /// <summary>
    /// Initializes the text
    /// </summary>
    public virtual void Awake() // CALL
    {
        Current = start;
        ourPrevious = start;
    }

    /// <summary>
    /// Clamps values to limits
    /// </summary>
    public virtual void Clamp() { /*Empty*/; }

    #endregion

    #region public Draw Functions: DrawName, DrawValue, ToString

    /// <summary>
    /// DrawLayout the Name or ShortName
    /// </summary>
    public void DrawLayoutName(bool isShort)
    {
        if (isShort)
            GUILayout.Label(displayData.shortName);
        else
            GUILayout.Label(displayData.name);
    }

    /// <summary>
    /// Draw the Name or ShortName
    /// </summary>
    public void DrawName(Rect argShape, bool isShort)
    {
        if (isShort)
            GUI.Label(argShape, displayData.shortName);
        else
            GUI.Label(argShape, displayData.name);
    }

    /// <summary>
    /// Draws the content text
    /// </summary>
    public void DrawValue(Rect argShape)
    {
        Rect myValueRect = argShape;
        myValueRect.x += 10;
        GUI.contentColor = displayData.ValueColor;
        GUI.Label(myValueRect, ToString());
    }

    public void DrawLayoutValue()
    {
        GUI.contentColor = displayData.ValueColor;
        GUILayout.Label(ToString());
    }

    /// <summary>
    /// Gets the display sting
    /// </summary>
    public override string ToString()
    {
        return ((int)Current).ToString();
    }

    #endregion

    #region Private Event Functions: OnStatChanged...

    /// <summary>
    /// Called when stat changes, and triggers event
    /// </summary>
    private void OnStatChanged(float argValue)
    {
        if (Current != argValue)
        {
            ourPrevious = Current; // Store Previous
            Current = argValue;

            StatChanged(this, EventArgs.Empty);

            Clamp();
        }
    }

    #endregion

    #region Operator Overloads: + , - ...

    /// <summary>
    /// Overloads the + operator, to increment the current value
    /// </summary>
    public static Stat operator +(Stat argLHS, float argRHS)
    {
        Stat myStat = new Stat(argLHS);
        myStat.Current += argRHS;
        return myStat;
    }

    /// <summary>
    /// Overloads the - operator, to decrement the current value
    /// </summary>
    public static Stat operator -(Stat argLHS, float argRHS)
    {
        Stat myStat = new Stat(argLHS);
        myStat.Current -= argRHS;
        return myStat;
    }

    #endregion
}


[System.Serializable]
public class MaxxedStat : Stat
{    
    #region Feilds

    // Designer Feilds
    [SerializeField] protected float min = 0;
    [SerializeField] protected float max = 99;

    // Class Events
    public event StatHandler MaxStatChange = delegate { };
    public event StatHandler StatEmpty = delegate { };
    public event StatHandler StatFull = delegate { };

    #endregion

    #region Properties : Min, Max, Percent...

    public float Min
    {
        set { min = value; }
        get { return min; }
    }

    public float Max
    {
        set { OnMaxStatChanged(value); }
        get { return max; }
    }

    public float Percent
    {
        get { return Current / Max; }
    }

    #endregion

    #region Constructor & Copy Constructor...

    public MaxxedStat(string argName, string argShortName)
        : base(argName, argShortName) { /*Empty*/; }

    public MaxxedStat(string argName, string argShortName, float argMin, float argMax)
        : base(argName, argShortName)
    {
        max = argMax;
        min = argMin;
    }

    public MaxxedStat(MaxxedStat argMaxxedStat)
        : base(argMaxxedStat)
    {
        max = argMaxxedStat.Max;
        min = argMaxxedStat.Min;
        MaxStatChange = argMaxxedStat.MaxStatChange;
        StatEmpty = argMaxxedStat.StatEmpty;
        StatFull = argMaxxedStat.StatFull;
    }

    #endregion

    #region Private Functions: OnMaxStatChanged, OnMaxStatFull, OnMaxStatEmpty...

    /// <summary>
    /// Called when max stat is changed, and triggers event
    /// </summary>
    private void OnMaxStatChanged(float argValue)
    {
        if (max != argValue)
        {
            max = argValue;
            Clamp();

            MaxStatChange(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Called when current stat is maxxed, and triggers event
    /// </summary>
    private void OnMaxStatFull()
    {
        if (Current != max)
        {
            Current = max;
            StatFull(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Called when current stat is empty, and triggers event
    /// </summary>
    private void OnMaxStatEmpty()
    {
        current = min;
        StatEmpty(this, EventArgs.Empty);
    }

    #endregion

    #region Operator Overloads: + , - ...

    public static MaxxedStat operator +(MaxxedStat argLHS, float argRHS)
    {
        MaxxedStat myStat = new MaxxedStat(argLHS);
        myStat.Current += argRHS;
        return myStat;
    }

    public static MaxxedStat operator -(MaxxedStat argLHS, float argRHS)
    {
        MaxxedStat myStat = new MaxxedStat(argLHS);
        myStat.Current -= argRHS;
        return myStat;
    }

    #endregion

    #region public Functions: Clamp

    /// <summary>
    /// Clamps values to limits
    /// </summary>
    public override void Clamp()
    {
        base.Clamp();
        ClampCurrent();
    }

    /// <summary>
    /// Clamps Current to limits
    /// </summary>
    public void ClampCurrent()
    {
        if (Current >= max)
            OnMaxStatFull();
        else if (Current <= min)
            OnMaxStatEmpty();
        current = Mathf.Clamp(Current, min, max);
    }

    #endregion

    #region public Draw Functions: DrawBar, ToString

    /// <summary>
    /// Draw the Bar of the Percentage value
    /// </summary>
    public void DrawBar(Rect argShape)
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.background = CreateTexture(displayData.backgroundColor);
        GUI.Box(argShape, "", myStyle);

        Rect myForeRect = argShape;
        myForeRect.width *= Percent;
        myStyle.normal.background = CreateTexture(displayData.foregroundColor);
        GUI.Box(myForeRect, "", myStyle);

        DrawValue(argShape);
    }

    /// <summary>
    /// DrawLayout the Bar of the Percentage value
    /// </summary>
    public void DrawLayoutBar(int argWidth, int argHeight)
    {
        /// Create A Texture
        Texture2D myTexture = new Texture2D(argWidth, argHeight);
        for(int i = 0; i < argWidth; i++)
            for (int j = 0; j < argHeight; j++)
                if (i <= (Percent * argWidth))
                    myTexture.SetPixel(i, j, displayData.foregroundColor);
                else
                    myTexture.SetPixel(i, j, displayData.backgroundColor);
        myTexture.Apply();

        /// Apply It To the Style
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.background = myTexture;
        GUILayout.Box("", myStyle, GUILayout.Width(argWidth),
            GUILayout.Height(argHeight));
    }

    public override string ToString()
    {
        return ((int)Current).ToString() + "/" + ((int)max).ToString();
    }

    #endregion

    /// <summary>
    /// Creates a New 1x1 Texture, with a set color.
    /// </summary>
    protected Texture2D CreateTexture(Color argColor)
    {
        Texture2D myTexture = new Texture2D(1, 1);
        myTexture.SetPixel(0, 0, argColor);
        myTexture.Apply();
        return myTexture;
    }
}

[System.Serializable]
public class CappedStat : MaxxedStat
{
    #region Properties: Cap, PercentCap...

    public float cap = 999;
    public float Cap
    {
        set { cap = value; }
        get { return cap; }
    }

    public float PercentCap
    {
        get { return Max / Cap; }
    }

    #endregion

    #region Constructors & Copy Constructor...

    public CappedStat(string argName, string argShortName)
        : base(argName, argShortName) { /*Empty*/; }

    public CappedStat(string argName, string argShortName, float argMin, float argMax)
        : base(argName, argShortName, argMin, argMax) { /*Empty*/; }

    public CappedStat(string argName, string argShortName, float argMin, float argMax, float argCap)
        : base(argName, argShortName, argMin, argMax)
    {
        cap = argCap;
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="argStat"></param>
    public CappedStat(CappedStat argStat)
        : base(argStat)
    {
        cap = argStat.Cap;
    }

    #endregion

    #region Operator Overloads: + , - ...

    public static CappedStat operator +(CappedStat argLHS, float argRHS)
    {
        CappedStat myStat = new CappedStat(argLHS);
        myStat.Current += argRHS;
        return myStat;
    }

    public static CappedStat operator -(CappedStat argLHS, float argRHS)
    {
        CappedStat myStat = new CappedStat(argLHS);
        myStat.Current -= argRHS;
        return myStat;
    }

    #endregion

    public override void Clamp()
    {
        max = Mathf.Clamp(max, min, cap);
        base.Clamp();
    }
}