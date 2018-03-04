using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    private Color color;
    public GameManager gm;

	// Use this for initialization
	void Start () {
		color = GetComponent<Button>().image.color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HighlightButton() {
        if (gm.editionMode) {
            color.a = 1.0f;
            GetComponent<Button>().image.color = color;
        }
    }

    public void HideButton() {
        color.a = 0.5f;
        GetComponent<Button>().image.color = color;
    }
}
