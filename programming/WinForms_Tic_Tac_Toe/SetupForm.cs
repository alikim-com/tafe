
namespace WinFormsApp1;

public partial class SetupForm : Form
{
    static public readonly UIColors.ColorTheme theme = UIColors.Steel;

    public SetupForm()
    {
        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        panelLeft.BackColor = UIColors.Transparent;
        panelRight.BackColor = UIColors.Transparent;

        foreach (Control ctrl in panelLeft.Controls) ctrl.Font = UIFonts.title;
        foreach (Control ctrl in panelRight.Controls) ctrl.Font = UIFonts.title;

        headerLeft.Font = headerRight.Font = UIFonts.header;
        comboLeft.Font = comboRight.Font = UIFonts.regular;

        button1.BackColor = theme.Light;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatAppearance.MouseDownBackColor = theme.Dark;
        button1.FlatAppearance.MouseOverBackColor = theme.Noon;

        var off = headerLeft.Location.X;
        foreach (Control ctrl in panelRight.Controls)
            ctrl.Location = new Point(ctrl.Location.X - off, ctrl.Location.Y);

        comboLeft.DataSource = Enum.GetNames(typeof(Game.Roster));

    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }
}

public class FlatCombo : ComboBox
{
    Color borderColor = Color.Yellow;

    static readonly public Color tintLeft = Color.FromArgb(75, 200, 104, 34);
    static readonly public Color tintRight = Color.FromArgb(80, 185, 36, 199);

    public FlatCombo()
    {
        //DrawMode = DrawMode.OwnerDrawFixed;
        //DrawItem += ComboBox_DrawItem;

        Paint += ComboBox_Paint;

        //BackColor = Color.Magenta;
        //ForeColor = Color.Blue;
    }

    public Color BorderColor
    {
        get { return borderColor; }
        set { borderColor = value; Invalidate(); }
    }
    private void ComboBox_Paint(object? sender, PaintEventArgs e)
    {
        if (!DroppedDown)
        {
            using SolidBrush brush = new(Color.LightBlue);
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }
    }
    private void ComboBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ComboBox combo) return;

        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            Color tint = combo.Name.Contains("Left") ? tintLeft : tintRight;
            using var brush = new SolidBrush(tint);
            e.Graphics.FillRectangle(brush, e.Bounds);
        } else
        {
            using var brush = new SolidBrush(SetupForm.theme.Dark);
            e.Graphics.FillRectangle(brush, e.Bounds);
        }

        using var brushText = new SolidBrush(SetupForm.theme.Text);
        e.Graphics.DrawString(
            combo?.Items[e.Index].ToString(),
            e.Font ?? UIFonts.Default,
            brushText,
            new Point(e.Bounds.X, e.Bounds.Y));
    }

    private const int WM_PAINT = 0xF;
    private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;

    //protected override void WndProc(ref Message m)
    //{
    //    base.WndProc(ref m);
    //    if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
    //    {
    //        using (var g = Graphics.FromHwnd(Handle))
    //        {
    //            using (var p = new Pen(BorderColor))
    //            {
    //                g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);

    //                var d = FlatStyle == FlatStyle.Popup ? 1 : 0;
    //                g.DrawLine(p, Width - buttonWidth - d,
    //                    0, Width - buttonWidth - d, Height);
    //            }
    //        }
    //    }
    //}
}
