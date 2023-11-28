
namespace WinFormsApp1;

/// <summary>
/// Controls board cells backgrounds and associated mouse events
/// </summary>
internal class CellWrapper : IComponent
{
    public string Name { get => box.Name; } // interface

    readonly Panel box;
    readonly Point rc;
    readonly Dictionary<BgMode, Image> backgr = new();
    readonly Dictionary<BgMode, EventHandler> evtDetail = new();

    /// <summary>
    /// Event driven readable state
    /// </summary>
    BgMode _curBgMode;
    public BgMode CurBgMode
    {
        get => _curBgMode;
        private set => _curBgMode = value;
    }

    public enum BgMode
    {
        // star
        Default,
        MouseEnter,
        MouseLeave,
        // tokens
        Player1,
        Player2,
        // grey tokens
        Lost1,
        Lost2
    }

    public CellWrapper(Panel _box, int _row, int _col)
    {
        box = _box;
        rc = new Point(_row, _col);

        SyncBoardUIHandler = (object? _, Dictionary<Point, BgMode> e) =>
        {
            if (!e.TryGetValue(rc, out BgMode val)) return;
            SetBg(val);
        };

        CreateBgSet();
        CreateEventHandlers();
    }

    /// <summary>
    /// Subscribed EM.EvtSyncBoardUI event
    /// </summary>
    public EventHandler<Dictionary<Point, BgMode>> SyncBoardUIHandler;

    void SetBg(BgMode mode) => evtDetail[mode](this, new EventArgs());

    void CreateBgSet()
    {
        Image bgHover = Resource.Star.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "center"
        );
        Image bgDef = bgHover.GetImageCopyWithAlpha(0.70f);

        backgr.Add(BgMode.Default, bgDef);
        backgr.Add(BgMode.MouseEnter, bgHover);
        backgr.Add(BgMode.MouseLeave, bgDef);

        Image token1 = Resource.TokenLeft.GetOverlayOnBackground(
            new Size(
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "center"
        );

        Image token2= Resource.TokenRight.GetOverlayOnBackground(
            new Size(
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "center"
        );

        backgr.Add(BgMode.Player1, token1);
        backgr.Add(BgMode.Player2, token2);

        backgr.Add(BgMode.Lost1, token1.Desaturate("PS"));
        backgr.Add(BgMode.Lost2, token2.Desaturate("PS"));

    }

    EventHandler CreateEventHandler(BgMode evtName)
    {
        return (object? sender, EventArgs e) =>
        {
            if (!backgr.TryGetValue(evtName, out Image? image)) return;
            if (box.BackgroundImage != image)
            {
                box.BackgroundImage = image;
                CurBgMode = evtName;
            }
        };
    }

    void CreateEventHandlers()
    {
        foreach (BgMode evtName in Enum.GetValues(typeof(BgMode)))
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

    void AddHoverEventHandlers()
    {
        box.MouseEnter += evtDetail[BgMode.MouseEnter];
        box.MouseLeave += evtDetail[BgMode.MouseLeave];
    }

    void RemoveHoverEventHandlers()
    {
        box.MouseEnter -= evtDetail[BgMode.MouseEnter];
        box.MouseLeave -= evtDetail[BgMode.MouseLeave];
    }

    public void Enable()
    {
        AddHoverEventHandlers();
        box.Click += OnClick;
        box.Cursor = Cursors.Hand;
    }

    public void Disable()
    {
        box.Click -= OnClick;
        RemoveHoverEventHandlers();
        box.Cursor = Cursors.Default;
    }

    /// <summary>
    /// Raises EM.EvtPlayerMoved event
    /// </summary>
    void OnClick(object? _, EventArgs __)
    {
        EM.Raise(EM.Evt.PlayerMoved, this, rc);
    }

    /// <summary>
    /// AI mouse clicks
    /// </summary>
    public void SimulateOnClick() => OnClick(null, new EventArgs());
}

