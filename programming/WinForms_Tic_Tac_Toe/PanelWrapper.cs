
namespace WinFormsApp1;

/// <summary>
/// Controls bottom panels backgrounds and associated mouse events<br/>
/// Player visualisation choice.
/// </summary>
internal class PanelWrapper : IComponent
{
    readonly Panel box;
    readonly Control[] extra;

    readonly string hAlign;
    readonly string vAlign;
    readonly Image bgResource;
    readonly Dictionary<BgMode, Image> backgr = new();

    readonly Dictionary<BgMode, Color?> bgColor;

    readonly Dictionary<BgMode, EventHandler> evtDetail = new();

    readonly CellWrapper.BgMode playerBg;

    public enum BgMode
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
        Dictionary<BgMode, Color?> _bgColor,
        Control[] _extra,
        CellWrapper.BgMode _playerBg)
    {
        box = _box;
        bgResource = _bgResource;
        hAlign = _hAlign;
        vAlign = _vAlign;
        bgColor = _bgColor;
        extra = _extra;
        playerBg = _playerBg;

        CreateBgSet();
        CreateEventHandlers();

    }

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public void ResetHandler(object? s, EventArgs e)
    {
        SetBgMode(BgMode.Default);
    }

    void CreateBgSet()
    {
        Image bgResourceDef = bgResource.GetImageCopyWithAlpha(0.66f);

        Image bgDef = bgResourceDef.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5), 
                (int)(box.Size.Height * 1.5)),
            bgColor[BgMode.Default],
            hAlign,
            vAlign
        );

        Image bgEnter = bgResource.GetOverlayOnBackground(
            new Size( // to improve quality in bigger app window
                (int)(box.Size.Width * 1.5),
                (int)(box.Size.Height * 1.5)),
            bgColor[BgMode.MouseEnter],
            hAlign,
            vAlign
        );

        backgr.Add(BgMode.MouseEnter, bgEnter);
        backgr.Add(BgMode.Default, bgDef);
        backgr.Add(BgMode.MouseLeave, bgDef);

    }

    EventHandler CreateEventHandler(BgMode evtName)
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
        foreach (BgMode evtName in Enum.GetValues(typeof(BgMode)))
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

    public void AddHoverEventHandlers()
    {
        box.MouseEnter += evtDetail[BgMode.MouseEnter];
        box.MouseLeave += evtDetail[BgMode.MouseLeave];
    }

    public void RemoveHoverEventHandlers()
    {
        box.MouseEnter -= evtDetail[BgMode.MouseEnter];
        box.MouseLeave -= evtDetail[BgMode.MouseLeave];
    }

    public void Enable()
    {
        RemoveHoverEventHandlers();
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

    void OnClick(object? sender, EventArgs e)
    {
        Disable();
        SetBgMode(BgMode.MouseEnter);

        // associate bg with the current player & turn the wheel
        EM.RaiseEvtPlayerConfigured(this, playerBg);
    }

    void SetBgMode(BgMode mode) => evtDetail[mode](this, new EventArgs());
}
