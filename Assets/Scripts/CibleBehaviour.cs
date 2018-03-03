using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CibleBehaviour : MonoBehaviour {
    Random random;

	private GameManager gameManager;

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}

	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Balle") {
			Debug.Log ("la balle touche la cible");

			gameManager.cibleTouched();
            
			

		} else {
			Debug.Log ("autre chose touche la cible ?!? (" + collider.name + ")" );
		}
	}
}
