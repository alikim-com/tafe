
namespace WinFormsApp1;

internal class CellWrapper
{
    readonly Panel box;
    readonly Dictionary<BgMode, Image> backgr = new();
    readonly Dictionary<BgMode, EventHandler> evtDetail = new();

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

    public CellWrapper(Panel _box)
    {
        box = _box;
        CreateBgSet();
        CreateEventHandlers();

        SetBgMode(BgMode.Default);
    }

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
            if (box.BackgroundImage != image) box.BackgroundImage = image;
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

        box.Cursor = Cursors.Hand;
    }

    public void RemoveHoverEventHandlers()
    {
        box.MouseEnter -= evtDetail[BgMode.MouseEnter];
        box.MouseLeave -= evtDetail[BgMode.MouseLeave];

        box.Cursor = Cursors.Default;
    }

    public void SetBgMode(BgMode mode) => evtDetail[mode](this, new EventArgs());

    public EventHandler? OnClick;
}
