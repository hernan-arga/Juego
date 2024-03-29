﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Jugador))]
public class UIManager : MonoBehaviour
{
	public Slider healthUI;
	public Image playerImage;
	public TextMeshProUGUI playerName;
	public Image powerIcon;

	public GameObject enemyUi;
	public Slider enemySlider;
	public TextMeshProUGUI enemyName;
	public Image enemyImage;
	public float enemyUiTime = 4f;

	float enemyTimer;

	public Jugador player;

    // Start is called before the first frame update
    void Start()
    {
		//player = FindObjectOfType<Jugador>();
		healthUI.maxValue = player.maxSalud;
		healthUI.value = healthUI.maxValue;
		playerName.text = player.nombre;
		playerImage.sprite = player.avatar;
    }

    // Update is called once per frame
    void Update()
    {
		enemyTimer += Time.deltaTime;
		if (enemyTimer >= enemyUiTime)
		{
			enemyUi.SetActive(false);
			enemyTimer = 0;
		}
    }

	public void updateHealth(float cantidad)
	{
		healthUI.value = cantidad;
	}

	public void updateEnemyUi(float maxSalud, float saludActual, string nombre, Sprite avatar)
	{
		enemySlider.maxValue = maxSalud;
		enemySlider.value = saludActual;
		enemyName.text = nombre;
		enemyImage.sprite = avatar;
		enemyTimer = 0;
		enemyUi.SetActive(true);
	}

	public void activatePowerIcon()
	{
		powerIcon.enabled = true;
	}

	public void desactivatePowerIcon()
	{
		powerIcon.enabled = false;
	}

}
