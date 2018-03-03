using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CibleBehaviour : MonoBehaviour {

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Balle") {
			gameManager.addScore (1);
			gameManager.launchModeEdit ();
		} else {
			Debug.Log ("autre chose touche la cible ?!? (" + collider.name + ")" );
		}
	}
}
