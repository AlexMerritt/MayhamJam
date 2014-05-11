using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");
		if (Camera.current != null)
			Camera.current.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0.0f));
	}
}
