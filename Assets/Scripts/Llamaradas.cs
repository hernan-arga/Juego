using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llamaradas : MonoBehaviour
{
	public float tiempoATardarEnGirarLaLlamarada = 1f;
	public float valorInicialDeRotacion = -120f;
	public float valorFinalDeRotacion = -60f;
	public float danio = 0.1f;
	private ParticleSystem ps;


    // Start is called before the first frame update
    void Start()
    {
		ps = GetComponent<ParticleSystem>();
		StartCoroutine(GirarLlamarada(valorInicialDeRotacion, valorFinalDeRotacion, true));	//inicia la rutina del IEnumerator girando hacia arriba
        //StartCoroutine(GirarLlamarada(valorInicialDeRotacion));	//vuelve a la posicion inicial

    }

    // Update is called once per frame
    void Update()
    {
		if(!ps.IsAlive())
         {
             Destroy(gameObject);
         }
    }


	//El yield corta la funcion para volver a ser llamada en esa posicion durante el siguiente frame (como si fuera un hilo)
	private IEnumerator GirarLlamarada(float desdeDondeRotar, float hastaDondeRotar, bool tieneQueVolverALaPosicionInicial)
	{
		Vector3 startRot = transform.rotation.eulerAngles;
		float lerpPorcentaje = 0f;
		float rateTiempo  = 1f/tiempoATardarEnGirarLaLlamarada; //Porcentaje de cuanto va girando por segundo
		//Ej si el tiempo es 5 seg, entonces gira 20% por cada segundo

		while (lerpPorcentaje <= 1f)
		{
			//como tarda 1 segundo en girar 20%, voy sumando el porcentaje que gira en cada frame
			//Ej si tardo 0,02 segundos en actualizar el frame, entonces giro 0,02*20% = 0.4% durante ese frame (regla de 3 simple)
			lerpPorcentaje += Time.deltaTime * rateTiempo;	
			transform.rotation = Quaternion.Euler(
					 new Vector3(
						startRot.x,
						Mathf.Lerp(desdeDondeRotar, hastaDondeRotar, lerpPorcentaje),
						startRot.z));   //En z y en x no gira
			yield return 0;	//Aca corta por este frame
		}

		if (tieneQueVolverALaPosicionInicial)
		{
			StartCoroutine(GirarLlamarada(valorFinalDeRotacion, valorInicialDeRotacion, false));
		}

		/*si me llego a perder en algo leer: 
		* https://arjierdagames.com/blog/unity/que-es-el-lerp-y-como-usarlo-correctamente/
		*/
	}

	void OnParticleCollision(GameObject col)
	{

		Jugador jugador = col.GetComponent<Jugador>();
		bool golpeoPorIzq = (transform.position.x - col.transform.position.x) < 0;

		if (jugador != null)
		{
			jugador.recibirDanio(danio, golpeoPorIzq);
		}	}

}
