using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class AnimalSoundtrack : MonoBehaviour {
	
	[SerializeField]
	private AudioClip[] startledSounds;
	
	[SerializeField]
	private AudioClip[] possessionSounds;
	
	[SerializeField]
	private AudioClip[] battleSounds;
	
	[SerializeField]
	private AudioClip[] deathSounds;
	
	private AudioSource theSource;
	
	// Use this for initialization
	void Start () {
		theSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void playStartled()
	{
        if (startledSounds != null)
        {
            if (startledSounds.Length > 0)
            {
                int random = Random.Range(0, startledSounds.Length - 1);
                theSource.clip = startledSounds[random];
                theSource.Play();
            }
        }
	}

    public void playPossession()
    {
        if (possessionSounds != null)
        {
            if (possessionSounds.Length > 0)
            {
                int random = Random.Range(0, possessionSounds.Length - 1);
                theSource.clip = possessionSounds[random];
                theSource.Play();
            }
        }
    }

    public void playBattle()
    {
        if (battleSounds != null)
        {
            if (battleSounds.Length > 0)
            {
                int random = Random.Range(0, battleSounds.Length - 1);
                theSource.clip = battleSounds[random];
                theSource.Play();
            }
        }
    }

    public void playDeath()
    {
        if (deathSounds != null)
        {
            if (deathSounds.Length > 0)
            {
                int random = Random.Range(0, deathSounds.Length - 1);
                theSource.clip = deathSounds[random];
                theSource.Play();
            }
        }
    }
}
