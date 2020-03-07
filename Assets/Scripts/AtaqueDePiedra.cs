using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueDePiedra : Attack
{
	private Animator animator;
	private AudioSource audioSource;
	public AudioClip collision;

    // Start is called before the first frame update
    void Start()
    {
		animator = gameObject.GetComponentInParent<Animator> ();
		audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	protected override void OnCollisionEnter(Collision col)
	{
		//base es lo mismo que super en java, o sea llamo a OnCollisionEnter de la clase attack
		base.OnCollisionEnter(col);
		PlaySound(collision);
		animator.SetTrigger("colision");
	}

	void destruirPiedra()
	{
		Destroy(gameObject);
	}

	public void PlaySound(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();	}
}
