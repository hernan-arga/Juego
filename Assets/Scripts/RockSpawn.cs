using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawn : MonoBehaviour
{
	public GameObject piedra1, piedra2, piedra3;

	private int totalDePiedrasTiradas = 0;
	private List <GameObject> piedras;
	private float minX, maxX;
	private Camera camara;
	private AudioSource audioSource;
	public AudioClip rocksFalling;

	public float minZ, maxZ, spawnTime;
	public int totalDePiedrasATirar;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		camara = FindObjectOfType<Camera>();
		minX = camara.ViewportToWorldPoint(new Vector3(0,0,0)).x;
		maxX = camara.ViewportToWorldPoint(new Vector3(1,0,0)).x;
		piedras = new List<GameObject>();
		piedras.Add(piedra1);
		piedras.Add(piedra2);
		piedras.Add(piedra3);
		tirarPiedra();
		PlaySound(rocksFalling);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void tirarPiedra()
	{
		Vector3 spawnPosition = new Vector3();
		int indiceRandom = Random.Range(0, piedras.Count);
		spawnPosition.z = Random.Range(minZ, maxZ);
		spawnPosition.y = transform.position.y;
		spawnPosition.x = Random.Range(minX, maxX);

		Instantiate(piedras[indiceRandom], spawnPosition, Quaternion.identity);
		totalDePiedrasTiradas++;
		if (totalDePiedrasTiradas < totalDePiedrasATirar)
		{
			Invoke("tirarPiedra", spawnTime);
		}
		else
		{
			//FixMe: cameraFollow camara1 y Camera camara son el mismo pero aludiendo a la clase diferente de donde parten
			CameraFollow camara1 = FindObjectOfType<CameraFollow>();
			camara1.setSacudirCamara(false);
			Destroy(gameObject);
		}
	}

	public void PlaySound(AudioClip song)
	{
		audioSource.clip = song;
		audioSource.Play();	}
}
