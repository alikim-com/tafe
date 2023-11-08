﻿
namespace WinFormsApp1;

internal class PanelWrapper
{
    readonly Panel box;
    readonly Control[] extra;

    readonly string hAlign;
    readonly string vAlign;
    readonly Image bgResource;
    readonly Dictionary<State, Image> backgr = new();

    readonly Dictionary<State, Color?> bgColor;

    readonly Dictionary<State, EventHandler> evtDetail = new();

    public enum State
    {
        Default,
        MouseEnter,
        MouseLeave,
    }

    public PanelWrapper(
        Panel _box,
        Image _bgResource,
        string _hAlign,
        string _vAlign,
        Dictionary<State, Color?> _bgColor,
        Control[] _extra)
    {
        box = _box;
        bgResource = _bgResource;
        hAlign = _hAlign;
        vAlign = _vAlign;
        bgColor = _bgColor;
        extra = _extra;

        CreateBgSet();
        CreateEventHandlers();

        AddHoverEventHandlers();

        evtDetail[State.Default](this, new EventArgs()); // set default state
    }

    void CreateBgSet()
    {
        Image bgResourceDef = bgResource.GetImageCopyWithAlpha(0.66f);

        Image bgDef = bgResourceDef.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5), 
                (int)(box.Size.Height * 1.5)),
            bgColor[State.Default],
            hAlign,
            vAlign
        );

        Image bgEnter = bgResource.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            bgColor[State.MouseEnter],
            hAlign,
            vAlign
        );

        backgr.Add(State.MouseEnter, bgEnter);
        backgr.Add(State.Default, bgDef);
        backgr.Add(State.MouseLeave, bgDef);

    }

    EventHandler CreateEventHandler(State evtName)
    {
        return (object? sender, EventArgs e) =>
        {
            if (!backgr.TryGetValue(evtName, out Image? image)) return;
            if (box.BackgroundImage != image) box.BackgroundImage = image;
            if (!bgColor.TryGetValue(evtName, out Color? color)) return;
            foreach(var ex in extra) ex.BackColor = color ?? Color.Transparent;
        };
    }

    void CreateEventHandlers()
    {
        foreach (State evtName in Enum.GetValues(typeof(State)))
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

    public void AddHoverEventHandlers()
    {
        box.MouseEnter += evtDetail[State.MouseEnter];
        box.MouseLeave += evtDetail[State.MouseLeave];

        box.Cursor = Cursors.Hand;
    }

    public void RemoveHoverEventHandlers()
    {
        box.MouseEnter -= evtDetail[State.MouseEnter];
        box.MouseLeave -= evtDetail[State.MouseLeave];

        box.Cursor = Cursors.Default;
    }
}