using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoDeEscena
{
	Intro, Combate, JefeDerrotado
}

public class GameController : MonoBehaviour
{
	public EstadoDeEscena EstadoDeEscena { get; set;}
	public CameraFollow Camara;
    // Start is called before the first frame update
    void Start()
    {
		EstadoDeEscena = EstadoDeEscena.Intro;
    }

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			EstadoDeEscena = EstadoDeEscena.Combate;
		}
	}

}
