using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetInterraction : MonoBehaviour {

	public GameManager gameManager;
	public bool canUpdate;

	public float diviseColorAlbedo;
	public float diviseColorEmissive;

	private Color ColorAlbedoNormal;
	private Color ColorEmissiveNormal;

	private Color ColorAlbedoChange;
	private Color ColorEmissiveChange;

	void Awake(){
		ColorAlbedoNormal = GetComponent<Renderer> ().material.GetColor ("_Color");
		ColorEmissiveNormal = GetComponent<Renderer> ().material.GetColor ("_EmissionColor");
	}

	// Use this for initialization
	public void Start () {
		canUpdate = true;
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		gameManager.myaction += setUpdate;

		prepareColor ();
		//Debug.Log (ColorAlbedoNormal);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			ternirObjet ();
		}
	}

	public void setUpdate(bool etat){
		//Debug.Log ("update" + etat);
		canUpdate = etat;
		gameManager.myaction -= setUpdate;
		gameManager.myActionColorDivise += ternirObjet; 
		gameManager.myactionColorReset += resetColor;
	}

	public void ternirObjet(){
		//Debug.Log ("mat.name");
		Renderer rend = GetComponent<Renderer> ();
		rend.material.SetColor ("_Color", ColorAlbedoChange);
		rend.material.SetColor ("_EmissionColor",ColorEmissiveChange );

	}

	public void resetColor(){
		GetComponent<Renderer> ().material.SetColor ("_Color", ColorAlbedoNormal);
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", ColorEmissiveNormal);

	}

	public void prepareColor(){
		ColorAlbedoChange.b = ColorAlbedoNormal.b / diviseColorAlbedo;
		ColorAlbedoChange.g = ColorAlbedoNormal.g / diviseColorAlbedo;
		ColorAlbedoChange.r = ColorAlbedoNormal.r / diviseColorAlbedo;

		ColorEmissiveChange.b = ColorAlbedoNormal.b / diviseColorEmissive;
		ColorEmissiveChange.g = ColorAlbedoNormal.g / diviseColorEmissive;
		ColorEmissiveChange.r = ColorAlbedoNormal.r / diviseColorEmissive;
	}


	public void resetEvenement(){
		gameManager.myActionColorDivise -= ternirObjet; 
		gameManager.myactionColorReset -= resetColor;
		gameManager.myaction -= setUpdate;
	}
}
