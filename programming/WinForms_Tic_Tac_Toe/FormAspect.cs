using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class FormAspect : Form
{
    double ratio;
    Size marginSize;

    public FormAspect()
    {
        DoubleBuffered = true;
        InitializeComponent();
    }

    void FormAspect_Load(object sender, EventArgs e)
    {
        marginSize = Size - ClientSize;
        ratio = (double)Size.Width / Size.Height;

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
        }
        base.WndProc(ref m);
    }

    bool tpl_lock = false;
    void tpl_SizeChanged(object sender, EventArgs e)
    {
        if (tpl_lock) return;
        tpl_lock = true;
        var width = tpl.Size.Width;
        tpl.Size = new Size(width, width);
        tpl_lock = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        tbox.Text += "clicked ";
    }
}