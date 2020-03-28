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
	bool typingText;

	// Start is called before the first frame update
	void Start()
	{
		typingText = false;
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
			StopTypingText();
			GameController.EstadoDeEscena = Dialogue.EstadoDeEscenaAlQueCambiar;
			DialoguePanel.SetActive(false);
			Destroy(gameObject);
			return;
		}

		ActiveSentence = Sentences.Dequeue();

        StopTypingText();
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
			typingText = true;
		}

		activeSentence.EventosADisparar.Invoke();

		typingText = false;
	}

	void FinishActiveSentence()
	{
        StopCoroutine(CoroutineTypeText);
		DisplayText.text = ActiveSentence.message;
		ActiveSentence.EventosADisparar.Invoke();
		typingText = false;
	}

	void StopTypingText()
	{
		if (CoroutineTypeText != null)
		{
        	StopCoroutine(CoroutineTypeText);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			if (typingText)
			{
				FinishActiveSentence();
			}

			else
			{
                DisplayNextSentence();
			}

		}

	}


}
