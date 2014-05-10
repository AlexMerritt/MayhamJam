using UnityEngine;
using System.Collections;

public class LevelSwitch : MonoBehaviour 
{
    public enum LevelCode
    {
        Exit,
        MainMenu,
        Game
    }
    public KeyCode Advance;
    public KeyCode Exit;

    public LevelCode NextLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(Advance))
        {
            //Application.LoadLevel("testscene");
            //SwitchScene();
        }
        if (Input.GetKeyDown(Exit))
        {
            Application.Quit();
        }
	}

    public void SwitchScene()
    {
        if (NextLevel == LevelCode.Exit)
        {
            Application.Quit();
        }

        else if (NextLevel == LevelCode.MainMenu)
        {
        }

    }
}
