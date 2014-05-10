using UnityEngine;
using System.Collections;
using System.Text;

public class PopUp : MonoBehaviour
{
    public string header;
    public string message;

    public Texture picture;

    const int MaxLineLength = 20;

    // Use this for initialization
    void Start()
    {
        UpdatePopUp();
    }

    void UpdatePopUp()
    {
        var tc = gameObject.GetComponent<GUIText>();



        string[] words = message.Split(' ');

        StringBuilder newSentence = new StringBuilder();

        string line = "";

        foreach(string word in words)
        {
            if ((line + word).Length > MaxLineLength)
            {
                newSentence.AppendLine(line);
                line = "";
            }

            line += string.Format("{0}", word) + " ";
        }

        if (line.Length > 0)
        {
            newSentence.AppendLine(line);
        }

        tc.text += header + "\n" + newSentence.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
