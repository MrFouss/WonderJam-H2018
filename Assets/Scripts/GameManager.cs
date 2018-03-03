using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float timeSec;
	public Vector3 balleStartPos;
	public bool edtionMode;
	private HudManager hud;
	private Balle balle;
	public List<PistonBehaviour> listPiston;

	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HudManager> ();
		balle = GameObject.FindGameObjectWithTag ("Balle").GetComponent<Balle> ();
		launchModeEdit ();
		edtionMode = true;
	}
	
	// Update is called once per frame
	void Update () {
		timeSec -= Time.deltaTime;
		hud.UpdateTimerText (timeSec);

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (edtionMode) {
				edtionMode = false;
				launchGame ();
			} else {
				edtionMode = true;
				launchModeEdit ();
			}
		}
	}

	public void launchGame(){
		foreach(PistonBehaviour piston in listPiston){
			piston.activatePiston ();
		}
		balle.restartBalle ();
	}

	public void launchModeEdit(){
		foreach(PistonBehaviour piston in listPiston){
			piston.desactivatePiston ();
		}
		balle.resetPosBalle (balleStartPos);

	}

	public void addPiston(PistonBehaviour piston){
		
	}
}
