using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
	public enum BuildingSize {
		Small,
		Medium,
		Large
	}

	//position of the lower left corner
	public int tileX, tileY;
	
	public float curHealth;
	public float maxHealth;
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
