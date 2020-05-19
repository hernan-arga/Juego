using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonAttack : Attack
{
	AudioSource MyAudio;
	public AudioClip HitSound;

    // Start is called before the first frame update
    void Start()
    {
        MyAudio = GetComponent<AudioSource>();
    }


	protected override void OnTriggerEnter(Collider col)
	{
		MyAudio.PlayOneShot(HitSound);
		base.OnTriggerEnter(col);
	}
}
