using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CibleBehaviour : MonoBehaviour {
    Random random;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Balle") {
			Debug.Log ("la balle touche la cible");
            
		} else {
			Debug.Log ("autre chose touche la cible ?!? (" + collider.name + ")" );
		}
	}
}
