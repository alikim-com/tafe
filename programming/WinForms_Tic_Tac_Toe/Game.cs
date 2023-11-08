
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

    static readonly Roster[] players = (Roster[])Enum.GetValues(typeof(Roster));

    Roster curPlayer = players[0];

    Roster[,] board = new Roster[3,3];

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public void ResetHandler(object? s, EventArgs e)
    {
        ResetBoard();
    }

    void ResetBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                board[i, j] = Roster.None;
    }

    void UpdateBoard(int row, int col)
    {
       // board[row, col] = CurTurn;
    }





}
