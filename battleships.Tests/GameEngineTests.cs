namespace battleships.Tests;

public class GameEngineTests
{
    [Test]
    public void Init_ResetsGameBoardAndShips()
    {
        var engine = new GameEngine();
        engine.Settings.Width = 10;
        engine.Settings.Height = 5;
        engine.AddShip(new Ship { Length = 3 });

        engine.Init();

        Assert.AreEqual(10, engine.GameBoard.Width);
        Assert.AreEqual(5, engine.GameBoard.Height);
        Assert.IsEmpty(engine.Ships);
    }

    [Test]
    public void IsGameInProgress_ReturnsFalseWhenAllShipsDestroyed()
    {
        var engine = new GameEngine();
        var ship1 = new Ship { Length = 3 };
        var ship2 = new Ship { Length = 2 };
        engine.AddShip(ship1);
        engine.AddShip(ship2);
        
        ship1.AddHit(0);
        ship1.AddHit(1);
        ship1.AddHit(2);
        ship2.AddHit(0);
        ship2.AddHit(1);

        Assert.IsFalse(engine.IsGameInProgress());
    }

    [Test]
    public void IsGameInProgress_ReturnsTrueWhenAnyShipNotDestroyed()
    {
        var engine = new GameEngine();
        var ship1 = new Ship { Length = 3 };
        var ship2 = new Ship { Length = 2 };
        engine.AddShip(ship1);
        engine.AddShip(ship2);

        ship1.AddHit(0);
        ship1.AddHit(1);
        ship1.AddHit(2);
        ship2.AddHit(0);

        Assert.IsTrue(engine.IsGameInProgress());
    }

    [Test]
    public void IsGameInProgress_ReturnsTrueWhenNoShipsAdded()
    {
        var engine = new GameEngine();

        Assert.IsFalse(engine.IsGameInProgress());
    }
    
    [Test]
    public void AddShip_AddsShipToList()
    {
        var engine = new GameEngine();
        var ship = new Ship { Length = 3 };

        engine.AddShip(ship);

        Assert.Contains(ship, engine.Ships.ToList());
    }

    [Test]
    public void AddShip_WithParameters_AddsShipToList()
    {
        var engine = new GameEngine();
        engine.AddShip(ShipType.Battleship, 2, 3, Orientation.Horizontal);

        var ship = engine.Ships.First();
        Assert.AreEqual(ShipType.Battleship, ship.ShipType);
        Assert.AreEqual(2, ship.X);
        Assert.AreEqual(3, ship.Y);
        Assert.AreEqual(5, ship.Length);
        Assert.AreEqual(Orientation.Horizontal, ship.Orientation);
    }

    [Test]
    public void ValidateMove_ReturnsFalseForInvalidMove()
    {
        var engine = new GameEngine();
        engine.Settings.Width = 10;
        engine.Settings.Height = 5;

        Assert.IsFalse(engine.ValidateMove(null));
        Assert.IsFalse(engine.ValidateMove(""));
        Assert.IsFalse(engine.ValidateMove("A"));
        Assert.IsFalse(engine.ValidateMove("AA"));
        Assert.IsFalse(engine.ValidateMove("A6"));
        Assert.IsFalse(engine.ValidateMove("K1"));
    }

    [Test]
    public void ValidateMove_ReturnsTrueForValidMove()
    {
        var engine = new GameEngine();
        engine.Settings.Width = 10;
        engine.Settings.Height = 5;

        Assert.IsTrue(engine.ValidateMove("A1"));
        Assert.IsTrue(engine.ValidateMove("J5"));
    }

    [Test]
    public void MakeMove_ReturnsCorrectResult()
    {
        var engine = new GameEngine();
        engine.Settings.Width = 10;
        engine.Settings.Height = 5;
        engine.Init();
        engine.AddShip(ShipType.Battleship, 2, 3, Orientation.Horizontal);

        Assert.AreEqual(3, engine.MakeMove('A', '1')); // Miss
        Assert.AreEqual(2, engine.MakeMove('C', '3')); // Hit
        Assert.AreEqual(1, engine.MakeMove('C', '3')); // Duplicate Move
        Assert.AreEqual(2, engine.MakeMove('D', '3')); // Hit
        Assert.AreEqual(2, engine.MakeMove('E', '3')); // Hit
        Assert.AreEqual(2, engine.MakeMove('F', '3')); // Hit
        Assert.AreEqual(12, engine.MakeMove('G', '3')); // Destroyed Battleship
    }

    [Test]
    public void ResultToString_ReturnsCorrectString()
    {
        var engine = new GameEngine();

        Assert.AreEqual("Duplicate Move", engine.ResultToString(1));
        Assert.AreEqual("Hit", engine.ResultToString(2));
        Assert.AreEqual("Miss", engine.ResultToString(3));
        Assert.AreEqual("Destroyed a Carrier", engine.ResultToString(11));
        Assert.AreEqual("Destroyed a Battleship", engine.ResultToString(12));
        Assert.AreEqual("Destroyed a Cruiser", engine.ResultToString(13));
        Assert.AreEqual("Destroyed a Submarine", engine.ResultToString(14));
        Assert.AreEqual("Destroyed a Destroyer", engine.ResultToString(15));
    }
}