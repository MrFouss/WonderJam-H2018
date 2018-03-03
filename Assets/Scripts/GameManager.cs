using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public delegate void MyDelegate(bool actif);
	public  MyDelegate myDelegate;



	// Use this for initialization
	void Start () {
		hud = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HudManager> ();
		balle = GameObject.FindGameObjectWithTag ("Balle").GetComponent<Balle> ();
        Xmin = gameZone.transform.position.x - gameZone.transform.localScale.x/2;
        Xmax = gameZone.transform.position.x + gameZone.transform.localScale.x/2;
        Ymin = gameZone.transform.position.y - gameZone.transform.localScale.y/2;
        Ymax = gameZone.transform.position.y + gameZone.transform.localScale.y/2;
        changeCible();
        
		hud.UpdateScoreText (0);
		launchModeEdit ();

	}
	
	// Update is called once per frame
	void Update () {
		timeSec -= Time.deltaTime;
		hud.UpdateTimerText (timeSec);
		if (timeSec == 0) {

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
        Debug.Log(editionMode);
        if (myDelegate != null) {
			myDelegate (true);
		}
		balle.restartBalle ();

	}

	public void launchModeEdit(){
        editionMode = true;
		if (myDelegate != null) {
			myDelegate (false);
		}

		balle.resetPosBalle (balleStartPos);


	}
    public void cibleTouched()
    {
        addScore(1);
        changeCible();
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
}
