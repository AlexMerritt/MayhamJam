using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Message
{
    public Message(string header, string text, Sprite sprite)
    {
        this.header = header;
        this.text = text;
        this.sprite = sprite;
    }

    public string header;
    public string text;
    public Sprite sprite;
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

    // Low Chaos messages
    MayorLow0,
    MayorLow1,
    MayorLow2,
    CarrieLow0,
    CarrieLow1,
    CarrieLow2,
    NewsLow0,
    NewsLow1,
    NewsLow2,

    // Med Chaos
    BurgerMed0,
    CarrieMed0,
    CarrieMed1,
    CarrieMed2,
    MayorMed0,
    MayorMed1,
    MayorMed2,
    NewsMed0,
    NewsMed1,
    NewsMed2,

    // High Messages
    RoadHoundHigh0,
    RoadHoundHigh1,
    RoadHoundHigh2,
    MayorHigh0,
    MayorHigh1,
    MayorHigh2,
    CarrieHigh0,
    CarrieHigh1,
    CarrieHigh2,
    NewsHigh0,
}

public class MessageHandler : MonoBehaviour 
{
    public List<Message> messages;

    public float messageDelay;
    float currentDelay;

    Queue<MessageIndex> activeMessages;

    const int MinGenericMessage = 0;
    const int MaxGenericMessage = 41;

    const int MinLowChaosMessage = MaxGenericMessage + 1;
    const int MaxLowChaosMessage = MinLowChaosMessage + 8;

    const int MinMedChaosMessage = MaxLowChaosMessage + 1;
    const int MaxMedChaosMessage = MinMedChaosMessage + 9;

    const int MinHighChaosMessage = MaxMedChaosMessage + 1;
    const int MaxHighChaosMessage = MinHighChaosMessage + 9;


    Sprite sprite;

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

        currentDelay = messageDelay;
	}

    private void LoadMessages()
    {
        LoadNormalMessages();

        LoadLowMessages();

        LoadMedMessages();

        LoadHighMessages();
    }

    void LoadNormalMessages()
    {
        sprite = Resources.Load<Sprite>("Sarah");
        // --- Sarah ---
        AddMessage("@SarahPark2016",
            "While I respect @MayorBob for his years of energetic service to our community, please consider voting for me!");

        AddMessage("SarahPark2016",
            "Our city needs a modern sewage system! Vote #SarahPark2016 if you agree with me!");

        AddMessage("SarahPark2016",
            "Have a minute to think about the future of El Dorado? Visit SarahPark2016.ca and read my campaign platform. Then, offer your thoughts!");

        sprite = Resources.Load<Sprite>("Bob");
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

        sprite = Resources.Load<Sprite>("News");
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

        sprite = Resources.Load<Sprite>("DetectiveBen");
        // --- Detective Ben Bright ---
        AddMessage("@DetectiveBenBright",
            "A victimless crime is as impossible as a crimeless victim");

        AddMessage("@DetectiveBenBright",
            "The only time a man should go to the hospital is if he can’t walk there on his own two legs");

        AddMessage("@DetectiveBenBright",
            "Investigating arson outside the Broadmoore Building - the rank stank of crime and kerosene");

        AddMessage("@DetectiveBenBright",
            "Jelly Donut. Tim Hortons. Grey Skies. A chance of snow. The hunt is on.");

        sprite = Resources.Load<Sprite>("RoadHound69");
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

        sprite = Resources.Load<Sprite>("Carrie");
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

        sprite = Resources.Load<Sprite>("Burger");
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

    void LoadLowMessages()
    {
        // Bob
        sprite = Resources.Load<Sprite>("Bob");

        AddMessage("@MayorBob",
            "I’ve been getting reports of a giant monster rampaging through town. Trust me, we’re imagining things, don’t shoot it.");

        AddMessage("@MayorBob",
            "I’m not calling the military until I’m really sure there IS a monster here");

        AddMessage("@MayorBob",
            "Just because you see, hear and feel a monster does NOT mean a monster is actually there");

        // Carrie
        sprite = Resources.Load<Sprite>("Carrie");

        AddMessage("@CarrieOn",
            "!!! AM I THE ONLY ONE WHO SEES THIS!? #GiantRampagingMonster #GiantRampagingMonster");

        AddMessage("@CarrieOn",
            "There’s a finch on its tail - swaying too fast to ID #Birding #GiantRampagingMonster");

        AddMessage("@CarrieOn",
            "I wonder how on earth #GiantRampagingMonster’s bone structure can support it’s mass O.o");

        // News
        sprite = Resources.Load<Sprite>("News");

        AddMessage("@EDRT",
            "#BREAKING #NEWS - VOLCANIC ACTIVITY REPORTED IN EL DORADO BY CITIZEN REPORTERS");

        AddMessage("@EDRT",
            "#BREAKING #NEWS - Volcano Warning Sidegraded to Tornado Warning");

        AddMessage("@EDRT",
            "#BREAKING #NEWS - #GiantRampagingMonster trending on Chirper- Tell Us What You Think It Means.");
    }

    void LoadMedMessages()
    {
        sprite = Resources.Load<Sprite>("Burger");
        // --- Burger ---
        AddMessage("@JoeBurgerCorp",
            "All El Dorado Joe Burger locations are offering the bereaved victims of the #GiantRampagingMonster half off Joe Combo coupons");

        // Carrie
        sprite = Resources.Load<Sprite>("Carrie");

        AddMessage("@CarrieOn",
            "That #GiantRampagingMonster looks unstoppable! I wonder what it’s dermis is made of… ");

        AddMessage("@CarrieOn",
            "Still following the #GiantRampagingMonster as it rampages. #monstering ?");

        AddMessage("@CarrieOn",
            "Still following the #GiantRampagingMonster as it rampages. #monstering ?");

        // Bob
        sprite = Resources.Load<Sprite>("Bob");

        AddMessage("@MayorBob",
            "CITIZENRY OF EL DORADO - Nothing to fear from the #GiantRampagingMonster, I’m on it");

        AddMessage("@MayorBob",
            "Since 1999, 40% of the El Dorado police budget has gone towards zombie / american invasion proofing #YourWelcome #MayorBob2015");

        AddMessage("@MayorBob",
            "I am on the phone with the military right now RE #GiantRampagingMonster But they Doubt My Sincerity Yet Again…");

        // News
        sprite = Resources.Load<Sprite>("News");

        AddMessage("@EDRT",
            "#BREAKING #NEWS Tornado Warning Upgraded to Giant Monster ");

        AddMessage("@EDRT",
            "#BREAKING #NEWS #GiantRampagingMonster #News #CurrentEvents #LocalEvents #Obituaries");

        AddMessage("@EDRT",
            "#BREAKING #NEWS EDRT Reporter Jessica Grandwallace Reporting Live Online On Chirper’s response to the #GiantRampagingMonster");
    }

    void LoadHighMessages()
    {
        sprite = Resources.Load<Sprite>("RoadHound69");
        // --- Road Hound ---
        AddMessage("@RoadHound69",
            "WOOOOOOOO!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Never get to take treads on highways #lovinit #LOVIN #IT");

        AddMessage("@RoadHound69",
            "HELL YEAH think someone in B Platoon hit that giant reptile fucker in the neck #BringItDown #lovinit");

        AddMessage("@RoadHound69",
            "@LopeGaoru32 HEY LOPEZ load those shells faster you skinny-ass fucker");

        sprite = Resources.Load<Sprite>("Bob");
        // --- Mayor ---
        AddMessage("@MayorBob",
            "THIS IS ALL THE FALT OF CANADIA TRAITORS WHO SAID NO TO MY NUCLEAR PROGRAM. i told you this would happen");

        AddMessage("@MayorBob",
            "GIANT MONSTER THIS IS YOR LAST CHANCE MY POWERS WILL SOON BE REVEALED AND YOU QILL DIE!!!!");

        AddMessage("@MayorBob",
            "wendigo, rise and save me! !!!! you proimised you’d save me in my time of direst neeed!!!!!");

        // Carrie
        sprite = Resources.Load<Sprite>("Carrie");

        AddMessage("@CarrieOn",
            "The #GiantRampagingMonster really did a number on our city…");

        AddMessage("@CarrieOn",
            "All the birds are gone :(");

        AddMessage("@CarrieOn",
            "Dear internet can we please switch #GiantRampagingMonster to #GRM my fingers are sore");

        sprite = Resources.Load<Sprite>("News");

        AddMessage("@EDRTNews",
            "#BREAKING #NEWS #ELDORADO_APOCALYPSE_2014 EPC UNTLD DETH N’ DSTRUCTN ACRS CTY #GiantRampagingMonster");
    }

    void AddMessage(string name, string message)
    {
        messages.Add(new Message(name, message, sprite));
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
            gameObject.GetComponent<AudioSource>().Play();

            //Debug.Log("Start next message");

            currentDelay = 0.0f;
        }
	}

    void FillQueue()
    {
        var chaosLevel = GameObject.Find("City").GetComponent<CityManager>().GetChaosLevel();

        //System.Random rand = new System.Random();

        if ((int)chaosLevel > (int)CityState.Medium)
        {
            activeMessages.Enqueue(GetRandomIndex(MinHighChaosMessage, MaxHighChaosMessage));
        }

        if ((int)chaosLevel > (int)CityState.Low)
        {
            activeMessages.Enqueue(GetRandomIndex(MinMedChaosMessage, MaxMedChaosMessage));
        }

        if ((int)chaosLevel > (int)CityState.Normal)
        {
            activeMessages.Enqueue(GetRandomIndex(MinLowChaosMessage, MaxLowChaosMessage));
        }

        for (int i = 0; i < 4 - (int)chaosLevel; i++)
        {
            MessageIndex mi;

            mi = GetRandomIndex(MinGenericMessage, MaxGenericMessage);

            activeMessages.Enqueue(mi);
        }
    }

    MessageIndex GetRandomIndex(int min, int max)
    {
        return (MessageIndex)Random.Range(min, max);
    }
}