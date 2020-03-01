using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtaqueDeBoss1
{
	NINGUNO, LLAMARADA, VUELOYPIEDRAS
}

public class Boss1 : Enemigo
{
	private AtaqueDeBoss1 golpeActual = AtaqueDeBoss1.NINGUNO;
	public GameObject llamaradas;
	private GameObject fuegoInvocado;
	private float tiempoTranscurridoVolando = 0f;
	private bool vuelo = false;
	public RockSpawn tiradorDeRocas;
	public float maximaAlturaDeVuelo;

    // Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
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

	void lanzarFuego()
	{
		if (!isDead)
		{
			Quaternion rotacionLlamarada = Quaternion.Euler(0f, -120f, 0f);
			fuegoInvocado = Instantiate(llamaradas, transform.position + 2f*Vector3.left + 0.2f*Vector3.down, rotacionLlamarada);
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
				golpeActual = AtaqueDeBoss1.NINGUNO;
				break;
			/*case (Ataque.PATADA):
				animator.SetTrigger("patada");
				golpeActual = Ataque.NINGUNO;
				break;*/
			default:
				break;
		}	}

	//Fixme: esto es super forzado, ver si queda mejor planteando una ruta que seguir
	private void controlarVuelo()
	{
		tiempoTranscurridoVolando += Time.deltaTime;
		if (transform.position.y < maximaAlturaDeVuelo)
		{
			Debug.Log("Fuerza");
			Vector3 fuerzaAAplicar = new Vector3(0,
										 200f,
										 0);
			rigidBody.AddForce(fuerzaAAplicar);
		}

		else
		{
			animator.SetBool("volando", false);
		}
	}

	/*void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			animator.SetTrigger("tocoPiso");
		}	}*/

}
