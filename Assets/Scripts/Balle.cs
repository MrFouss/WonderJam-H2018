using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balle : MonoBehaviour {


	private RigidbodyConstraints  defaultRigid;
	private AudioSource source;
    public Transform particle;

	// Use this for initialization
	void Awake () {
		defaultRigid = GetComponent<Rigidbody> ().constraints;
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)){
			stopBalle();
		}
		if(Input.GetKeyDown(KeyCode.B)){
			restartBalle();
		}
	}


	public void stopBalle(){
		GetComponent<Rigidbody> ().constraints = ~RigidbodyConstraints.FreezePositionY | ~RigidbodyConstraints.FreezePositionX;
	}

	public void restartBalle(){
		GetComponent<Rigidbody> ().constraints = defaultRigid;
	}
		

	public void resetPosBalle(Vector3 defaultPos){
		stopBalle ();
		GetComponent<Transform> ().position = defaultPos;
	}

	void OnCollisionEnter (Collision coll)
    {
        // source.pitch = Random.Range (lowPitchRange,highPitchRange);
        // float hitVol = coll.relativeVelocity.magnitude * velToVol;

		// int i = (int)Mathf.Min(Mathf.Floor(coll.relativeVelocity.magnitude / maxVelocity * crashSounds.Length), crashSounds.Length-1);
		// Debug.Log(i);
        // source.PlayOneShot(crashSounds[i],hitVol);
        Instantiate(particle, coll.contacts[0].point, Quaternion.identity);
    }
}
