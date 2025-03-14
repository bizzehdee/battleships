namespace battleships;

public class GameBoard
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private int[,] gameBoard;

    //a validated game board, prevent looking outside of bounds
    public int this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= Width)
            {
                return 0;
            }
            if (y < 0 || y >= Height)
            {
                return 0;
            }
            
            return gameBoard[x, y];
        }
    }
    
    public void Init(int width, int height)
    {
        Width = width;
        Height = height;
        
        gameBoard = new int[width, height];
    }

    public void MarkHit(int guessX, int guessY)
    {
        MarkBoard(true, guessX, guessY);
    }

    public void MarkMiss(int guessX, int guessY)
    {
        MarkBoard(false, guessX, guessY);
    }
    
    private void MarkBoard(bool isHit, int guessX, int guessY)
    {
        //if the guess is out of bounds, ignore it
        if (guessX < 0 || guessX >= Width)
        {
            return;
        }
        if (guessY < 0 || guessY >= Height)
        {
            return;
        }
        
        gameBoard[guessX, guessY] = isHit ? 1 : -1;
    }
}
