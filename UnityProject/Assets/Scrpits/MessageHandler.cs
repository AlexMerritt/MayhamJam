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
    Sarah2,
    Mayor0,
    Mayor1,
    Mayor2,
    Mayor3,
    Mayor4,
    Mayor5,
    Mayor6,
    News0,
    News1,
    News2,
    News3,
    News4,
    News5,
    News6,
    Detective0,
    Detective1,
    Detective2,
    Detective3,
    RoadHound0,
    RoadHound1,
    RoadHound2,
    RoadHound3,
    RoadHound4,
    Carrie0,
    Carrie1,
    Carrie2,
    Carrie3,
    Carrie4,
    Carrie5,
    Carrie6,
    Carrie7,
    Carrie8,
    Burger0,
    Burger1,
    Burger2,
    Burger3,
    Burger4,
    Burger5,
    Burger6,
}

public class MessageHandler : MonoBehaviour 
{
    public List<Message> messages;

    public float messageDelay;
    float currentDelay;

    Queue<MessageIndex> activeMessages;

    const int MinGenericMessage = 0;
    const int MaxGenericMessage = 41;

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

        AddMessage("SarahPark2016",
            "Have a minute to think about the future of El Dorado? Visit SarahPark2016.ca and read my campaign platform. Then, offer your thoughts!");

        // --- Mayor ---
        AddMessage("@MayorBob",
            "PARKING CHEATS ARE CITY THIEVES - if this keeps up we are adding chains to the meters");

        AddMessage("@MayorBob",
            "@SallyHFreed Yes Bob Rivera ‘15 campaign contributions give you tickets in the mayor’s raffle ball. Grand prize: school zone immunity");

        AddMessage("@MayorBob",
            "@LiveTeaOrDie Everything you have ever heard about my relationship with this city’s crystal meth community is a RED LIE");

        AddMessage("@MayorBob",
            "Note that while the bridge toll is an optional donation to the municipality, the tax and the trade tariff ARE NOT");

        AddMessage("@MayorBob",
            "Thank you @ConcernedCitizensOfCanada for your endorsement in the upcoming election- your support is valued and respected.");

        AddMessage("@MayorBob",
            "I am flattered and humbled every day to be mayor of Canada’s 109th most populous city");

        AddMessage("@MayorBob",
            "@DreamKitten22 Press conferences are over. I said that at the Last press conference- Chirps are how I will inform you all from now on");

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

        AddMessage("@EDRTNews",
            "You Won’t Believe What This Mother Found in Her Garden");

        AddMessage("@EDRTNews",
            "7@7 - Seven Vacation Ideas That Won’t Stretch the Family Budget");

        // --- Detective Ben Bright ---
        AddMessage("@DetectiveBenBright",
            "A victimless crime is as impossible as a crimeless victim");

        AddMessage("@DetectiveBenBright",
            "The only time a man should go to the hospital is if he can’t walk there on his own two legs");

        AddMessage("@DetectiveBenBright",
            "Investigating arson outside the Broadmoore Building - the rank stank of crime and kerosene");

        AddMessage("@DetectiveBenBright",
            "Jelly Donut. Tim Hortons. Grey Skies. A chance of snow. The hunt is on.");

        // --- Road Hound ---
        AddMessage("@RoadHound69",
            "oh man synthetic jp-8 gives a really nasty head rush #notlovinit");

        AddMessage("@RoadHound69",
            "‘you miss 100% of the shots you don’t take’ -the great one");

        AddMessage("@RoadHound69",
            "Ninety percent of tank driving is mental. the other half is physical. soul is definately in there too");

        AddMessage("@RoadHound69",
            "FYCK YES RCS SALE ON RED BULL FLATS! #lovinit");
        
        AddMessage("@RoadHound69",
            "*pumping iron* woah there dude in the mirror, let me get those for you *does double sets while reality twin stands awed, slowly clapping");

        // --- Carrie ---
        AddMessage("@CarrieOn",
            "Found a peregrine falcon!!! And ⅔ of some waterfowl! #birding #birdsofprey");

        AddMessage("@CarrieOn",
            "@BirdsEyeVu - No idea why a snowy owl would even want to nest there");

        AddMessage("@CarrieOn",
            "@LiveTeaOrDie There really is nothing like going birding early morning with a mug of tea #Bliss");

        AddMessage("@CarrieOn",
            "Spilt tea on my birding blanket :( #notlovinit #birding");

        AddMessage("@CarrieOn",
            "Just saw an Alder Flycatcher! Pretty early in the year for them to be around #birding ");

        AddMessage("@CarrieOn",
            "@BiNox afaik there is no version of that #birding app for Windows Phone :(");

        AddMessage("@CarrieOn",
            "@BirdsEyeVu Last time I was #birding in the states we were in Washington (the state not the capital :) )");

        AddMessage("@CarrieOn",
            "Something about spring makes me feel hopeful for the future. Must be all the #birds around :)");

        AddMessage("@CarrieOn",
            "Wow- just saw a flock of geese overhead! :D #birding #honkhonk");

        // --- Burger ---
        AddMessage("@JoeBurgerCorp",
            "NEW at Joe Burger- the Brunch Implosion Sandwich! Three breakfast meats in a cinnamon-toasted bun for only $3.99!");

        AddMessage("@JoeBurgerCorp",
            "@RedManFan Joe is no longer the King of Burgerdom, but president of Burger Nation. He gave up the throne in 1971 ");

        AddMessage("@JoeBurgerCorp",
            "@Freddinaut The secret to Joe’s Secret Sauce is… a secret ;-)");

        AddMessage("@JoeBurgerCorp",
            "@Freddinaut Joe does serve as commander-in-chief of the Burger Nation army, but the nation has always been at peace");

        AddMessage("@JoeBurgerCorp",
            "Have a craving for that south of the border taste? Burger Pizza Combos are back at Joe Burger for a limited time!");

        AddMessage("@JoeBurgerCorp",
            "@eye88kraze Joe doesn’t have a favorite burger. If he didn’t put heart into every last beefy morsel he wouldn’t even cook it");

        AddMessage("@JoeBurgerCorp",
            "@QueenGreen All of our franchises share in the jb 20% sustainability by 2020 commitment-philosophy (non-binding)");
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