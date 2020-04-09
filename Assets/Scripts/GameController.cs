using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoDeEscena
{
	Intro, Combate, JefeDerrotado, FinalDeEscena
}

public class GameController : MonoBehaviour
{
	public EstadoDeEscena EstadoDeEscena { get; set;}
	public CameraFollow Camara;
	public ColoreadorDeCamara coloreadorDeCamara;
	public float tiempoDeAparicionDeEscena = 5f;
	public MusicController controladorDeMusica;

    // Start is called before the first frame update
    void Start()
    {
		EstadoDeEscena = EstadoDeEscena.Intro;
		ResaltarEscena();
    }

	void FixedUpdate()
	{
		if (EstadoDeEscena.Equals(EstadoDeEscena.FinalDeEscena))
		{
			DesvanecerEscena();
			Invoke("CargarSiguienteEscena", tiempoDeAparicionDeEscena);
		}
	}

	public void DesvanecerEscena()
	{
		coloreadorDeCamara.oscurecerEscena(tiempoDeAparicionDeEscena);
		controladorDeMusica.decrementarVolumenGeneral(tiempoDeAparicionDeEscena);
	}

	public void ResaltarEscena()
	{
		coloreadorDeCamara.aclararEscena(tiempoDeAparicionDeEscena);
		controladorDeMusica.aumentarVolumenGeneral(tiempoDeAparicionDeEscena);
		coloreadorDeCamara.desvanecerTituloDeEscena(tiempoDeAparicionDeEscena);
	}

	void CargarSiguienteEscena()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
