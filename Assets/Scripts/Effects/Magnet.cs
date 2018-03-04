using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {
    
    public MagnetField Field;
    public float MaxRotationPerSecond = 10;

	// Update is called once per frame
	void Update () {
        Vector2 up = transform.up;
        Vector2 dir = Field.Target - new Vector2(transform.position.x, transform.position.y);
        Quaternion rotation = Quaternion.FromToRotation(up, dir);
        float angle = Quaternion.Angle(Quaternion.identity, rotation);
        if (angle > 0)
        {
            Quaternion finalAngle = Quaternion.Slerp(Quaternion.identity, rotation, 
                Mathf.Min(angle, Time.deltaTime * MaxRotationPerSecond) / angle);
            transform.Rotate(finalAngle.eulerAngles, Space.Self);

        }
	}
}
