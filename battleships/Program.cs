﻿namespace battleships;

public static class Program
{
    private static void Main(string[] args)
    {
        //IF this was a full implementation, i would look to use DI and IoC to manage the game engine
        //and all of its dependencies, but for now, we will just create a new instance
        //this would also allow us to have different rendering engines which could be swapped between at the developers whim
        var game = new GameEngine();
        
        //define settings (get from cmd line or appsettings.json in future)
        game.Settings = new GameSettings
        {
            Width = 10,
            Height = 10,
            NumberOfBattleships = 1,
            NumberOfDestroyers = 2
        };
        
        //init the engine
        game.Init();
        
        //create required number of ships, and place them to create the board
        /*   A B C D E F G H I J
         *   0 1 2 3 4 5 6 7 8 9
         * 0
         * 1   # # # # #
         * 2               #
         * 3               #
         * 4               #
         * 5               #
         * 6         # # # #
         * 7
         * 8
         * 9
         */
        
        //if this was a full implementation, i would like to have a ShipFactory class to create ships
        //and each ship type would be its own class, inheriting from a base Ship class
        //this would allow for more control and enforcing the rules, while still allowing for customisation
        
        //ideally, placement and orientation would be randomised but feels like overkill for now
        //number of ships and locations could also come from a game settings file, which would be placed in the GameSettings class
        
        //battleships first
        game.AddShip(ShipType.Battleship, 1, 1, Orientation.Horizontal);
        
        //destroyers
        game.AddShip(ShipType.Destroyer, 4, 6, Orientation.Horizontal);
        game.AddShip(ShipType.Destroyer, 7, 2, Orientation.Vertical);

        var messageString = string.Empty;
        
        //game loop
        while (game.IsGameInProgress())
        {
            //rendering engine is console based and distinct from the game engine
            
            //clear frame
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            
            //write out any game messages we need
            Console.WriteLine(messageString);
            Console.WriteLine();
            
            //draw grid
            Console.Write("  ");
            
            //draw A to whatever the width is in letters
            for (var x = 0; x < game.Settings.Width; x++)
            {
                Console.Write((char)('A' + x));
                Console.Write(' ');
            }
            Console.WriteLine();
            
            // draw each row
            for (var y = 0; y < game.Settings.Height; y++)
            {
                //first write in the row number
                Console.Write(y);
                Console.Write('|');
                for (var x = 0; x < game.Settings.Width; x++)
                {
                    switch (game.GameBoard[x, y])
                    {
                        case 0:
                            Console.Write(' ');
                            break;
                        case 1:
                            Console.Write('*');
                            break;
                        case -1:
                            Console.Write('#');
                            break;
                    }
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            
            //ask for guess
            Console.Write("Enter your next move (eg A1): ");
            var attempt = Console.ReadLine();
            
            //validate input
            if (!game.ValidateMove(attempt))
            {
                messageString = "Invalid move";
                continue;
            }
            
            //check and make the move
            var result = game.MakeMove(attempt[0], attempt[1]);
            
            //convert result to string for displaying to user
            messageString = game.ResultToString(result);
        }
    }
}