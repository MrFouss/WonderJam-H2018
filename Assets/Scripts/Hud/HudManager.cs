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
	public RectTransform textSuppression;

    private bool removeMode = false;
	private bool wasLeftClick = true;
	private bool wasRightClick = true;
	private Vector3 mousePosition;
    private GameObject spawnedObject;
    private MeshRenderer spawnedObjectRenderer;
    private Material spawnedObjectRealMaterial;
	private GameManager gm;
	private GameObject previousHovered;
	private bool afficheText;

    private void Start()
    {
        Cursor.SetCursor(DefaultCursorTexture, Vector2.zero, CursorMode.Auto);
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		afficheText = false;
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

    public void TogglePlayPauseButtonClick() {
        gm.ToggleGameEdit();
    }

    private void Update()
    {
		// Empty user's hand when in play mode
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

			if (isRightClick()) {
				DestroyObjet(spawnedObject);
				spawnedObject = null;
			}
        }

		if (afficheText) {
			textSuppression.position = new Vector3 (mouseScreenPos.x, mouseScreenPos.y, 0);
		} else {
			textSuppression.position = new Vector3(-5000.0f, -5000.0f, 0.0f);
		}

        // check if mouse in game zone
        RaycastHit gameZoneHit = new RaycastHit();
        int gameZoneMask = LayerMask.GetMask(new string[] {"GameZone"});
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseScreenPos), out gameZoneHit, Mathf.Infinity, gameZoneMask))
        {
            // in game zone

			/*if (removeMode) {
				// remove mode
				Cursor.SetCursor (RemoveTexture, Vector2.zero, CursorMode.Auto);

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
					Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
				}

			} else*/ if (spawnedObject != null) {
				// manipulate an object
				CollisionChecker check = spawnedObject.GetComponent<CollisionChecker> ();
				if (check.collisions == 0 && check.triggers == 0) {
					spawnedObjectRenderer.material = spawnedObjectRealMaterial;
                    
					// check if left mouse button clicked
					if (isLeftClick()) {
						// forget currently manipulated object
						spawnedObject = null;
					}
				} else {
					spawnedObjectRenderer.material = TransparentMaterial;
				}
			} else if (gm.editionMode) {
				
				RaycastHit hit = new RaycastHit ();
				int nonGameZoneMask = LayerMask.GetMask (new string[] { "Default" });

				if (Physics.Raycast (Camera.main.ScreenPointToRay (mouseScreenPos), out hit, Mathf.Infinity, nonGameZoneMask)) { // dans la game zone

					if (hit.transform.tag != "Balle" && hit.transform.tag != "Cible" && hit.transform.gameObject.GetComponent<ObjetInterraction> ()) { // sur un objet interaction

						GameObject hovered = hit.transform.gameObject;

						if (!hovered.Equals (previousHovered)) { // on a change d'objet survole
							removeHoverEffect ();
							previousHovered = hovered;
						}

						if (hovered.GetComponent<ObjetInterraction> ().canUpdate) {
							hovered.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color (1.0f, 1.0f, 1.0f));
						} else {
							string text = "-" + gm.timePerdu + " sec";
							int i = 1;
							while (i < text.Length) {
								text = text.Insert (i, " ");
								i += 2;
							}
							Debug.Log (text);
							textSuppression.GetChild (0).gameObject.GetComponent<Text> ().text = text;
							afficheText = true;
							hovered.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color (0.0f, 0.0f, 0.0f));
						}

						if (isLeftClick()) {
							if (hit.transform.gameObject.GetComponent<ObjetInterraction> ().canUpdate) {
								MoveObject (hit.transform.gameObject);
							}
						} else if (isRightClick()) {
							DestroyObjet (hit.transform.gameObject);
							if (!hit.transform.gameObject.GetComponent<ObjetInterraction> ().canUpdate) {
								gm.useDelete ();
							}
						}
					}
				} else {
					removeHoverEffect ();
				}
			} else {
				removeHoverEffect ();
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

			removeHoverEffect ();
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
		//Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		spawnedObjectRenderer = spawnedObject.GetComponent<MeshRenderer>();
		spawnedObjectRealMaterial = spawnedObjectRenderer.material;
	}

	private void DestroyObjet(GameObject go){
		
		if (go.GetComponent<ObjetInterraction> () != null) {
			go.GetComponent<ObjetInterraction> ().resetEvenement ();
		}
		Destroy (go);
		removeHoverEffect ();
	}

	private void removeHoverEffect(){
		if (previousHovered) {
			Material m = previousHovered.GetComponent<Renderer> ().material;
			m.SetColor ("_EmissionColor", m.GetColor ("_Color"));
//			Cursor.SetCursor(DefaultCursorTexture, Vector2.zero, CursorMode.Auto);
			previousHovered = null;
			afficheText = false;
		}
	}

	private bool isLeftClick(){
		if (Input.GetMouseButton (0)) {
			if (!wasLeftClick) {
				wasLeftClick = true;
				return true;
			}
		} else {
			wasLeftClick = false;
		}
		return false;
	}

	private bool isRightClick(){
		if (Input.GetMouseButton (1)) {
			if (!wasRightClick) {
				wasRightClick = true;
				return true;
			}
		} else {
			wasRightClick = false;
		}
		return false;
	}
}
