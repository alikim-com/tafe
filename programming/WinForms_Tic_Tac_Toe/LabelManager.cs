
using System.ComponentModel;

namespace WinFormsApp1;

/// <summary>
/// 
/// </summary>
internal class LabelManager : INotifyPropertyChanged
{
    /// <summary>
    /// Data bindings
    /// </summary>
    public string ChoicePanel { get; private set; } = "";
    public string InfoPanel { get; private set; } = "";

    public enum Choice
    {
        None,
        HumanLeft,
        HumanRight
    }
    public enum Info
    {
        None,

    }

    static public readonly Dictionary<Enum, string> labels = new()
    {
        { Choice.None, "CHOOSE\nYOUR\nSIDE" },
        { Choice.HumanLeft, "YOU  -vs-  AI   " },
        { Choice.HumanRight, "   AI  -vs-  YOU" },
        //
        { Info.None, "" },
    };

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public void ResetHandler(object? s, EventArgs e)
    {
        SetLabels(Choice.None);
        SetLabels(Info.None);
    }

    void SetLabels(Enum state)
    {
        switch (state)
        {
            case Choice:
                ChoicePanel = labels[state];
                OnPropertyChanged(nameof(ChoicePanel));
                break;
            case Info:
                InfoPanel = labels[state];
                OnPropertyChanged(nameof(InfoPanel));
                break;
            default:
                throw new NotImplementedException($"LabelManager.SetLabels : state '{state}'");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string property)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
