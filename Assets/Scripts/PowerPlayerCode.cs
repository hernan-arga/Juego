using UnityEngine;

public class PowerPlayerCode : SpecialCode
{
	public bool EstaActivadoElPoder { get; private set; }

    // Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		keys.Add(KeyCode.J);
		keys.Add(KeyCode.D);
		keys.Add(KeyCode.L);
    }

	protected override void Update()
	{
		if (!EstaActivadoElPoder)
		{
			base.Update();
		}
	}

	public void DesactivarPoder()
	{
		EstaActivadoElPoder = false;	}

	public void FinalizarCodigo(){
		CodigoActivado = false;
	}

	public void ActivarPoder()
	{
		EstaActivadoElPoder = true;
	}

}
