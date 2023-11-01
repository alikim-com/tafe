using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class FormAspect : Form
{
    double ratio; // main window
    Size marginSize; // non-client area

    double pRatio; // ratio for the bottom panel

    public FormAspect()
    {
        DoubleBuffered = true;
        InitializeComponent();
        typeof(TableLayoutPanel).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, tpl, new object[] { true });
        typeof(Panel).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, panel, new object[] { true });

    }

    void FormAspect_Load(object sender, EventArgs e)
    {
        marginSize = Size - ClientSize;
        ratio = (double)Size.Width / Size.Height;

        pRatio = panel.Width / panel.Height;

    }

    public static Image TransparentCopy(Image src, float opacity)
    {
        Bitmap dst = new(src.Width, src.Height);
        using (Graphics g = Graphics.FromImage(dst))
        {
            ColorMatrix matrix = new()
            {
                Matrix33 = opacity
            };
            ImageAttributes attributes = new();
            attributes.SetColorMatrix(
                matrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap
            );
            g.DrawImage(
                src,
                new Rectangle(0, 0, dst.Width, dst.Height),
                0, 0, src.Width, src.Height,
                GraphicsUnit.Pixel,
                attributes
            );
        }
        return dst;
    }

    static void ExpandPanel(Control control, Dictionary<string, Image?> images)
    {
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

    private void FormAspect_ControlAdded(object sender, ControlEventArgs e)
    {
        if (e.Control?.Name == "panel")
            ExpandPanel(
                e.Control,
                new Dictionary<string, Image?>() {
                { "Default", Resource.FaceLeft },
                { "MouseHover", Resource.FaceLeft },
                { "MouseLeave", null },
                }
            );
    }

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