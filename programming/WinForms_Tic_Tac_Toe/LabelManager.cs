
using System.ComponentModel;

namespace WinFormsApp1;

/// <summary>
/// Updates UI labels based on subscribed events
/// </summary>
internal class LabelManager : INotifyPropertyChanged
{
    /// <summary>
    /// Middle info panel states
    /// </summary>
    public enum Info
    {
        None,
        HumanTurn,
        AITurn,
    }
    /// <summary>
    /// Pre-game countdown info panel states
    /// </summary>
    public enum Countdown
    {
        Three,
        Two,
        One
    }

    static public readonly Dictionary<Enum, string> labels = new()
    {
        { Info.None, "" },
        { Info.HumanTurn, "Your turn..." },
        { Info.AITurn, "AI is thinking..." },
        //
        { Countdown.Three, "Game starts in 3..." },
        { Countdown.Two, "Game starts in 2..." },
        { Countdown.One, "Game starts in 1..." },
    };

    /// <summary>
    /// Subscribed to EM.EvtUpdateLabels event
    /// </summary>
    /// <param name="e">An array of states to set for each panel</param>
    static public EventHandler<Enum[]> UpdateLabelsHandler = (object? _, Enum[] e) =>
    {
        foreach (Enum state in e) SetLabel(state);
    };

    static public void Reset()
    {
        SetLabel(Info.None);
    }

    static void SetLabel(Enum state)
    {
        if (_this == null) return;
        switch (state)
        {
            case Info:
                _this.InfoPanel = labels[state];
                RaiseEvtPropertyChanged(nameof(InfoPanel));
                break;
            case Countdown:
                _this.InfoPanel = labels[state];
                RaiseEvtPropertyChanged(nameof(InfoPanel));
                break;
            default:
                throw new NotImplementedException($"LabelManager.SetLabels : state '{state}'");
        }
    }


    /* 
     * A workaround to implement the data binding interface that requires
     * PropertyChanged event and binding properties to be instanced
     * 
     */

    static void RaiseEvtPropertyChanged(string property)
    {
        var handler = _this?.PropertyChanged;
        handler?.Invoke(_this, new PropertyChangedEventArgs(property));
    }

    static LabelManager? _this;

    /// <summary>
    /// Data bindings
    /// </summary>
    public string ChoicePanel { get; private set; } = "";
    public string InfoPanel { get; private set; } = "";

    public event PropertyChangedEventHandler? PropertyChanged;
    public LabelManager()
    {
        _this = this;
    }
}
