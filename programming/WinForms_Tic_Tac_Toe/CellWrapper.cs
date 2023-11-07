
namespace WinFormsApp1;

internal class CellWrapper
{
    readonly Panel box;
    readonly Dictionary<State, Image> backgr = new();
    readonly Dictionary<State, EventHandler> evtDetail = new();

    enum State
    {
        // star
        Default,
        MouseEnter,
        MouseLeave,
        // token
        PlayerLeft,
        PlayerRight,
        // grey token
        LostLeft,
        LostRight
    }

    public CellWrapper(Panel _box)
    {
        box = _box;
        CreateBgSet();
        CreateEventHandlers();

        AddHoverEventHandlers();

        evtDetail[State.Default](this, new EventArgs()); // set default state
    }

    void CreateBgSet()
    {
        Image bgHover = Resource.Star.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "top"
        );
        Image bgDef = bgHover.GetImageCopyWithAlpha(0.70f);

        backgr.Add(State.Default, Resource.TokenLeft);// bgDef);
        backgr.Add(State.MouseEnter, bgHover);
        backgr.Add(State.MouseLeave, bgDef);

        backgr.Add(State.PlayerLeft, Resource.TokenLeft);
        backgr.Add(State.PlayerRight, Resource.TokenRight);

        backgr.Add(State.LostLeft, Resource.TokenLeft.Desaturate("PS"));
        backgr.Add(State.LostRight, Resource.TokenRight.Desaturate("PS"));

    }

    EventHandler CreateEventHandler(State evtName)
    {
        return (object? sender, EventArgs e) =>
        {
            if (!backgr.TryGetValue(evtName, out Image? image)) return;
            if (box.BackgroundImage != image) box.BackgroundImage = image;
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
