using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class FormAspect : Form
{
    double ratio; // main window
    Size marginSize; // non-client area

    double pRatio; // ratio for the bottom panel

    public static readonly Dictionary<string, Image> ResourceEx = new()
    {
        { "FaceLeftDefault", ImageUtility.GetImageCopyWithAlpha(Resource.FaceLeft, 0.5f) },
        { "FaceRightDefault", ImageUtility.GetImageCopyWithAlpha(Resource.FaceRight, 0.5f) },
    };

    public FormAspect()
    {
        // to extend the behaviour of sub components
        ControlAdded += FormAspect_ControlAdded;

        // prevent main window flickering
        DoubleBuffered = true;

        InitializeComponent();

        // prevent backgound flickering for components
        typeof(TableLayoutPanel).InvokeMember(
            "DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, tpl, new object[] { true }
        );
        typeof(Panel).InvokeMember(
            "DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, panel, new object[] { true }
        );
    }

    void FormAspect_ControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control == panel)
        {
            AddControlBackgroundEvents(
                pLeft,
                new Dictionary<string, Image?>() {
                    { "Default", ResourceEx["FaceLeftDefault"] },
                    { "MouseHover", Resource.FaceLeft },
                    { "MouseLeave", ResourceEx["FaceLeftDefault"] },
                }
            );
            AddControlBackgroundEvents(
                pRight,
                new Dictionary<string, Image?>() {
                    { "Default", ResourceEx["FaceRightDefault"] },
                    { "MouseHover", Resource.FaceRight },
                    { "MouseLeave", ResourceEx["FaceRightDefault"] },
                }
            );
        }
    }

    void FormAspect_Load(object sender, EventArgs e)
    {
        marginSize = Size - ClientSize;
        ratio = (double)Size.Width / Size.Height;

        pRatio = (double)panel.Width / panel.Height;
    }

    static void AddControlBackgroundEvents(
        Control control,
        Dictionary<string, Image?> images)
    {
        if (
            control is PictureBox box &&
            images.TryGetValue("Default", out Image? image))
        {
            box.Image = image;
        }

        control.MouseHover += (object? sender, EventArgs e) =>
        {
            if (!images.TryGetValue("MouseHover", out Image? image)) return;
            if (control.BackgroundImage != image) control.BackgroundImage = image;
        };

        control.MouseLeave += (object? sender, EventArgs e) =>
        {
            if (!images.TryGetValue("MouseLeave", out Image? image)) return;
            if (control.BackgroundImage != image) control.BackgroundImage = image;
        };
    }

    // ----------------------------------------------------------------------

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    private const int WM_SIZING = 0x214;
    public enum WMSZ
    {
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        TOPLEFT = 4,
        TOPRIGHT = 5,
        BOTTOM = 6,
        BOTTOMLEFT = 7,
        BOTTOMRIGHT = 8
    }

    /// <summary>
    /// Implementation of a constant client aspect ratio
    /// </summary>
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_SIZING)
        {
            var rc = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT))!;
            var rc2 = Rectangle.FromLTRB(rc.Left, rc.Top, rc.Right, rc.Bottom);
            rc2.Width -= marginSize.Width;
            rc2.Height -= marginSize.Height;

            switch ((WMSZ)m.WParam.ToInt32())
            {
                case WMSZ.LEFT:
                case WMSZ.RIGHT:
                case WMSZ.TOPLEFT:
                case WMSZ.TOPRIGHT:
                case WMSZ.BOTTOMLEFT:
                case WMSZ.BOTTOMRIGHT:
                    // width has changed, adjust height
                    rc.Bottom = rc.Top + (int)(rc2.Width / ratio) + marginSize.Height;
                    break;
                case WMSZ.TOP:
                case WMSZ.BOTTOM:
                    // height has changed, adjust width
                    rc.Right = rc.Left + (int)(rc2.Height * ratio) + marginSize.Width;
                    break;
            }
            Marshal.StructureToPtr(rc, m.LParam, true);

            // adjust components

            var width = tpl.Size.Width;
            tpl.Size = new Size(width, width);

            panel.Height = (int)(panel.Width / pRatio);

        }

        base.WndProc(ref m);
    }
}