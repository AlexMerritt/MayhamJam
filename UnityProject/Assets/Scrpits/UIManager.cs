using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        var ct = GameObject.Find("Main Camera").transform;

        gameObject.transform.position = new Vector3(ct.position.x, ct.position.y, ct.position.z + 2);
    }
}
