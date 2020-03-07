using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llamaradas : MonoBehaviour
{
	public float tiempoATardarEnGirarLaLlamarada = 1f;
	//public float valorInicialDeRotacion = -120f;
	//public float valorFinalDeRotacion = -60f;
	public float danio = 0.1f;
	private ParticleSystem ps;
	Quaternion startRot, finalRot;

	Quaternion rotacionFinalPorDerecha = Quaternion.Euler(0f, -60f, 0f);
	Quaternion rotacionFinalPorIzquierda = Quaternion.Euler(0f, 60f, 0f);

	private AudioSource audioSource;
	public AudioClip fire;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();

		startRot = transform.rotation;
		configurarRotacionFinal();
		//Debug.Log(startRot.eulerAngles + " - " + finalRot.eulerAngles);

        StartCoroutine(GirarLlamarada(startRot, finalRot, true));	//inicia la rutina del IEnumerator girando hacia arriba
		PlaySound(fire);
		ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
		if(!ps.IsAlive())
         {
             Destroy(gameObject);
         }
    }

	void configurarRotacionFinal()
	{
		if (estoyDelLadoDerecho())
		{
			finalRot = rotacionFinalPorDerecha;
		}
		else
		{
			finalRot = rotacionFinalPorIzquierda;
		}
	}
	 
	//Hardcodea valores, hace comparaciones rebuscadas, viola principios de diseño, pero ahi va el codigo que mejor funciono

	bool estoyDelLadoDerecho()
	{
		return startRot.Equals(Quaternion.Euler(0f, -120f, 0f));
	}

	//El yield corta la funcion para volver a ser llamada en esa posicion durante el siguiente frame (como si fuera un hilo)
	private IEnumerator GirarLlamarada(Quaternion desdeDondeRotar, Quaternion hastaDondeRotar, bool tieneQueVolverALaPosicionInicial)
	{
		float lerpPorcentaje = 0f;
		float rateTiempo  = 1f/tiempoATardarEnGirarLaLlamarada; //Porcentaje de cuanto va girando por segundo
		//Ej si el tiempo es 5 seg, entonces gira 20% por cada segundo

		while (lerpPorcentaje <= 1f)
		{
			//como tarda 1 segundo en girar 20%, voy sumando el porcentaje que gira en cada frame
			//Ej si tardo 0,02 segundos en actualizar el frame, entonces giro 0,02*20% = 0.4% durante ese frame (regla de 3 simple)
			lerpPorcentaje += Time.deltaTime * rateTiempo;	

			transform.rotation = Quaternion.Lerp (desdeDondeRotar, 
			                                      hastaDondeRotar, 
			                                      lerpPorcentaje);
			yield return 0;	//Aca corta por este frame
		}

		if (tieneQueVolverALaPosicionInicial)
		{
			StartCoroutine(GirarLlamarada(hastaDondeRotar, desdeDondeRotar, false));
		}

	}

	void OnParticleCollision(GameObject col)
	{

		Jugador jugador = col.GetComponent<Jugador>();
		bool golpeoPorIzq = (transform.position.x - col.transform.position.x) < 0;

		if (jugador != null)
		{
			jugador.recibirDanio(danio, golpeoPorIzq);
		}
	}

	public void PlaySound(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();	}

}
