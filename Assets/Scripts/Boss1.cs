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
	private AtaqueDeBoss1 golpeActual = AtaqueDeBoss1.NINGUNO;
	public GameObject llamaradas;
	private GameObject fuegoInvocado;
	private bool vuelo = false, estaDelLadoDerechoDeLaPantalla = true, estaAterrizando = false;
	public RockSpawn tiradorDeRocas;
	public float velocidad = 0.1f;

	private GameObject[] nodos;
	private Vector3 posicionNodoActualASeguir, posicionUltimoNodo;
	private float rateVelocidad = 0f, porcentajeLerp = 0f;
	private int nodoActual = 0;
	private GameObject pathFlying;

    // Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		tiempoActualDeAtaque = 0f;
		nodos = GameObject.FindGameObjectsWithTag("Node");
		pathFlying = GameObject.FindGameObjectWithTag("Path Flying");

		if (pathFlying != null)
		{
			pathFlying.transform.parent = null;
		}

        reiniciarPathVuelo();
	}

    // Update is called once per frame
    void Update()
    {
		if (!isDead)
		{
			atacarJugador();

			if (vuelo)
			{
				controlarVuelo();
			}

		}
    }

	void OnCollisionEnter(Collision col)
	{
		if(col.collider.CompareTag("Piso") && estaAterrizando){
			animator.SetTrigger("tocoPiso");
			estaAterrizando = false;
			tirarPiedras();
			reiniciarPathVuelo();
			tiempoActualDeAtaque = 0f;;
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
				//Fixme: no se esta instanciando con esos 180 en x
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
			default:
				break;
		}
	}

	private void controlarGiro()
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

	private bool vaHaciaElUltimoNodo()
	{
		return nodoActual.Equals(nodos.Length - 1) && estaDelLadoDerechoDeLaPantalla;
	}

	private bool vaHaciaElPrimerNodo()
	{
		return nodoActual.Equals(0) && !estaDelLadoDerechoDeLaPantalla;
	}

	private void controlarVuelo()
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

	private void avanzarAlSiguienteNodo()
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

	private void avanzarAlNodoPorDerecha()
	{
		if (nodoActual < nodos.Length-1)
		{
			nodoActual++;
			controlarNodos();
		}

		else 
		{
			aterrizar();
			estaDelLadoDerechoDeLaPantalla = false;;
		}
	}

	private void avanzarAlNodoPorIzquierda()
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

	/*private void voltearSprite()
	{
		transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
	}*/

	private void aterrizar()
	{
		porcentajeLerp = 0f;
		rigidBody.isKinematic = false;
		vuelo = false;
		estaAterrizando = true;
		animator.SetBool("volando", false);
	}

	private void reiniciarPathVuelo()
	{
		posicionUltimoNodo = transform.position;
		posicionNodoActualASeguir = transform.position;
        controlarNodos();
	}

	private void controlarNodos()
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
		base.Morir();
		Invoke("LoadScene", 8f);
	}

	private void LoadScene()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
