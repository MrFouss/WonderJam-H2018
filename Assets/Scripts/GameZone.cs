using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit(Collider other){
		if(other.tag.Equals("Balle")){
			Debug.Log ("Retour Mode Edition");
			//placer fonction pour relancer le mode édition
		}
	}
}
