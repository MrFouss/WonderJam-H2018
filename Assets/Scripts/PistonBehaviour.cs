using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonBehaviour : MonoBehaviour {
	public float deplacementMax;
	public float tempsExtension;
	public float tempsRepli;
	public float tempsPause;
	public AudioClip sonActivation;

    private float deplacementRepli;
    private float tempsFin;
	private PistonEtat etat;
	private Rigidbody rb;
	private float vitesseExtension;
	private float vitesseRepli;
	private bool doitEtreEnclanche;
	public bool actif;
	private AudioSource source;
	private GameManager gameManager;

	void Awake () {
        source = GetComponent<AudioSource>();
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		gameManager.myDelegate += setActif;

    }

	// Use this for initialization
	void Start () {
		Debug.Log ("mode EXTENSION");
		etat = PistonEtat.REPLI;
		tempsFin = -1.0f;
        deplacementRepli = (transform.position - this.transform.parent.position).magnitude;
		rb = GetComponent<Rigidbody> ();
		vitesseExtension = deplacementMax / tempsExtension;
		vitesseRepli = -deplacementMax / tempsRepli;
		doitEtreEnclanche = false;
		actif = false;
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
				transform.position = transform.parent.position + transform.parent.up*deplacementMax;
				doitEtreEnclanche = false;
				break;
			case PistonEtat.PAUSE:
				Debug.Log ("mode REPLI");
				etat = PistonEtat.REPLI;
				tempsFin = Time.time + tempsRepli;
				rb.velocity = Vector3.zero;
				rb.AddForce (transform.up * vitesseRepli, ForceMode.VelocityChange);
				break;
			case PistonEtat.REPLI:
				rb.velocity = Vector3.zero;
                transform.position = transform.parent.position + transform.parent.up * deplacementRepli;
                if (doitEtreEnclanche && actif){
					Debug.Log ("mode EXTENSION");
					etat = PistonEtat.EXTENSION;
					tempsFin = Time.time + tempsExtension;
					rb.AddForce (transform.up * vitesseExtension, ForceMode.VelocityChange);
					source.PlayOneShot(sonActivation);
				}
				break;
			}
		}
	}

	public void setActif(bool actif){
		this.actif = actif;
	}

	void OnTriggerStay(Collider collider){
		if (collider.tag == "Balle" && etat == PistonEtat.REPLI && actif) {
			this.doitEtreEnclanche = true;
		}
	}

}


public enum PistonEtat {
	EXTENSION,
	REPLI,
	PAUSE
}
