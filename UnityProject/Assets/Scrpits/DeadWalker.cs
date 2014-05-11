using UnityEngine;
using System.Collections;

public class DeadWalker : MonoBehaviour {
	float fadeTime = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		fadeTime -= Time.deltaTime;
		
		if (fadeTime <= 0)
			Destroy(gameObject);
	}
}
