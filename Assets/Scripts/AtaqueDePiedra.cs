using UnityEngine;

public class AtaqueDePiedra : Attack
{
	Animator animator;
	AudioSource audioSource;
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

	protected void OnCollisionEnter(Collision col)
	{
		OnTriggerEnter(col.collider);
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
		audioSource.Play();
	}
}
