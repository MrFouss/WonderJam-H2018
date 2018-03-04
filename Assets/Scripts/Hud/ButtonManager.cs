using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HighlightButton() {
        Color color = GetComponent<Button>().image.color;
        color.a = 1.0f;
        GetComponent<Button>().image.color = color;
    }

    public void HideButton() {
        Color color = GetComponent<Button>().image.color;
        color.a = 0.5f;
        GetComponent<Button>().image.color = color;
    }
}
