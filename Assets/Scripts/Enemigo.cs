﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemigo : MonoBehaviour
{
	public string nombre;
	public Sprite avatar;
	protected bool isDead = false;
	protected bool daniado = false;
	public float damageTime = 0.3f;
	protected float damageTimer = 0f, tiempoActualDeAtaque;
	public float maxSalud = 10f, tiempoPorDefectoDeAtaque;
	protected float saludActual;
	protected SpriteRenderer spriteRenderer;
	protected Rigidbody rigidBody;
	protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		tiempoActualDeAtaque = tiempoPorDefectoDeAtaque;
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
        saludActual = maxSalud;
    }

    // Update is called once per frame
    void Update()
    {
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

	protected virtual void atacarJugador()
	{
		tiempoActualDeAtaque += Time.deltaTime;
		if (tiempoActualDeAtaque > tiempoPorDefectoDeAtaque)
		{
            golpear();
			tiempoActualDeAtaque = 0f;
		}
	}

	protected virtual void golpear()
	{
		//Cada enemigo implementa el suyo
	}

	public void recibirDanio(float danio, bool golpeadoPorIzq)
	{
		if (!isDead)
		{
			daniado = true;
			saludActual -= danio;
			if (saludActual <= 0f)
			{
				if (golpeadoPorIzq)
				{
					animator.SetTrigger("matadoPorIzq");
				}
				else
				{
					animator.SetTrigger("matadoPorDer");
				}

				saludActual = 0f;
				isDead = true;
				rigidBody.AddRelativeForce(new Vector3(3f, 5f, 0f), ForceMode.Impulse);
			}
			else
			{
				animator.SetTrigger("daniado");
			}

			FindObjectOfType<UIManager>().updateEnemyUi(maxSalud, saludActual, nombre, avatar);

		}
	}

	public void Morir()
	{
		Destroy(gameObject);
	}
}
