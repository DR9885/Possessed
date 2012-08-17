using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BackgroundLayer
{
    [SerializeField]
    public string name = "Layer";

    [SerializeField]
    public AudioSource source = null;

    [SerializeField]
    public AudioClip clip = null;

}

[AddComponentMenu("Possessed/Soundtrack")]
public class Soundtrack : MonoBehaviour
{

    [SerializeField]
    private static int audioFiles = 5;

    private static Soundtrack s_Instance = null;

    private BackgroundLayer layer2;

    private static Soundtrack _Instance;
    private static Soundtrack Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType(typeof(Soundtrack)) as Soundtrack;
            if (_Instance == null)
            {
                var obj = new GameObject(typeof(Soundtrack).ToString());
                _Instance = obj.AddComponent<Soundtrack>();
            }
            return _Instance;
        }


    }

    public List<BackgroundLayer> _audioLayers = new List<BackgroundLayer>();
    public static List<BackgroundLayer> audioLayers
    {
        get { return Instance._audioLayers; }
        set { Instance._audioLayers = value; }
    }

    public AudioClip[] _audioClips = new AudioClip[audioFiles];
    public static AudioClip[] audioClips
    {
        get { return Instance._audioClips; }
        set { Instance._audioClips = value; }
    }

    public BackgroundLayer _layer1;
    public static BackgroundLayer layer1
    {
        get { return Instance._layer1; }
        set { Instance._layer1 = value; }
    }


    public static void PlayClip(string whichLayer, int whichClip)
    {
        foreach (BackgroundLayer bl in audioLayers)
        {
            if (bl.name == whichLayer)
            {
                bl.source.Stop();
                if (whichClip == -1)
                    bl.source.clip = bl.clip;
                else
                    bl.source.clip = audioClips[whichClip];
                bl.source.Play();

            }
        }
    }

    void ChangeSpeakerMode(int speakerMode)
    {
        if (speakerMode == 1)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Mono;
        }
        else if (speakerMode == 2)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
        }
        else if (speakerMode == 3)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Quad;
        }
        else if (speakerMode == 4)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Surround;
        }
        else if (speakerMode == 5)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Mode5point1;
        }
        else if (speakerMode == 6)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Mode7point1;
        }
    }

    public static void FadeIn(string fadeLayer, float duration, int whichClip)
    {
        float startTime = Time.time;
        float endFadeOutTime = startTime + (duration / 2);
        float endFadeIn = endFadeOutTime + (duration / 2);

        foreach (BackgroundLayer bl in audioLayers)
        {
            if (bl.name == fadeLayer)
            {
                layer1 = bl;
            }
        }

        float t = Time.time;
        while (t < endFadeOutTime)
        {
            float i = (t - startTime) / (duration/2);
            if (layer1 != null)
            {
            if (layer1.source != null)
            {
                if (layer1.source.volume != null)
                {
                    layer1.source.volume = i;
                }
            }
        }
            t = t + Time.deltaTime;
        }

        layer1.source.Stop();
        if (whichClip == -1)
            layer1.source.clip = layer1.clip;
        else
            layer1.source.clip = audioClips[whichClip];
        layer1.source.Play();

        t = Time.time;
        while (t > endFadeOutTime && t < endFadeIn)
        {
            float i = (t - endFadeOutTime) / (duration / 2);
            ////debug.log(i);
            layer1.source.volume = (1 - i);
            t = t + Time.deltaTime;
        }
    }

    public void CrossFade(string fadeLayer1, string fadeLayer2, float duration)
    {

        float startTime = Time.time;
        float endTime = startTime + duration;

        int x = 0;
        foreach (BackgroundLayer bl in audioLayers)
        {
            if (bl.name == fadeLayer1)
            {
                layer1 = bl;
                x++;
            }
            else if (bl.name == fadeLayer2)
            {
                layer2 = bl;
                x++;
            }
        }


        if (x == 2)
        {

            layer2.source.volume = 0;
            layer2.source.clip = layer2.clip;
            layer2.source.Play();

            float t = Time.time;
            while (t < endTime)
            {
                float i = (t - startTime) / duration;
                ////debug.log(i);
                layer1.source.volume = (1 - i);
                layer2.source.volume = i;
                t = t + Time.deltaTime;
            }

            layer1.source.Stop();
        }
        else
        {
            //debug.log("One of the layers was not found");
        }
    }
}