using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class AppForm : Form
{
    double cRatio; // main window
    Size marginSize; // non-client area

    readonly int lChoiceWidth;
    readonly float lChoiceFontSize;

    // player cfg panels bg manager
    PanelWrapper? pwLeft;
    PanelWrapper? pwRight;

    // board cell bg manager
    readonly CellWrapper[,] cellWrap = new CellWrapper[3, 3];

    readonly LabelManager labMgr;

    static void ApplyDoubleBuffer(Control control)
    {
        control.GetType().InvokeMember(
            "DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, control, new object[] { true }
        );
    }

    static public AppForm? instance; // UI thread ref for TurnWheel

    public AppForm()
    {
        instance = this;

        // set the order of players turns, needed before clicking on cfg panels
        Game.SetTurns("random");

        // init label manager
        labMgr = new();

        // to extend the behaviour of sub components
        ControlAdded += FormAspect_ControlAdded;

        // prevent main window flickering
        DoubleBuffered = true;

        InitializeComponent();

        // for scaling font
        lChoiceWidth = choice.Width;
        lChoiceFontSize = choice.Font.Size;

        // prevent backgound flickering for components
        Control[] dbuffed =
            new Control[]
            {
                pLeft, pRight, tLayout, tSplit, sTL, sTR, sBL, sBR, choice
            };

        foreach (var ctrl in dbuffed) ApplyDoubleBuffer(ctrl);

        // events subscriptions to reset from Game.Reset()
        EM.EvtReset += LabelManager.ResetHandler;
        if (pwLeft != null) EM.EvtReset += pwLeft.ResetHandler;
        if (pwRight != null) EM.EvtReset += pwRight.ResetHandler;

        // update labels
        EM.EvtUpdateLabels += LabelManager.UpdateLabelsHandler;

        // issued after Game board changes 
        EM.EvtSyncBoard += VBridge.SyncBoardHandler;
        // translation to board cell bgs
        foreach (var cw in cellWrap) EM.EvtSyncBoardUI += cw.SyncBoardUIHandler;

        // cfg panels clicks
        EM.EvtPlayerConfigured += VBridge.PlayerConfiguredHandler;

        // board cell clicks ---------- TODO ------------

        // raise reset event
        Game.Reset();

        // start listening to players config choices
        if (pwLeft != null && pwRight != null)
            TurnWheel.Start(
                new List<IComponent>() { pwLeft, pwRight },
                AI.Logic.Config
            );

        // when config is done, start game
        EM.EvtConfigFinished += OnConfigFinished;
    }

    void OnConfigFinished(object? _, EventArgs __)
    {
        TurnWheel.Start(
            cellWrap.Cast<IComponent>().ToList(),
            AI.Logic.Board
        );
    }

    void FormAspect_ControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control == tLayout)
        {
            Color defColor = Color.FromArgb(128, 0, 0, 0);
            Dictionary<PanelWrapper.BgMode, Color?> colorsLeft = new() {
                { PanelWrapper.BgMode.Default, defColor },
                { PanelWrapper.BgMode.MouseEnter,
                  ColorExtensions.BlendOver(Color.FromArgb(15, 200, 104, 34), defColor) },
                { PanelWrapper.BgMode.MouseLeave, defColor }
            };
            Dictionary<PanelWrapper.BgMode, Color?> colorsRight = new() {
                { PanelWrapper.BgMode.Default, defColor },
                { PanelWrapper.BgMode.MouseEnter,
                  ColorExtensions.BlendOver(Color.FromArgb(20, 185, 36, 199), defColor) },
                { PanelWrapper.BgMode.MouseLeave, defColor }
            };

            pwLeft = new PanelWrapper(
                pLeft,
                Resource.FaceLeft,
                "left",
                "top",
                colorsLeft,
                new Control[] { sTL, sBL },
                CellWrapper.BgMode.Player1
            );
            pwRight = new PanelWrapper(
                pRight,
                Resource.FaceRight,
                "right",
                "top",
                colorsRight,
                new Control[] { sTR, sBR },
                CellWrapper.BgMode.Player2
            );

            // data bindings
            choice.DataBindings.Add(new Binding("Text", labMgr, "ChoicePanel"));
            info.DataBindings.Add(new Binding("Text", labMgr, "InfoPanel"));

            // --------- board cells ----------

            int tabInd = 0;
            cells = new Panel[3, 3];
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                {
                    Panel p = cells[row, col] = new Panel();
                    p.BackgroundImageLayout = ImageLayout.Stretch;
                    p.Dock = DockStyle.Fill;
                    p.Margin = new Padding(12);
                    p.Name = $"cell{row}{col}";
                    p.Size = new Size(109, 108);
                    p.TabIndex = tabInd++;

                    tLayout.Controls.Add(p, col, row);

                    ApplyDoubleBuffer(p);

                    cellWrap[row, col] = new CellWrapper(p, row, col);
                }
        }
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
                    // width has changed, adjust height
                    newWidth = rc.Right - rc.Left - marginSize.Width;
                    newHeight = (int)(newWidth / cRatio) + marginSize.Height;
                    rc.Bottom = rc.Top + newHeight;
                    break;
                case WMSZ.TOP:
                case WMSZ.BOTTOM:
                case WMSZ.TOPLEFT:
                case WMSZ.TOPRIGHT:
                case WMSZ.BOTTOMLEFT:
                case WMSZ.BOTTOMRIGHT:
                    // height has changed, adjust width
                    newHeight = rc.Bottom - rc.Top - marginSize.Height;
                    newWidth = (int)(newHeight * cRatio) + marginSize.Width;
                    rc.Right = rc.Left + newWidth;
                    break;
            }
            Marshal.StructureToPtr(rc, m.LParam, true);

            // adjust components

            float newFontSize = lChoiceFontSize * choice.Width / lChoiceWidth;
            choice.Font = new Font(choice.Font.FontFamily, newFontSize);
        }

        base.WndProc(ref m);
    }

}