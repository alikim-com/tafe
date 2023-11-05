
namespace WinFormsApp1;

internal class PanelWrapper
{
    readonly Panel box;
    readonly Control[] extra;

    readonly string bgAlign;
    readonly Image bgResource;
    readonly Dictionary<string, Image> backgr = new();

    readonly Dictionary<string, Color?> bgColor;

    readonly Dictionary<string, EventHandler> evtDetail = new();

    public PanelWrapper(
        Panel _box,
        Image _bgResource,
        string _bgAlign,
        Dictionary<string, Color?> _bgColor,
        Control[] _extra)
    {
        box = _box;
        bgResource = _bgResource;
        bgAlign = _bgAlign;
        bgColor = _bgColor;
        extra = _extra;

        CreateBgSet();
        CreateEventHandlers();

        AddEventHandlers();

        evtDetail["Default"](this, new EventArgs()); // set default state
    }

    void CreateBgSet()
    {
        Image bgResourceDef = ImageUtility.GetImageCopyWithAlpha(bgResource, 0.66f);

        Image bgDef = ImageUtility.GetOverlayOnBackground(
            box.Size * 2, // to improve quality in bigger app window
            bgResourceDef,
            bgColor["Default"], 
            bgAlign
        );

        Image bgEnter = ImageUtility.GetOverlayOnBackground(
            box.Size * 2,
            bgResource,
            bgColor["MouseEnter"],
            bgAlign
        );

        backgr.Add("MouseEnter", bgEnter);
        backgr.Add("Default", bgDef);
        backgr.Add("MouseLeave", bgDef);

    }

    EventHandler CreateEventHandler(string evtName)
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
        foreach (var evtName in new string[] { "Default", "MouseEnter", "MouseLeave" })
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

    public void AddEventHandlers()
    {
        box.MouseEnter += evtDetail["MouseEnter"];
        box.MouseLeave += evtDetail["MouseLeave"];
        box.Cursor = Cursors.Hand;
    }

    public void RemoveEventHandlers()
    {
        box.MouseEnter -= evtDetail["MouseEnter"];
        box.MouseLeave -= evtDetail["MouseLeave"];
        box.Cursor = Cursors.Default;
    }

}
