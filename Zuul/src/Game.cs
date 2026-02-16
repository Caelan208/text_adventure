using System;
using System.ComponentModel;


class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("down", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);
		theatre.AddExit("up", outside);

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		Item PlasmaCutter = new Item("PlasmaCutter", 2, "this is a plasmacutter you can use this to shoot the limbs off a necromorph.");
		Item KeyCard = new Item("KeyCard", 1, "main keycard to acces general vicinities.");
		Item NurseryKeyCard = new Item("NurseryKeyCard", 1, "gives acces to the nursery.");
		Item IdWristband = new Item("IdWristband", 0, "gives acces to healing station.");
		Item MedicalSpray = new Item("MedicalSpray", 3, "heals your wounds.");
		
		// And add them to the Rooms
		outside.AddItem(PlasmaCutter);
		theatre.AddItem(KeyCard);
		pub.AddItem(NurseryKeyCard);
		lab.AddItem(IdWristband);
		office.AddItem(MedicalSpray);

		// Start game outside
		player.CurrentRoom = outside;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished && player.IsAlive())
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}

		if (!player.IsAlive())
		{
			Console.WriteLine("\nYou have died! Game Over.");
		}
		
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		PrintStatus();
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "look":
				Console.WriteLine(player.CurrentRoom.GetLongDescription());
				break;
			case "get":
				GetItem(command);
				break;
			case "inventory":
				ShowInventory();
				break;
			case "inspect":
				InspectItem(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
		}

		return wantToQuit;
	}

	// Print the player's current status
	private void PrintStatus()
	{
		Console.WriteLine($"Health: {player.GetHealth()}/100");
	}

	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		}

		player.CurrentRoom = nextRoom;
		// Player takes damage when moving to another room
		player.Damage(10);
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		PrintStatus();

		// Check if player reached the office (win condition)
		if (player.CurrentRoom.GetShortDescription() == "in the computing admin office")
		{
			Console.WriteLine("\nCongratulations! You have reached the office and won the game!");
		}
	}

	// Try to get an item from the room and put it in inventory.
	private void GetItem(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know what to get...
			Console.WriteLine("Get what?");
			return;
		}

		string itemName = command.SecondWord;

		// Try to get the item from the room.
		Item item = player.CurrentRoom.GetItem(itemName);
		if (item == null)
		{
			Console.WriteLine("There is no "+itemName+" here!");
			return;
		}

		// Try to add the item to inventory.
		if (player.GetInventory().Put(itemName, item))
		{
			Console.WriteLine("You picked up the " + itemName + ".");
		}
		else
		{
			Console.WriteLine("You couldn't pick up the " + itemName + ".");
		}
	}

	// Show the player's inventory.
	private void ShowInventory()
	{
		Inventory inventory = player.GetInventory();
		Console.WriteLine("\n=== INVENTORY ===");
		
		if (inventory.ItemCount() == 0)
		{
			Console.WriteLine("Your inventory is empty.");
		}
		else
		{
			int count = 1;
			foreach (var kvp in inventory.GetItems())
			{
				Console.WriteLine(count + ". " + kvp.Key);
				count++;
			}
		}
		Console.WriteLine();
	}

	// Inspect an item from inventory to see its description.
	private void InspectItem(Command command)
	{
		if(!command.HasSecondWord())
		{
			Console.WriteLine("Inspect what?");
			return;
		}

		string itemName = command.SecondWord;
		Inventory inventory = player.GetInventory();

		// Check if item is in inventory
		if (!inventory.HasItem(itemName))
		{
			Console.WriteLine("You don't have a " + itemName + "!");
			return;
		}

		// Get the item and show its description
		Item item = inventory.GetItems()[itemName];
		Console.WriteLine("\n--- " + itemName + " ---");
		Console.WriteLine(item.Description);
		Console.WriteLine();
	}
}