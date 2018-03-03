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
        Debug.Log("la balle touche la cible");
        if (collider.tag == "Balle") {
			Debug.Log ("la balle touche la cible");

			gameManager.cibleTouched();
            
			

		} else {
            if (!gameManager.editionMode)
            {
                gameManager.changeCible();
            }
		}
	}
}
