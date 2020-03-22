using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diego : Jugador
{
    // Update is called once per frame
    protected override void FixedUpdate()
    {
		base.FixedUpdate();

		if (EscenaEnIntroduccion())
		{
			animator.SetBool("Leyendo", true);
		}

		else
		{
			animator.SetBool("Leyendo", false);
			puedeMoverse = true;
		}
    }

	protected override bool EstaDisponibleParaJugar()
	{
		return base.EstaDisponibleParaJugar() && puedeMoverse;
	}
}
