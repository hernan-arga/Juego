using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Jugador))]
public class CameraFollow : MonoBehaviour
{

	public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
	public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
	public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

	public Jugador jugadorTarget;
	private Transform m_Player; // Reference to the player's transform.
	private bool sacudirCamara = false;
	private float posicionInicialAntesDeSacudirLaCamara;

	[SerializeField]
	float shakeSpeed = 25f;

	[SerializeField]
	float maximumTranslationShake = 0.5f;

	private void Awake()
	{
		// Setting up the reference.
		m_Player = jugadorTarget.transform;
	}


	private bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return (transform.position.x - m_Player.position.x) < xMargin;
	}


	private void Update()
	{
		if (sacudirCamara)
		{
			//Fixme: cuando se sacude y me pongo contra el borde izquierdo, la camara se mueve.
			shakeCamera();
		}

		TrackPlayer();
	}


	private void TrackPlayer()
	{
		// By default the target x coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;

		// If the player has moved beyond the x margin...
		if (CheckXMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.deltaTime);
		}

		// The target x coordinate should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);

		// Set the camera's position to the target position with the same z and y components.
		transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
	}

	void shakeCamera()
	{
		/*
		 * Time.time es el tiempo que transcurrio desde que se inicio el juego.
         * Mathf.PerlinNoise genera psuedo-random numeros entre 0 y 1 (parecen randoms pero hay un algoritmo detras).
		 * Multiplico por 2 y resto 1 para pasar el rango de random de (0,1) a (-1,1)
		*/

		float movimientoRandom = (Mathf.PerlinNoise(0, Time.time * shakeSpeed) * 2 - 1) * maximumTranslationShake;
		float posicionEnX = transform.position.x;

		//Clampeo hasta cierto margen para que la camara no se empieze a mover hacia atras cuando se sacude
		posicionEnX = Mathf.Clamp(posicionEnX, posicionInicialAntesDeSacudirLaCamara, maxXAndY.x);
		posicionEnX += movimientoRandom;

		transform.localPosition = new Vector3(
				posicionEnX,
			Mathf.Clamp(transform.position.y + movimientoRandom, minXAndY.y, maxXAndY.y),
			transform.position.z);
	}

	public void setSacudirCamara(bool valor)
	{
		sacudirCamara = valor;
		posicionInicialAntesDeSacudirLaCamara = transform.position.x;
	}

	public bool getSacudirCamara()
	{
		return sacudirCamara;
	}

}

