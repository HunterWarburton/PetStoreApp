using System.Timers;
using Timer = System.Timers.Timer;

//Made By Hunter Warburton
//January 25th 2023

class Pet
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int HungerLevel { get; set; }
    public int Happiness { get; set; }
    public int MaxHunger { get; set; }
    public int MaxHappiness { get; set; }
    public int TimerIncrement {get; set;}
    public Timer HungerTimer {get; set;}
    public List<string> LikedFoods { get; set; }
    public List<string> LikedInteractions { get; set;}
    public int[,] FoodChangeArray { get; set;}
    public int[,] InteractionArray { get; set;}

    public Pet(string name, int hunger, int happiness)
    {
        Name = name;
        Type = "";
        HungerLevel = hunger;
        Happiness = happiness;
        MaxHunger = 2;
        MaxHappiness = 2;
        LikedFoods = new List<string> {"food"};
        LikedInteractions = new List<string> {"provide affection"};
        TimerIncrement = 12000;
        
    }

    public void Feed(string food)
    {
        //If the fed food is in the list of the pet's Liked Foods, we find the index of it
        //Using the index, we can get the change numbers to put into the happiness and hunger change
        int index = LikedFoods.IndexOf(food);
        if (index != -1)
        {
            HungerChange(FoodChangeArray[index,0]);
            HappinessChange(FoodChangeArray[index,1]);
            //Console.WriteLine("Found " + food + " at position " + index);
            Console.WriteLine("Fed " + Name + " " + food + ".\nIt was tasty!");
        }
        else //given food is not liked
        {
            HungerLevel += 2;
            Happiness -= 2;
            Console.WriteLine(Name + " did not like " + food);
        }
    }

    public void Interact(string interaction)
    {
        int index = LikedInteractions.IndexOf(interaction);
        //randomly the cat hates being Pet
        if (Type == "Cat" && interaction == "pet") {
            //50% chance
            Random rnd = new Random();
            if (rnd.Next(11) > 5)  {
               index = -1;
               Console.WriteLine("Sometimes Cats get overstimulated"); 
            }
        }
        if (index != -1)
        {
            HungerChange(InteractionArray[index,0]);
            HappinessChange(InteractionArray[index,1]);

            if (InteractionArray[index, 1] > 0)
                Console.WriteLine(Name + " likes when you " + interaction + ".");
            else
                Console.WriteLine(Name + " does NOT like when you " + interaction + ".");
        }
        else //given interaction is not liked
        {
            HungerLevel += 2;
            Happiness -= 2;
            Console.WriteLine(Name + " did not like " + interaction);
        }
    }

    public void HungerChange(int amount)
    {
        //Halve if fed an amount of -99
        if (amount == -99) HungerLevel = HungerLevel / 2;
        //Set to zero if changing by 0 amount
        else if (amount == 0) HungerLevel = 0;
        else HungerLevel += amount;
        //Warn about hunger
        if (HungerLevel > MaxHunger)
        {
            Console.WriteLine(Name + " is very hungry! It needs food!");
        }
    }

    public void HappinessChange(int amount)
    {
        Happiness += amount;
        //check maxHappiness
        if (Happiness > MaxHappiness)
        {
            Happiness = MaxHappiness;
            Console.WriteLine(Name + " has reached its maximum happiness!");
        }
        if (Happiness <= 0)
        {
            Console.WriteLine(Name + " is unhappy! It needs food!");
        }
    }
}

class Dog : Pet
{
    public Dog() : base("Dog", 2, 5)
    {
        Type = "Dog";
        MaxHunger = 10;
        MaxHappiness = 10;

        TimerIncrement = 120000;
        HungerTimer = new Timer(TimerIncrement);

        LikedFoods = new List<string> { "bacon snack", "dry dog food" };
        //Declare Food modifier array
        //HungerChange,HappinessChange
        FoodChangeArray = new int[,] { { -99, 1 }, { 0, 1 }};

        LikedInteractions = new List<string> {  "rub belly", "play", "scold" };
        //HungerChange,HappinessChange
        InteractionArray = new int[,] { { 1, 1 }, { 3, 2 }, { 2, -2 }};
    }
}

class Cat : Pet
{
    public Cat() : base("Cat", 2, 4)
    {
        Type = "Cat";
        MaxHunger = 8;
        MaxHappiness = 5;

        TimerIncrement = 120000;
        HungerTimer = new Timer(TimerIncrement);

        LikedFoods = new List<string> { "tuna", "dry cat food" };
        //Declare Food modifier array
        //HungerChange,HappinessChange
        FoodChangeArray = new int[,] { { 0, 3 }, { -99, 1 }};

        LikedInteractions = new List<string> { "pet", "ignore", "scold" };
        //HungerChange,HappinessChange
        InteractionArray = new int[,] { { 1, 1 }, { 1, 2 }, { 2, -2 }};
    }
}

class Plant : Pet
{
    public Plant() : base("Plant", 5, 2)
    {
        Type = "Plant";
        MaxHunger = 6;
        MaxHappiness = 5;

        TimerIncrement = 60000;
        HungerTimer = new Timer(TimerIncrement);

        LikedFoods = new List<string> { "water", "plant food" };
        //Declare Food modifier array
        //HungerChange, HappinessChange
        FoodChangeArray = new int[,] { { -99, 1 }, { 0, 1 }};

        LikedInteractions = new List<string> { "talk to them", "play music", "ignore" };
        //HungerChange, HappinessChange
        InteractionArray = new int[,] { { 1, 1 }, { 1, 2 }, { 3, -3 }};
    }
}

class Fish : Pet
{
    public Fish() : base("Fish", 2, 2)
{
        Type = "Fish";
        MaxHunger = 5;
        MaxHappiness = 5;

        TimerIncrement = 180000;
        HungerTimer = new Timer(TimerIncrement);

        LikedFoods = new List<string> { "fish food" };
        //Declare Food modifier array
        //HungerChange, HappinessChange
        FoodChangeArray = new int[,] { { 0, 1 }};

        LikedInteractions = new List<string> { "play music", "talk to them", "tap on glass" };
        //HungerChange, HappinessChange
        InteractionArray = new int[,] { { 1, 1 }, { 1, 1 }, { 3, -2 }};
}
}