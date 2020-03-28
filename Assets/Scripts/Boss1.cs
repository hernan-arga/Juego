using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AtaqueDeBoss1
{
	NINGUNO, LLAMARADA, VUELOYPIEDRAS
}

public class Boss1 : Enemigo
{
	AtaqueDeBoss1 golpeActual = AtaqueDeBoss1.NINGUNO;
	public GameObject llamaradas;
	GameObject fuegoInvocado;
	bool vuelo, estaDelLadoDerechoDeLaPantalla = true, estaAterrizando, puedeAtacar, pathFijado,
		 alcanzoPosicionDeCombate;
	public RockSpawn tiradorDeRocas;
	public float velocidad = 0.1f, velocidadAlCorrer = 3f;

	GameObject[] nodos;
	Vector3 posicionNodoActualASeguir, posicionUltimoNodo;
	float rateVelocidad = 0f, porcentajeLerp = 0f;
	int nodoActual = 0;
	GameObject pathFlying;

	public Vector3 posicionInicialParaCombatir;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		tiempoActualDeAtaque = 0f;
		nodos = GameObject.FindGameObjectsWithTag("Node");
		pathFlying = GameObject.FindGameObjectWithTag("Path Flying");
		FijarPosicionInicialDeCombate();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isDead)
		{
			controlarOrdenDeCapa();

			if (!alcanzoPosicionDeCombate)
			{
				controlarMoverseHastaLaCamara();
			}

			if (EscenaEnCombate())
			{
				controlarPath();
				atacarJugador();

				if (vuelo)
				{
					controlarVuelo();
				}
			}

		}
	}

	void controlarPath()
	{
		if (!pathFijado)
		{
			FijarPath();
			pathFijado = true;
		}
	}

	void controlarMoverseHastaLaCamara()
	{
		if (!LlegoALaPosicionInicialDeCombate())
		{
			MoverseHastaEnFrenteDeLaCamara();
		}

		else
		{
			alcanzoPosicionDeCombate = true;
			animator.SetBool("estaCorriendo", false);
		}

	}

	void FijarPath()
	{
		if (pathFlying != null)
		{
			pathFlying.transform.parent = null;

		}
		reiniciarPathVuelo();
	}

	void FijarPosicionInicialDeCombate()
	{
		Camera camara = FindObjectOfType<Camera>();
		float cincoSextosDeLaCamara = camara.pixelWidth - camara.pixelWidth / 6f;
		float posicionEnXInicial = Camera.main.ScreenToWorldPoint(new Vector3(cincoSextosDeLaCamara, 0, 0)).x;
		posicionInicialParaCombatir = new Vector3(posicionEnXInicial,
										  transform.position.y,
										  transform.position.z);
	}

	bool LlegoALaPosicionInicialDeCombate()
	{
		return (transform.position - posicionInicialParaCombatir).sqrMagnitude < 1f;
	}

	void MoverseHastaEnFrenteDeLaCamara()
	{
		transform.position = Vector3.MoveTowards(transform.position, posicionInicialParaCombatir, velocidadAlCorrer * Time.deltaTime);
		animator.SetBool("estaCorriendo", true);
	}

	public void SetPuedeAtacar(bool valor)
	{
		puedeAtacar = valor;
	}

	void controlarOrdenDeCapa()
	{
		//Para que el jugador no parezca que lo esta pisando
		spriteRenderer.sortingOrder = -(int)(transform.position.z * 90);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.collider.CompareTag("Piso") && estaAterrizando)
		{
			animator.SetTrigger("tocoPiso");
			estaAterrizando = false;
			tirarPiedras();
			reiniciarPathVuelo();
			tiempoActualDeAtaque = 0f;
		}
	}

	void lanzarFuego()
	{
		if (!isDead)
		{
			Quaternion startRot;


			if (estaDelLadoDerechoDeLaPantalla)
			{
				//rotacionLlamarada = Quaternion.Euler(0f, -120f, 0f);
				startRot = Quaternion.Euler(0f, -120f, 0f);
				fuegoInvocado = Instantiate(llamaradas, transform.position + 2f * Vector3.left + 0.2f * Vector3.down,
											startRot);
			}
			else
			{
				startRot = Quaternion.Euler(0f, 120f, 0f);
				fuegoInvocado = Instantiate(llamaradas, transform.position - 2f * Vector3.left + 0.2f * Vector3.down,
											startRot);
			}

		}
	}

	void tirarPiedras()
	{
		CameraFollow camara = FindObjectOfType<CameraFollow>();
		Vector3 posicionAInstanciar = camara.transform.position;
		posicionAInstanciar.y += 4f; //Dejo un margen para que las piedras no aparezcan en medio de la escena
		camara.setSacudirCamara(true);
		Instantiate(tiradorDeRocas, posicionAInstanciar, Quaternion.identity);

	}

	protected override void golpear()
	{
		golpeActual++;

		switch (golpeActual)
		{
			case (AtaqueDeBoss1.LLAMARADA):
				animator.SetTrigger("llamarada");
				break;
			case (AtaqueDeBoss1.VUELOYPIEDRAS):
				animator.SetBool("volando", true);
				vuelo = true;
				rigidBody.isKinematic = true;
				golpeActual = AtaqueDeBoss1.NINGUNO;
				break;
				/*case (Ataque.PATADA):
					animator.SetTrigger("patada");
					golpeActual = Ataque.NINGUNO;
					break;*/
		}
	}

	void controlarGiro()
	{
		Vector3 startRot = transform.rotation.eulerAngles;

		if (vaHaciaElUltimoNodo())
		{
			transform.rotation = Quaternion.Euler(
					 new Vector3(
						startRot.x,
						Mathf.Lerp(0f, 180f, porcentajeLerp),
						startRot.z));   //En z y en x no gira
		}

		else if (vaHaciaElPrimerNodo())
		{
			transform.rotation = Quaternion.Euler(
					 new Vector3(
						startRot.x,
						Mathf.Lerp(180f, 0f, porcentajeLerp),
						startRot.z));   //En z y en x no gira
		}
	}

	bool vaHaciaElUltimoNodo()
	{
		return nodoActual.Equals(nodos.Length - 1) && estaDelLadoDerechoDeLaPantalla;
	}

	bool vaHaciaElPrimerNodo()
	{
		return nodoActual.Equals(0) && !estaDelLadoDerechoDeLaPantalla;
	}

	void controlarVuelo()
	{
		//como tarda 1 segundo en girar 50%, voy sumando el porcentaje que gira en cada frame
		//Ej si tardo 0,02 segundos en actualizar el frame, entonces se desplazo 0,02*50% = 1% de la distancia durante ese framel
		porcentajeLerp += Time.deltaTime * rateVelocidad;

		if (porcentajeLerp < 1f)
		{
			transform.position = Vector3.Lerp(posicionUltimoNodo, posicionNodoActualASeguir, porcentajeLerp);
			controlarGiro();
		}

		else
		{
			avanzarAlSiguienteNodo();
		}
	}

	void avanzarAlSiguienteNodo()
	{
		if (estaDelLadoDerechoDeLaPantalla)
		{
			avanzarAlNodoPorDerecha();
		}

		else
		{
			avanzarAlNodoPorIzquierda();
		}

	}

	void avanzarAlNodoPorDerecha()
	{
		if (nodoActual < nodos.Length - 1)
		{
			nodoActual++;
			controlarNodos();
		}

		else
		{
			aterrizar();
			estaDelLadoDerechoDeLaPantalla = false; ;
		}
	}

	void avanzarAlNodoPorIzquierda()
	{
		if (nodoActual > 0)
		{
			nodoActual--;
			controlarNodos();
		}

		else
		{
			aterrizar();
			estaDelLadoDerechoDeLaPantalla = true;
		}
	}

	/* void voltearSprite()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
	}*/

	void aterrizar()
	{
		porcentajeLerp = 0f;
		rigidBody.isKinematic = false;
		vuelo = false;
		estaAterrizando = true;
		animator.SetBool("volando", false);
	}

	void reiniciarPathVuelo()
	{
		posicionUltimoNodo = transform.position;
		posicionNodoActualASeguir = transform.position;
		controlarNodos();
	}

	void controlarNodos()
	{
		posicionUltimoNodo = posicionNodoActualASeguir;
		posicionNodoActualASeguir = nodos[nodoActual].transform.position;
		porcentajeLerp = 0f;

		//Porcentaje de cuanto va desplazandose por segundo
		//Ej si la distancia es de 2 y la velocidad es de 1, entonces se desplaza 50% por cada segundo
		rateVelocidad = (1f / Vector3.Distance(posicionUltimoNodo, posicionNodoActualASeguir)) * velocidad;
	}

	public override void Morir()
	{
		ControladorDelJuego.EstadoDeEscena = EstadoDeEscena.JefeDerrotado;
		base.Morir();
		//Invoke("LoadScene", 8f);
	}

	/*void LoadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}*/
}
