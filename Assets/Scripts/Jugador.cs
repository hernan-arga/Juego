using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Ataque{
	NINGUNO, GOLPEDERECHO, GOLPEIZQUIERDO, PATADA
}

//[ExecuteInEditMode]
[RequireComponent (typeof(SpriteRenderer))]
public class Jugador : MonoBehaviour
{
	public delegate void _levantarItem(Jugador jugador);
	public static event _levantarItem eventoLevantarItem;
	public delegate void _tirarItem(float direccionATirar);
	public static event _tirarItem eventoTirarItem;
	
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Rigidbody rigidBody;
	private bool estaCaminando = false;
	private bool estaCorriendo = false;
	private bool tieneItem = false;
	private GameObject itemTomado = null;
	private bool tocandoPiso = false;

	private bool reinicioDeComboTimer;
	private float tiempoComboTimerActual;
	private Ataque golpeActual;

	public float minHeight, maxHeight;
	public float poderDeVelocidad = 2f;
	public float poderDeSalto = 6.5f;
	public float tiempoComboTimerDefault = 0.4f;
	public float modificadorDeAceleracion;
	public float distanciaMinimaParaLevantar1Objeto;
	public string nombre;
	public Sprite avatar;
	Vector3 control;

	private bool isDead = false, daniado = false;
	public float damageTime = 0.3f;
	private float damageTimer = 0f;
	public int maxSalud = 10;
	private int saludActual;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		//gameObject == Self
		animator = gameObject.GetComponentInParent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();

		tiempoComboTimerActual = tiempoComboTimerDefault;
		golpeActual = Ataque.NINGUNO;
		saludActual = maxSalud;
	}

	void Update()
	{
		if (daniado && !isDead)
		{
			damageTimer += Time.deltaTime;
			if (damageTimer >= damageTime)
			{
				daniado = false;
				damageTimer = 0f;
			}
		}
	}

	void FixedUpdate ()
	{
		if (!isDead && !daniado)
		{
			chequearMoverse();
			chequearGolpe();
			chequearLevantarItem();
			chequearTirarItem();
			chequearSaltar();
			reiniciarComboTimer();
			controlarOrdenDeCapa();
			setearAnimaciones();
		}

		//Defino el maximo espacio en el que se puede mover el personaje
		float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10)).x;
		float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
		rigidBody.position = new Vector3(Mathf.Clamp(rigidBody.position.x, minWidth + 1, maxWidth - 1),
			                          rigidBody.position.y, Mathf.Clamp(rigidBody.position.z, minHeight, maxHeight));
	}

	void chequearSaltar(){
		if(Input.GetKeyDown (KeyCode.K) && tocandoPiso){
			rigidBody.velocity = Vector3.zero;
			rigidBody.AddForce(Vector3.up * poderDeSalto);
		}

	}


	void chequearTirarItem(){
		if(tieneItem &&  Input.GetKeyDown (KeyCode.J)){
			animator.SetTrigger ("tirandoItem");
		}
	}

	void chequearLevantarItem(){
		List<GameObject> itemsActivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
		List<GameObject> itemsCercanos = itemsActivos
			.Where( unItem => cumpleCondicionesParaSerLevantado(unItem))
			.ToList();
		bool laListaEstaVacia = itemsCercanos.Count.Equals(0);
		bool hayItemsCerca = itemsCercanos != null && !laListaEstaVacia;

		if(hayItemsCerca && Input.GetKeyDown (KeyCode.J) && !tieneItem){
			animator.SetTrigger ("levantandoItem");
			itemTomado = itemsCercanos [0];
		}

	}


	bool cumpleCondicionesParaSerLevantado(GameObject unItem){
		return distanciaAObjeto(unItem) < distanciaMinimaParaLevantar1Objeto && unItem.transform.position.y < this.transform.position.y;
	}

	//Comparar distancias al cuadrado es mas rapido que usar la funcion distance
	float distanciaAObjeto(GameObject unObjeto){
		return (transform.position - unObjeto.transform.position).sqrMagnitude;
	}

	void chequearGolpe(){
		if (Input.GetKeyDown(KeyCode.L) && tocandoPiso)
		{
			reinicioDeComboTimer = true;
			//tiempoComboTimerActual = tiempoComboTimerDefault;
			golpeActual++;

			switch (golpeActual)
			{
				case (Ataque.GOLPEDERECHO):
					animator.SetTrigger("golpeDerecho");
					break;
				case (Ataque.GOLPEIZQUIERDO):
					animator.SetTrigger("golpeIzquierdo");
					break;
				case (Ataque.PATADA):
					animator.SetTrigger("patada");
					golpeActual = Ataque.NINGUNO;
					break;
				default:
					break;
			}
		}

		else if (Input.GetKeyDown(KeyCode.L) && !tocandoPiso)
		{
			animator.SetTrigger("patada");
		}
	}

	void reiniciarComboTimer(){
		if(reinicioDeComboTimer){
			tiempoComboTimerActual -= Time.deltaTime;
			if(tiempoComboTimerActual<=0){
				golpeActual = Ataque.NINGUNO;
				reinicioDeComboTimer = false;
				tiempoComboTimerActual = tiempoComboTimerDefault;
			}	
		}
	}

	void chequearMoverse ()
	{	
		bool hayMovimientoHorizontal = control.x != 0f;
		bool hayMovimientoVertical = control.z != 0f;
		bool hayMovimiento = hayMovimientoHorizontal || hayMovimientoVertical;
		float acelerador = 1f;

		if (hayMovimientoHorizontal) {
			controlarVolteoDeSprite ();
		}
		if (Input.GetKey (KeyCode.LeftShift)) {
			estaCorriendo = hayMovimiento;
			estaCaminando = false;
			acelerador = modificadorDeAceleracion;
		} else {
			estaCorriendo = false;
			estaCaminando = hayMovimiento;
		}


		control = new Vector3 (Input.GetAxisRaw("Horizontal"), 0f,
			Input.GetAxisRaw("Vertical"));

		rigidBody.velocity = new Vector3(control.x * poderDeVelocidad * acelerador,
		                                 rigidBody.velocity.y, 
		                                 control.z * poderDeVelocidad * acelerador);
	}


	void controlarVolteoDeSprite(){
		transform.localScale = new Vector3(Mathf.Sign(control.x), 1f, 1f);
	}

	void controlarOrdenDeCapa(){
		/*
		 * fijo la capa dependiendo de la altura, multiplico por 100 
		 * para agarrar algunos decimales y el menos es porque cuanto mas abajo mayor 
		 * orden de capa
		*/
		spriteRenderer.sortingOrder = -(int)(transform.position.z * 100);
	}

	void setearAnimaciones(){
		animator.SetBool ("estaCaminando", estaCaminando);
		animator.SetBool ("estaCorriendo", estaCorriendo);
		animator.SetBool("OnGround", tocandoPiso);
		/*animator.SetBool ("estaGolpeando", estaGolpeando);
		animator.SetBool ("estaGolpeandoDeNuevo", estaGolpeandoDeNuevo);*/
	}

	void onAgarrarItem(){
		if (eventoLevantarItem != null && !tieneItem) {
			eventoLevantarItem (this);
			tieneItem = true;
		}
	}

	void onTirarItem(){	
		if (eventoTirarItem != null && tieneItem) {
			float aQueLadoTirar = Mathf.Sign(transform.localScale.x);
			eventoTirarItem (aQueLadoTirar);
			itemTomado = null;
			tieneItem = false;
		}
	}

	public GameObject getItemTomado(){
		return itemTomado;
	}

	public void setTieneItem(bool valor){
		tieneItem = valor;
	}

	public void GolpeadoPorAdelante(){
		animator.SetTrigger("esGolpeadoPorAdelante");
	}

	public void recibirDanio(int danio, bool golpeadoPorIzq)
	{
		if (!isDead)
		{
			daniado = true;
			saludActual -= danio;
			if (saludActual <= 0f)
			{
				if (golpeadoPorIzq)
				{
					animator.SetTrigger("matadoPorIzq");
				}
				else
				{
					animator.SetTrigger("matadoPorDer");
				}

				saludActual = 0;
				isDead = true;
				rigidBody.AddRelativeForce(new Vector3(3f, 5f, 0f), ForceMode.Impulse);
			}
			else
			{
				animator.SetTrigger("daniado");
			}

			FindObjectOfType<UIManager>().updateHealth(saludActual);

		}
	}

	//TODO: aparte de desactivarse supongo que tendria que volver a la pantalla principal o algo
	public void Morir()
	{
		gameObject.SetActive(false);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = true;
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = true;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Piso")
		{
			tocandoPiso = false;
		}
	}

}