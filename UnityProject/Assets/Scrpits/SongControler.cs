using UnityEngine;
using System.Collections;

public class SongControler : MonoBehaviour 
{
    public float startDelay;

    public float fadeInDuration;
    float fadeIn;
    float volume;

    bool playing;
    bool paused;

	// Use this for initialization
	void Start () 
    {
        fadeIn = 0;
        volume = 0.0f;

        playing = false;
        paused = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (startDelay > 0.0f)
        {
            startDelay -= Time.deltaTime;
        }
        else
        {
            if (!playing && !paused)
            {
                PlayAudio();
            }
        }


        if (playing)
        {
            if (fadeIn < fadeInDuration)
            {
                fadeIn += Time.deltaTime;
            }

            volume = fadeIn / fadeInDuration;
            var s = gameObject.GetComponent<AudioSource>();
            s.volume = Mathf.Min(Mathf.Max(volume, 0.0f), 1.0f);
        }
	}

    void PlayAudio()
    {
        var s = gameObject.GetComponent<AudioSource>();
        s.Play();

        playing = true;
    }
}
