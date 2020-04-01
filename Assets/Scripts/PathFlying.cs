using UnityEngine;

public class PathFlying : MonoBehaviour
{
	Vector3 posicionInicial;
	public Vector3 posicionNodoActualASeguir{ get; private set;}
	public Vector3 posicionUltimoNodo { get; private set; }
	Transform parentTransform;
	int nodoActual = 0;
	bool pathFijado;
	public float rateVelocidad { get; private set;}

	public Boss1 Dragon;
	public GameObject[] nodos;

	// Start is called before the first frame update
	void Start()
	{
		parentTransform = transform.parent;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void reiniciarPathVuelo()
	{
		posicionUltimoNodo = Dragon.transform.position;
		posicionNodoActualASeguir = Dragon.transform.position;
		controlarNodos();
	}


	public void avanzarAlSiguienteNodo()
	{
		if (Dragon.estaDelLadoDerechoDeLaPantalla)
		{
			avanzarAlNodoPorDerecha();
		}

		else
		{
			avanzarAlNodoPorIzquierda();
		}

	}

	void avanzarAlNodoPorDerecha()
	{
		if (nodoActual < nodos.Length - 1)
		{
			nodoActual++;
			controlarNodos();
		}

		else
		{
			Dragon.Aterrizar();
			Dragon.estaDelLadoDerechoDeLaPantalla = false;
		}
	}

	void avanzarAlNodoPorIzquierda()
	{
		if (nodoActual > 0)
		{
			nodoActual--;
			controlarNodos();
		}

		else
		{
			Dragon.Aterrizar();
			Dragon.estaDelLadoDerechoDeLaPantalla = true;
		}
	}

	void controlarNodos()
	{
		posicionUltimoNodo = posicionNodoActualASeguir;
		posicionNodoActualASeguir = nodos[nodoActual].transform.position;
		Dragon.porcentajeLerp = 0f;

		//Porcentaje de cuanto va desplazandose por segundo
		//Ej si la distancia es de 2 y la velocidad es de 1, entonces se desplaza 50% por cada segundo
		rateVelocidad = (1f / Vector3.Distance(posicionUltimoNodo, posicionNodoActualASeguir)) * Dragon.velocidad;
	}

	public bool NodoActualEsElUltimo()
	{
		return nodoActual.Equals(nodos.Length - 1);
	}

	public bool NodoActualEsElPrimero()
	{
		return nodoActual.Equals(0);
	}

	public void DesasignarPadre()
	{
		transform.parent = null;
	}

	public void AsignarPadre()
	{
		transform.parent = parentTransform;
	}
}
