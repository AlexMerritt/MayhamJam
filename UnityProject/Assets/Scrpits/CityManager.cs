using UnityEngine;
using System.Collections;

public enum CityState
{
    Normal = 0,
    Low = 1,
    Medium = 2,
    High = 3
}

public class CityManager : MonoBehaviour
{
    CityState cityState;

    float chaosLevel;

    float maxChaosLevel;

    public int chaosLossPerSecond;

    float timer;

	// Use this for initialization
	void Start ()
    {
        cityState = CityState.Normal;

        maxChaosLevel = 99.0f;

        chaosLevel = 0.0f;

        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddChaos(5);
        }

        timer += Time.deltaTime;

        if (timer >= 1.0f / (float)chaosLossPerSecond)
        {
            timer = 0.0f;
            AddChaos(-1);
        }
	}

    public void SetState(CityState state)
    {
        cityState = state;
    }

    public CityState GetCityState()
    {
        int level = GetChaosLevel();

        return (CityState)(level / 25);
    }

    public void AddChaos(float value)
    {
        //Debug.Log("Adding Chaos");
        chaosLevel += value;

        if (chaosLevel >= maxChaosLevel)
        {
            //Debug.Log("At Max Chaos level");
            chaosLevel = maxChaosLevel;
        }

        if (chaosLevel <= 0)
        {
            chaosLevel = 0;
        }

        CityState cs = GetCityState();

        if (cityState != cs)
        {
            SetState(cs);
        }
    }

    public int GetChaosLevel()
    {
        return (int)chaosLevel;
    }

    public float GetChaosLevelf()
    {
        return chaosLevel / maxChaosLevel;
    }
}