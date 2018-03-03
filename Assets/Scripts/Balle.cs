using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balle : MonoBehaviour {


	private RigidbodyConstraints  defaultRigid;
	public Vector3 defaultPos;

	// Use this for initialization
	void Start () {
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
		if(Input.GetKeyDown(KeyCode.C)){
			resetPosBalle();
		}
	}


	public void stopBalle(){
		GetComponent<Rigidbody> ().constraints = ~RigidbodyConstraints.FreezePositionY | ~RigidbodyConstraints.FreezePositionX;
	}

	public void restartBalle(){
		GetComponent<Rigidbody> ().constraints = defaultRigid;
	}
		

	public void resetPosBalle(){
		stopBalle ();
		GetComponent<Transform> ().position = defaultPos;
	}
}
