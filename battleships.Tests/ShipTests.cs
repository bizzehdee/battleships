namespace battleships.Tests;

public class ShipTests
{
    [Test]
    public void AddHit_AddsHitToShip()
    {
        var ship = new Ship { Length = 3 };
        var result = ship.AddHit(1);

        Assert.AreEqual(0, result);
        Assert.IsFalse(ship.IsDestroyed());
    }

    [Test]
    public void AddHit_DoesNotAddDuplicateHit()
    {
        var ship = new Ship { Length = 3 };
        ship.AddHit(1);
        var result = ship.AddHit(1);

        Assert.AreEqual(0, result);
        Assert.IsFalse(ship.IsDestroyed());
    }

    [Test]
    public void IsDestroyed_ReturnsTrueWhenAllPositionsHit()
    {
        var ship = new Ship { Length = 3 };
        ship.AddHit(0);
        ship.AddHit(1);
        ship.AddHit(2);

        Assert.IsTrue(ship.IsDestroyed());
    }

    [Test]
    public void IsDestroyed_ReturnsFalseWhenNotAllPositionsHit()
    {
        var ship = new Ship { Length = 3 };
        ship.AddHit(0);
        ship.AddHit(1);

        Assert.IsFalse(ship.IsDestroyed());
    }

    [Test]
    public void AddHit_ReturnsOneWhenShipIsDestroyed()
    {
        var ship = new Ship { Length = 2 };
        ship.AddHit(0);
        var result = ship.AddHit(1);

        Assert.AreEqual(1, result);
        Assert.IsTrue(ship.IsDestroyed());
    }
}