using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class AppForm : Form
{
    double cRatio; // main window
    Size marginSize; // non-client area

    //double pRatio; // ratio for the bottom panel

    //struct SizeD
    //{
    //    public double Width;
    //    public double Height;
    //}

    //readonly int labelChoiceWidth;
    //readonly float labelChoiceFontSize;
    //SizeD labelChoiceRatio; // size of the label as a fraction of the parent size

    PanelWrapper? pwLeft;
    PanelWrapper? pwRight;

    public AppForm()
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
        Control[] dbuffed = 
            new Control[] { pLeft, pRight, tLayout, tSplit, sTL, sTR, sBL, sBR, choice };
        
        foreach(var ctrl in dbuffed) ApplyDoubleBuffer(ctrl);

        //// init
        //labelChoiceWidth = labelChoice.Width;
        //labelChoiceFontSize = labelChoice.Font.Size;
        //labelChoiceRatio = new SizeD
        //{
        //    Width = (double)labelChoice.Width / panel.Width,
        //    Height = (double)labelChoice.Height / panel.Height,
        //};

    }

    void FormAspect_ControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control == tLayout)
        {
            Color defColor = Color.FromArgb(128, 0, 0, 0);
            Dictionary<string, Color?> colorsLeft = new() {
                { "Default", defColor },
                { "MouseEnter",
                  ColorExtensions.BlendOver(Color.FromArgb(12, 200, 104, 34), defColor) },
                { "MouseLeave", defColor }
            };
            Dictionary<string, Color?> colorsRight = new() {
                { "Default", defColor },
                { "MouseEnter",
                  ColorExtensions.BlendOver(Color.FromArgb(16, 185, 36, 199), defColor) },
                { "MouseLeave", defColor }
            };

            pwLeft = new PanelWrapper(
                pLeft, 
                Resource.FaceLeft, 
                "left", 
                colorsLeft, 
                new Panel[] { sTL, sBL }
            );
            pwRight = new PanelWrapper(
                pRight, 
                Resource.FaceRight, 
                "right", 
                colorsRight,
                new Panel[] { sTR, sBR }
            );
        }
    }

    private void pLeft_Click(object sender, EventArgs e)
    {
        // TODO remove click, 2 more labels, "you won\n reset"
        pwLeft?.RemoveEventHandlers();
        pwRight?.RemoveEventHandlers();
        Player = "playerLeft";
    }

    private void pRight_Click(object sender, EventArgs e)
    {
        pwLeft?.RemoveEventHandlers();
        pwRight?.RemoveEventHandlers();
        Player = "playerRight";
    }

    void FormAspect_Load(object sender, EventArgs e)
    {
        marginSize = Size - ClientSize;
        cRatio = (double)ClientSize.Width / ClientSize.Height;
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
            int newWidth, newHeight;

            switch ((WMSZ)m.WParam.ToInt32())
            {
                case WMSZ.LEFT:
                case WMSZ.RIGHT:
                case WMSZ.TOPLEFT:
                case WMSZ.TOPRIGHT:
                case WMSZ.BOTTOMLEFT:
                case WMSZ.BOTTOMRIGHT:
                    // width has changed, adjust height
                    newWidth = rc.Right - rc.Left - marginSize.Width;
                    newHeight = (int)(newWidth / cRatio) + marginSize.Height;
                    rc.Bottom = rc.Top + newHeight;
                    break;
                case WMSZ.TOP:
                case WMSZ.BOTTOM:
                    // height has changed, adjust width
                    newHeight = rc.Bottom - rc.Top - marginSize.Height;
                    newWidth = (int)(newHeight * cRatio) + marginSize.Width;
                    rc.Right = rc.Left + newWidth;
                    break;
            }
            Marshal.StructureToPtr(rc, m.LParam, true);

            // adjust components

            var width = tLayout.Size.Width;
            tLayout.Size = new Size(width, width);

            //panel.Size = new Size(panel.Width, (int)(panel.Width / pRatio));

            //labelChoice.Size = new Size(
            //    (int)(panel.Width * labelChoiceRatio.Width),
            //    (int)(panel.Height * labelChoiceRatio.Height)
            //);
            //labelChoice.Location = new Point(
            //    (panel.Width - labelChoice.Width) / 2,
            //    (panel.Height - labelChoice.Height) / 2
            //);
            //float newFontSize = labelChoiceFontSize * labelChoice.Width / labelChoiceWidth;
            //labelChoice.Font = new Font(labelChoice.Font.FontFamily, newFontSize);
        }

        base.WndProc(ref m);
    }
}