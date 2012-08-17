#define Camera_Effects

using UnityEngine;
using System.Linq;

public enum Mode { None, Spirit, Halfway, Living }

[AddComponentMenu("Possessed/DNU/WorldGUI")]
public class WorldGUI : MonoBehaviour
{
    private static WorldGUI _Instance;
    private static WorldGUI Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(WorldGUI)) as WorldGUI;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(WorldGUI).ToString());
                _Instance = obj.AddComponent<WorldGUI>();
            }
            return _Instance;
        }
    }

    [SerializeField]
    private Mode _Mode = Mode.None;
    public static Mode Mode
    {
        get { return Instance._Mode; }
        set 
        {
			if(Instance._Mode != value)
			{
	            ChangeMode(Instance._Mode, value);
	            Instance._Mode = value;
			}
        }
    }

    #region Camera Properties

    private Camera _MainCamera;
    private static Camera MainCamera
    {
        get
        {
            if (Instance._MainCamera == null)
                Instance._MainCamera = Camera.main;
            return Instance._MainCamera;
        }
    }

    private static ColorCorrectionCurves _ColorCorrectionCurves;
    private static ColorCorrectionCurves ColorCorrectionCurves
    {
        get
        {
            if (_ColorCorrectionCurves == null)
                _ColorCorrectionCurves = MainCamera.GetComponent<ColorCorrectionCurves>();
            return _ColorCorrectionCurves;
        }
    }

    private static GlowEffect _GlowEffect;
    private static GlowEffect GlowEffect
    {
        get
        {
            if (_GlowEffect == null)
                _GlowEffect = MainCamera.GetComponent<GlowEffect>();
            return _GlowEffect;
        }
    }

    private static NoiseEffect _NoiseEffect;
    private static NoiseEffect NoiseEffect
    {
        get
        {
            if (_NoiseEffect == null)
                _NoiseEffect = MainCamera.GetComponent<NoiseEffect>();
            return _NoiseEffect;
        }
    }

    private static DepthOfField _DepthOfField;
    private static DepthOfField DepthOfField
    {
        get
        {
            if (_DepthOfField == null)
                _DepthOfField = MainCamera.GetComponent<DepthOfField>();
            return _DepthOfField;
        }
    }

    private static SSAOEffect _SSAOEffect;
    private static SSAOEffect SSAOEffect
    {
        get
        {
            if (_SSAOEffect == null)
                _SSAOEffect = MainCamera.GetComponent<SSAOEffect>();
            return _SSAOEffect;
        }
    }

    private static Fisheye _Fisheye;
    private static Fisheye Fisheye
    {
        get
        {
            if (_Fisheye == null)
                _Fisheye = MainCamera.GetComponent<Fisheye>();
            return _Fisheye;
        }
    }


    private static Vignetting _Vignetting;
    private static Vignetting Vignetting
    {
        get
        {
            if (_Vignetting == null)
                _Vignetting = MainCamera.GetComponent<Vignetting>();
            return _Vignetting;
        }
    }

    #endregion


    public static void ChangeMode(Mode oldMode, Mode newMode)
    {
        Object[] humans = FindObjectsOfType(typeof(HumanScript));
        Object[] ghosts = FindObjectsOfType(typeof(GhostScript));
        Object[] energySpawners = FindObjectsOfType(typeof(EnergySpawner));
        Object[] energy = FindObjectsOfType(typeof(Energy));
        Object[] doors = FindObjectsOfType(typeof(Door));

        switch(newMode)
        {
            case Mode.Spirit:
                foreach (HumanScript h in humans) h.ShowMood();
                foreach (GhostScript g in ghosts) g.Show();
                foreach (EnergySpawner es in energySpawners) es.EnableEnergy();
                foreach (Door d in doors) d.PhaseOut();

                Instance.StartCoroutine(SSAOEffect.Activate());

                Fisheye._X = 0.05f;
                Fisheye._Y = 0.05f;
                Instance.StartCoroutine(Fisheye.Activate());
                Instance.StartCoroutine(Vignetting.Activate());
                Instance.StartCoroutine(ColorCorrectionCurves.Activate());
                Instance.StartCoroutine(GlowEffect.Activate());

                break;

            case Mode.Halfway:
                break;

			case Mode.Living:
                foreach (HumanScript h in humans) h.HideMood();
                foreach (GhostScript g in ghosts) g.Hide();
                foreach (EnergySpawner es in energySpawners) es.DisableEnergy();
                foreach (Door d in doors)         d.PhaseIn();


                Instance.StartCoroutine(SSAOEffect.Activate());

                if (MainPlayerGhost.Instance.PossessionRemote.Target is Cat)
                {
                    Fisheye._X = 0.05f;
                    Fisheye._Y = 0.05f;
                    Instance.StartCoroutine(Fisheye.Activate());
                    Instance.StartCoroutine(Vignetting.Activate());
                }
                else if (MainPlayerGhost.Instance.PossessionRemote.Target is Squirrel)
                {
                    Fisheye._X = 0.1f;
                    Fisheye._Y = 0.1f;
                    Instance.StartCoroutine(Fisheye.Activate());
                    Instance.StartCoroutine(Vignetting.Activate());
                }
                else
                {
                    Instance.StartCoroutine(Fisheye.Deactivate());
                    Instance.StartCoroutine(Vignetting.Deactivate());
                }

                Instance.StartCoroutine(ColorCorrectionCurves.Deactivate());
                Instance.StartCoroutine(GlowEffect.Deactivate());

                // FishEye

                break;			
        }
    }
	
    //void Awake()
    //{
    //    DialogueGUI.CurrentLine = null;
    //    RemoteGUI.Action = null;
    //}

    //public void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Z))
    //    {
    //        //debug.log(Application.loadedLevel);
    //        Application.LoadLevel(Application.loadedLevel+1);
			
    //    }
    //}
}