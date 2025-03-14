namespace battleships.Tests;

public class GameBoardTests
{
    [Test]
    public void Init_SetsWidthAndHeight()
    {
        var board = new GameBoard();
        board.Init(10, 5);

        Assert.AreEqual(10, board.Width);
        Assert.AreEqual(5, board.Height);
    }

    [Test]
    public void Indexer_ReturnsCorrectValue()
    {
        var board = new GameBoard();
        board.Init(10, 5);
        board.MarkHit(2, 3);

        Assert.AreEqual(1, board[2, 3]);
    }

    [Test]
    public void MarkHit_SetsHitOnBoard()
    {
        var board = new GameBoard();
        board.Init(10, 5);
        board.MarkHit(2, 3);

        Assert.AreEqual(1, board[2, 3]);
    }

    [Test]
    public void MarkMiss_SetsMissOnBoard()
    {
        var board = new GameBoard();
        board.Init(10, 5);
        board.MarkMiss(2, 3);

        Assert.AreEqual(-1, board[2, 3]);
    }

    [Test]
    public void MarkHit_IgnoresOutOfBounds()
    {
        var board = new GameBoard();
        board.Init(10, 5);
        board.MarkHit(11, 3);

        Assert.AreEqual(0, board[11, 3]);
    }

    [Test]
    public void MarkMiss_IgnoresOutOfBounds()
    {
        var board = new GameBoard();
        board.Init(10, 5);
        board.MarkMiss(2, 6);

        Assert.AreEqual(0, board[2, 6]);
    }
}