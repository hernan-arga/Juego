using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diego : Jugador
{
	public Jugador Companiero;
	Vector3 posicionDetrasDeCompañero;
	public GameObject objetoDialogoFinalPrimerEscena;

	protected override void Start()
	{
		base.Start();
		SetEstaLeyendo(true);
		posicionDetrasDeCompañero = Companiero.transform.position + new Vector3(0f, 0f, 2f);
	}

    // Update is called once per frame
    protected override void FixedUpdate()
    {
		base.FixedUpdate();

		if (EscenaEnCombate())
		{
			puedeMoverse = true;
		}

		else
		{
			puedeMoverse = false;

			if (EscenaEnJefeDerrotado())
			{
				if (!LlegoADondeEstaElTarget(posicionDetrasDeCompañero))
				{
					CorrerHaciaTarget(posicionDetrasDeCompañero);
				}
				else
				{
					animator.SetTrigger("Llorando");
					objetoDialogoFinalPrimerEscena.SetActive(true);
				}
			}
		}
    }


	bool LlegoADondeEstaElTarget(Vector3 target)
	{
		return (transform.position - target).sqrMagnitude < 1f;
	}

	void CorrerHaciaTarget(Vector3 target)
	{
		//hago que el sprite del  rival mire hacia donde esta el target
		float direccionX = Mathf.Sign(target.x - transform.position.x);
		transform.localScale = new Vector3(direccionX, 1f, 1f);

		transform.position = Vector3.MoveTowards(transform.position, target, 
		                                         poderDeVelocidad* Time.deltaTime);
		animator.SetBool("estaCorriendo", true);
	}

	protected override bool EstaDisponibleParaJugar()
	{
		return base.EstaDisponibleParaJugar() && puedeMoverse;
	}

	public void SetEstaLeyendo(bool valor)
	{
		animator.SetBool("Leyendo", valor);
	}
}
