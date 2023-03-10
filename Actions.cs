using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

//Made By Hunter Warburton
//January 25th 2023

class PetCollection
{
    public List<Pet> MyPets { get; set; }
    private string filePath = "pets.txt";
    public void SavePets()
    {
        using (StreamWriter writer = new StreamWriter("pets.txt"))
        {
            foreach (Pet pet in MyPets)
            {
                string petType = pet.GetType().ToString();
                writer.WriteLine(petType +","+ pet.Name + "," + pet.HungerLevel + "," + pet.Happiness);
            }
        }
        /*
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
        */
    }

    public void LoadPets(PetCollection collection)
    {
            try
            {
                using (StreamReader sr = new StreamReader("pets.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        string type = parts[0];
                        string name = parts[1];
                        int happiness = int.Parse(parts[2]);
                        int hunger = int.Parse(parts[3]);
                        Pet newPet;
                        if (type == "Dog")
                        {
                            newPet = new Dog();
                        }
                        else if (type == "Cat")
                        {
                            newPet = new Cat();
                        }
                        else if (type == "Plant")
                        {
                            newPet = new Plant();
                        }
                        else if (type == "Fish")
                        {
                            newPet = new Fish();
                        }
                        else
                        {
                            newPet = new Pet(name, happiness, hunger);
                        }
                        newPet.Name = name;
                        newPet.Happiness = happiness;
                        newPet.HungerLevel = hunger;
                        collection.AddPet(newPet);
                    }
                }
                Console.WriteLine("Past Pets Loaded!");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("No save data found.");
            }
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
    private Validation validation;

    public PetApplication(PetStore store, PetCollection collection)
    {
        this.store = store;
        this.collection = collection;
        this.validation = new Validation();
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
            string petType = "";

            bool isValid2 = false;
            while (!isValid2)
            {
                // Display available pets
                Console.WriteLine("0. Go Back Without A Pet");
                ListPetsInStore();
                // Acquire pet code
                Console.WriteLine("Type the kind of the pet you want to acquire:");

                petType = Console.ReadLine();
                //check to see if user entered a number
                if (int.TryParse(petType, out int value))
                {
                    if (value > 0 && value <= store.AvailablePets.Count)
                    {
                        //Yes, the user entered a valid number. Now convert that to the pet name 
                        isValid2 = true;
                        petType = store.AvailablePets[value - 1].Type;
                    }
                    else if (value == 0)
                    {
                        isValid2 = true;
                        petType = "0";
                    }
                    else //some outside number
                    {
                        Console.WriteLine("Invalid input. Please enter a number between " + 0 + " and " + store.AvailablePets.Count);
                    }
                } else isValid2 = true;

            }

            //make the entered pet name lowercase and compare to also lowercased names
            petType = petType.ToLower();
            Pet selectedPet = store.AvailablePets.Find(p => p.Type.ToLower() == petType);
            if(selectedPet != null)
            {
                isValid = true;

                Pet newPet = null;
                switch (petType)
                {
                    case "dog":
                        newPet = new Dog();
                        break;
                    case "cat":
                        newPet = new Cat();
                        break;
                    case "plant":
                        newPet = new Plant();
                        break;
                    case "fish":
                        newPet = new Fish();
                        break;
                    default:
                        Console.WriteLine("Invalid pet name. Please type the pet name or number.");
                        return;
                }

                collection.AddPet(newPet);

                Console.WriteLine("Pet acquired!");

                // Prompt the user to add a prefix to the pet's name
                Console.WriteLine("What would you like to be this pet's name:");
                //Set a default name
                string newName = "My " + newPet.Type + " " + collection.MyPets.Count;
                //If the player makes a new name (not blank) name the pet
                string nameMod = Console.ReadLine();
                if (nameMod != "")
                {
                    newName = nameMod;
                }

                // Add the prefix to the pet's name
                newPet.Name = newName;

                // Start the pet's hunger and happiness timers
                collection.StartHungerTimer(newPet);
            }
            else if (petType == "0")
            {
                isValid = true;
                Console.WriteLine("You left without a new pet");
            }
            else //to handle more errors
            {
                Console.WriteLine("Invalid pet name. Please type the pet name or number.");
            }

        }

    }

    public void InteractWithPet(PetCollection collection)
    {
        if (collection.MyPets.Count == 0)
        {
            Console.WriteLine("You need to Aquire a pet first!\n");
        }
        else
        {

            // Interact with pet code
            Console.WriteLine("Select a pet number to interact with:");
            Console.WriteLine("0. Go back");
            ListMyPets(collection);

            int choice = validation.GetValidNumber( 0 , collection.MyPets.Count);

            if (choice != 0)
            {

                Pet selectedPet = collection.MyPets[choice - 1];

                //Check status of pet
                //If Hapiness < 0 OR Hunger > pet'sMaxHunger then no interaction possible
                if (selectedPet.Happiness <= 0 || selectedPet.HungerLevel > selectedPet.MaxHunger)
                {
                    Console.WriteLine(selectedPet.Name + " needs food before it will interact with you.");
                }
                else
                {
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
            }
        }
    }

    public void FeedPet(PetCollection collection)
    {
        if (collection.MyPets.Count == 0)
        {
            Console.WriteLine("You need to Aquire a pet first!\n");
        }
        else
        {
            // Feed pet code
            Console.WriteLine("Select a pet to feed:");
            Console.WriteLine("0. Go back");
            ListMyPets(collection);

            int choice = validation.GetValidNumber(0, collection.MyPets.Count);

            if (choice != 0)
            {
                Pet selectedPet = collection.MyPets[choice - 1];

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

                int foodChoice = validation.GetValidNumber(1, possibleFoods.Count);

                string selectedFood = possibleFoods[foodChoice - 1].ToLower();
                collection.FeedPet(selectedPet, selectedFood);
            }
        }
    }

    public void RemovePet(PetCollection collection)
    {
        if (collection.MyPets.Count == 0)
        {
            Console.WriteLine("You have no pets in your collection!");
        }
        else
        {
            // List all the pets in collection
            Console.WriteLine("Select a pet to remove from your collection:");
            Console.WriteLine("0. None, keep them all");
            ListMyPets(collection);

            int choice = validation.GetValidNumber(0, collection.MyPets.Count);
            if (choice == 0)
            {
                Console.WriteLine("No pets removed!");
            }
            else
            {
                Pet selectedPet = collection.MyPets[choice - 1];
                collection.RemovePet(selectedPet);
                Console.WriteLine("Pet removed from collection!");
            }
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