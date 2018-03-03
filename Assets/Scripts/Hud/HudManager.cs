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
    public Material TransparentMaterial;
    
    public float rotationRate = 10;

    private bool removeMode = false;
    private Vector3 mousePosition;
    private GameObject spawnedObject;
    private MeshRenderer spawnedObjectRenderer;
    private Material spawnedObjectRealMaterial;
	private GameManager gm;

    public void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void UpdateTimerText(float remainingTimeInSeconds)
    {
        int min = (int)remainingTimeInSeconds / 60;
        int sec = (int)remainingTimeInSeconds % 60;
        TimerText.text = (min < 10 ? "0" : "") + min + ":" + (sec < 10 ? "0" : "") + sec;
		gm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
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
            Destroy(spawnedObject);

        }
        Cursor.SetCursor(ImpossibleRemoveTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
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

                if (Input.GetMouseButtonDown(0))
                {
                    removeMode = false;
                    // check what needs to be removed
                    RaycastHit hit = new RaycastHit();
                    int nonGameZoneMask = LayerMask.GetMask(new string[] { "Default" });
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseScreenPos), out hit, Mathf.Infinity, nonGameZoneMask))
                    {
                        Destroy(hit.transform.gameObject);
						gm.useDelete ();
                    }

                    // change remove sprite
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }

            }
            else if (spawnedObject != null)
            {
                // manipulate na object
                CollisionChecker check = spawnedObject.GetComponent<CollisionChecker>();
                if (check.collisions == 0 && check.triggers == 0)
                {
                    spawnedObjectRenderer.material = spawnedObjectRealMaterial;
                    
                    // check if left mouse button clicked
                    if (Input.GetMouseButtonDown(0))
                    {
                        // forget currently manipulated object
                        spawnedObject = null;
                    }
                } else
                {
                    spawnedObjectRenderer.material = TransparentMaterial;
                }
            }
        }
        else
        {
            // out of game zone
            if (removeMode)
            {
                Cursor.SetCursor(ImpossibleRemoveTexture, Vector2.zero, CursorMode.Auto);
            }
            else if (spawnedObject != null)
            {
                spawnedObjectRenderer.material = TransparentMaterial;

            }
        }
        
    }

    private void SpawnObject(GameObject gameObject)
    {
        // destroy currently manipulated object to replace it
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
        removeMode = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        spawnedObject = Instantiate(gameObject, mousePosition, Quaternion.identity);
        spawnedObjectRenderer = spawnedObject.GetComponent<MeshRenderer>();
        spawnedObjectRealMaterial = spawnedObjectRenderer.material;
    }
}
