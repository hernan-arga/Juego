using UnityEngine;

public class Diego : Jugador
{
	public Jugador Companiero;
	Vector3 posicionDetrasDeCompañero;
	public GameObject objetoDialogoFinalPrimerEscena;
	public GameObject Poder;

	protected override void Start()
	{
		base.Start();
		SetEstaLeyendo(true);
		posicionDetrasDeCompañero = Companiero.transform.position + new Vector3(0f, 0f, 2f);
		CodigoDePoder.enabled = false;
	}

    // Update is called once per frame
    protected override void FixedUpdate()
    {
		base.FixedUpdate();

		if (EscenaEnCombate())
		{
			puedeMoverse = true;
			CodigoDePoder.enabled = true;
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

	protected override void ActivarPoder()
	{
		Vector3 posicionAInstanciarPoder;
		/*if (EstaMirandoALaDerecha())
		{
			posicionAInstanciarPoder = new Vector3(0.55f, 0.9f, 0f);
		}
		else { 
			posicionAInstanciarPoder = new Vector3(-0.7f, 0.9f, 0f);
		}*/posicionAInstanciarPoder = new Vector3(0.55f, 0.9f, 0f);
		//posicionAInstanciarPoder += transform.position;
		Instantiate(Poder, transform);
		Poder.transform.position = posicionAInstanciarPoder;
	}
}
