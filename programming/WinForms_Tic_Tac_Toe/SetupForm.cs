
namespace WinFormsApp1;

public partial class SetupForm : Form
{
    static public readonly UIColors.ColorTheme theme = UIColors.Steel;

    Game.Roster? choiceLeft, choiceRight;
    readonly Dictionary<Game.Roster, Label> rosterLeft = new();
    readonly Dictionary<Game.Roster, Label> rosterRight = new();

    void CreateChoiceLists()
    {
        // left side

        List<Label> aux = new();

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

            aux.Add(labAlleg); // for mirroring on the right side

            rosterLeft.Add(rostItem, labName);
        }

        // right side

        static Label mirrorLabel(Control parent, Label src, string name, int tab, string text)
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
        mirrorLabel(panelRight, headerLeft, "headerRight", tab++, "Player Right");
        mirrorLabel(panelRight, allegLeft, "allegRight", tab++, "Origin");
        mirrorLabel(panelRight, identityLeft, "identityRight", tab++, "Identity");

        foreach(var (rostItem, identityLab) in rosterLeft)
        {
            var lab = mirrorLabel(panelRight, identityLab, identityLab.Name.Replace("Left", "Right"), tab++, identityLab.Text);

            rosterRight.Add(rostItem, lab);
        }

        foreach(var rec in aux)
            mirrorLabel(panelRight, rec, "Right" + rec.Name, tab++, rec.Text);
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

    public SetupForm()
    {
        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

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

        // mouse events

        AddClickHandlers(rosterLeft);
        AddClickHandlers(rosterRight);

        // panelLeft.BackgroundImage = Resource.AILeft;
    }

    void AddClickHandlers(Dictionary<Game.Roster, Label> roster)
    {
        var otherRoster = (roster == rosterLeft) ? rosterLeft : rosterRight;    

        foreach (var (rostItem, identLab) in roster) identLab.Click += OnChoiceClick;
    }

    readonly EventHandler OnChoiceClick = (object? sender, EventArgs e) =>
    {
        Utils.Msg("boom");
    };

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

}

