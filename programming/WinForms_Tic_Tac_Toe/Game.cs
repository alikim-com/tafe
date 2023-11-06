using System.ComponentModel;

namespace WinFormsApp1;

public class LabelTexts
{
    public string Choice { get; set; } = "";
}

public class Game : INotifyPropertyChanged
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

    string _choice = "";
    public string Choice
    {
        get => _choice;
        set
        {
            _choice = value;
            OnPropertyChanged(nameof(Choice));
        }
    }

    public State CurState
    {
        set => SetLabels(value);
    }

    public Game() => CurState = State.Start;

    void SetLabels(State state)
    {
        Choice = stateInfo[state].Choice;
        OnPropertyChanged(nameof(Choice));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string property)
    {
        var handler = PropertyChanged; // multi-thread safety
        handler?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
