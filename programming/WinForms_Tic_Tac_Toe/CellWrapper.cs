
namespace WinFormsApp1;

/// <summary>
/// Controls board cells backgrounds and associated mouse events
/// </summary>
internal class CellWrapper
{
    readonly Panel box;
    readonly int row, col;
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
        // token
        PlayerLeft,
        PlayerRight,
        // grey token
        LostLeft,
        LostRight
    }

    public CellWrapper(Panel _box, int _row, int _col)
    {
        box = _box;
        row = _row;
        col = _col;
        CreateBgSet();
        CreateEventHandlers();
    }

    public void Settle(BgMode mode)
    {
        RemoveHoverEventHandlers();
        SetBg(mode);
    } 

    /// <summary>
    /// Subscribed EM.EvtReset event
    /// </summary>
    public void ResetHandler(object? s, EventArgs e)
    {
        RemoveHoverEventHandlers();
        AddHoverEventHandlers();
        SetBg(BgMode.Default);
    }

    /// <summary>
    /// Raises EM.EvtPlayerMoved event
    /// </summary>
    void OnClick(object? s, EventArgs e)
    {
        EM.RaiseEvtPlayerMoved(this, new Point(row, col));
    }

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

        Image tokenLeft = Resource.TokenLeft.GetOverlayOnBackground(
            new Size(
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "center"
        );

        Image tokenRight = Resource.TokenRight.GetOverlayOnBackground(
            new Size(
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            null,
            "center",
            "center"
        );

        backgr.Add(BgMode.PlayerLeft, tokenLeft);
        backgr.Add(BgMode.PlayerRight, tokenRight);

        backgr.Add(BgMode.LostLeft, tokenLeft.Desaturate("PS"));
        backgr.Add(BgMode.LostRight, tokenRight.Desaturate("PS"));

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

        box.Click += OnClick;
        box.Cursor = Cursors.Hand;
    }

    void RemoveHoverEventHandlers()
    {
        box.MouseEnter -= evtDetail[BgMode.MouseEnter];
        box.MouseLeave -= evtDetail[BgMode.MouseLeave];

        box.Click -= OnClick;
        box.Cursor = Cursors.Default;
    }
}
