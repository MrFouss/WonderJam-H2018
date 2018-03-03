using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float timeSec;
	public Vector3 balleStartPos;
	public bool edtionMode;
	public int score;
	private HudManager hud;
	private Balle balle;

	public delegate void MyDelegate(bool actif);
	public  MyDelegate myDelegate;



	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HudManager> ();
		balle = GameObject.FindGameObjectWithTag ("Balle").GetComponent<Balle> ();
		hud.UpdateScoreText (0);
		launchModeEdit ();

	}
	
	// Update is called once per frame
	void Update () {
		timeSec -= Time.deltaTime;
		hud.UpdateTimerText (timeSec);

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (edtionMode) {
				launchGame ();
			} else {
				launchModeEdit ();
			}
		}
	}

	public void launchGame(){
		edtionMode = false;
		myDelegate (true);
		balle.restartBalle ();

	}

	public void launchModeEdit(){
		edtionMode = true;
		myDelegate (false);
		balle.resetPosBalle (balleStartPos);


	}

	public void addScore(int score){
		this.score += score;
		hud.UpdateScoreText (this.score);
	}
}
