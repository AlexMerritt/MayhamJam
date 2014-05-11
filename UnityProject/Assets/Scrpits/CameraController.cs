using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float scrollSpeed = 100.0f;
	// Use this for initialization

    Vector3 pos;

	void Start () 
    {
        pos = new Vector3(0, 0, -10);
	
	}
	
	// Update is called once per frame
	void Update () {
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");

        if (Camera.current != null)
        {
            Vector3 movement = Vector3.zero;

            movement.x += xAxisValue;
            movement.y += yAxisValue;

            pos += movement;

            Camera.current.transform.position = pos;// Translate(movement * scrollSpeed * Time.deltaTime);// * Time.deltaTime);//new Vector3(xAxisValue, yAxisValue, 0.0f));
        }
	}
}
