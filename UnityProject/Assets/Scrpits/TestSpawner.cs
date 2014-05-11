using UnityEngine;
using System.Collections;

public class TestSpawner : MonoBehaviour {
	public GameObject humanToSpawn, vehicleToSpawn;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.H)) {
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = -1.0f;
			
			GameObject.Instantiate(humanToSpawn, worldPos, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.V)) {
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 5;
			
			GameObject.Instantiate(vehicleToSpawn, worldPos, Quaternion.identity);
		}
	}
}
