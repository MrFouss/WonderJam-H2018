using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetInterraction : MonoBehaviour {

	public GameManager gameManager;
	public bool canUpdate;

	void Awake(){

	}

	// Use this for initialization
	public void Start () {
		canUpdate = true;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		gameManager.myaction += setUpdate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setUpdate(bool etat){
		canUpdate = etat;
		gameManager.myaction -= setUpdate;
	}

	public void ternirObjet(){

	}
}
