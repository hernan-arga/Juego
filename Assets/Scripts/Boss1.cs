using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
	private bool isDead = false;
	public GameObject llamaradas;
	private GameObject fuegoInvocado;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void lanzarFuego()
	{
		if (!isDead)
		{
			Quaternion rotacionLlamarada = Quaternion.Euler(30f, -90f, 0f);
			fuegoInvocado = Instantiate(llamaradas, transform.position - 1.5f * Vector3.right, rotacionLlamarada);
		}
	}

	void terminarFuego()
	{
		Destroy(fuegoInvocado);
	}
}
