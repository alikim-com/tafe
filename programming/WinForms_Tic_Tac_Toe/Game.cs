
namespace WinFormsApp1;

/// <summary>
/// Game engine, controls players, moves and the board
/// </summary>
internal class Game
{
    public enum Roster
    {
        None,
        Human,
        AI
    }

    static readonly Roster[] roster = (Roster[])Enum.GetValues(typeof(Roster));

    public Roster _curPlayer;
    public Roster CurPlayer
    {
        get => _curPlayer;
        private set => _curPlayer = value;
    }

    public Roster[,] board = new Roster[3, 3];

    public Game()
    {
        CurPlayer = Roster.None;
    }

    public void Reset()
    {
        CurPlayer = Roster.None;
        ResetBoard();

        board[1, 1] = Roster.Human;
        board[0, 2] = Roster.AI;

        // add all the board cells to the update
        var update = new Dictionary<Point, Roster>();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                update.Add(new Point(i, j), board[i, j]);

        // sync the board
        EM.RaiseEvtSyncBoard(this, update);
        
        // reset panels
        EM.RaiseEvtReset(this, new EventArgs());
    }

    void ResetBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                board[i, j] = Roster.None;
    }

    //void UpdateBoard(int row, int col)
    //{
    //    // board[row, col] = CurTurn;
    //}





}
