﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private float timeSec;
    public float timeToLose;
	public Vector3 balleStartPos;
	public bool editionMode;
	public int score;
	private HudManager hud;
    public GameObject buttonRestart;
    public GameObject buttonMenu;
    public GameObject buttonQuit;
    public GameObject cible;
    public GameObject textPlay;
    public GameObject textEdit;
    public GameObject PlayButton;
    public GameObject EditButton;
    public GameObject MenuGameOver;
    private GameObject cibleObj;
	private Balle balle;
    public GameObject gameZone;
    private float Xmin;
    private float Xmax;
    private float Ymin;
    private float Ymax;

	private AudioSource source;
	public AudioClip sonActivation;


    public delegate void MyDelegate(bool actif);
	public  MyDelegate myDelegate;

	public delegate void Myaction(bool actif);
	public  Myaction myaction;

	public delegate void MyactionColorDivise();
	public  MyactionColorDivise myActionColorDivise;

	public delegate void MyactionColorReset();
	public  MyactionColorReset myactionColorReset;


	private bool sonPlay;

	public int timePerdu;
	public int timeGagneCible;

	public int highScore;
	private GameController gameController;
	private bool isGameActive;


	// Use this for initialization
	void Start () {
        timeSec = timeToLose;
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HudManager> ();

        balle = GameObject.FindGameObjectWithTag("Balle").GetComponent<Balle>();

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
        Xmin = gameZone.transform.position.x - gameZone.transform.localScale.x/2;
        Xmax = gameZone.transform.position.x + gameZone.transform.localScale.x/2;
        Ymin = gameZone.transform.position.y - gameZone.transform.localScale.y/2;
        Ymax = gameZone.transform.position.y + gameZone.transform.localScale.y/2;
        MenuGameOver.SetActive(false);
        changeCible();
		source= GetComponent<AudioSource>();
		hud.UpdateScoreText (0);
        textPlay.SetActive(false);
        textEdit.SetActive(true);
        launchModeEdit ();
		sonPlay = true;
		isGameActive = true;

	}
    public void restart()
    {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("objectInteraction");
        foreach (GameObject item in allItems)
        {

            Destroy(item);
        }
        timeSec = timeToLose;
        MenuGameOver.SetActive(false);
        changeCible();
        source = GetComponent<AudioSource>();
        hud.UpdateScoreText(0);
        textPlay.SetActive(false);
        textEdit.SetActive(true);
        launchModeEdit();
        sonPlay = true;
    }


    void Update () {
        if (editionMode)
        {
            textPlay.SetActive(false);
            textEdit.SetActive(true);
        }
        else
        {
            textPlay.SetActive(true);
            textEdit.SetActive(false);
        }
		if (timeSec <= 12.1 && sonPlay) {
			sonPlay = false;
			source.PlayOneShot (sonActivation);
		}

		if (timeSec > 12.1 && !sonPlay) {
			sonPlay = true;
			source.Stop ();
		}

        if (timeSec <= 0)
        {
            editionMode = false;
            PlayButton.SetActive(false);
            EditButton.SetActive(false);
            MenuGameOver.SetActive(true);
        }

		if (timeSec >= 0) {
			timeSec -= Time.deltaTime;
			hud.UpdateTimerText (timeSec);
		} else {
			if (isGameActive) {
				endGame ();
			}
		}
			

		if (Input.GetButtonDown ("ToggleGameEdit")) {
            ToggleGameEdit();			
		}
	}

    public void ToggleGameEdit() {
        if (editionMode) {
            //Debug.Log("launchGame");
            launchGame();
        }
        else {
            //Debug.Log("launchEdit");
            launchModeEdit();
        }
    }

	public void launchGame(){
        
        editionMode = false;

        PlayButton.SetActive(false);
        EditButton.SetActive(true);

        //        Debug.Log(editionMode);
        if (myDelegate != null) {
			myDelegate (true);
		}
		if (myactionColorReset != null) {
			//Debug.Log ("Launch");
			myactionColorReset ();
		}
		balle.restartBalle ();

	}

	public void launchModeEdit(){
        editionMode = true;
        
        PlayButton.SetActive(true);
        EditButton.SetActive(false);

        if (myDelegate != null) {
			myDelegate (false);
		}

		balle.resetPosBalle (balleStartPos);

		if (myActionColorDivise != null) {
			//Debug.Log ("Pause");
			myActionColorDivise ();
		}
	


	}
    public void cibleTouched()
    {
        addScore(1);
		addTime (timeGagneCible);
        changeCible();
		if (myaction != null) {
			myaction (false);
		}

        launchModeEdit();

    }
    public void changeCible()
    {
            Vector3 position;
        do {
            position = new Vector3(Random.Range(Xmin + cible.GetComponent<SphereCollider>().radius * 2, Xmax - cible.GetComponent<SphereCollider>().radius * 2), Random.Range(Ymin + cible.GetComponent<SphereCollider>().radius * 2, Ymax - cible.GetComponent<SphereCollider>().radius * 2), 0);
        } while (Physics.CheckSphere(position, cible.GetComponent<SphereCollider>().radius * 2));
        
            
            
        Destroy(cibleObj);
    
            cibleObj = Instantiate(cible, position, Quaternion.identity);
        
    }

	public void addScore(int score){
		this.score += score;
		hud.UpdateScoreText (this.score);
	}

	public void deleteTime(float time){
		timeSec -= time;
	}

	public void addTime(float time){
		timeSec += time;
	}

	public void useDelete(){
		deleteTime (timePerdu);
		timePerdu += 2;
	}

	public void endGame(){
		isGameActive = false;
		gameController.submitPlayerScoring (score);
		highScore = gameController.playerScoring.highScore;
		Transform[] listTransform = MenuGameOver.GetComponentsInChildren<Transform> (true);

		foreach (Transform t in listTransform) {
			if (t.name.Equals ("Score")) {
				t.GetComponent<UnityEngine.UI.Text> ().text = "Score : " + score;
			}

			if (t.name.Equals ("HighScore")) {
				t.GetComponent<UnityEngine.UI.Text> ().text = "High Score : " + highScore;
			}

		}
	}
}
