using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

[RequireComponent (typeof(SpriteRenderer))]
public class Objeto : MonoBehaviour {
	
	private SpriteRenderer spriteRenderer;
	private Jugador unJugador = null;
	//private bool objetoTirado = false;
	private Rigidbody rigidBody;

	void OnEnable(){
		//Me suscribo al evento de levantar item
		Jugador.eventoLevantarItem += levantarItem;
		Jugador.eventoTirarItem += tirarItem;
	}

	void OnDisable(){
		//Me desuscribo al evento de levantar item
		Jugador.eventoLevantarItem -= levantarItem;
		Jugador.eventoTirarItem -= tirarItem;

	}

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
        rigidBody = GetComponent<Rigidbody>();
	}

	void FixedUpdate(){
		chequearMovimiento();
		spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
	}

	void chequearMovimiento(){
		if(unJugador!=null){
			gameObject.transform.position = unJugador.transform.position + Vector3.up * 1.2f;
		}
	}

	void levantarItem(Jugador jugador){
		if(jugador.getItemTomado() != null){
			string nombreItemTomado = jugador.getItemTomado().name;
			bool esElObjetoTomado = string.Equals(nombreItemTomado, this.name);
			if(esElObjetoTomado){
				unJugador = jugador;
			}
		}
	}

	void tirarItem(float direccionATirar){
		if (unJugador != null)
		{
			//objetoTirado = true;
			//rigidBody.bodyType = RigidbodyType2D.Dynamic;
			rigidBody.AddForce(new Vector3(direccionATirar,.5f,0f)*400f);
			unJugador.setTieneItem(false);
		}
		unJugador = null;
	}

}