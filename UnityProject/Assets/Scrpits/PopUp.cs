using UnityEngine;
using System.Collections;
using System.Text;

public class PopUp : MonoBehaviour
{
    public MessageIndex messageIndex;

    string header;
    string text;

    public Sprite image;

    const int MaxLineLength = 50;

    // Use this for initialization
    void Start()
    {
        UpdatePopUp();
    }

    public void UpdatePopUp()
    {
        var messageHandler = GameObject.Find("MessageHandler").GetComponent<MessageHandler>();

        Message message = messageHandler.GetMessage(messageIndex);

        header = message.header;
        text = message.text;
        image = message.sprite;

        UpdateHeader();

        UpdateText();

        UpdateImage();
    }

    void UpdateHeader()
    {
        var hc = gameObject.transform.Find("Header").gameObject.GetComponent<TextMesh>();
        hc.text = header;
    }

    void UpdateText()
    {
        var tc = gameObject.transform.Find("Text").gameObject.GetComponent<TextMesh>();
        tc.text = "";

        string[] words = text.Split(' ');

        StringBuilder newSentence = new StringBuilder();

        string line = "";

        foreach (string word in words)
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

        tc.text += newSentence.ToString();
    }

    void UpdateImage()
    {
        var sc = gameObject.transform.Find("Image").gameObject.GetComponent<SpriteRenderer>();

        sc.sprite = image;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
