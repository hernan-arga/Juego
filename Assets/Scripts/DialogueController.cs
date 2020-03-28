using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
	public Dialogue Dialogue;
	Queue<messageDialog> Sentences;
	IEnumerator CoroutineTypeText;
	public GameObject DialoguePanel;
	public TextMeshProUGUI DisplayText;
	public TextMeshProUGUI SpeakerName;
	messageDialog ActiveSentence;
	AudioSource MyAudio;
	public GameController GameController;

    // Start is called before the first frame update
    void Start()
    {
		Sentences = new Queue<messageDialog>();
		MyAudio = GetComponent<AudioSource>();
		StartDialogueSystem();

		DialoguePanel.SetActive(true);
        DisplayNextSentence();
    }

	void StartDialogueSystem()
    {
		Sentences.Clear();

		foreach (messageDialog sentence in Dialogue.SentenceList)
		{
			Sentences.Enqueue(sentence);
		}

	}

	void DisplayNextSentence()
	{
		if (Sentences.Count <= 0)
		{
			if (CoroutineTypeText != null)
			{
	            StopCoroutine(CoroutineTypeText);
			}

			GameController.EstadoDeEscena = Dialogue.EstadoDeEscenaAlQueCambiar;
			DialoguePanel.SetActive(false);            
			return;
		}

		ActiveSentence = Sentences.Dequeue();

		if (CoroutineTypeText != null)
		{
			StopCoroutine(CoroutineTypeText);
		}

		SpeakerName.text = ActiveSentence.speakerName;
		CoroutineTypeText = TypeTheSentence(ActiveSentence);
		StartCoroutine(CoroutineTypeText);
	}

	IEnumerator TypeTheSentence(messageDialog activeSentence)
	{
		DisplayText.text = "";

		foreach (char letter in activeSentence.message)
		{
			DisplayText.text += letter;
			MyAudio.PlayOneShot(activeSentence.SpeakSound);
			yield return new WaitForSeconds(activeSentence.TypingSpeed);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J) && DisplayText.text.Equals(ActiveSentence.message))
		{
			DisplayNextSentence();
		}
	}
}
