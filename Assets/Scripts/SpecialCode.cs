using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialCode : MonoBehaviour
{
	public const float WaitTime = 0.5f;
	public bool CodigoActivado { get; protected set; }

	protected List<KeyCode> keys;

	float timer;
	int index;

	protected virtual void Start()
	{
		keys = new List<KeyCode>();
	}

	protected virtual void Update()
	{
		VerificarCodigo();
	}

	void VerificarCodigo()
	{
		if (Input.anyKeyDown)
		{
			EvaluarTecla();
		}

       controlarTimerEIndex();

	}

	void controlarTimerEIndex()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;

			if (timer < 0)
			{
				reiniciarTimerEIndex();
			}
		}

	}

	void EvaluarTecla()
	{
		if (Input.GetKeyDown(keys[index]))
		{
			EvaluarSiSeActivoElCodigo();
		}

		else
		{
			reiniciarTimerEIndex();
		}
	}

	void EvaluarSiSeActivoElCodigo()
	{
		if (index.Equals(keys.Count-1))
		{
			CodigoActivado = true;
			reiniciarTimerEIndex();
		}

		else
		{
			timer = WaitTime;
			index++;
		}
	}

	void reiniciarTimerEIndex()
	{
		timer = 0f;
		index = 0;
	}

}
