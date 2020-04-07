using System;

public class Ezequiel : Jugador
{
	// Update is called once per frame
	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (EscenaEnIntroduccion() || EscenaEnCombate())
		{
			animator.SetBool("Moribundo", true);
		}

		else
		{
			animator.SetBool("Moribundo", false);
		}
    }

	protected override bool EstaDisponibleParaJugar()
	{
		return base.EstaDisponibleParaJugar() && puedeMoverse;
	}

	protected override void ActivarPoder()
	{
		//Implementar el poder aca
		throw new NotImplementedException();
	}
}
