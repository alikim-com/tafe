
namespace WinFormsApp1;

partial class SetupForm : Form
{
    static readonly UIColors.ColorTheme theme = UIColors.Steel;

    readonly Color foreLeft, foreRight, foreLeftDim, foreRightDim;

    static internal readonly List<ChoiceItem> roster = new();

    readonly ButtonToolStripRenderer buttonRenderer;

    enum BtnMessage
    {
        Ready_AI,
        Ready_Human,
        Ready_Mix,
        Both_Missing,
        Left_Missing,
        Right_Missing,
    }

    internal SetupForm()
    {
        // prevent main window flickering
        DoubleBuffered = true;

        ControlBox = false;

        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        foreLeft = UIColors.ForeLeft;
        foreRight = UIColors.ForeRight;
        foreLeftDim = ColorExtensions.BlendOver(Color.FromArgb(96, 0, 0, 0), foreLeft);
        foreRightDim = ColorExtensions.BlendOver(Color.FromArgb(96, 0, 0, 0), foreRight);

        panelLeft.SuspendLayout();
        panelRight.SuspendLayout();

        panelLeft.ForeColor = foreLeft;
        panelRight.ForeColor = foreRight;
        panelLeft.BackColor = UIColors.Transparent;
        panelRight.BackColor = UIColors.Transparent;

        panelLeft.BackgroundImageLayout =
        panelRight.BackgroundImageLayout = ImageLayout.Stretch;

        var off = headerLeft.Location.X;
        foreach (Control ctrl in panelRight.Controls)
            ctrl.Location = new Point(ctrl.Location.X - off, ctrl.Location.Y);

        allegLeft.Font = identityLeft.Font = UIFonts.small;
        foreach (Control ctrl in panelRight.Controls) ctrl.Font = UIFonts.small;

        headerLeft.Font = UIFonts.header;

        CreateChoiceLists();

        buttonRenderer = UIRenderer.ButtonTSRenderer(toolStrip, ButtonColors.Sunrise);
        toolStrip.Renderer = buttonRenderer;

        toolStrip.Font = UIFonts.button;
        toolStrip.BackColor = toolStripLabel.BackColor = UIColors.Transparent;

        UpdateButton(BtnMessage.Both_Missing);

        AppForm.ApplyDoubleBuffer(panelLeft);
        AppForm.ApplyDoubleBuffer(panelRight);

        panelLeft.ResumeLayout(false);
        panelRight.ResumeLayout(false);
        panelLeft.PerformLayout();
        panelRight.PerformLayout();

    }

    static Label AddLabel(Control parent, Point loc, string name, int tab, string text)
    {
        var label = new Label
        {
            AutoSize = true,
            Location = loc,
            Name = name,
            Size = new Size(62, 15),
            TabIndex = tab,
            Text = text
        };

        parent.Controls.Add(label);

        return label;
    }

    void CreateChoiceLists()
    {
        // left side

        int tab = -1;
        Point locId = identityLeft.Location;
        int allegX = allegLeft.Location.X + allegLeft.Width;
        locId.Y += 15;

        foreach (var (rostItem, identity) in Game.rosterIdentity)
        {
            tab++;
            locId.Y += 25;

            var labName = AddLabel(panelLeft, locId, $"{identity}{tab}", tab, identity);
            labName.Font = UIFonts.regular;
            labName.Cursor = Cursors.Hand;

            var alleg = rostItem.ToString().Split('_')[0];
            var labAlleg = AddLabel(panelLeft, Point.Empty, $"{alleg}{tab}", tab, alleg);
            labAlleg.Font = UIFonts.tiny;

            labAlleg.Location = new Point(
                allegX - labAlleg.Width,
                labName.Location.Y + (labName.Height - labAlleg.Height) / 2
            );

            roster.Add(new(rostItem, ChoiceItem.Side.Left, labAlleg, labName, foreLeft, foreLeftDim));
        }

        // right side

        static Label AddMirrorLabel(Control parent, Label src, string name, int tab, string text)
        {
            var lab = AddLabel(parent, Point.Empty, name, tab, text);
            lab.Font = src.Font;

            lab.Location = new Point(
                parent.Width - lab.Width - src.Location.X,
                src.Location.Y
            );

            lab.Cursor = src.Cursor;

            return lab;
        }

        tab = 0;
        AddMirrorLabel(panelRight, headerLeft, "headerRight", tab++, "Right Player");
        AddMirrorLabel(panelRight, allegLeft, "allegRight", tab++, "Origin");
        AddMirrorLabel(panelRight, identityLeft, "identityRight", tab++, "Identity");

        var rosterLeft = roster.ToList();
        foreach (var choiceItem in rosterLeft)
        {
            var (origLab, identLab) = choiceItem;
            var _origLab = AddMirrorLabel(panelRight, origLab, "Right" + origLab.Name, tab++, origLab.Text);
            var _identLab = AddMirrorLabel(panelRight, identLab, identLab.Name.Replace("Left", "Right"), tab++, identLab.Text);

            roster.Add(new(choiceItem.RosterId, ChoiceItem.Side.Right, _origLab, _identLab, foreRight, foreRightDim));
        }

        foreach (var rec in roster)
            MakeOnClickHandler(rec, roster);
    }

    void MakeOnClickHandler(ChoiceItem choiceItem, List<ChoiceItem> roster)
    {
        EventHandler handler = (object? sender, EventArgs e) =>
        {
            var side = choiceItem.side;
            var rosterThisSide = roster.Where(itm => itm.side == side && itm != choiceItem);
            var rosterOtherSide = roster.Where(itm => itm.side != side);

            var (panelThisSide, panelOtherSide) = side == ChoiceItem.Side.Left ? (panelLeft, panelRight) : (panelRight, panelLeft);

            panelThisSide.BackgroundImage = GetBackgroundImage(choiceItem.RosterId, side);

            choiceItem.chosen = true;
            choiceItem.Activate();

            foreach (var rec in rosterThisSide)
            {
                rec.chosen = false;
                rec.Deactivate();
            }

            var rosterId = choiceItem.RosterId;

            var mirrorItem = rosterOtherSide.FirstOrDefault(itm => itm.chosen && itm.RosterId == rosterId);

            if (mirrorItem != null)
            {
                panelOtherSide.BackgroundImage = null;

                mirrorItem.chosen = false;
                foreach (var rec in rosterOtherSide) rec.Activate();
            }

            // update button messages

            var thisChosen = choiceItem;
            var otherChosen = rosterOtherSide.FirstOrDefault(itm => itm.chosen);

            if (thisChosen != null && otherChosen != null)
            {
                if (thisChosen.OriginType != otherChosen.OriginType)
                {
                    UpdateButton(BtnMessage.Ready_Mix);

                } else if (Enum.TryParse($"Ready_{thisChosen.OriginType}", out BtnMessage msg))
                {
                    UpdateButton(msg);
                }

            } else
            {
                var missingSide = side == ChoiceItem.Side.Left ? BtnMessage.Right_Missing : BtnMessage.Left_Missing;
                UpdateButton(missingSide);
            }

        };

        choiceItem.SetOnClickHandler(handler);
    }

    void UpdateButton(BtnMessage msg)
    {
        Dictionary<BtnMessage, string> buttonMessages = new() {
            { BtnMessage.Ready_AI, "I like to watch...o_o" },
            { BtnMessage.Ready_Human, "Fight, Mortals!" },
            { BtnMessage.Ready_Mix, "For the Organics!" },
            { BtnMessage.Both_Missing, "Choose players" },
            { BtnMessage.Left_Missing, "Choose left player" },
            { BtnMessage.Right_Missing, "Choose right player" },
        };

        toolStripLabel.Text = buttonMessages[msg];
        buttonRenderer.Disabled = msg.ToString().Contains("Missing");
    }

    static Image? GetBackgroundImage(Game.Roster rosterId, ChoiceItem.Side side)
    {
        var imageName = $"{rosterId}_{side}";
        return (Image?)Resource.ResourceManager.GetObject(imageName);
    }

    private void ToolStrip_SizeChanged(object sender, EventArgs e)
    {
        if (sender is Control ctrl) ctrl.Location = new((ClientSize.Width - ctrl.Width) / 2, ctrl.Location.Y);
    }

    private void ToolStrip_Click(object sender, EventArgs e)
    {
        if (buttonRenderer.Disabled) return;
        DialogResult = DialogResult.OK;
    }
    private void ToolStrip_MouseEnter(object sender, EventArgs e) => buttonRenderer.SetOverState(sender, true);
    private void ToolStrip_MouseLeave(object sender, EventArgs e) => buttonRenderer.SetOverState(sender, false);
}

class ChoiceItem
{
    internal bool chosen;

    public Game.Roster RosterId { get; set; } = Game.Roster.None;

    internal Label origin = new();
    internal Label identity = new();
    public string IdentityName { get; set; } = "";

    public enum Side
    {
        None,
        Left,
        Right
    }

    public Side side { get; set; } = Side.None;
    readonly Color foreOn;
    readonly Color foreOff;

    public string OriginType { get; set; } = "";

    internal ChoiceItem(Game.Roster _rosterId, Side _side, Label _origin, Label _identity, Color _foreOn, Color _foreOff)
    {
        chosen = false;
        RosterId = _rosterId;
        side = _side;
        origin = _origin;
        identity = _identity;
        foreOn = _foreOn;
        foreOff = _foreOff;

        OriginType = _rosterId.ToString().Split('_')[0];
        IdentityName = identity.Text;
    }

    public ChoiceItem()
    {
        // for JsonSerializer.Deserialize<P>(input);
    }

    internal void Activate()
    {
        identity.ForeColor = foreOn;
        origin.ForeColor = foreOn;
    }
    internal void Deactivate()
    {
        identity.ForeColor = foreOff;
        origin.ForeColor = foreOff;
    }
    internal void SetOnClickHandler(EventHandler handler)
    {
        identity.Click += handler;
        identity.Cursor = Cursors.Hand;
    }

    internal void Deconstruct(out Label _origin, out Label _identity)
    {
        _identity = identity;
        _origin = origin;
    }

    public override string ToString() => $"{identity.Text} | {side} | chosen: {chosen}";
}

