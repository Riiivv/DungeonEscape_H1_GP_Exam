using System;
namespace DungeonEscape_H1.NewFolder
{
    public class Dungeon
    {
        // Define a 10x10 dungeon array
        static char[,] dungeon = new char[10, 10];

        // Player's starting position
        public static int playerX = 9;
        public static int playerY = 0;

        // Constants for different dungeon elements
        const char EMPTY = '.';
        const char PLAYER = 'P';
        const char TRAP = 'T';
        const char KEY = 'K';
        const char EXIT = 'E';

        // Game state variable for the key
        static bool hasKey = false;

        // Random number generator for random dungeon layout
        private static Random rnd = new Random();

        // Entry point for starting the game
        public static void StartGame()
        {
            bool keepPlaying = true;

            // Loop to allow restarting or exiting
            while (keepPlaying)
            {
                ResetGameLayout(); // Generate a new randomized dungeon layout
                keepPlaying = GameRunning(); // Run the main game loop
            }
            Console.WriteLine("Thank you for playing!");
        }

        // Main game loop to keep the game running until completion
        public static bool GameRunning()
        {
            Console.WriteLine("Press a key to start\nThe Dungeon Escape\nEnjoy & Have fun :)");
            Console.ReadKey();
            while (true)
            {
                DisplayDungeon(); // Display the dungeon grid
                DisplayStatus();  // Display the player's current status (key possession)

                Console.WriteLine("Enter direction (N/S/E/W)"); // Prompt for player movement
                char direction = Console.ReadKey().KeyChar; // Get user input for direction
                Console.WriteLine();

                UpdatePlayerPosition(direction); // Move player based on input

                // Check if the player has won the game
                if (CheckWinCondition())
                {
                    Console.WriteLine("You escaped the dungeon!");
                    return PromptRestartOrExit(); // Ask if player wants to restart or exit
                }
            }
        }

        // Display the current layout of the dungeon
        public static void DisplayDungeon()
        {
            for (int i = 0; i < dungeon.GetLength(0); i++)
            {
                for (int j = 0; j < dungeon.GetLength(1); j++)
                {
                    // Display the player's current position
                    if (i == playerX && j == playerY)
                        Console.Write(PLAYER + " ");
                    else
                        Console.Write(dungeon[i, j] + " ");
                }
                Console.WriteLine(); // Move to next line after each row
            }
        }

        // Display the player's current status: if they have the key or not
        public static void DisplayStatus()
        {
            Console.WriteLine(hasKey ? "You have the key!" : "You need the key to escape.");
        }

        // Update player's position based on the direction input
        public static void UpdatePlayerPosition(char direction)
        {
            int newX = playerX;
            int newY = playerY;

            // Adjust newX and newY based on direction
            switch (direction.ToString().ToUpper())
            {
                case "N":
                    newX--; // Move up
                    break;
                case "S":
                    newX++; // Move down
                    break;
                case "E":
                    newY++; // Move right
                    break;
                case "W":
                    newY--; // Move left
                    break;
                default:
                    Console.WriteLine("Invalid Direction! Please enter N, S, E, or W.");
                    return; // Exit if input is invalid
            }

            // Check if the new position is within the dungeon boundaries
            if (newX >= 0 && newX < dungeon.GetLength(0) && newY >= 0 && newY < dungeon.GetLength(1))
            {
                // Retrieve the element at the new position
                char nextCell = dungeon[newX, newY];

                // Actions based on what's in the next cell
                switch (nextCell)
                {
                    case TRAP:
                        Console.WriteLine("You walked into a trap! Game over.");
                       bool exit = PromptRestartOrExit(); // Restart game if player hits a trap
                        if (exit == false)
                        {
                          Environment.Exit(0);
                        }
                        return;
                    case KEY:
                        hasKey = true; // Player collects the key
                        Console.WriteLine("You found the key!");
                        break;
                    case EXIT:
                        // The exit is handled in the CheckWinCondition method
                        break;
                    default:
                        Console.WriteLine("You moved into an empty space."); // No action on empty space
                        break;
                }

                // Update player's position in the dungeon
                dungeon[playerX, playerY] = EMPTY; // Clear old position
                playerX = newX; // Set new X position
                playerY = newY; // Set new Y position
                dungeon[playerX, playerY] = PLAYER; // Place player in new position
            }
        }

        // Check if player meets the win condition
        public static bool CheckWinCondition()
        {
            // Check if player is at the exit position
            if (playerX == 9 && playerY == 9)
            {
                if (hasKey) // If player has the key, they can escape
                {
                    Console.WriteLine("Congratulations! You have escaped the dungeon!");
                    return true; // Win condition met
                }
                else // If player is at exit without key
                {
                    Console.WriteLine("You need the key to escape!");
                    return false; // Can't win without the key
                }
            }
            return false; // Not at exit position
        }

        // Reset the dungeon layout to generate a new random setup
        public static void ResetGameLayout()
        {
            // Fill the dungeon with empty spaces
            for (int i = 0; i < dungeon.GetLength(0); i++)
                for (int j = 0; j < dungeon.GetLength(1); j++)
                    dungeon[i, j] = EMPTY;

            // Place player at starting position and the exit at the bottom right corner
            dungeon[9, 0] = PLAYER;
            dungeon[9, 9] = EXIT;

            // Randomly place the key within the dungeon
            PlaceItemRandomly(KEY);

            // Place traps in random locations throughout the dungeon
            for (int i = 0; i < 25; i++) // Number of traps
            {
                PlaceItemRandomly(TRAP);
            }
        }

        // Places an item at a random empty cell in the dungeon
        private static void PlaceItemRandomly(char item)
        {
            int x, y;
            // Find a random empty cell to place the item
            do
            {
                x = rnd.Next(dungeon.GetLength(0));
                y = rnd.Next(dungeon.GetLength(1));
            }
            while (dungeon[x, y] != EMPTY); // Ensure placement is only on an empty cell

            dungeon[x, y] = item; // Place the item in the randomly selected cell
        }

        private static bool PromptRestartOrExit()
        {
            Console.WriteLine("Press 'R' to restart or 'E' to exit.");

            // Wait for a valid key press
            while (true)
            {
                char choice = Console.ReadKey(true).KeyChar;
                if (choice == 'R' || choice == 'r')
                {
                    RestartGame(); // Restart the game
                    return true;
                }
                else if (choice == 'E' || choice == 'e')
                {
                    Console.WriteLine("\nExiting the game. Goodbye!");
                    return false; // Exit the game
                }
                else
                {
                    Console.WriteLine("\nInvalid choice! Please press 'R' to restart or 'E' to exit.");
                }
            }


            // Restart the game after a game-over event
            static void RestartGame()
            {
                Console.WriteLine("Restarting the game...");
                playerX = 9; // Reset player X position
                playerY = 0; // Reset player Y position
                hasKey = false; // Reset key possession status
                ResetGameLayout(); // Generate a new dungeon layout
                Console.WriteLine("Game restarted! Press any key to continue.");
                Console.ReadKey();
                GameRunning(); // Start the main game loop again
            }
        }
    }
}