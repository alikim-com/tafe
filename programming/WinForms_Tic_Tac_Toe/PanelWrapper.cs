
namespace WinFormsApp1;

internal class PanelWrapper
{
    readonly Panel box;

    readonly Size bgOffset;
    readonly Image bgResource;
    readonly Dictionary<string, Image> backgr = new();

    readonly Dictionary<string, Color?> bgColor;

    readonly Dictionary<string, EventHandler> evtDetail = new();

    public PanelWrapper(
        Panel _box,
        Image _bgResource,
        Size _bgOffset,
        Dictionary<string, Color?> _bgColor)
    {
        box = _box;
        bgResource = _bgResource;
        bgOffset = _bgOffset;
        bgColor = _bgColor;

        CreateBgSet();
        CreateEventHandlers();

        // evtDetail["Default"](this, new EventArgs()); // set default state
    }

    void CreateBgSet()
    {
        Image offsetImage =
            ImageUtility.GetOverlayOnBackground(box.Size, bgResource, bgOffset);

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
        foreach(var evtName in new string[] { "Default", "MouseEnter", "MouseLeave" })
            evtDetail.Add(evtName, CreateEventHandler(evtName));
    }

//        box.MouseEnter += AssertDetail("MouseEnter");
//        box.MouseLeave += AssertDetail("MouseLeave");

}
