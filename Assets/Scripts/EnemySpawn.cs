using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	public float minZ, maxZ;
	public GameObject[] enemy;
	public int numberOfEnemies;
	public float spawnTime;
	private float maxXInicial;
	private int currentEnemies;

    // Start is called before the first frame update
    void Start()
    {
		maxXInicial = FindObjectOfType<CameraFollow>().maxXAndY.x;
		currentEnemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (currentEnemies >= numberOfEnemies)
		{
			int enemies = FindObjectsOfType<Rival1>().Length;
			if (enemies <= 0)
			{
				FindObjectOfType<CameraFollow>().maxXAndY.x = maxXInicial;
				gameObject.SetActive(false);
			}
		}
    }

	void SpawnEnemy()
	{
		bool positionX = Random.Range(0, 2) == 0 ? true : false;
		Vector3 spawnPosition;
		spawnPosition.z = Random.Range(minZ, maxZ);
		float piso = GameObject.FindWithTag("Piso").transform.position.y;

		if (positionX)
		{
			spawnPosition = new Vector3(transform.position.x + 10, piso, spawnPosition.z);
		}
		else
		{
			spawnPosition = new Vector3(transform.position.x - 10, piso, spawnPosition.z);
		}
		Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPosition, Quaternion.identity);
		currentEnemies++;
		if (currentEnemies < numberOfEnemies)
		{
			Invoke("SpawnEnemy", spawnTime);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GetComponent<BoxCollider>().enabled = false;
			FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
			SpawnEnemy();
		}
	}
}
