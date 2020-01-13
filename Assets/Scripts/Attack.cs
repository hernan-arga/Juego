using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public int danio;

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//fixme: esto no esta agarrando
	void OnCollisionEnter(Collision col)
	{
		Debug.Log("Golpeando");	}

	/*void OnCollisionEnter (Collision otro)
	{
		Debug.Log("Golpeando");
		Rival1 rival = otro.collider.GetComponent<Rival1>();
		if (rival != null)
		{
			rival.recibirDanio(danio);
		}
	}*/
}
