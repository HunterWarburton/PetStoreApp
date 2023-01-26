using System;
using System.Collections.Generic;
using System.IO;

class PetStore
{
    public List<Pet> AvailablePets { get; set;}
    public PetStore()
    {
        AvailablePets = new List<Pet> { new Dog(), new Cat(), new Plant(), new Fish() };
    }
}

class Validation
{
    //Validate Number Selection
    /*while loop to repeatedly prompt the user for input until they provide a valid number.
     * If the input is not a valid number, the loop continues and the user is prompted again.
     * If the input is a valid number, the function returns the parsed integer.*/
    public int GetValidNumber()
    {
        int userInput;
        while (true)
        {
            Console.WriteLine("Enter a number: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out userInput))
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
    //Same input validation, but this time with a max and min requirement
    public int GetValidNumber(int min, int max)
    {
        int userInput;
        while (true)
        {
            Console.WriteLine("Enter a number between " + min + " and " + max + ": ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out userInput) && userInput >= min && userInput <= max)
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between " + min + " and " + max);
            }
        }
    }
}

//PROGRAM!!
class PetAppMainProgram
{
    private static PetCollection collection;
    static void Main(string[] args)
    {
        // Instantiate the PetStore and PetCollection
        PetStore store = new PetStore();
        collection = new PetCollection();

        Validation validation = new Validation();

        // Instantiate the PetApplication class
        IPetInteraction petApp = new PetApplication(store, collection);

        //Introduce Start Screen
        Console.WriteLine("          .__....._           _.....__,\n            .^: o :':         ;': o :^.\n            `. `-' .'.       .'. `-'.'\n              `---'             `---'\n              \n    _...----...      ...   ...      ...----..._\n .- '__..-^^'----    `.  `^`  .'    ----'^^-..__`-.\n'.-'   _.--^^^'       `-._.-'       '^^^--._   `-.`\n'  .-^'                  :                  `^-.  `\n  '   `.              _.'^'._              .'   `\n        `.       ,.-'^       ^' -.,       .'\n          `.                           .'\n            `-._                   _.- '\n                `^'--...___...--'^`\n");

        Console.WriteLine("**Pet Store App**");
        Console.WriteLine("\n In this program you may aquire and take care of multiple pets.\nThey get hungry in real-time, so be sure to feed them what they like!");
        Console.WriteLine("\n Type Anything and press Enter to Start...");
        Console.ReadLine();
        Console.Clear();

        // Start the main menu loop
        //The while statement makes it so that if the thread reaches the end of the loop (close bracket) it will jump back to the start of the loop
        while (true)
        {
            Console.WriteLine("******\nWhat would you like to do? Type the number for your action.");
            Console.WriteLine("1. List pets in the store");
            Console.WriteLine("2. Acquire pets from the store");
            Console.WriteLine("3. Remove pets from your collection");
            Console.WriteLine("4. Feed your pets");
            Console.WriteLine("5. Interact with your pets");
            Console.WriteLine("6. View your pet's state");
            Console.WriteLine("7. Exit and Save");


            int choice = validation.GetValidNumber(1,7);
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