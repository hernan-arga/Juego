using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public int danio;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter(Collision col)
	{
		Rival1 rival = col.collider.GetComponent<Rival1>();
		Jugador jugador = col.collider.GetComponent<Jugador>();
		bool golpeoPorIzq = (transform.position.x - col.transform.position.x) < 0;

		if (rival != null)
		{
			rival.recibirDanio(danio, golpeoPorIzq);
		}

		else if (jugador != null)
		{
			jugador.recibirDanio(danio, golpeoPorIzq);
		}
	}
}
