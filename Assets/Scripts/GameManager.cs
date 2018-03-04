using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float timeSec;
	public Vector3 balleStartPos;
	public bool editionMode;
	public int score;
	private HudManager hud;
    public GameObject cible;
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


	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HudManager> ();
		balle = GameObject.FindGameObjectWithTag ("Balle").GetComponent<Balle> ();
        Xmin = gameZone.transform.position.x - gameZone.transform.localScale.x/2;
        Xmax = gameZone.transform.position.x + gameZone.transform.localScale.x/2;
        Ymin = gameZone.transform.position.y - gameZone.transform.localScale.y/2;
        Ymax = gameZone.transform.position.y + gameZone.transform.localScale.y/2;
        changeCible();
		source= GetComponent<AudioSource>();
		hud.UpdateScoreText (0);
		launchModeEdit ();
		sonPlay = true;

	}

	// Update is called once per frame
	void Update () {
		timeSec -= Time.deltaTime;
		hud.UpdateTimerText (timeSec);

		if (timeSec <= 12.1 && sonPlay) {
			sonPlay = false;
			source.PlayOneShot (sonActivation);
		}

		if (timeSec > 12.1 && !sonPlay) {
			sonPlay = true;
			source.Stop ();
		}

		if (timeSec <= 0) {
			SceneManager.LoadScene ("GameOverScene");
		}

		if (Input.GetKeyUp (KeyCode.Tab)) {
            
			if (editionMode) {
                Debug.Log("launchGame");

                launchGame ();
			} else {
                Debug.Log("launchEdit");
                launchModeEdit ();
			}
		}
	}

	public void launchGame(){
        
        editionMode = false;
//        Debug.Log(editionMode);
        if (myDelegate != null) {
			myDelegate (true);
		}
		if (myactionColorReset != null) {
			Debug.Log ("Launch");
			myactionColorReset ();
		}
		balle.restartBalle ();

	}

	public void launchModeEdit(){
        editionMode = true;
		if (myDelegate != null) {
			myDelegate (false);
		}

		balle.resetPosBalle (balleStartPos);

		if (myActionColorDivise != null) {
			Debug.Log ("Pause");
			myActionColorDivise ();
		}
	


	}
    public void cibleTouched()
    {
        addScore(1);
		addTime (timeGagneCible);
        changeCible();
		myaction (false);
        launchModeEdit();

    }
    public void changeCible()
    {
        
            Vector3 position = new Vector3(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax), 0);
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
	}
}
