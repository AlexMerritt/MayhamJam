using UnityEngine;
using System.Collections;

public class ChaosBarManager : MonoBehaviour 
{
    public float max;
    public float current;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        var city = GameObject.Find("City").GetComponent<CityManager>();

        var bar = gameObject.transform.Find("Bar");
        bar.localScale = new Vector3(city.GetChaosLevelf(), 1);
	}
}
