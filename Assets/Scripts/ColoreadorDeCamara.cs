using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoreadorDeCamara : MonoBehaviour
{
	private Image fondo;

	void Awake()
	{
		fondo = GetComponent<Image>();
		fondo.color = Color.black;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void aclararEscena(float tiempoDeColoreado)
	{
		StartCoroutine(Opacador(tiempoDeColoreado, 0f));	}

	public void oscurecerEscena(float tiempoDeColoreado)
	{
		StartCoroutine(Opacador(tiempoDeColoreado, 1f));
	}

	private IEnumerator Opacador(float tiempoDeColoreado, float alphaFinal)
	{
		Color colorActual = fondo.color;
		float alphaInicial = colorActual.a;
		float lerpPorcentaje = 0f;
		float rateTiempo = 1f / tiempoDeColoreado; //Porcentaje de cuanto va coloreando por segundo
		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime * rateTiempo;	
			colorActual.a = Mathf.Lerp(alphaInicial, alphaFinal, lerpPorcentaje);
			fondo.color = colorActual;
			yield return 0;
		}
	}
}
