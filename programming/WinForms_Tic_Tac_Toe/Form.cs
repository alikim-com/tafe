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

    static readonly Dictionary<KeyValuePair<Game.Roster, Game.Roster>, Image> mainBg = new();

    static public void ApplyDoubleBuffer(Control control)
    {
        control.GetType().InvokeMember(
            "DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            null, control, new object[] { true }
        );
    }

    UIColors.ColorTheme theme;

    void InitializeMenu()
    {
        // menu appearance
        menuStrip1.BackColor = theme.Prime;
        menuStrip1.ForeColor = theme.Text;
        menuStrip1.Renderer = UIRenderer.TSRenderer(theme, "ColorTableMain");

        ToolStripMenuItem[] expandableItems = new[] { menuLoad, menuLoadCollection, menuSave, menuHelp };

        foreach (var item in expandableItems)
        {
            if (item.DropDown is ToolStripDropDownMenu dropDownMenu)
                dropDownMenu.ShowImageMargin = false;

            foreach (ToolStripItem subItem in item.DropDownItems) subItem.ForeColor = theme.Text;
        }

        menuLayout.BackColor = theme.Dark;
        menuLayout.ForeColor = theme.Text;
        menuLayout.BorderStyle = BorderStyle.None;

        menuDummy.BackColor = theme.Dark;
        menuDummy.ForeColor = theme.Text;
        menuDummy.BorderStyle = BorderStyle.None;
    }

    SetupForm? setupForm;

    void SetupFormPopup()
    {
        setupForm ??= new SetupForm();

        if (setupForm.ShowDialog(this) == DialogResult.OK) return;
    }

    public AppForm()
    {
        theme = UIColors.Steel;

        EM.uiThread = this;

        // set the order of players turns, needed before clicking on cfg panels
        Game.SetTurns("random");

        // init label manager
        labMgr = new();

        // to extend the behaviour of sub components
        ControlAdded += FormAspect_ControlAdded;

        // prevent main window flickering
        DoubleBuffered = true;

        InitializeComponent();

        InitializeMenu();

        // for scaling font
        //lChoiceWidth = choice.Width;
        //lChoiceFontSize = choice.Font.Size;

        // prevent backgound flickering for components
        Control[] dbuffed =
            new Control[]
            {
                tLayout,
               //pLeft, pRight, tLayout, tSplit, sTL, sTR, sBL, sBR, choice
            };

        foreach (var ctrl in dbuffed) ApplyDoubleBuffer(ctrl);

        // subscriptions to reset from Game.Reset()
        EM.Subscribe(EM.Evt.Reset, LabelManager.ResetHandler);
        if (pwLeft != null)
            EM.Subscribe(EM.Evt.Reset, pwLeft.ResetHandler);
        if (pwRight != null)
            EM.Subscribe(EM.Evt.Reset, pwRight.ResetHandler);

        // update labels
        EM.Subscribe(EM.Evt.UpdateLabels, LabelManager.UpdateLabelsHandler);

        // issued after game board changes 
        EM.Subscribe(EM.Evt.SyncBoard, VBridge.SyncBoardHandler);
        // translation to board cell bgs
        foreach (var cw in cellWrap)
            EM.Subscribe(EM.Evt.SyncBoardUI, cw.SyncBoardUIHandler);

        // raised by bot panels after clicking 
        EM.Subscribe(EM.Evt.PlayerConfigured, VBridge.PlayerConfiguredHandler);
        EM.Subscribe(EM.Evt.PlayerConfigured, TurnWheel.PlayerConfiguredHandler);

        // raised by board cells after clicking
        EM.Subscribe(EM.Evt.PlayerMoved, Game.PlayerMovedHandler);

        // BLOCKS: player setup pop-up 
        SetupFormPopup();

        CreateBackground();

        // raise reset event
        Game.Reset();

        // start listening to players config choices
        if (pwLeft != null && pwRight != null)
            TurnWheel.Start(
                new List<IComponent>() { pwLeft, pwRight },
                AI.Logic.ConfigRNG
            );

        // when config is done, start game
        EventHandler OnConfigFinished = (object? _, EventArgs __) =>
        {
            TurnWheel.Start(
                cellWrap.Cast<IComponent>().ToList(), // unwraps in row-major order
                AI.Logic.BoardRNG
            );
        };
        EM.Subscribe(EM.Evt.ConfigFinished, OnConfigFinished);
    }

    void CreateBackground()
    {
        var chosen = SetupForm.roster.Where(itm => itm.chosen);
        var chosenArr = chosen.ToArray();
        if (chosenArr.Length != 2) throw new Exception($"Form.CreateBackground : wrong number of players '{chosenArr.Length}'");

        var firstChosenIsLeft = chosenArr[0].side == ChoiceItem.Side.Left;

        KeyValuePair<Game.Roster, Game.Roster> leftRightBg = firstChosenIsLeft ? 
            new(chosenArr[0].rosterId, chosenArr[1].rosterId) :
            new(chosenArr[1].rosterId, chosenArr[0].rosterId);

        foreach(var (_leftRightBg, bgImage) in mainBg)
            if(_leftRightBg.Key == leftRightBg.Key && _leftRightBg.Value == leftRightBg.Value) // cache exists
            {
                BackgroundImage = bgImage;
                return;
            }

        Image?[] headImage = chosen.Select(itm => 
            (Image?)Resource.ResourceManager.GetObject($"{itm.rosterId}_{itm.side}_Head")).ToArray();

        KeyValuePair<Image?, Image?> leftRightImage = firstChosenIsLeft ?
            new(headImage[0], headImage[1]) : new(headImage[1], headImage[0]);

        if (leftRightImage.Key != null && leftRightImage.Value != null)
        {
            int botPanelHeight = 206;
            var botPanelOff = new Point(0, Resource.GameBackImg.Height - botPanelHeight);
            var botPanelSize = new Size(Resource.GameBackImg.Width, botPanelHeight);

            BackgroundImage = Resource.GameBackImg;
            using var g = Graphics.FromImage(BackgroundImage);

            Color dimColor = Color.FromArgb(128, 0, 0, 0);
            var rect = new Rectangle(botPanelOff, botPanelSize);

            using var brush = new SolidBrush(dimColor);
            g.FillRectangle(brush, rect);

            leftRightImage.Key.GetOverlayOnBackground(
                BackgroundImage,
                botPanelOff,
                new Size(BackgroundImage.Width / 2, botPanelHeight),
                "left",
                "top");

            leftRightImage.Value.GetOverlayOnBackground(
                BackgroundImage,
                new Point(Resource.GameBackImg.Width / 2, botPanelOff.Y),
                new Size(BackgroundImage.Width / 2, botPanelHeight),
                "right",
                "top");

            mainBg.Add(leftRightBg, BackgroundImage);

            return;
        }

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

            //pwLeft = new PanelWrapper(
            //    pLeft,
            //    Resource.FaceLeft,
            //    "left",
            //    "top",
            //    colorsLeft,
            //    new Control[] { sTL, sBL },
            //    CellWrapper.BgMode.Player1
            //);
            //pwRight = new PanelWrapper(
            //    pRight,
            //    Resource.FaceRight,
            //    "right",
            //    "top",
            //    colorsRight,
            //    new Control[] { sTR, sBR },
            //    CellWrapper.BgMode.Player2
            //);

            // data bindings
            //  choice.DataBindings.Add(new Binding("Text", labMgr, "ChoicePanel"));
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
    enum WMSZ
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

            //float newFontSize = lChoiceFontSize * choice.Width / lChoiceWidth;
            //choice.Font = new Font(choice.Font.FontFamily, newFontSize);
            //info.Font = new Font(info.Font.FontFamily, newFontSize);

        }

        base.WndProc(ref m);
    }

}