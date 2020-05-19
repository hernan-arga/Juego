using UnityEngine;

public abstract class Attack : MonoBehaviour
{
	public float danio;

	protected virtual void OnTriggerEnter(Collider col)
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
