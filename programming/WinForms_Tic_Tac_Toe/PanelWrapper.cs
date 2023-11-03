
namespace WinFormsApp1;

internal class PanelWrapper
{
    readonly Panel box;

    readonly string bgAlign;
    readonly Image bgResource;
    readonly Dictionary<string, Image> backgr = new();

    readonly Dictionary<string, Color?> bgColor;

    readonly Dictionary<string, EventHandler> evtDetail = new();

    public PanelWrapper(
        Panel _box,
        Image _bgResource,
        string _bgAlign,
        Dictionary<string, Color?> _bgColor)
    {
        box = _box;
        bgResource = _bgResource;
        bgAlign = _bgAlign;
        bgColor = _bgColor;

        CreateBgSet();
        CreateEventHandlers();

        AddEventHandlers();

        evtDetail["Default"](this, new EventArgs()); // set default state
    }

    void CreateBgSet()
    {
        Image offsetImage =
            ImageUtility.GetOverlayOnBackground(box.Size, bgResource, bgAlign);

        backgr.Add("MouseEnter", offsetImage);
        backgr.Add("Default", ImageUtility.GetImageCopyWithAlpha(offsetImage, 0.5f));
        backgr.Add("MouseLeave", backgr["Default"]);

    }

    EventHandler CreateEventHandler(string evtName)
    {
        return (object? sender, EventArgs e) =>
        {
            if (!backgr.TryGetValue(evtName, out Image? image)) return;
            if (box.BackgroundImage != image) box.BackgroundImage = image;
            if (!bgColor.TryGetValue(evtName, out Color? color)) return;
            box.BackColor = color ?? Color.Transparent;
        };
    }

    void CreateEventHandlers()
    {
        foreach (var evtName in new string[] { "Default", "MouseEnter", "MouseLeave" })
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

    void AddEventHandlers()
    {
        box.MouseEnter += evtDetail["MouseEnter"];
        box.MouseLeave += evtDetail["MouseLeave"];
    }

}
