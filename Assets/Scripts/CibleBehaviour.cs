using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CibleBehaviour : MonoBehaviour {
    Random random;
	[Tooltip("en battements par seconde")]
	public float beatRate;
	public float dureeBeat;
	public float amplitudeBeat;
	public AudioClip son;

	private GameManager gameManager;
	private Vector3 originalScale;
	private float timerStartNextBeat;
	// private float timerEndNextBeat;
	private bool inBeat;
	private float timeStartActualBeat;
	private AudioSource source;

	void Start () {
		source = GetComponent<AudioSource>();
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		originalScale = this.transform.localScale;
		inBeat = false;
		this.startBeat();
	}

	void Update () {
		if(Time.time >= timerStartNextBeat && !inBeat){
			startBeat();
		}

		if(inBeat){

			float sin = Mathf.Sin((Time.time - timeStartActualBeat) * Mathf.PI / dureeBeat);

			if(sin >= 0.0f){
				this.transform.localScale = originalScale * (1.0f + sin * amplitudeBeat);
			}
			else {
				this.transform.localScale = originalScale;
				inBeat = false;
			}
		}
	}

	void FixedUpdate() {
		
	}

	void OnTriggerEnter(Collider collider){
        if (collider.tag == "Balle") {
			gameManager.cibleTouched();  
		} else {
            if (!gameManager.editionMode)
            {
                gameManager.changeCible();
            }
		}
	}

	private void startBeat(){
		
		timeStartActualBeat = Time.time;
		inBeat = true;
		source.PlayOneShot(son, 0.5f);

		this.planifNextBeat();
	}

	private void planifNextBeat(){
		timerStartNextBeat = Time.time + 1.0f / beatRate - dureeBeat / 2.0f;
		// timerEndNextBeat = timerStartNextBeat + dureeBeat;
	}
}
