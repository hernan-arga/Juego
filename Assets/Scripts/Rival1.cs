using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rival1 : MonoBehaviour
{
	private Rigidbody rigidBody;
	private Animator animator;
	private bool seguirJugador = true, atacar = false;
	private List<GameObject> jugadoresObjetivos;
	public float distanciaDeAtaque = 1f, rangoPerseguirJugadorDespuesDeAtaque = 4f, 
				 tiempoPorDefectoDeAtaque = 2f, velocidad = 3f;
	private float tiempoActualDeAtaque;
	private Transform jugadorObjetivo = null;

    // Start is called before the first frame update
    void Start()
    {
		tiempoActualDeAtaque = tiempoPorDefectoDeAtaque;
        animator = gameObject.GetComponentInParent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		jugadoresObjetivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

		//jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
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
			bool hayJugadoresAPerseguir = !jugadoresObjetivos.Count.Equals(0);
			if(hayJugadoresAPerseguir){
				jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
			}
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
			perseguir(objetivo);
		}

		else
		{
			if(distanciaAObjetivoEnElEjeZ(jugadorObjetivo) > 0.1f){
				acomodarPersecucionEnZ(jugadorObjetivo);
				return;
			}

			rigidBody.velocity = new Vector3(0f, 0f, 0f);
			seguirJugador = false;
			animator.SetBool("estaCorriendo", false);
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

		//rangoPerseguirJugadorDespuesDeAtaque le da cierto rango para que pueda escapar el jugador
		if (distanciaAObjetivo(jugadorObjetivo) > distanciaDeAtaque + rangoPerseguirJugadorDespuesDeAtaque)
		{
			seguirJugador = true;
			atacar = false;
		}
	}

	public void acomodarPersecucionEnZ(Transform target){
		Vector3 targetEnZ = new Vector3(transform.position.x, transform.position.y, target.position.z);
		transform.position = Vector3.MoveTowards(transform.position, targetEnZ, velocidad * Time.deltaTime);
	}

	public float distanciaAObjetivo(Transform objetivo)
	{
		//return Mathf.Abs(transform.position.x - objetivo.position.x);
		return (transform.position - objetivo.position).sqrMagnitude;
	}

	public float distanciaAObjetivoEnElEjeZ(Transform objetivo)
	{
		return Mathf.Abs(transform.position.z - objetivo.position.z);
	}

	public void perseguir(Transform target)
	{
		//hago que el sprite del  rival mire hacia donde esta el target
		float direccionX = Mathf.Sign(target.position.x - transform.position.x);
		transform.localScale = new Vector3(direccionX, 1f, 1f);

		transform.position = Vector3.MoveTowards(transform.position, target.position, velocidad * Time.deltaTime);
		animator.SetBool("estaCorriendo", true);

	}
}
