using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HudManager : MonoBehaviour {

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;

    public GameObject BallLauncherPrefab;
    public GameObject TargetPrefab;
    public GameObject RectangleTrampolinePrefab;
    public GameObject SphereTrampolinePrefab;
    public GameObject TriangleTrampolinePrefab;
    public GameObject WallPrefab;
    public Texture2D RemoveTexture;
    public Texture2D ImpossibleRemoveTexture;
    public Texture2D DefaultCursorTexture;
    public Material TransparentMaterial;
    
    public float rotationRate = 10;

    private bool removeMode = false;
    private Vector3 mousePosition;
    private GameObject spawnedObject;
    private MeshRenderer spawnedObjectRenderer;
    private Material spawnedObjectRealMaterial;
	private GameManager gm;

    private void Start()
    {
        Cursor.SetCursor(DefaultCursorTexture, Vector2.zero, CursorMode.Auto);
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
    }

    public void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void UpdateTimerText(float remainingTimeInSeconds)
    {
        int min = (int)remainingTimeInSeconds / 60;
        int sec = (int)remainingTimeInSeconds % 60;
        TimerText.text = (min < 10 ? "0" : "") + min + ":" + (sec < 10 ? "0" : "") + sec;
    }

    public void BallLaucherButtonClick()
    {
        SpawnObject(BallLauncherPrefab);
    }

    public void TargetButtonClick()
    {
        SpawnObject(TargetPrefab);
    }

    public void RectangleTrampolineButtonClick()
    {
        SpawnObject(RectangleTrampolinePrefab);
    }

    public void SphereTrampolineButtonClick()
    {
        SpawnObject(SphereTrampolinePrefab);
    }

    public void TriangleTrampolineButtonClick()
    {
        SpawnObject(TriangleTrampolinePrefab);
    }

    public void WallButtonClick()
    {
        SpawnObject(WallPrefab);
    }

    public void RemoveButtonClick()
    {
        //SpawnObject(RemovePrefab);
        removeMode = true;
        if (spawnedObject != null)
        {
			DestroyObjet(spawnedObject);

        }
        //Cursor.SetCursor(ImpossibleRemoveTexture, Vector2.zero, CursorMode.Auto);
    }


    private void Update()
    {

		if (!gm.editionMode && spawnedObject != null) {
			DestroyObjet(spawnedObject);
			spawnedObject = null;
		}
        // retrieve mouse position in world space + 50
        Vector2 mouseScreenPos = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0));
        mousePosition.z = 0;

        // if currently manipulating an object
        if (spawnedObject != null)
        {
            // update its position
            spawnedObject.transform.position = mousePosition;

            // check if wheel active and rotate object
            float wheel = Input.mouseScrollDelta.y;
            spawnedObject.transform.Rotate(Vector3.forward, wheel * rotationRate);

			if (Input.GetMouseButtonDown (1)) {
				DestroyObjet(spawnedObject);
				spawnedObject = null;
			}
        }

        // check if mouse in game zone
        RaycastHit gameZoneHit = new RaycastHit();
        int gameZoneMask = LayerMask.GetMask(new string[] {"GameZone"});
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseScreenPos), out gameZoneHit, Mathf.Infinity, gameZoneMask))
        {
            // in game zone
            if (removeMode)
            {
                // remove mode
                Cursor.SetCursor(RemoveTexture, Vector2.zero, CursorMode.Auto);

				if (Input.GetMouseButtonDown (0)) {
					removeMode = false;
					// check what needs to be removed
					RaycastHit hit = new RaycastHit ();
					int nonGameZoneMask = LayerMask.GetMask (new string[] { "Default" });
					if (Physics.Raycast (Camera.main.ScreenPointToRay (mouseScreenPos), out hit, Mathf.Infinity, nonGameZoneMask)) {
						DestroyObjet (hit.transform.gameObject);
						gm.useDelete ();
					}

                    // change remove sprite
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }

			} else if (spawnedObject != null) {
				// manipulate an object
				CollisionChecker check = spawnedObject.GetComponent<CollisionChecker> ();
				if (check.collisions == 0 && check.triggers == 0) {
					spawnedObjectRenderer.material = spawnedObjectRealMaterial;
                    
					// check if left mouse button clicked
					if (Input.GetMouseButtonDown (0)) {
						// forget currently manipulated object
						spawnedObject = null;
					}
				} else {
					spawnedObjectRenderer.material = TransparentMaterial;
				}
			} else if (gm.editionMode) {

				if (Input.GetMouseButtonDown (0)) {
					// check what needs to be moved
					RaycastHit hit = new RaycastHit ();
					int nonGameZoneMask = LayerMask.GetMask (new string[] { "Default" });
					if (Physics.Raycast (Camera.main.ScreenPointToRay (mouseScreenPos), out hit, Mathf.Infinity, nonGameZoneMask)) {
						if (hit.transform.tag != "Balle" && hit.transform.tag != "Cible" && hit.transform.gameObject.GetComponent<ObjetInterraction>()) {
                            if(hit.transform.gameObject.GetComponent<ObjetInterraction>().canUpdate){
                                MoveObject (hit.transform.gameObject);
                            }
						}
					}
				} else if (Input.GetMouseButtonDown (1)) {
					// check what needs to be deleted
					RaycastHit hit = new RaycastHit ();
					int nonGameZoneMask = LayerMask.GetMask (new string[] { "Default" });
					if (Physics.Raycast (Camera.main.ScreenPointToRay (mouseScreenPos), out hit, Mathf.Infinity, nonGameZoneMask)) {
						if (hit.transform.tag != "Balle" && hit.transform.tag != "Cible") {
                            ObjetInterraction o = hit.transform.gameObject.GetComponent<ObjetInterraction>();
                            if(o != null){
								DestroyObjet (hit.transform.gameObject);
                                if(!o.canUpdate){
                                    gm.useDelete ();
                                }
                            }
						}
					}
				}
			}
        }
        else
        {
            // out of game zone
            if (removeMode)
            {
                //Cursor.SetCursor(ImpossibleRemoveTexture, Vector2.zero, CursorMode.Auto);
            }
            else if (spawnedObject != null)
            {
                spawnedObjectRenderer.material = TransparentMaterial;

            }
        }
        
    }

    private void SpawnObject(GameObject gameObject)
    {

		if (gm.editionMode) {
			// destroy currently manipulated object to replace it
			if (spawnedObject != null)
			{
				DestroyObjet(spawnedObject);
			}
			removeMode = false;
			//Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

			spawnedObject = Instantiate(gameObject, mousePosition, Quaternion.identity);
			spawnedObjectRenderer = spawnedObject.GetComponent<MeshRenderer>();
			spawnedObjectRealMaterial = spawnedObjectRenderer.material;
		}
      
    }

	private void MoveObject(GameObject gameObject) {
		spawnedObject = gameObject;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		spawnedObjectRenderer = spawnedObject.GetComponent<MeshRenderer>();
		spawnedObjectRealMaterial = spawnedObjectRenderer.material;
	}

	private void DestroyObjet(GameObject go){
		
		if (go.GetComponent<ObjetInterraction> () != null) {
			go.GetComponent<ObjetInterraction> ().resetEvenement ();
		}
		Destroy (go);

	}
}
