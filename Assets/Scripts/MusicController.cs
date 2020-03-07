using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioClip levelSong, bossSong;
	private AudioSource audioSource;

	public float bossSongLoopLength = 45f;
	public float bossSongEndOfLoop = 70.5f;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		PlaySong(bossSong);
    }

	public void PlaySong(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();
	}

	public void Update()
	{
		if (audioSource.timeSamples > bossSongEndOfLoop* bossSong.frequency)
		{
			audioSource.timeSamples -= Mathf.RoundToInt(bossSongLoopLength * bossSong.frequency);  
		}
	}
}
