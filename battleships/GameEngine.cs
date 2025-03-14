namespace battleships;

public class GameEngine
{
    public GameSettings Settings { get; set; } = new GameSettings();
    public GameBoard GameBoard { get; set; } = new GameBoard();
    public IEnumerable<Ship> Ships => ships;

    private List<Ship> ships = new List<Ship>();

    public void Init()
    {
        GameBoard = new GameBoard();
        GameBoard.Init(Settings.Width, Settings.Height);
        
        ships.Clear();
    }

    public bool IsGameInProgress()
    {
        //check all ships, if any are not destroyed, the game is still in progress
        foreach (var ship in ships)
        {
            if (!ship.IsDestroyed())
            {
                //at least 1 ship is not destroyed
                return true;
            }
        }
        
        //looks like all ships are done, game over
        return false;
    }

    public void AddShip(Ship ship)
    {
        ships.Add(ship);
    }

    //add a ship in a particular place in a particular orientation
    //not yet validated to prevent collisions
    public void AddShip(ShipType shipType, int x, int y, Orientation orientation)
    {
        ships.Add(new Ship
        {
            ShipType = shipType,
            X = x,
            Y = y,
            Length = GetShipLengthFromType(shipType),
            Orientation = orientation
        });
    }

    //default ship lengths
    private int GetShipLengthFromType(ShipType shipType)
    {
        switch (shipType)
        {
            case ShipType.Carrier:
                return 5;
            case ShipType.Battleship:
                return 5; //usually 4
            case ShipType.Destroyer:
                return 4; //usually 2
            case ShipType.Cruiser:
                return 3;
            case ShipType.Submarine:
                return 3;
        }
        return 0;
    }

    public bool ValidateMove(string? attempt)
    {
        //empty guess is invalid
        if (string.IsNullOrWhiteSpace(attempt))
        {
            return false;
        }

        //right now, we only accept a single letter and a single number, so 2 characters
        if (attempt.Length != 2)
        {
            return false;
        }
        
        //is the first character a capital letter within the range we accept
        if (attempt[0] < 'A' || attempt[0] >= 'A' + Settings.Width)
        {
            return false;
        }
        
        //is the second character, a number character within the range we accept
        if (attempt[1] < '0' || attempt[1] > '0' + Settings.Height)
        {
            return false;
        }
        
        return true;
    }

    public int MakeMove(char xChar, char yChar)
    {
                    
        //translate input
        var guessX = xChar - 'A';
        var guessY = yChar - '0';

        //check guess
        if (GameBoard[guessX, guessY] != 0)
        {
            return 1;
        }

        var isHit = false;
        var hitPosition = -1;
        var wasShipDestroyed = ShipType.None;
        
        foreach (var ship in ships)
        {
            //
            if (!(ship.X == guessX || ship.Y == guessY))
            {
                //this ship is not on the guess row or column, miss
                isHit = false;
                continue;
            }
                
            if (ship.Orientation == Orientation.Horizontal && ship.Y == guessY)
            {
                //if the ship is across the board, we need to care about the row first
                if (guessX >= ship.X && guessX < ship.X + ship.Length)
                {
                    hitPosition = guessX - ship.X;
                    isHit = true;
                }
            } 
            else if (ship.Orientation == Orientation.Vertical && ship.X == guessX)
            {
                //we care about the column first if its vertical
                if (guessY >= ship.Y && guessY < ship.Y + ship.Length)
                {
                    hitPosition = guessY - ship.Y;
                    isHit = true;
                }
            }

            if (isHit)
            {
                if (ship.AddHit(hitPosition) == 1)
                {
                    wasShipDestroyed = ship.ShipType;
                }
                break;
            }
        }

        if (isHit)
        {
            GameBoard.MarkHit(guessX, guessY);
            if (wasShipDestroyed != ShipType.None)
            {
                var returnVal = 10 + (int)wasShipDestroyed;
                wasShipDestroyed = ShipType.None;
                
                return returnVal;
            }
            return 2;
        }

        GameBoard.MarkMiss(guessX, guessY);
        return 3;
    }

    public string ResultToString(int result)
    {
        switch (result)
        {
            case 1: return "Duplicate Move";
            case 2: return "Hit";
            case 3: return "Miss";
            
            case 11: return "Destroyed a Carrier";
            case 12: return "Destroyed a Battleship";
            case 13: return "Destroyed a Cruiser";
            case 14: return "Destroyed a Submarine";
            case 15: return "Destroyed a Destroyer";
        }
        
        return string.Empty;
    }
}
