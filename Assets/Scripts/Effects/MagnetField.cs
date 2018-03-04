using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour {

    public Magnet MagnetPrefab;
    public float InterSpace = 0.1f;
    public Vector2 Target = Vector2.zero;

    private Balle ball;

	// Use this for initialization
	void Start () {
        ball = GameObject.Find("Balle").GetComponent<Balle>();
        GenerateMagnets();
	}

    private void GenerateMagnets()
    {
        Bounds backgroundBounds = GetComponent<SpriteRenderer>().bounds;

        Vector2 backgroundSize = backgroundBounds.size;
        //Debug.Log(backgroundSize);
        Vector3 backgroundTopLeftCorner = backgroundBounds.center - new Vector3(backgroundBounds.extents.x, backgroundBounds.extents.y, 0.1f);
        Vector2 magnetSize = MagnetPrefab.GetComponent<SpriteRenderer>().bounds.size;
        //Debug.Log(magnetSize);
        int lineLength = (int) ((backgroundSize.x-InterSpace) / (magnetSize.x+InterSpace));
        int columnLength = (int)((backgroundSize.y-InterSpace) / (magnetSize.y+InterSpace));

        //Debug.Log(lineLength + " ; " + columnLength);

        for (int i = 0; i < lineLength; ++i)
        {
            for (int j = 0; j < columnLength; ++j)
            {
                Vector3 position =
                    new Vector3(i * (magnetSize.x+InterSpace) + magnetSize.x / 2 + InterSpace,
                                j * (magnetSize.y+InterSpace) + magnetSize.y / 2 + InterSpace, 0)
                    + backgroundTopLeftCorner;
                Instantiate<Magnet>(MagnetPrefab, position, Quaternion.identity, this.transform).Field = this;
            }
        }

        MagnetPrefab.transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Vector3 cursorPos = Input.mousePosition;
        //Target = Camera.main.ScreenToWorldPoint(cursorPos);
        Target = ball.transform.position;
    }
}
