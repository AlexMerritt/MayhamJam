using UnityEngine;
using System.Collections;

public enum CityState
{
    Normal,
    Low,
    Medium,
    High
}

public class CityManager : MonoBehaviour
{
    CityState cityState;

    float chaosLevel;

	// Use this for initialization
	void Start ()
    {
        cityState = CityState.Normal;

        chaosLevel = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetState(CityState state)
    {
        cityState = state;
    }
}
