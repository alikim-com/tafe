
using System.ComponentModel;

namespace WinFormsApp1;

/// <summary>
/// 
/// </summary>
internal class LabelManager : INotifyPropertyChanged
{
    /// <summary>
    /// Event driven readable state
    /// </summary>
    string _curState = "";
    public string CurState
    {
        get => _curState;
        private set => _curState = value;
    }

    public enum State
    {
        Start,
        PlayerLeft,
        PlayerRight
    }

    static public readonly Dictionary<State, string> stateInfo = new()
    {
        { State.Start, "CHOOSE\nYOUR\nSIDE" },
        { State.PlayerLeft, "YOU  -vs-  AI   " },
        { State.PlayerRight, "   AI  -vs-  YOU" },
    };

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public void ResetHandler(object? s, EventArgs e)
    {
        SetLabels(State.Start);
    }

    void SetLabels(State state)
    {
        CurState = stateInfo[state];
        OnPropertyChanged(nameof(CurState));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string property)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
