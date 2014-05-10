using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Message
{
    public Message(string header, string text)
    {
        this.header = header;
        this.text = text;
    }

    public string header;
    public string text;
}

public enum MessageIndex
{
    Sarah0 = 0,
    Sarah1,
    Mayor0,
    Mayor1,
    News0,
    News1,
    News2,
    News3,
    News4,
    Detective0,
    Detective1,
    Detective2,
    RoadHound69,
    Carrie0,
    Carrie1,
    Carrie2,
    Burger0,
    Burger1,
    Burger2,
}


public class MessageHandler : MonoBehaviour 
{
    public List<Message> messages;

    public float messageDelay;
    float currentDelay;

    Queue<MessageIndex> activeMessages;

    const int MinGenericMessage = 0;
    const int MaxGenericMessage = 18;

    public Message GetMessage(MessageIndex index)
    {
        return messages[(int)index];
    }

	// Use this for initialization
	void Start () 
    {
        messages = new List<Message>();

        LoadMessages();
        
        activeMessages = new Queue<MessageIndex>();

        currentDelay = 0.0f;
	}

    private void LoadMessages()
    {
        // --- Sarah ---
        AddMessage("@SarahPark2016",
            "While I respect @MayorBob for his years of energetic service to our community, please consider voting for me!");

        AddMessage("SarahPark2016",
            "Our city needs a modern sewage system! Vote #SarahPark2016 if you agree with me!");

        // --- Mayor ---
        AddMessage("@MayorBob",
            "PARKING CHEATS ARE CITY THIEVES - if this keeps up we are adding chains to the meters");

        AddMessage("@MayorBob",
            "@SallyHFreed Yes Bob Rivera ‘15 campaign contributions give you tickets in the mayor’s raffle ball. Grand prize: school zone immunity");


        // --- News ---
        AddMessage("@EDRTNews",
            "NEW 7@7 - Seven Things Your Doctor Doesn’t Want You to Know about Yogurt #Health #News");

        AddMessage("@EDRTNews",
            "Cold Weather Alert issued for this evening - Stay Toasty El Dorado! #Weather #News");

        AddMessage("@EDRTNews",
            "NEW 7@7 - Seven Ways to Trim Fat While Playing With Your Cat! #Health #Cats #News");

        AddMessage("@EDRTNews",
            "IN CONCERT This Weekend - Michael Bublé LIVE @ the Mike G. Perf. Arts. Ctr. @ 9PM! #News #Concerts #Bublebath");

        AddMessage("@EDRTNews",
            "NEW 7@7 - Seven Pets You Didn’t Know Made Great Pets For Your Grandparents #Living #News");

        // --- Detective Ben Bright ---
        AddMessage("@DetectiveBenBright",
            "A victimless crime is as impossible as a crimeless victim");

        AddMessage("@DetectiveBenBright",
            "The only time a man should go to the hospital is if he can’t walk there on his own two legs");

        AddMessage("@DetectiveBenBright",
            "Investigating arson outside the Broadmoore Building - the rank stank of crime and kerosene");

        // --- Road Hound ---
        AddMessage("@RoadHound69",
            "oh man synthetic jp-8 gives a really nasty head rush #notlovinit");

        // --- Carrie ---
        AddMessage("@CarrieOn",
            "Found a peregrine falcon!!! And ⅔ of some waterfowl! #birding #birdsofprey");

        AddMessage("@CarrieOn",
            "@BirdsEyeVu - No idea why a snowy owl would even want to nest there");

        AddMessage("@CarrieOn",
            "There really is nothing like going birding early morning with a mug of tea #Bliss");

        // --- Burger ---
        AddMessage("@JoeBurgerCorp",
            "NEW at Joe Burger- the Brunch Implosion Sandwich! Three breakfast meats in a cinnamon-toasted bun for only $3.99!");

        AddMessage("@JoeBurgerCorp",
            "@RedManFan Joe is no longer the King of Burgerdom, but president of Burger Nation. He gave up the throne in 1971 ");

        AddMessage("@JoeBurgerCorp",
            "@Freddinaut The secret to Joe’s Secret Sauce is… a secret ;-)");
    }

    void AddMessage(string name, string message)
    {
        messages.Add(new Message(name, message));
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (activeMessages.Count <= 0)
        {
            FillQueue();
        }

        currentDelay += Time.deltaTime;
        if (currentDelay >= messageDelay)
        {
            var popUp = GameObject.Find("PopUp").GetComponent<PopUp>();
            popUp.messageIndex = activeMessages.Dequeue();
            popUp.UpdatePopUp();

            Debug.Log("Start next message");

            currentDelay = 0.0f;
        }
	}

    void FillQueue()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < 4; i++)
        {
            
            MessageIndex mi = (MessageIndex)rand.Next(MinGenericMessage, MaxGenericMessage);

            activeMessages.Enqueue(mi);
        }
    }
}
