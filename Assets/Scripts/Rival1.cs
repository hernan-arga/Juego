using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rival1 : MonoBehaviour
{
	private Rigidbody2D rigidBody2D;
	private Animator animator;
	private bool seguirJugador = true, atacar = false;
	private List<GameObject> jugadoresObjetivos;
	public float distanciaDeAtaque = 1f, tiempoPorDefectoDeAtaque = 2f;
	private float tiempoActualDeAtaque;
	private Transform jugadorObjetivo;

    // Start is called before the first frame update
    void Start()
    {
		tiempoActualDeAtaque = tiempoPorDefectoDeAtaque;
        animator = gameObject.GetComponentInParent<Animator> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();
		jugadoresObjetivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

		jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
    }

    // Update is called once per frame
    void Update()
    {
		if (jugadorObjetivo != null)
		{
			atacarJugador();
		}

		else
		{
			jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
		}
    }

	void FixedUpdate()
	{
		if (jugadorObjetivo != null)
		{
			perseguirObjetivo(jugadorObjetivo);
		}

		else
		{
			jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
		}
	}

	public void perseguirObjetivo(Transform objetivo)
	{
		if (!seguirJugador)
			return;

		if (distanciaAObjetivo(objetivo) > distanciaDeAtaque)
		{
			/*
				float sentidoADondeIr = Mathf.Sign(transform.position.x - objetivo.position.x);
				Debug.Log(sentidoADondeIr);
				transform.localScale = new Vector3(sentidoADondeIr, 1f, 1f);
			*/

			//rota el transform, asi el vector "forward" apunta a la posicion actual del objetivo
			LookAt2D(objetivo);
			//transform.LookAt(objetivo);

			//transform.LookAt(objetivo);
			//forward da como el vector direccion
			rigidBody2D.velocity = Vector3.forward * 2f;

			if (rigidBody2D.velocity.sqrMagnitude != 0)
			{
				animator.SetBool("estaCaminando", true);
			}

		}

		else
		{
			rigidBody2D.velocity = new Vector2(0f, 0f);
			seguirJugador = false;
			animator.SetBool("estaCaminando", false);
			atacar = true;
		}
		
	}

	public void atacarJugador()
	{
		if (!atacar)
			return;

		tiempoActualDeAtaque += Time.deltaTime;
		if (tiempoActualDeAtaque > tiempoPorDefectoDeAtaque)
		{
			animator.SetTrigger("golpeDerecho");
			tiempoActualDeAtaque = 0f;
		}

		if (distanciaAObjetivo(jugadorObjetivo) > distanciaDeAtaque)
		{
			seguirJugador = true;
			atacar = false;
		}
	}

	public float distanciaAObjetivo(Transform objetivo)
	{
		return (transform.position - objetivo.position).sqrMagnitude;
	}

	//Fixme arreglar esto para que funcion como el lookat en 3D
	public void LookAt2D(Transform target)
	{
		float direccionX = Mathf.Sign(target.position.x - transform.position.x);
		transform.localScale = new Vector3(direccionX, 1f, 1f);

		transform.right = target.position - transform.position;
	}
}
