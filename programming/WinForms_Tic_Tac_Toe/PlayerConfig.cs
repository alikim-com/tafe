
namespace WinFormsApp1;

internal class PlayerConfig
{
    int head;
    Game.Roster[] players;

    public PlayerConfig() 
    {
        players = (Game.Roster[])Enum.GetValues(typeof(Game.Roster));
        head = 0;
    }

    void AdvancePlayer()
    {
        if(head == players.Length) ConfigComplete();
        head++;

    }

    void ConfigComplete()
    {

    }
}
