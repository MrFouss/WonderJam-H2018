using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balle : MonoBehaviour {


	private RigidbodyConstraints  defaultRigid;

	// Use this for initialization
	void Awake () {
		defaultRigid = GetComponent<Rigidbody> ().constraints;
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
}
