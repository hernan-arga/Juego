using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct messageDialog
{
	[TextArea(1, 2)]
	public string speakerName;
	[TextArea(3, 10)]
	public string message;
	public AudioClip SpeakSound;
	public float TypingSpeed;
	public UnityEvent EventosADisparar;
}

[System.Serializable]
public class Dialogue
{
	public messageDialog[] SentenceList;
	public EstadoDeEscena EstadoDeEscenaAlQueCambiar;
}
