using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioClip levelSong, bossSong;
	private AudioSource audioSource;

	public float bossSongLoopLength = 45f;
	public float bossSongEndOfLoop = 70.5f;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		PlaySong(bossSong);
    }

	public void PlaySong(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();
	}

	public void Update()
	{
		if (audioSource.timeSamples > bossSongEndOfLoop* bossSong.frequency)
		{
			audioSource.timeSamples -= Mathf.RoundToInt(bossSongLoopLength * bossSong.frequency);  
		}
	}

	public void decrementarVolumenGeneral(float tiempoQueTardaEnCambiarVolumen)
	{
        StartCoroutine(EcualizadorGeneral(tiempoQueTardaEnCambiarVolumen, 0f));
	}

	public void aumentarVolumenGeneral(float tiempoQueTardaEnCambiarVolumen)
	{
		StartCoroutine(EcualizadorGeneral(tiempoQueTardaEnCambiarVolumen, 1f));	}

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
		}	}
}
