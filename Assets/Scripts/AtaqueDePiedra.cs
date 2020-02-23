using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueDePiedra : Attack
{
	private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
		animator = gameObject.GetComponentInParent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	protected override void OnCollisionEnter(Collision col)	{
		//base es lo mismo que super en java, o sea llamo a OnCollisionEnter de la clase attack
		base.OnCollisionEnter(col);
		animator.SetTrigger("colision");
	}

	void destruirPiedra()
	{
		Destroy(gameObject);
	}
}
