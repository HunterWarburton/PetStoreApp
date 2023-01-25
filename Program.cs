using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Text;

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

    public Pet(string name, string type, int hunger, int happiness, int maxHunger, int maxHappiness, int timerIncrement)
    {
        Name = name;
        Type = type;
        HungerLevel = hunger;
        Happiness = happiness;
        MaxHunger = maxHunger;
        MaxHappiness = maxHappiness;
        LikedFoods = new List<string> {"food"};
        LikedInteractions = new List<string> {"provide affection"};
        TimerIncrement = timerIncrement;
        HungerTimer = new Timer(TimerIncrement);
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

            Console.WriteLine(Name + " likes when you " + interaction + ".");
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
        if (amount == -99) HungerLevel = HungerLevel/2;
        //Set to zero if changing by 0 amount
        else if (amount == 0) HungerLevel = 0;
        else HungerLevel += amount;
    }

    public void HappinessChange(int amount)
    {
        Happiness += amount;
    }
}

class Dog : Pet
{
    public Dog() : base("Dog", "Dog", 2, 5, 10, 10, 120000)
    {
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
    public Cat() : base("Cat", "Cat", 2, 4, 8, 5, 120000)
    {
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
    public Plant() : base("Plant", "Plant", 5, 2, 6, 5, 60000)
    {
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
    public Fish() : base("Fish", "Fish", 2, 2, 5, 5, 180000)
{
        LikedFoods = new List<string> { "fish food" };
        //Declare Food modifier array
        //HungerChange, HappinessChange
        FoodChangeArray = new int[,] { { 0, 1 }};

        LikedInteractions = new List<string> { "play music", "talk to them", "tap on glass" };
        //HungerChange, HappinessChange
        InteractionArray = new int[,] { { 1, 1 }, { 1, 1 }, { 3, -2 }};
}
}

class PetStore
{
public List<Pet> AvailablePets { get; set;}
public PetStore()
{
    AvailablePets = new List<Pet> { new Dog(), new Cat(), new Plant(), new Fish() };
}
}

class PetCollection
{
    public List<Pet> MyPets { get; set; }
    private string filePath = "pets.txt";
    public void SavePets()
    {
        // Create a string builder to store the pet information
        StringBuilder sb = new StringBuilder();

        // Iterate through the pets in the collection
        foreach (Pet pet in MyPets)
        {
            // Append the pet's information to the string builder
            sb.AppendLine(pet.Name + " - " + pet.Type);
            sb.AppendLine("Hunger: " + pet.HungerLevel.ToString());
            sb.AppendLine("Happiness: " + pet.Happiness.ToString());
        }

        // Write the string builder to the file
        File.WriteAllText(filePath, sb.ToString());
    }

    public PetCollection()
    {
        MyPets = new List<Pet>();
    }

    public void AddPet(Pet pet)
    {  
        MyPets.Add(pet);
    }

    public void RemovePet(Pet pet)
    {
        MyPets.Remove(pet);
    }

    public void FeedPet(Pet pet, string food)
    {
        pet.Feed(food);
    }

    public void InteractWithPet(Pet pet, string interaction)
    {
        pet.Interact(interaction);
    }

    public void StartHungerTimer(Pet pet)
    {
        pet.HungerTimer.Elapsed += (s, e) => {
            pet.HungerChange(1);
            pet.HappinessChange(-1);
        };
        pet.HungerTimer.Start();
    }

}

interface IPetInteraction
{
    void ListPetsInStore();
    void AcquirePet(PetStore store, PetCollection collection);
    void RemovePet(PetCollection collection);
    void FeedPet(PetCollection collection);
    void InteractWithPet(PetCollection collection);
    void ViewPets(PetCollection collection);
}

class PetApplication : IPetInteraction
{
    // Reference to the PetStore and PetCollection
    private PetStore store;
    private PetCollection collection;

    public PetApplication(PetStore store, PetCollection collection)
    {
        this.store = store;
        this.collection = collection;
    }

    public void ListPetsInStore()
    {
        Console.WriteLine("Pets available in the store:");
        int index = 1;
        foreach (Pet pet in store.AvailablePets)
        {
            Console.WriteLine(index + ". " + pet.Type);
            index++;
        }
    }

    public void AcquirePet(PetStore store, PetCollection collection)
    {
        bool isValid = false;
        while(!isValid)
        {
            // Display available pets
            ListPetsInStore();
            // Acquire pet code
            Console.WriteLine("Type the kind of the pet you want to acquire:");
        
            string petType = Console.ReadLine();
            //make the entered pet name lowercase and compare to also lowercased names
            petType = petType.ToLower();
            Pet selectedPet = store.AvailablePets.Find(p => p.Type.ToLower() == petType);
            if(selectedPet != null)
            {
                isValid = true;
                collection.AddPet(selectedPet);
                Console.WriteLine("Pet acquired!");

                // Prompt the user to add a prefix to the pet's name
                Console.WriteLine("What would you like to be this pet's name:");
                string newName = Console.ReadLine();

                // Add the prefix to the pet's name
                selectedPet.Name = newName;

                // Start the pet's hunger and happiness timers
                collection.StartHungerTimer(selectedPet);
            }
            else
            {
                Console.WriteLine("Invalid pet name. Please type the pet name.");
            }
        }
    }

    public void InteractWithPet(PetCollection collection)
    {
        bool isValid = false;
        while(!isValid)
        {
        // Interact with pet code
        Console.WriteLine("Select a pet number to interact with:");
        ListMyPets(collection);

        int choice = int.Parse(Console.ReadLine());
        if (choice > 0 && choice <= collection.MyPets.Count)
        {
            isValid = true;
            Pet selectedPet = collection.MyPets[choice - 1];
            Console.WriteLine("Interacting with " + selectedPet.Name + ".\nWhat would you like to do? Type an action. Suggestions:");

            // Display the available actions for the selected pet
            switch (selectedPet.GetType().Name)
            {
                case "Dog":
                    Console.WriteLine("Pet, Rub Belly, Play, Ignore, Scold");
                    break;
                case "Cat":
                    Console.WriteLine("Pet, Rub Belly, Play, Ignore, Scold");
                    break;
                case "Plant":
                    Console.WriteLine("Talk to them, Play Music, Ignore, Pet");
                    break;
                case "Fish":
                    Console.WriteLine("Play Music, Talk to them, Tap on Glass, Rub Belly");
                    break;
                default:
                    Console.WriteLine("Invalid pet type");
                    break;
            }

            string interactionChoice = Console.ReadLine();
            interactionChoice = interactionChoice.ToLower();
            collection.InteractWithPet(selectedPet, interactionChoice);

        }
        else
        {
            Console.WriteLine("Invalid choice. Please select a valid number.");
        }
        }
    }

    public void FeedPet(PetCollection collection)
    {
        bool isValid = false;
        while(!isValid)
        {
        // Feed pet code
        Console.WriteLine("Select a pet to feed:");
        ListMyPets(collection);

        int choice = int.Parse(Console.ReadLine());
        if (choice > 0 && choice <= collection.MyPets.Count)
        {
            isValid=true;
            Pet selectedPet = collection.MyPets[choice - 1];

            bool isValid2 = false;
            while(!isValid2)
            {
            Console.WriteLine("Select a food to feed " + selectedPet.Name + ":");
            List<string> possibleFoods = new List<string>
            { 
                "Bacon Snack", "Dry Dog Food", "Tuna", "Dry Cat Food", "Water", "Plant Food", "Fish Food"
            };
            int index = 1;
            foreach (string food in possibleFoods)
            {
                Console.WriteLine(index + ". " + food);
                index++;
            }

            int foodChoice = int.Parse(Console.ReadLine());
            if (foodChoice > 0 && foodChoice <= possibleFoods.Count)
            {
                isValid2 = true;
                string selectedFood = possibleFoods[foodChoice - 1].ToLower();
                collection.FeedPet(selectedPet, selectedFood);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select a valid number.");
            }
            }
        }
        else
        {
            Console.WriteLine("Invalid choice. Please select a valid number.");
        }
        }
    }

    public void RemovePet(PetCollection collection)
    {
        // List all the pets in collection
        Console.WriteLine("Select a pet to remove from your collection:");
        Console.WriteLine("0. None, keep them all");
        ListMyPets(collection);

        int choice = int.Parse(Console.ReadLine());
        if (choice == 0) {
            Console.WriteLine("No pets removed!");
        }
        else if (choice > 0 && choice <= collection.MyPets.Count)
        {
            Pet selectedPet = collection.MyPets[choice - 1];
            collection.RemovePet(selectedPet);
            Console.WriteLine("Pet removed from collection!");
        }
        else
        {
            Console.WriteLine("Invalid choice.Please select a valid number.");
        }
    }

    public void ViewPets(PetCollection collection)
    {
        if (collection.MyPets.Count == 0)
        {
            Console.WriteLine("You have no pets in your collection!");
        }
        else
        {
            Console.WriteLine("Pets in your collection:");
            foreach (Pet pet in collection.MyPets)
            {
                Console.WriteLine(pet.Name + " - Hunger: " + pet.HungerLevel + " - Happiness: " + pet.Happiness);
            }
        }
    }

    public void ListMyPets(PetCollection collection)
    {
        int index = 1;
        foreach (Pet pet in collection.MyPets)
        {
            Console.WriteLine(index + ". " + pet.Name + " - " + pet.Type);
            index++;
        }
    }

}

//PROGRAM!!
class Program
{
    private static PetCollection collection;
static void Main(string[] args)
{
    // Instantiate the PetStore and PetCollection
    PetStore store = new PetStore();
    collection = new PetCollection();

    // Instantiate the PetApplication class
        IPetInteraction petApp = new PetApplication(store, collection);
        // Acquire a pet
        //petApp.AcquirePet(store, collection);

        // Start the main menu loop
        while (true)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. List pets in the store");
            Console.WriteLine("2. Acquire pets from the store");
            Console.WriteLine("3. Remove pets from your collection");
            Console.WriteLine("4. Feed your pets");
            Console.WriteLine("5. Interact with your pets");
            Console.WriteLine("6. View your pet's state");
            Console.WriteLine("7. Exit");

            int choice = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (choice)
            {
                case 1:
                    petApp.ListPetsInStore();
                    break;
                case 2:
                    petApp.AcquirePet(store, collection);
                    break;
                case 3:
                    petApp.RemovePet(collection);
                    break;
                case 4:
                    petApp.FeedPet(collection);
                    break;
                case 5:
                    petApp.InteractWithPet(collection);
                    break;
                case 6:
                    petApp.ViewPets(collection);
                    break;
                case 7:
                    collection.SavePets();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }

        // Add a handler for the application's exit event
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
    }

    private static void OnProcessExit(object sender, EventArgs e)
    {
        collection.SavePets();
    }


}