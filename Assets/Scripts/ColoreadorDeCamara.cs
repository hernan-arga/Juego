using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColoreadorDeCamara : MonoBehaviour
{
	Image fondo;
	public TextMeshProUGUI tituloDeEscena;

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
		StartCoroutine(Opacador(tiempoDeColoreado, 0f));
	}

	public void oscurecerEscena(float tiempoDeColoreado)
	{
		StartCoroutine(Opacador(tiempoDeColoreado, 1f));
	}

	public void desvanecerTituloDeEscena(float tiempoDeDesaparicion)
	{
		StartCoroutine(OpacadorDeTitulo(tiempoDeDesaparicion, 0f));
	}

	IEnumerator Opacador(float tiempoDeColoreado, float alphaFinal)
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

	IEnumerator OpacadorDeTitulo(float tiempoDeColoreado, float alphaFinal)
	{
		Color colorActual = tituloDeEscena.color;
		float alphaInicial = colorActual.a;
		float lerpPorcentaje = 0f;
		float rateTiempo = 1f / tiempoDeColoreado; //Porcentaje de cuanto va coloreando por segundo
		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime * rateTiempo;
			colorActual.a = Mathf.Lerp(alphaInicial, alphaFinal, lerpPorcentaje);
			tituloDeEscena.color = colorActual;
			yield return 0;
		}	}
}
