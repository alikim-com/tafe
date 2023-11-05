namespace WinFormsApp1;

public class LabelTexts
{
    public string Choice { get; set; } = "";
}

static public class Game
{
    public enum State
    {
        Start,
        PlayerLeft,
        PlayerRight
    }

    static public readonly Dictionary<State, LabelTexts> stateInfo = new()
    {
        { State.Start, new LabelTexts { Choice = "CHOOSE\nYOUR\nSIDE" } },
        { State.PlayerLeft, new LabelTexts { Choice = "YOU  -vs-  AI   " } },
        { State.PlayerRight, new LabelTexts { Choice = "   AI  -vs-  YOU" } },
    };

    static public LabelTexts boundData = new();

    static public State CurState
    {
        set => SetLabels(value);
    }

    static public void Init()
    {
        CurState = State.Start;
    }

    static void SetLabels(State state)
    {
        boundData.Choice = stateInfo[state].Choice;
    }

}
