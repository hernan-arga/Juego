using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider healthUI;
	public Image playerImage;
	public Text playerName;

	public GameObject enemyUi;
	public Slider enemySlider;
	public Text enemyName;
	public Image enemyImage;
	public float enemyUiTime = 4f;

	private float enemyTimer;
	private Jugador player;

    // Start is called before the first frame update
    void Start()
    {
		player = FindObjectOfType<Jugador>();
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

	public void updateHealth(int cantidad)
	{
		healthUI.value = cantidad;
	}

	public void updateEnemyUi(int maxSalud, int saludActual, string nombre, Sprite avatar)
	{
		enemySlider.maxValue = maxSalud;
		enemySlider.value = saludActual;
		enemyName.text = nombre;
		enemyImage.sprite = avatar;
		enemyTimer = 0;
		enemyUi.SetActive(true);
	}
}
