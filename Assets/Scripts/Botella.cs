using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botella : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private Rigidbody rigidBody;
	private Collider colliderBox;
	private bool golpeoElPiso = false;

	// Start is called before the first frame update
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigidBody = GetComponent<Rigidbody> ();
		colliderBox = GetComponent<Collider>();
	}

	// Update is called once per frame
	void Update()
	{
		controlarOrdenDeCapa();
	}

	void FixedUpdate()
	{
		bool terminoDeCaer = Mathf.Abs(rigidBody.velocity.y) < Mathf.Epsilon;

		if (golpeoElPiso)
		{
			if (terminoDeCaer)
			{
				rigidBody.isKinematic = true;
				colliderBox.isTrigger = true;
			}
		}

		else
		{
			if (!terminoDeCaer)
			{
				transform.Rotate(Vector3.forward,Time.deltaTime*180,Space.Self);
			}
		}


	}

	void OnCollisionEnter(Collision unaColision)
	{
		if (unaColision.collider.gameObject.CompareTag("Piso"))
		{
			golpeoElPiso = true;
		}
	}

	void controlarOrdenDeCapa()
	{
		/*
		 * fijo la capa dependiendo de la altura, multiplico por 100 
		 * para agarrar algunos decimales y el menos es porque cuanto mas abajo mayor 
		 * orden de capa
		*/
		spriteRenderer.sortingOrder = -(int)(transform.position.z * 108f);	}
}
