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

	protected void OnTriggerEnter(Collider col)
	{
		Enemigo enemigo = col.GetComponent<Enemigo>();
		Jugador jugador = col.GetComponent<Jugador>();
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
