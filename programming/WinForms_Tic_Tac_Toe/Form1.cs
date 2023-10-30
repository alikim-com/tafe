using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    int cWidth, cHeight;
    double ratio;
    Size nonClient;

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cWidth = ClientSize.Width;
        cHeight = ClientSize.Height;
        ratio = (double)cWidth / cHeight;
        nonClient = Size - ClientSize;
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
            rc2.Width -= nonClient.Width;
            rc2.Height -= nonClient.Height;

            switch ((WMSZ)m.WParam.ToInt32())
            {
                case WMSZ.LEFT:
                case WMSZ.RIGHT:
                case WMSZ.TOPLEFT:
                case WMSZ.TOPRIGHT:
                case WMSZ.BOTTOMLEFT:
                case WMSZ.BOTTOMRIGHT:
                    // width has changed, adjust height
                    rc.Bottom = rc.Top + (int)(rc2.Width / ratio) + nonClient.Height;
                    break;
                case WMSZ.TOP:
                case WMSZ.BOTTOM:
                    // height has changed, adjust width
                    rc.Right = rc.Left + (int)(rc2.Height * ratio) + nonClient.Width;
                    break;
            }
            Marshal.StructureToPtr(rc, m.LParam, true);
        }
        base.WndProc(ref m);
    }

    private void tpl_SizeChanged(object sender, EventArgs e)
    {
        var width = tpl.Size.Width;
        tpl.Size = new Size(width, width);
    }
}