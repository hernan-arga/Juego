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
	public delegate void _tirarItem();
	public static event _tirarItem eventoTirarItem;
	
	private SpriteRenderer spriteRenderer;
	private Animator animator;
	private Rigidbody2D rigidBody2D;
	private bool estaCaminando = false;
	private bool estaCorriendo = false;
	private bool tieneItem = false;
	private GameObject itemTomado = null;
	private bool estaSaltando = false;
	private bool sePuedeMover = true;

	private bool reinicioDeComboTimer;
	private float tiempoComboTimerActual;
	private Ataque golpeActual;

	public float poderDeVelocidad = 2;
	public float maxVelocidad = 5;
	public float poderDeSalto = 6.5f;
	public float tiempoComboTimerDefault = 0.4f;
	public float velocidadHorizontal;
	public float velocidadVertical;
	public float modificadorDeAceleracion;
	public float distanciaMinimaParaLevantar1Objeto;
	Vector2 control;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		//gameObject == Self
		animator = gameObject.GetComponentInParent<Animator> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();

		tiempoComboTimerActual = tiempoComboTimerDefault;
		golpeActual = Ataque.NINGUNO;
	}
	


	void FixedUpdate ()
	{
		
		chequearMoverse ();
		chequearGolpe ();
		chequearLevantarItem();
		chequearTirarItem ();
		chequearSaltar();
		reiniciarComboTimer();
		controlarOrdenDeCapa ();
		setearAnimaciones ();


	}

	void chequearSaltar(){
		if(Input.GetKeyDown (KeyCode.K) && !estaSaltando){
 			rigidBody2D.gravityScale = 1f;
			animator.SetBool ("estaSaltando", true);
			rigidBody2D.velocity = Vector2.up * poderDeSalto;
			estaSaltando = true;
			sePuedeMover = false;
			Invoke ("FrenarSalto", 2f);
		}
	}

	void FrenarSalto(){
		rigidBody2D.gravityScale = 0f;

		Vector2 frenoDeVelocidad;
		frenoDeVelocidad.x = 0f;
		frenoDeVelocidad.y = 0f;

		rigidBody2D.velocity = frenoDeVelocidad;

		animator.SetBool ("estaSaltando", false);
		estaSaltando = false;
		sePuedeMover = true;
	}

	void chequearTirarItem(){
		if(tieneItem &&  Input.GetKeyDown (KeyCode.J)){
			animator.SetTrigger ("tirandoItem");
		}
	}

	void chequearLevantarItem(){
		List<GameObject> itemsActivos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
		List<GameObject> itemsCercanos = itemsActivos
			.Where( unItem => distanciaAObjeto(unItem) < distanciaMinimaParaLevantar1Objeto)
			.ToList();
		bool laListaEstaVacia = itemsCercanos.Count.Equals(0);
		bool hayItemsCerca = itemsCercanos != null && !laListaEstaVacia;

		if(hayItemsCerca && Input.GetKeyDown (KeyCode.J) && !tieneItem){
			animator.SetTrigger ("levantandoItem");
			itemTomado = itemsCercanos [0];
		}

	}

	//Comparar distancias al cuadrado es mas rapido que usar la funcion distance
	float distanciaAObjeto(GameObject unObjeto){
		return (transform.position - unObjeto.transform.position).sqrMagnitude;
	}

	void chequearGolpe(){
		if (Input.GetKeyDown (KeyCode.L)) {
			reinicioDeComboTimer = true;
			//tiempoComboTimerActual = tiempoComboTimerDefault;
			golpeActual++;

			switch(golpeActual){
				case (Ataque.GOLPEDERECHO):
					animator.SetTrigger ("golpeDerecho");
					break;
				case (Ataque.GOLPEIZQUIERDO):
					animator.SetTrigger ("golpeIzquierdo");
					break;
				case (Ataque.PATADA):
					animator.SetTrigger ("patada");
					golpeActual = Ataque.NINGUNO;
					break;
				default:
					break;
			}
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
		bool hayMovimientoVertical = control.y != 0f;
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


		desacelerarMovimiento();

		agregarFuerzaDeMovimiento();

		float limitedSpeedX = Mathf.Clamp(rigidBody2D.velocity.x, -maxVelocidad, maxVelocidad);
		float limitedSpeedY = Mathf.Clamp(rigidBody2D.velocity.y, -maxVelocidad, maxVelocidad);

		rigidBody2D.velocity = new Vector2(limitedSpeedX * acelerador, limitedSpeedY * acelerador);

		/*if(hayMovimientoHorizontal || hayMovimientoVertical){
			rigidBody2D.velocity = new Vector2 (control.x * velocidadHorizontal * acelerador,
				control.y * velocidadVertical * acelerador);
		}*/
	}

	void agregarFuerzaDeMovimiento(){
		
		control = new Vector2 (Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));

		rigidBody2D.AddForce(Vector2.right * control.x * poderDeVelocidad);

		if(sePuedeMover){
			rigidBody2D.AddForce(Vector2.up * control.y * poderDeVelocidad);
		}

	}

	void desacelerarMovimiento(){
		
		Vector2 fixedVelocity = rigidBody2D.velocity;
		fixedVelocity.x *= 0.75f;

		//Si esta saltando lo desacelero cuando llega al piso
		if(!estaSaltando){
			fixedVelocity.y *= 0.75f;
		}

		rigidBody2D.velocity = fixedVelocity;

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
		spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
	}

	void setearAnimaciones(){
		animator.SetBool ("estaCaminando", estaCaminando);
		animator.SetBool ("estaCorriendo", estaCorriendo);
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
			eventoTirarItem ();
			itemTomado = null;
		}
	}

	public GameObject getItemTomado(){
		return itemTomado;
	}

	public void setTieneItem(bool valor){
		tieneItem = valor;
	}

}