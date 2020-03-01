using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float danio;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	//virtual: The virtual keyword is used to modify a method and allow for it to be overridden in a derived class.
	protected virtual void OnCollisionEnter(Collision col)
	{
		Enemigo enemigo = col.collider.GetComponent<Enemigo>();
		Jugador jugador = col.collider.GetComponent<Jugador>();
		bool golpeoPorIzq = (transform.position.x - col.transform.position.x) < 0;

		if (enemigo != null)
		{
			enemigo.recibirDanio(danio, golpeoPorIzq);
		}

		else if (jugador != null)
		{
			jugador.recibirDanio(danio, golpeoPorIzq);
		}
	}
}
