﻿
using System.ComponentModel;

namespace WinFormsApp1;

/// <summary>
/// Updates UI labels based on subscribed events
/// </summary>
class LabelManager : INotifyPropertyChanged
{
    /// <summary>
    /// Middle & bottom info panel states
    /// </summary>
    internal enum Info
    {
        None,
        //
        Player1,
        Player2,
        Player1Move,
        Player2Move,
        Player1Won,
        Player2Won,
    }
    /// <summary>
    /// Pre-game countdown info panel states
    /// </summary>
    internal enum Countdown
    {
        Three,
        Two,
        One
    }
    /// <summary>
    /// Messages from AI to append to the current info
    /// </summary>
    internal enum AIMsg
    {
        Attack,
        Defend,
        Random,
    }

    // <----------------how to add extra messages from Ai

    static readonly Dictionary<Enum, string> stateToString = new()
    {
        { Info.None, "" },
        // the rest is filled by VBridge.Reset()

        { AIMsg.Attack, " (attacking)"},
        { AIMsg.Defend, " (defending)"},
        { AIMsg.Random, " (random choice)"},
        
        { Countdown.Three, "Game starts in 3..." },
        { Countdown.Two, "Game starts in 2..." },
        { Countdown.One, "Game starts in 1..." },
    };

    /// <summary>
    /// Subscribed to EM.EvtUpdateLabels event
    /// </summary>
    /// <param name="e">An array of states to set for each panel</param>
    static internal readonly EventHandler<Enum[]> UpdateLabelsHandler = (object? _, Enum[] e) =>
    {
        foreach (Enum state in e) SetLabel(state);
    };

    /// <summary>
    /// Called from VBridge.Reset
    /// defines Info labels when the game starts
    /// </summary>
    static internal void Reset(Dictionary<Info, string> playerInfo)
    {
        foreach (var (enm, msg) in playerInfo)
        {
            if (!stateToString.ContainsKey(enm))
                stateToString.Add(enm, msg);
            else
                stateToString[enm] = msg;
        }
    }

    static void SetLabel(Enum state)
    {
        if (_this == null) return;
        switch (state)
        {
            case AIMsg:
                _this.InfoPanelBind += stateToString[state];
                RaiseEvtPropertyChanged(nameof(InfoPanelBind));
                break;
            case Info.Player1:
                _this.LabelLeftBind = stateToString[state];
                RaiseEvtPropertyChanged(nameof(LabelLeftBind));
                break;
            case Info.Player2:
                _this.LabelRightBind = stateToString[state];
                RaiseEvtPropertyChanged(nameof(LabelRightBind));
                break;
            case Info:
            case Countdown:
                _this.InfoPanelBind = stateToString[state];
                RaiseEvtPropertyChanged(nameof(InfoPanelBind));
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
    public string LabelLeftBind { get; set; } = "";
    public string LabelRightBind { get; set; } = "";
    public string InfoPanelBind { get; set; } = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    internal LabelManager()
    {
        _this = this;
    }
}
