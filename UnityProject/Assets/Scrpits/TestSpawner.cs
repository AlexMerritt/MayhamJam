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
			Intersection inter = GameObject.FindObjectOfType<City>().RandomIntersection();
			Debug.Log(string.Format("Intersection is null? {0}", inter == null));
			GameObject newVehicle = (GameObject) GameObject.Instantiate(vehicleToSpawn);
			newVehicle.GetComponent<Driver>().SetIntersection(inter);
		}
	}
}
