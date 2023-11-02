using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class FormAspect : Form
{
    double ratio; // main window
    Size marginSize; // non-client area

    double pRatio; // ratio for the bottom panel

    struct SizeD
    {
        public double Width;
        public double Height;
    }

    readonly int labelChoiceWidth;
    readonly float labelChoiceFontSize;
    SizeD labelChoiceRatio; // size of the label as a fraction of the parent size


    public static Dictionary<string, Image> ResourceEx = new(); // graphics assets

    public FormAspect()
    {
        static void ApplyDoubleBuffer(Control control)
        {
            control.GetType().InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true }
            );
        }


        // to extend the behaviour of sub components
        ControlAdded += FormAspect_ControlAdded;

        // prevent main window flickering
        DoubleBuffered = true;

        InitializeComponent();

        // prevent backgound flickering for components
        ApplyDoubleBuffer(panel);
        ApplyDoubleBuffer(tpl);
        ApplyDoubleBuffer(labelChoice);

        // init
        labelChoiceWidth = labelChoice.Width;
        labelChoiceFontSize = labelChoice.Font.Size;
        labelChoiceRatio = new SizeD
        {
            Width = (double)labelChoice.Width / panel.Width,
            Height = (double)labelChoice.Height / panel.Height,
        };
    }

    void FormAspect_ControlAdded(object? sender, ControlEventArgs e)
    {
        static void CreateExtendedImage(Image src, Control control, Size offset, string name)
        {
            Image offsetImage =
                ImageUtility.GetOverlayOnBackground(control.Size, src, offset);

            ResourceEx.Add(name, offsetImage);
            ResourceEx.Add(name + "Default", ImageUtility.GetImageCopyWithAlpha(offsetImage, 0.5f));
        }

        if (e.Control == panel)
        {
            // create left & right panel extended images
            Size offsetLeft = new(0, 0);
            Size offsetRight = pRight.Size - Resource.FaceRight.Size;
            CreateExtendedImage(Resource.FaceLeft, pLeft, offsetLeft, "FaceLeft");
            CreateExtendedImage(Resource.FaceRight, pRight, offsetRight, "FaceRight");

            AddControlBackgroundEvents(
                pLeft,
                new Dictionary<string, Image?>() {
                    { "Default", ResourceEx["FaceLeftDefault"] },
                    { "MouseEnter", ResourceEx["FaceLeft"] },
                    { "MouseLeave", ResourceEx["FaceLeftDefault"] },
                },
                new Dictionary<string, Color?>() {
                    { "Default", Color.Transparent },
                    { "MouseEnter", Color.FromArgb(12, 200, 104, 34) },
                    { "MouseLeave", Color.Transparent },
                }
            );
            AddControlBackgroundEvents(
                pRight,
                new Dictionary<string, Image?>() {
                    { "Default", ResourceEx["FaceRightDefault"] },
                    { "MouseEnter", ResourceEx["FaceRight"] },
                    { "MouseLeave", ResourceEx["FaceRightDefault"] },
                },
                new Dictionary<string, Color?>() {
                    { "Default", Color.Transparent },
                    { "MouseEnter", Color.FromArgb(16, 185, 36, 199) },
                    { "MouseLeave", Color.Transparent },
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
        Dictionary<string, Image?> images,
        Dictionary<string, Color?> bgColors)
    {
        if (control is not PictureBox box) return;

        if (images.TryGetValue("Default", out Image? image))
            box.Image = image;

        if (!bgColors.TryGetValue("Default", out Color? bgColor)) return;
        box.BackColor = bgColor ?? Color.Transparent;

        box.MouseEnter += (object? sender, EventArgs e) =>
        {
            if (!images.TryGetValue("MouseEnter", out Image? image)) return;
            if (box.Image != image) box.Image = image;
            if (!bgColors.TryGetValue("MouseEnter", out Color? bgColor)) return;
            box.BackColor = bgColor ?? Color.Transparent;
        };

        box.MouseLeave += (object? sender, EventArgs e) =>
        {
            if (!images.TryGetValue("MouseLeave", out Image? image)) return;
            if (box.Image != image) box.Image = image;
            if (!bgColors.TryGetValue("MouseLeave", out Color? bgColor)) return;
            box.BackColor = bgColor ?? Color.Transparent;
        };
    }

    // ---------------   constant client aspect ratio   ---------------

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

            labelChoice.Size = new Size(
                (int)(panel.Width * labelChoiceRatio.Width),
                (int)(panel.Height * labelChoiceRatio.Height)
            );
            labelChoice.Location = new Point(
                (panel.Width - labelChoice.Width) / 2,
                (panel.Height - labelChoice.Height) / 2
            );
            float newFontSize = labelChoiceFontSize * labelChoice.Width / labelChoiceWidth;
            labelChoice.Font = new Font(labelChoice.Font.FontFamily, newFontSize);

        }

        base.WndProc(ref m);
    }

    private void pLeft_SizeChanged(object sender, EventArgs e)
    {
        pLeft.Width = panel.Width / 2;
    }

    private void pRight_SizeChanged(object sender, EventArgs e)
    {
        int hw = panel.Width / 2;
        pRight.Location = new Point(hw, pRight.Location.Y);
        pRight.Width = hw;
    }
}