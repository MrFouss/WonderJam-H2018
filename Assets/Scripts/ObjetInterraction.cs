using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetInterraction : MonoBehaviour {

	public GameManager gameManager;
	public bool canUpdate;

	public float diviseColorAlbedo;
	public float diviseColorEmissive;

	private Color ColorAlbedo;
	private Color ColorEmissive;

	void Awake(){

	}

	// Use this for initialization
	public void Start () {
		canUpdate = true;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		gameManager.myaction += setUpdate;
		ColorAlbedo = GetComponent<Renderer> ().material.GetColor ("_Color");
		ColorEmissive = GetComponent<Renderer> ().material.GetColor ("_EmissionColor");

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			ternirObjet ();
		}
	}

	public void setUpdate(bool etat){
		Debug.Log ("update" + etat);
		canUpdate = etat;
		gameManager.myaction -= setUpdate;
		gameManager.myActionColorDivise += ternirObjet; 
		gameManager.myactionColorReset += resetColor;
	}

	public void ternirObjet(){
		Debug.Log ("mat.name");
		Renderer rend = GetComponent<Renderer> ();
		foreach (Material mat in rend.materials) {
			Color temp = mat.GetColor ("_Color");
			temp.b = temp.b / diviseColorAlbedo;
			temp.g = temp.g / diviseColorAlbedo;
			temp.r = temp.r / diviseColorAlbedo;
			mat.SetColor ("_Color", temp);

			temp = mat.GetColor ("_EmissionColor");
			temp.b = temp.b / diviseColorEmissive;
			temp.g = temp.g / diviseColorEmissive;
			temp.r = temp.r / diviseColorEmissive;
			mat.SetColor ("_EmissionColor", temp);


		}

	}

	public void resetColor(){
		GetComponent<Renderer> ().material.SetColor ("_Color", ColorAlbedo);
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", ColorEmissive);

	}
}
