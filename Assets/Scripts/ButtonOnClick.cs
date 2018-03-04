using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void retourMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }


	public void LancerPartie(){
		SceneManager.LoadScene ("MainGame");

	}

	public void QuitterLeJeu(){
		Application.Quit();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
