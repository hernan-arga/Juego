using UnityEngine;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class NotasMusicales : Attack
{
	public GameObject target;
	public float poderDeVelocidad = 20f, DistanciaMaximaParaSeguirATarget = 8f;
	Vector3 targetPosition;
	ParticleSystem ps;
	ParticleSystem.Particle[] particulas;
	float velocidadDeParticulas;
	Camera camara;
	// Start is called before the first frame update
	void Start()
	{
		ps = GetComponent<ParticleSystem>();
		particulas = new ParticleSystem.Particle[ps.main.maxParticles];
		velocidadDeParticulas = ps.main.startSpeed.constant;
		camara = FindObjectOfType<Camera>();
		IniciarTarget();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		InicializarVariablesParaLasParticulas();

		int numeroDeParticulasVivas = ps.GetParticles(particulas);
		for (int i = 0; i < numeroDeParticulasVivas; i++)
		{
			if (Vector3.Distance(particulas[i].position, targetPosition) > DistanciaMaximaParaSeguirATarget)
			{
				particulas[i].velocity = (targetPosition - particulas[i].position).normalized * velocidadDeParticulas;

			}
		}

		ps.SetParticles(particulas, numeroDeParticulasVivas);
	}

	// Update is called once per frame
	void Update()
	{
		if (!ps.IsAlive())
		{
			Destroy(gameObject);
		}	 }

	void InicializarVariablesParaLasParticulas()
	{
		ControlarTargetPosition();
		//|| particulas.Length < ps.main.maxParticles
		if (particulas == null)
			particulas = new ParticleSystem.Particle[ps.main.maxParticles];
	}

	void ControlarTargetPosition()
	{
		if (target)
		{
			targetPosition = target.transform.position;
		}

		else
		{
			targetPosition = Camera.main.ScreenToWorldPoint(new Vector2(camara.pixelWidth, camara.pixelHeight));
		}
	}

	void IniciarTarget()
	{
		List<GameObject> enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemigo"));
		if (enemigos.Count > 0f)
		{
			int pos = Random.Range(0, enemigos.Count - 1);
			target = enemigos[pos];
		}
	}

	void OnParticleCollision(GameObject col)
	{
		Enemigo enemigo = col.GetComponent<Enemigo>();
		bool golpeoPorIzq = (transform.position.x - col.transform.position.x) < 0;

		if (enemigo != null)
		{
			enemigo.recibirDanio(danio, golpeoPorIzq);
		}
	}
}
