
namespace WinFormsApp1;

internal class AI
{
    /// <summary>
    /// Called from TurnWheel to choose a config panel
    /// </summary>
    /// <param name="count"></param>
    public static void MakeConfigMove(int count)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(1000);

            Random random = new();

            EM.RaiseEvtAIConfigMoved(new { }, random.Next(count));
            
        });

        thread.Start();
    }

    public static void MakePlayMove()
    {

    }
}
