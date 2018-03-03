using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int collisions = 0;
    public int triggers = 0;

    private void OnTriggerEnter(Collider other)
    {
        triggers++;
    }

    private void OnTriggerExit(Collider other)
    {
        triggers--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
}
