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
    public GameObject RemovePrefab;

    private bool removeMode = false;
    private Vector3 mousePosition;
    private GameObject spawnedObject;

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
        SpawnObject(RemovePrefab);
        removeMode = true;
    }

    private void OnGUI()
    {
        // retrieve mouse position in world space + 50
        Vector2 mouseScreenPos = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane+50));

        // if currently manipulating an object
        if (spawnedObject != null)
        {
            // update its position
            spawnedObject.transform.position = mousePosition;

            // check if left mouse button clicked
            if (Input.GetMouseButtonDown(0))
            {
                // if trying to remove an object
                if (removeMode)
                {
                    removeMode = false;
                    // check what needs to be removed
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseScreenPos), out hit))
                    {
                        Destroy(hit.transform.gameObject);
                    }

                    // destroy remove sprite
                    Destroy(spawnedObject);
                }

                // forget currently manipulated object
                spawnedObject = null;
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
        spawnedObject = Instantiate(gameObject, mousePosition, Quaternion.identity);
    }
}
