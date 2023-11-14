
namespace WinFormsApp1;

internal class AI
{
    public enum Logic
    {
        Config,
        Board
    }

    static readonly Dictionary<Logic, Action<int>> action = new()
    {
        { Logic.Config, MakeConfigMove },
        { Logic.Board, MakeBoardMove }
    };

    /// <summary>
    /// Called from TurnWheel to choose a UI element
    /// </summary>
    public static void MakeMove(Logic L, int count) => action[L](count);

    /// <summary>
    /// Choose a config panel
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public static void MakeConfigMove(int count)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(1000);

            Random random = new();

            EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, random.Next(count)));
            
        });

        thread.Start();
    }

    /// <summary>
    /// Choose a board cell
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public static void MakeBoardMove(int count)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(1000);

            Random random = new();

            EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, random.Next(count)));

        });

        thread.Start();
    }
}
