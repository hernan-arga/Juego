using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rival1 : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private Rigidbody rigidBody;
	private Animator animator;
	private bool seguirJugador = true, atacar = false, tocandoPiso = false;
	private List<GameObject> jugadoresObjetivos;
	public float distanciaDePersecucionEnX = 1f, distanciaDePersecucionEnZ = 1f, rangoPerseguirJugadorDespuesDeAtaque = 4f,
				 tiempoPorDefectoDeAtaque = 2f, velocidad = 3f, minHeight, maxHeight;
	private float tiempoActualDeAtaque;
	private Transform jugadorObjetivo = null;

	private Ataque golpeActual = Ataque.NINGUNO;
	private bool isDead = false, daniado = false;
	public float damageTime = 0.3f;
	private float damageTimer;
	public int maxSalud;
	private int saludActual;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		tiempoActualDeAtaque = tiempoPorDefectoDeAtaque;
		animator = gameObject.GetComponentInParent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		jugadoresObjetivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		saludActual = maxSalud;
		//jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isDead)
		{
			if (jugadorObjetivo != null)
			{
				atacarJugador();
			}

			else
			{
				bool hayJugadoresAPerseguir = !jugadoresObjetivos.Count.Equals(0);
				if (hayJugadoresAPerseguir)
				{
					jugadorObjetivo = jugadoresObjetivos[(int)Random.Range(0f, jugadoresObjetivos.Count)].transform;
				}
			}
		}

		animator.SetBool("Dead", isDead);
		animator.SetBool("OnGround", tocandoPiso);

		if (daniado && !isDead)
		{
			damageTimer += Time.deltaTime;
			if (damageTimer >= damageTime)
			{
				daniado = false;
				damageTimer = 0f;
			}
		}
	}

	void FixedUpdate()
	{
		if (!isDead && !daniado)
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


		//Defino el maximo espacio en el que se puede mover el personaje
		rigidBody.position = new Vector3(rigidBody.position.x,
										rigidBody.position.y, Mathf.Clamp(rigidBody.position.z, minHeight, maxHeight));
		controlarOrdenDeCapa();
	}

	public void perseguirObjetivo(Transform objetivo)
	{
		if (!seguirJugador)
		{
			return;
		}


		if (cumpleDistanciaParaPerseguir(objetivo, 0f))
		{
			perseguir(objetivo);
		}

		else
		{
			bool estaAcomodadoConRespectoAZ = distanciaAObjetivoEnElEjeZ(jugadorObjetivo) <= 0.1f;
			if (!estaAcomodadoConRespectoAZ)
			{
				acomodarPersecucionEnZ(jugadorObjetivo);
				return;
			}

			rigidBody.velocity = new Vector3(0f, 0f, 0f);
			seguirJugador = false;
			animator.SetBool("estaCorriendo", false);
			atacar = true;
		}

	}

	public bool cumpleDistanciaParaPerseguir(Transform objetivo, float rangoExtra)
	{
		return distanciaAObjetivoEnElEjeX(objetivo) > distanciaDePersecucionEnX + rangoExtra || distanciaAObjetivoEnElEjeZ(objetivo) > distanciaDePersecucionEnZ + rangoExtra;
	}

	public void atacarJugador()
	{
		if (!atacar)
			return;

		tiempoActualDeAtaque += Time.deltaTime;
		if (tiempoActualDeAtaque > tiempoPorDefectoDeAtaque)
		{
			golpear();
			tiempoActualDeAtaque = 0f;
		}

		//rangoPerseguirJugadorDespuesDeAtaque le da cierto rango para que pueda escapar el jugador
		if (cumpleDistanciaParaPerseguir(jugadorObjetivo, rangoPerseguirJugadorDespuesDeAtaque))
		{
			seguirJugador = true;
			atacar = false;
		}
	}

	public void golpear()
	{
		golpeActual++;

		switch (golpeActual)
		{
			case (Ataque.GOLPEDERECHO):
				animator.SetTrigger("golpeDerecho");
				break;
			case (Ataque.GOLPEIZQUIERDO):
				animator.SetTrigger("golpeIzquierdo");
				golpeActual = Ataque.NINGUNO;
				break;
			/*case (Ataque.PATADA):
				animator.SetTrigger("patada");
				golpeActual = Ataque.NINGUNO;
				break;*/
			default:
				break;
		}
	}

	public void acomodarPersecucionEnZ(Transform target)
	{
		Vector3 targetEnZ = new Vector3(transform.position.x, transform.position.y, target.position.z);
		transform.position = Vector3.MoveTowards(transform.position, targetEnZ, velocidad * Time.deltaTime);
	}

	public float distanciaAObjetivoEnElEjeX(Transform objetivo)
	{
		//return Mathf.Abs(transform.position.x - objetivo.position.x);
		return Mathf.Abs(transform.position.x - objetivo.position.x);
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

	public void recibirDanio(int danio)
	{
		if (!isDead)
		{
			daniado = true;
			saludActual -= danio;
			Debug.Log(saludActual);
			animator.SetTrigger("daniado");
			if (saludActual <= 0f)
			{
				isDead = true;
				rigidBody.AddRelativeForce(new Vector3(3f, 5f, 0f), ForceMode.Impulse);
			}
		}
	}

	//TODO: cuando haga la animacion de morir llamo a esta funcion en el final
	public void DesactivarEnemigo()
	{
		gameObject.SetActive(false);
	}

	void controlarOrdenDeCapa()
	{
		/*
		 * fijo la capa dependiendo de la altura, multiplico por 100 
		 * para agarrar algunos decimales y el menos es porque cuanto mas abajo mayor 
		 * orden de capa
		*/
		spriteRenderer.sortingOrder = -(int)(transform.position.z * 100);	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = true;
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = true;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = false;
		}	}
}
