using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonBehaviour : MonoBehaviour {
	public float deplacement;
	public float tempsExtension;
	public float tempsRepli;
	public float tempsPause;
	private float tempsFin;
	private PistonEtat etat;
	private Vector3 positionExtension;
	private Vector3 positionRepli;
	private Rigidbody rb;
	private Vector3 vitesseExtension;
	private Vector3 vitesseRepli;
	private bool doitEtreEnclanche;

	// Use this for initialization
	void Start () {
		Debug.Log ("mode EXTENSION");
		etat = PistonEtat.REPLI;
		tempsFin = -1.0f;
		positionExtension = transform.position + transform.up * deplacement;
		positionRepli = transform.position;
		rb = GetComponent<Rigidbody> ();
		vitesseExtension = transform.up * deplacement / tempsExtension;
		vitesseRepli = -transform.up * deplacement / tempsRepli;
		doitEtreEnclanche = false;
	}
	
	void FixedUpdate () {
		if ( Time.time >= tempsFin) {
			//On passe à l'état suivant
			switch (etat) {
			case PistonEtat.EXTENSION:
				Debug.Log ("mode PAUSE");
				etat = PistonEtat.PAUSE;
				tempsFin = Time.time + tempsPause;
				rb.velocity = Vector3.zero;
				transform.position = positionExtension;
				doitEtreEnclanche = false;
				break;
			case PistonEtat.PAUSE:
				Debug.Log ("mode REPLI");
				etat = PistonEtat.REPLI;
				tempsFin = Time.time + tempsRepli;
				rb.velocity = Vector3.zero;
				rb.AddForce (vitesseRepli, ForceMode.VelocityChange);
				break;
			case PistonEtat.REPLI:
				rb.velocity = Vector3.zero;
				transform.position = positionRepli;
				if(doitEtreEnclanche){
					Debug.Log ("mode EXTENSION");
					etat = PistonEtat.EXTENSION;
					tempsFin = Time.time + tempsExtension;
					rb.AddForce (vitesseExtension, ForceMode.VelocityChange);
				}
				break;
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Player") {
			this.doitEtreEnclanche = true;
		}
	}
}


public enum PistonEtat {
	EXTENSION,
	REPLI,
	PAUSE
}
