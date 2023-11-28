using System.Reflection;
using System.Runtime.InteropServices;

namespace WinFormsApp1;

public partial class AppForm : Form
{
    double cRatio; // main window
    Size ncSize; // non-client area

    readonly int lChoiceWidth;
    readonly float lChoiceFontSize;

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

    IEnumerable<ChoiceItem> chosen = Enumerable.Empty<ChoiceItem>();

    ChoiceItem[] chosenArr = Array.Empty<ChoiceItem>();

    bool firstChosenIsLeft = false;

    readonly List<AI> AIAgents = new();

    void SetupFormPopup()
    {
        setupForm ??= new SetupForm();

        if (setupForm.ShowDialog(this) == DialogResult.OK) return;
    }

    public AppForm()
    {
        theme = UIColors.Steel;

        EM.uiThread = this;

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

        // prevent backgound flickering components
        var doubleBuffed = new Control[] { tLayout, labelLeft, labelRight, labelVS };
        foreach (var ctrl in doubleBuffed) ApplyDoubleBuffer(tLayout);

        // BLOCKS: player setup pop-up 
        SetupFormPopup();

        // adjust labels & setup percentage positioning
        SetupLabels();

        // LabelManager properties -> Labels
        SetupBinds();

        // Event subscriptions & callbacks
        SetupSubsAndCb();

        // MULTI-USE: reset everything and start the game
        StartGame();

        //menuHelpAbout.Click += (object? sender, EventArgs e) => { BackgroundImage = Resource.GameBackImg; };
    }

    void EnableUI()
    {
        foreach (var cw in cellWrap)
            if (cw is IComponent iComp) iComp.Enable();
    }
    void DisableUI()
    {
        foreach (var cw in cellWrap)
            if (cw is IComponent iComp) iComp.Disable();
    }

    void StartGame()
    {
        // retrieve players list
        AssertPlayers();

        // create player defined bg
        CreateBackground();

        // ready new game
        Reset();

        // create AIs, if needed
        AIAgents.Clear();
        foreach(var chItm in chosenArr)
            if(chItm.originType == "AI")
            {
                var logic = chItm.rosterId == Game.Roster.AI_One ? AI.Logic.RNG : AI.Logic.Easy;
                var aiAgent = new AI(logic, chItm.rosterId);
                AIAgents.Add(aiAgent);
                EM.Subscribe(EM.Evt.AIMakeMove, aiAgent.AIMakeMoveHandler());
            }

        // start game
        TurnWheel.GameCountdown();
    }

    void SetupSubsAndCb()
    {
        EM.Subscribe(EM.Evt.UpdateLabels, LabelManager.UpdateLabelsHandler);

        EM.Subscribe(EM.Evt.SyncBoard, VBridge.SyncBoardHandler);
        EM.Subscribe(EM.Evt.SyncMoveLabels, VBridge.SyncMoveLabelsHandler);

        foreach (var cw in cellWrap)
            EM.Subscribe(EM.Evt.SyncBoardUI, cw.SyncBoardUIHandler);

        EM.Subscribe(EM.Evt.PlayerMoved, TurnWheel.PlayerMovedHandler);

        TurnWheel.SetCallbacks(EnableUI, DisableUI);
    }

    void SetupBinds()
    {
        info.DataBindings.Add(new Binding("Text", labMgr, "InfoPanelBind"));
        labelLeft.DataBindings.Add(new Binding("Text", labMgr, "LabelLeftBind"));
        labelRight.DataBindings.Add(new Binding("Text", labMgr, "LabelRightBind"));
    }

    void Reset()
    {
        // rebuild visual bridge for translation between
        // Game <-> (CellWrapper, LabelManager)
        VBridge.Reset(chosen);

        // reset the game and the board
        Game.Reset(chosen.Select(chItm => chItm.rosterId).ToArray());
        // Game.SetTurns("random");

        TurnWheel.Reset();
    }

    void AssertPlayers()
    {
        chosen = SetupForm.roster.Where(itm => itm.chosen);
        chosenArr = chosen.ToArray();
        if (chosenArr.Length != 2)
            throw new Exception($"Form.CreateBackground : wrong number of players '{chosenArr.Length}'");

        firstChosenIsLeft = chosenArr[0].side == ChoiceItem.Side.Left;
    }

    void CreateBackground()
    {
        KeyValuePair<Game.Roster, Game.Roster> leftRightBg = firstChosenIsLeft ?
            new(chosenArr[0].rosterId, chosenArr[1].rosterId) :
            new(chosenArr[1].rosterId, chosenArr[0].rosterId);

        foreach (var (_leftRightBg, bgImage) in mainBg)
            if (_leftRightBg.Equals(leftRightBg)) // cache exists
            {
                BackgroundImage = bgImage;
                return;
            }

        BackgroundImage = Resource.GameBackImg;

        int botPanelHeight = 206;
        var botPanelOff = new Point(0, Resource.GameBackImg.Height - botPanelHeight);
        var botPanelSize = new Size(Resource.GameBackImg.Width, botPanelHeight);

        var rect = new Rectangle(botPanelOff, botPanelSize);

        Color dimColor = Color.FromArgb(128, 0, 0, 0);
        using var brush = new SolidBrush(dimColor);
        using var g = Graphics.FromImage(BackgroundImage);

        g.FillRectangle(brush, rect);

        Image?[] headImage = chosen.Select(itm =>
            (Image?)Resource.ResourceManager.GetObject($"{itm.rosterId}_{itm.side}_Head")).ToArray();

        KeyValuePair<Image?, Image?> leftRightImage = firstChosenIsLeft ?
            new(headImage[0], headImage[1]) : new(headImage[1], headImage[0]);

        if (leftRightImage.Key != null && leftRightImage.Value != null)
        {
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
        }

    }

    void SetupLabels()
    {
        Color foreLeft = ColorExtensions.BlendOver(SetupForm.tintLeft, theme.Text);
        Color foreRight = ColorExtensions.BlendOver(SetupForm.tintRight, theme.Text);

        labelLeft.ForeColor = foreLeft;
        labelRight.ForeColor = foreRight;
        labelLeft.BackColor = labelRight.BackColor = UIColors.Transparent;
        labelLeft.Font = labelRight.Font = UIFonts.regular;

        labelLeft.Anchor = labelRight.Anchor = AnchorStyles.None;

        int botPanelHeight = 206;
        var off = new Point(160, 50);
        labelLeft.Location = new Point(off.X, ClientSize.Height - botPanelHeight + off.Y);
        labelRight.Location = new Point(ClientSize.Width - labelRight.Width - off.X, ClientSize.Height - labelRight.Height - off.Y);

        RatioPosition.Add(labelLeft, this, RatioPosControl.Anchor.Left, RatioPosControl.Anchor.Top);
        RatioPosition.Add(labelRight, this, RatioPosControl.Anchor.Right, RatioPosControl.Anchor.Bottom);
    }

    void FormAspect_ClientSizeChanged(object? sender, EventArgs e)
    {
        // adjust components
        RatioPosition.Update(labelLeft);
        RatioPosition.Update(labelRight);
    }

    void FormAspect_ControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control == tLayout)
        {
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
        ncSize = Size - ClientSize;
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
                    newWidth = rc.Right - rc.Left - ncSize.Width;
                    newHeight = (int)(newWidth / cRatio) + ncSize.Height;
                    rc.Bottom = rc.Top + newHeight;
                    break;
                case WMSZ.TOP:
                case WMSZ.BOTTOM:
                case WMSZ.TOPLEFT:
                case WMSZ.TOPRIGHT:
                case WMSZ.BOTTOMLEFT:
                case WMSZ.BOTTOMRIGHT:
                    // height has changed, adjust width
                    newHeight = rc.Bottom - rc.Top - ncSize.Height;
                    newWidth = (int)(newHeight * cRatio) + ncSize.Width;
                    rc.Right = rc.Left + newWidth;
                    break;
            }
            Marshal.StructureToPtr(rc, m.LParam, true);

            //float newFontSize = lChoiceFontSize * choice.Width / lChoiceWidth;
            //choice.Font = new Font(choice.Font.FontFamily, newFontSize);
            //info.Font = new Font(info.Font.FontFamily, newFontSize);

        }

        base.WndProc(ref m);
    }

}