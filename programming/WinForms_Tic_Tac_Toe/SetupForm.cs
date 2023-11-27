﻿
using System.Drawing.Drawing2D;

namespace WinFormsApp1;

public partial class SetupForm : Form
{
    static public readonly UIColors.ColorTheme theme = UIColors.Steel;

    public static readonly Color tintLeft = Color.FromArgb(15 * 4, 200, 104, 34);
    public static readonly Color tintRight = Color.FromArgb(20 * 4, 185, 36, 199);

    public Color foreLeft, foreRight, foreLeftDim, foreRightDim;

    public static readonly List<ChoiceItem> roster = new();

    readonly ToolStripRendererOverride buttonRenderer;

    public enum btnMessage
    {
        Ready_AI,
        Ready_Human,
        Ready_Mix,
        Both_Missing,
        Left_Missing,
        Right_Missing,
    }

    public SetupForm()
    {
        // prevent main window flickering
        DoubleBuffered = true;

        ControlBox = false;

        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        foreLeft = ColorExtensions.BlendOver(tintLeft, ForeColor);
        foreRight = ColorExtensions.BlendOver(tintRight, ForeColor);
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

        buttonRenderer = new ToolStripRendererOverride(toolStrip);
        toolStrip.Renderer = buttonRenderer;

        toolStrip.BackColor = toolStripLabel.BackColor = UIColors.Transparent;

        UpdateButton(btnMessage.Both_Missing);

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

            roster.Add(new(choiceItem.rosterId, ChoiceItem.Side.Right, _origLab, _identLab, foreRight, foreRightDim));
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

            panelThisSide.BackgroundImage = GetBackgroundImage(choiceItem.rosterId, side);

            choiceItem.chosen = true;
            choiceItem.Activate();

            foreach (var rec in rosterThisSide)
            {
                rec.chosen = false;
                rec.Deactivate();
            }

            var rosterId = choiceItem.rosterId;

            var mirrorItem = rosterOtherSide.FirstOrDefault(itm => itm.chosen && itm.rosterId == rosterId);

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
                if (thisChosen.originType != otherChosen.originType)
                {
                    UpdateButton(btnMessage.Ready_Mix);

                } else if (Enum.TryParse($"Ready_{thisChosen.originType}", out btnMessage msg))
                {
                    UpdateButton(msg);
                }

            } else
            {
                var missingSide = side == ChoiceItem.Side.Left ? btnMessage.Right_Missing : btnMessage.Left_Missing;
                UpdateButton(missingSide);
            }

        };

        choiceItem.SetOnClickHandler(handler);
    }

    void UpdateButton(btnMessage msg)
    {
        Dictionary<btnMessage, string> buttonMessages = new() {
            { btnMessage.Ready_AI, "I like to watch...o_o" },
            { btnMessage.Ready_Human, "Fight, Mortals!" },
            { btnMessage.Ready_Mix, "For the Organics!" },
            { btnMessage.Both_Missing, "Choose players" },
            { btnMessage.Left_Missing, "Choose left player" },
            { btnMessage.Right_Missing, "Choose right player" },
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

public class ChoiceItem
{
    public bool chosen;

    public readonly Game.Roster rosterId;

    public readonly Label origin;
    public readonly Label identity;

    public enum Side
    {
        None,
        Left,
        Right
    }

    public readonly Side side;
    readonly Color foreOn;
    readonly Color foreOff;

    public readonly string originType;

    public ChoiceItem(Game.Roster _rosterId, Side _side, Label _origin, Label _identity, Color _foreOn, Color _foreOff)
    {
        chosen = false;
        rosterId = _rosterId;
        side = _side;
        origin = _origin;
        identity = _identity;
        foreOn = _foreOn;
        foreOff = _foreOff;

        originType = _rosterId.ToString().Split('_')[0];
    }

    public void Activate()
    {
        identity.ForeColor = foreOn;
        origin.ForeColor = foreOn;
    }
    public void Deactivate()
    {
        identity.ForeColor = foreOff;
        origin.ForeColor = foreOff;
    }
    public void SetOnClickHandler(EventHandler handler)
    {
        identity.Click += handler;
        identity.Cursor = Cursors.Hand;
    }

    public void Deconstruct(out Label _origin, out Label _identity)
    {
        _identity = identity;
        _origin = origin;
    }

    public override string ToString() => $"{identity.Text} | {side} | chosen: {chosen}";
}

public class ToolStripRendererOverride : ToolStripProfessionalRenderer
{
    Color gTop, gBot;
    static readonly Color gradTop = Color.FromArgb(52, 26, 79).ScaleRGB(1.25);
    static readonly Color gradBot = Color.FromArgb(110, 18, 0).ScaleRGB(1.25);
    static readonly Color gradBotOver = Color.FromArgb(52, 26, 79).ScaleRGB(1.4);
    static readonly Color gradTopOver = Color.FromArgb(110, 18, 0).ScaleRGB(1.4);
    static readonly Color gradTopDisabled = Color.FromArgb(200, 104, 34).ScaleRGB(0.10);
    static readonly Color gradBotDisabled = Color.FromArgb(185, 36, 199).ScaleRGB(0.10);

    bool _disabled = false;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            if (value != _disabled)
            {
                _disabled = value;

                UpdateColors(false);
                parent.Invalidate();
            }
        }
    }

    readonly Control parent;

    public ToolStripRendererOverride(Control _parent)
    {
        parent = _parent;
        this.RoundedEdges = false;
    }

    void UpdateColors(bool state)
    {
        gTop = Disabled ? gradTopDisabled : (state ? gradTopOver : gradTop);
        gBot = Disabled ? gradBotDisabled : (state ? gradBotOver : gradBot);
    }

    public void SetOverState(object sender, bool state)
    {
        UpdateColors(state);

        if (sender is Control ctrl)
        {
            ctrl.Cursor = (!Disabled && state) ? Cursors.Hand : Cursors.Default;
            ctrl.Invalidate();
        }
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        base.OnRenderToolStripBackground(e);

        using var brush = new LinearGradientBrush(
            e.AffectedBounds,
            gTop,
            gBot,
            LinearGradientMode.Vertical
        );

        e.Graphics.FillRectangle(brush, e.AffectedBounds);
    }
}

