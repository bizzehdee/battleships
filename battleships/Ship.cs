namespace battleships;

//this could be an abstract class that defines the base behaviour of a ship
//but right now, its a concrete class that is used by multiple ship types
public class Ship
{
    public ShipType ShipType { get; set; }
    public int Length { get; set; }
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
    public int X { get; set; }
    public int Y { get; set; }
    
    private IDictionary<int, bool> hits = new Dictionary<int, bool>();

    //a better implementation of this would be to track the position, so that additional UI could be added to show the hits
    public int AddHit(int hitPosition)
    {
        if (!hits.ContainsKey(hitPosition))
        {
            hits.Add(hitPosition, true);
        }
        
        return IsDestroyed() ? 1 : 0;
    }

    //quite easy in this iteration, just count the number of hits, all the validation is done in the game grid
    public bool IsDestroyed()
    {
        if (hits.Count != Length)
        {
            return false;
        }

        return true;
    }
}