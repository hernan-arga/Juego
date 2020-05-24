using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioClip levelSong, bossSong;
	private AudioSource audioSource;

	public float transicionEntreCanciones = 2f;
	public float levelSongLoopLength = 40f;
	public float levelSongEndOfLoop = 40f;
	public float bossSongLoopLength = 45f;
	public float bossSongEndOfLoop = 70.5f;

	private AudioClip currentSong;
	private float currentSongLoopLength, currentSongEndOfLoop;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		PlaySong(levelSong);
		ChangeCurrentSongInfo(levelSong, levelSongLoopLength, levelSongEndOfLoop);
		//Empiezo el volumen general bajo asi lo aumento de a poco
		AudioListener.volume = 0.5f;
    }

	private void ChangeCurrentSongInfo(AudioClip song, float songLoopLength, float songEndOfLoop)
	{
		currentSong = song;
		currentSongLoopLength = songLoopLength;
		currentSongEndOfLoop = songEndOfLoop;
	}

	public void PlaySong(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();
	}

	public void PlayBossSong()
	{
		StartCoroutine(TransicionarCancion(transicionEntreCanciones, bossSong, bossSongLoopLength, bossSongEndOfLoop));
	}

	public void Update()
	{
		if (audioSource.timeSamples > currentSongEndOfLoop * currentSong.frequency)
		{
			audioSource.timeSamples -= Mathf.RoundToInt(currentSongLoopLength * currentSong.frequency);  
		}
	}

	public void decrementarVolumenGeneral(float tiempoQueTardaEnCambiarVolumen)
	{
        StartCoroutine(EcualizadorGeneral(tiempoQueTardaEnCambiarVolumen, 0f));
	}

	public void decrementarVolumenMusica()
	{
		StartCoroutine(EcualizadorMusica(transicionEntreCanciones, 0f));	}

	public void aumentarVolumenGeneral(float tiempoQueTardaEnCambiarVolumen)
	{
		StartCoroutine(EcualizadorGeneral(tiempoQueTardaEnCambiarVolumen, 1f));
	}

	//No solo ajusta el volumen de la musica sino de todos los sonidos que haya en el juego
	private IEnumerator EcualizadorGeneral(float tiempoQueTardaEnCambiarVolumen, float volumenFinal)
	{
		float volumenInicial = AudioListener.volume;
		float lerpPorcentaje = 0f;
		float rateTiempo = 1f / tiempoQueTardaEnCambiarVolumen; //Porcentaje de cuanto va coloreando por segundo
		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime * rateTiempo;
			AudioListener.volume = Mathf.Lerp(volumenInicial, volumenFinal, lerpPorcentaje);
			yield return 0;
		}
	}

	//Ajusta solo el volumen de la musica
	private IEnumerator EcualizadorMusica(float tiempoQueTardaEnCambiarVolumen, float volumenFinal)
	{
		float volumenInicial = audioSource.volume;
		float lerpPorcentaje = 0f;
		float rateTiempo = 1f / tiempoQueTardaEnCambiarVolumen; //Porcentaje de cuanto va coloreando por segundo
		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime * rateTiempo;
			audioSource.volume = Mathf.Lerp(volumenInicial, volumenFinal, lerpPorcentaje);
			yield return 0;
		}	}


	private IEnumerator TransicionarCancion(float tiempoDeTransicion, AudioClip temaACambiar, 
	                                        float songLoopLength, float songEndOfLoop)
	{
		//La mitad para silenciar un tema y la otra mitad para activar el siguiente
		float tiempoQueTardaEnCambiar = tiempoDeTransicion / 2f;
		float volumenFinal = 0f;
		float volumenInicial = audioSource.volume;
		float lerpPorcentaje = 0f;
		float rateTiempo = 1f / tiempoQueTardaEnCambiar; //Porcentaje de cuanto va coloreando por segundo
		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime * rateTiempo;
			audioSource.volume = Mathf.Lerp(volumenInicial, volumenFinal, lerpPorcentaje);
			yield return 0;
		}

		audioSource.Stop();
		PlaySong(temaACambiar);
        ChangeCurrentSongInfo(temaACambiar, songLoopLength, songEndOfLoop);

		// Aumento el volumen
		volumenFinal = volumenInicial;
		volumenInicial = audioSource.volume;
		lerpPorcentaje = 0f;

		while (lerpPorcentaje <= 1f)
		{
			lerpPorcentaje += Time.deltaTime* rateTiempo;
			audioSource.volume = Mathf.Lerp(volumenInicial, volumenFinal, lerpPorcentaje);
			yield return 0;
		}	}
}
