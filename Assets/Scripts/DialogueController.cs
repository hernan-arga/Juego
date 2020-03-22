using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
	public Dialogue Dialogue;
	Queue<string> Sentences;
	IEnumerator CoroutineTypeText;
	public GameObject DialoguePanel;
	public TextMeshProUGUI DisplayText;
	string ActiveSentence;
	public float TypingSpeed;
	AudioSource MyAudio;
	public AudioClip SpeakSound;

    // Start is called before the first frame update
    void Start()
    {
		Sentences = new Queue<string>();
		MyAudio = GetComponent<AudioSource>();
		StartDialogueSystem();

		DialoguePanel.SetActive(true);
        DisplayNextSentence();
    }

	void StartDialogueSystem()
    {
		Sentences.Clear();

		foreach (string sentence in Dialogue.SentenceList)
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

			DialoguePanel.SetActive(false);            
			return;
		}

		ActiveSentence = Sentences.Dequeue();
		DisplayText.text = ActiveSentence;

		if (CoroutineTypeText != null)
		{
			StopCoroutine(CoroutineTypeText);
		}
		CoroutineTypeText = TypeTheSentence(ActiveSentence);
		StartCoroutine(CoroutineTypeText);
	}

	IEnumerator TypeTheSentence(string sentence)
	{
		DisplayText.text = "";

		foreach (char letter in sentence)
		{
			DisplayText.text += letter;
			//MyAudio.PlayOneShot(SpeakSound);
			yield return new WaitForSeconds(TypingSpeed);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.D) && DisplayText.text.Equals(ActiveSentence))
		{
			DisplayNextSentence();
		}
	}
}
