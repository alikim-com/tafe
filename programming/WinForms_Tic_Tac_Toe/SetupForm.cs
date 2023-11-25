
using System.Linq;

namespace WinFormsApp1;

public partial class SetupForm : Form
{
    static public readonly UIColors.ColorTheme theme = UIColors.Steel;

    static public readonly Color tintLeft = Color.FromArgb(15 * 4, 200, 104, 34);
    static public readonly Color tintRight = Color.FromArgb(20 * 4, 185, 36, 199);

    public Color foreLeft, foreRight, foreLeftDim, foreRightDim;

    readonly List<ChoiceItem> roster = new();

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

    public SetupForm()
    {
        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        foreLeft = ColorExtensions.BlendOver(tintLeft, ForeColor);
        foreRight = ColorExtensions.BlendOver(tintRight, ForeColor);
        foreLeftDim = ColorExtensions.BlendOver(UIColors.Black, foreLeft);
        foreRightDim = ColorExtensions.BlendOver(foreRight, UIColors.Black);

        panelLeft.ForeColor = foreLeft;
        panelRight.ForeColor = foreRight;
        panelLeft.BackColor = UIColors.Transparent;
        panelRight.BackColor = UIColors.Transparent;

        button1.BackColor = theme.Light;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatAppearance.MouseDownBackColor = theme.Dark;
        button1.FlatAppearance.MouseOverBackColor = theme.Noon;

        var off = headerLeft.Location.X;
        foreach (Control ctrl in panelRight.Controls)
            ctrl.Location = new Point(ctrl.Location.X - off, ctrl.Location.Y);

        allegLeft.Font = identityLeft.Font = UIFonts.small;
        foreach (Control ctrl in panelRight.Controls) ctrl.Font = UIFonts.small;

        headerLeft.Font = UIFonts.header;

        CreateChoiceLists();

        // panelLeft.BackgroundImage = Resource.AILeft;
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

    EventHandler MakeOnClickHandler(ChoiceItem choiceItem, List<ChoiceItem> roster)
    {
        EventHandler handler = (object? sender, EventArgs e) =>
        {
            choiceItem.chosen = true;

            var side = choiceItem.side;

            var rosterThisSide = roster.Where(itm => itm.side == side && itm != choiceItem);
            var rosterOtherSide = roster.Where(itm => itm.side != side);

            foreach (var rec in rosterThisSide)
            {
                choiceItem.chosen = false;
                rec.Deactivate();
            }

            var rosterId = choiceItem.rosterId;

            var mirrorItem = rosterOtherSide.FirstOrDefault(itm => itm.chosen && itm.rosterId == rosterId);

            if (mirrorItem != null)
                foreach(var rec in rosterOtherSide) rec.Deactivate();
        };

        choiceItem.SetOnClickHandler(handler);

        return handler;
    }


    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }
}

class ChoiceItem
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

    public ChoiceItem(Game.Roster _rosterId, Side _side, Label _origin, Label _identity, Color _foreOn, Color _foreOff)
    {
        chosen = false;
        rosterId = _rosterId;
        side = _side;
        origin = _origin;
        identity = _identity;
        foreOn = _foreOn;
        foreOff = _foreOff;
    }

    public void Activate()
    {
        identity.Cursor = Cursors.Hand;
        identity.ForeColor = foreOn;
        origin.ForeColor = foreOn;
    }
    public void Deactivate()
    {
        identity.Cursor = Cursors.Default;
        identity.ForeColor = foreOff;
        origin.ForeColor = foreOff;
    }
    public void SetOnClickHandler(EventHandler handler) => identity.Click += handler;

    public void Deconstruct(out Label _origin, out Label _identity)
    {
        _identity = identity;
        _origin = origin;
    }
}

