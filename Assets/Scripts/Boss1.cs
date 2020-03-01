using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemigo
{
	public GameObject llamaradas;
	private GameObject fuegoInvocado;

	public RockSpawn tiradorDeRocas;

    // Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.H))
		{
			animator.SetTrigger("tocoPiso");
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



}
