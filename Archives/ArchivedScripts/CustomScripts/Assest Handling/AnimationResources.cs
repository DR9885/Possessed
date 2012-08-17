using UnityEngine;

public class AnimationResources : ResourceLoader<Animation>
{
    #region Properties

    // Movement
    public static string Idle           { get { return "Idle"; } }
    public static string Jump           { get { return "Jump"; } }    
    public static string Walk           { get { return "Walk"; } }
    public static string Run            { get { return "Run"; } }
    public static string Climb            { get { return "Climb"; } }

    // Possession
    public static string PossBegin      { get { return "Poss_Begin"; } }
    public static string PossBeginIdle  { get { return "Poss_Begin_Idle"; } }
    public static string PossInput1     { get { return "Poss_Input1"; } }
    public static string PossInput1A    { get { return "Poss_Input1A"; } }
    public static string PossInput1B    { get { return "Poss_Input1B"; } }
    public static string PossInput1Idle { get { return "Poss_Input1_Idle"; } }
    public static string PossInput2     { get { return "Poss_Input2"; } }
    public static string PossInput2A    { get { return "Poss_Input2A"; } }
    public static string PossInput2B    { get { return "Poss_Input2B"; } }
    public static string PossInput2Idle { get { return "Poss_Input2_Idle"; } }
    public static string PossInput3     { get { return "Poss_Input3"; } }
    public static string PossInput3A    { get { return "Poss_Input3A"; } }
    public static string PossInput3B    { get { return "Poss_Input3B"; } }
    public static string PossInput3Idle { get { return "Poss_Input3_Idle"; } }
    public static string PossInput4     { get { return "Poss_Input4"; } }
    public static string PossInput4A    { get { return "Poss_Input4A"; } }
    public static string PossInput4B    { get { return "Poss_Input4B"; } }
    public static string PossInput4Idle { get { return "Poss_Input4_Idle"; } }
    public static string PossComplete   { get { return "Poss_Complete"; } }
    public static string PossFail       { get { return "Poss_Fail"; } }
    public static string PossWrongInput { get { return "Poss_Wrong_Input"; } }
    public static string PossExit       { get { return "Poss_Exit"; } }

    // Interaction
    public static string Dialogue       { get { return "Dialogue"; } }
    public static string Pickup         { get { return "Pickup"; } }
    public static string PutDown        { get { return "PutDown"; } }
    public static string AngryEmote     { get { return "AngryEmote"; } }
    public static string HappyEmote     { get { return "HappyEmote"; } }
    public static string Drunk          { get { return "Drunk"; } }    

    // Battle
    public static string Attack         { get { return "Attack"; } }
    public static string AttackDance    { get { return "AttackDance"; } }
    public static string Freeze         { get { return "Freeze"; } }
    public static string Dodge          { get { return "Dodge"; } }
    public static string Flinch         { get { return "Flinch"; } }
    public static string Death          { get { return "Death"; } }

    #endregion

    #region Methods

    /// <summary>
    /// Init The Animations
    /// </summary>
    /// <param name="being"></param>
    public void InitAnimations(AbstractBeing being, bool reset)
    {
        if (being is HumanScript)
            InitHumanAnimations(being as HumanScript, reset);
        else if (being is GhostScript)
            InitGhostAnimations(being as GhostScript, reset);
        else if (being is Squirrel)
            InitSquirrelAnimations(being as Squirrel, reset);
        else if (being is Cat)
            InitCatAnimations(being as Cat, reset);
        else if (being is Wolf)
            InitWolfAnimations(being as Wolf, reset);
    }

    private static void AddIdleEvent(AbstractBeing being, string start, string idle)
    {
        ////debug.log(being.name+": " + start + " -> " + idle);
        AnimationClip clip = being.Animation[start].clip;
        clip.AddEvent(new AnimationEvent
                          {
                              functionName = "IdleEvent",
                              objectReferenceParameter = being,
                              stringParameter = idle,
                              time = clip.length// - 0.1f
                          });
    }

    /// <summary>
    /// Setup Human Animations
    /// </summary>
    private static void InitHumanAnimations(HumanScript being, bool reset)
    {
        Animation master = ResourceManager.AnimationResources["FBX_HUMANANIMATIONS"];
        if (master != null)
        {
            AnimationState state = master["Take 001"];
            if (state != null)
            {
                if (being.Animation.clip == null || reset)
                {
                    being.Animation.clip = state.clip;
                    AnimationClip masterClip = state.clip;
                    being.Animation.RequireClip(masterClip, Idle, 140, 280, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Jump, 0, 30, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Walk, 330, 354, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Run, 450, 474, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBegin, 605, 770, false, WrapMode.Once, 0, 2);
                    being.Animation.RequireClip(masterClip, PossBeginIdle, 820, 880, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1, 881, 939, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1A, 881, 890, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1B, 891, 939, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1Idle, 940, 991, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2, 992, 1075, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2A, 992, 1003, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2B, 1004, 1075, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2Idle, 1082, 1111, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 1112, 1160, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 1112, 1126, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3B, 1127, 1160, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3Idle, 1161, 1195, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4, 1196, 1242, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4A, 1196, 1202, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4B, 1203, 1242, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4Idle, 1243, 1270, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossComplete, 1271, 1309, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossFail, 1271, 1309, false, WrapMode.Once, 0, -1);
                    being.Animation.RequireClip(masterClip, Dialogue, 1500, 1560, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, HappyEmote, 1340, 1380, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, AngryEmote, 1420, 1463, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Drunk, 1680, 1799, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Death, 1820, 1874, false, WrapMode.Once, 0, 1)   ;
//                    being.Animation.RequireClip(masterClip, Death, 1820, 1874, false, WrapMode.Once, 0, 1)
                    being.Animation.clip = being.Animation[Idle].clip;
                }
            }
        }
        AddIdleEvent(being, PossBegin, PossBeginIdle);
        AddIdleEvent(being, PossInput1, PossInput1Idle);
        AddIdleEvent(being, PossInput2, PossInput2Idle);
        AddIdleEvent(being, PossInput3, PossInput3Idle);
        AddIdleEvent(being, PossInput4, PossInput4Idle);
    }

    /// <summary>
    /// Setup Ghost Animations
    /// </summary>
    private static void InitGhostAnimations(GhostScript being, bool reset)
    {
        Animation master = ResourceManager.AnimationResources["FBX_GHOSTANIMATIONS"];
        if (master != null)
        {
            AnimationState state = master["Take 001"];
            if (state != null)
            {
                if (being.Animation.clip == null || reset)
                {
                    being.Animation.clip = state.clip;
                    AnimationClip masterClip = state.clip;
                    being.Animation.RequireClip(masterClip, Idle, 0, 60, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Jump, 0, 35, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Walk, 90, 120, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Run, 264, 294, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBegin, 562, 784, false, WrapMode.Once, 0, 2);
                    being.Animation.RequireClip(masterClip, PossBeginIdle, 792, 860, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1, 865, 927, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1A, 865, 874, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1B, 875, 927, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1Idle, 928, 977, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2, 978, 1024, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2A, 978, 989, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2B, 990, 1024, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2Idle, 1025, 1075, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 1100, 1167, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3A, 1100, 1111, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3B, 1112, 1167, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3Idle, 1168, 1196, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4, 1210, 1242, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4A, 1210, 1218, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4B, 1219, 1242, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4Idle, 1243, 1263, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossComplete, 1265, 1325, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossWrongInput, 1340, 1365, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossExit, 1370, 1450, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossFail, 1370, 1450, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Dialogue, 140, 200, false, WrapMode.Once, 0, 1);
                    being.Animation.clip = being.Animation[Idle].clip;
                }
            }
        }
        AddIdleEvent(being, PossBegin, PossBeginIdle);
        AddIdleEvent(being, PossInput1, PossInput1Idle);
        AddIdleEvent(being, PossInput2, PossInput2Idle);
        AddIdleEvent(being, PossInput3, PossInput3Idle);
        AddIdleEvent(being, PossInput4, PossInput4Idle);
        AddIdleEvent(being, PossInput1B, PossInput1Idle);
        AddIdleEvent(being, PossInput2B, PossInput2Idle);
        AddIdleEvent(being, PossInput3B, PossInput3Idle);
        AddIdleEvent(being, PossInput4B, PossInput4Idle);
    }

    /// <summary>
    /// Setup Squirrel Animations
    /// </summary>
    private static void InitSquirrelAnimations(Squirrel being, bool reset)
    {
        Animation master = ResourceManager.AnimationResources["FBX_SQUIRRELANIMATIONS"];
        if (master != null)
        {
            AnimationState state = master["Take 001"];
            if (state != null)
            {
                if (being.Animation.clip == null || reset)
                {
                    being.Animation.clip = state.clip;
                    AnimationClip masterClip = state.clip;
                    being.Animation.RequireClip(masterClip, Idle, 50, 200, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Jump, 10, 30, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Walk, 0, 21, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Run, 30, 42, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Freeze, 220, 233, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Dodge, 250, 295, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Flinch, 300, 330, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Death, 340, 390, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Pickup, 610, 649, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PutDown, 650, 675, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBegin, 400, 489, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBeginIdle, 490, 550, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1, 551, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1A, 551, 580, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1B, 581, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1Idle, 490, 550, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2, 551, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2A, 551, 570, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2B, 571, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2Idle, 490, 550, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 551, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3A, 551, 570, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3B, 571, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3Idle, 490, 550, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4, 551, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4A, 551, 570, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4B, 571, 595, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4Idle, 490, 550, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossComplete, 400, 489, false, WrapMode.Once, 0, -1);
					 being.Animation.RequireClip(masterClip, PossFail, 400, 489, false, WrapMode.Once, 0, -1);
                    being.Animation.RequireClip(masterClip, Climb, 760, 779, false, WrapMode.Loop, 0, 20);
                    being.Animation.clip = being.Animation[Idle].clip;
                }
            }
        }
    }

    /// <summary>
    /// Setup Cat Animations
    /// </summary>
    private static void InitCatAnimations(Cat being, bool reset)
    {
        Animation master = ResourceManager.AnimationResources["FBX_CATANIMATIONS"];
        if (master != null)
        {
            AnimationState state = master["Take 001"];
            if (state != null)
            {
                if (being.Animation.clip == null || reset)
                {
                    being.Animation.clip = state.clip;
                    AnimationClip masterClip = state.clip;
                    being.Animation.RequireClip(masterClip, Idle, 1, 45, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Jump, 10, 30, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Walk, 62, 112, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Run, 137, 153, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Freeze, 460, 470, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Dodge, 480, 520, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Flinch, 530, 560, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, Death, 570, 600, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBegin, 300, 359, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBeginIdle, 360, 390, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1, 400, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1A, 400, 415, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1B, 416, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1Idle, 360, 390, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2, 400, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2A, 400, 415, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2B, 416, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2Idle, 360, 390, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 400, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3A, 400, 415, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3B, 416, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3Idle, 360, 390, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4, 400, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4A, 400, 415, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4B, 416, 440, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4Idle, 360, 390, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossComplete, 300, 359, false, WrapMode.Once, 0, -1);
                    being.Animation.RequireClip(masterClip, AttackDance, 640, 670, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Attack, 160, 215, false, WrapMode.Once, 0, 1);
					 being.Animation.RequireClip(masterClip, PossFail, 300, 359, false, WrapMode.Once, 0, -1);
                    being.Animation.clip = being.Animation[Idle].clip;
                }
            }
        }
    }

    /// <summary>
    /// Setup Wolf Animations
    /// </summary>
    private static void InitWolfAnimations(Wolf being, bool reset)
    {
        Animation master = ResourceManager.AnimationResources["FBX_WOLFANIMATIONS"];
        if (master != null)
        {
            AnimationState state = master["Take 001"];
            if (state != null)
            {
                if (being.Animation.clip == null || reset)
                {
                    being.Animation.clip = state.clip;
                    AnimationClip masterClip = state.clip;
                    being.Animation.RequireClip(masterClip, Idle, 0, 30, false, WrapMode.Loop, -1, 1);
                    being.Animation.RequireClip(masterClip, Jump, 0, 35, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Walk, 50, 70, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Run, 90, 111, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, AttackDance, 120, 160, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, Attack, 170, 190, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBegin, 210, 271, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossBeginIdle, 310, 370, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1, 281, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1A, 281, 290, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1B, 291, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput1Idle, 310, 370, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2, 281, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2A, 281, 290, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2B, 291, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput2Idle, 310, 370, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3, 281, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3A, 281, 290, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3B, 291, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput3Idle, 310, 370, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4, 281, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4A, 281, 290, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4B, 291, 300, false, WrapMode.Once, 0, 1);
                    being.Animation.RequireClip(masterClip, PossInput4Idle, 310, 370, false, WrapMode.Loop, 0, 1);
                    being.Animation.RequireClip(masterClip, PossComplete, 210, 271, false, WrapMode.Once, 0, -1);
					being.Animation.RequireClip(masterClip, PossFail, 210, 271, false, WrapMode.Once, 0, -1);
                    being.Animation.clip = being.Animation[Idle].clip;
                }
            }
        }
    }

    #endregion
}