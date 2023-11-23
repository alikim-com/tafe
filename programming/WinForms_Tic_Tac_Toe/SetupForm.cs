
namespace WinFormsApp1;

public partial class SetupForm : Form
{
    UIColors.ColorTheme theme;

    public SetupForm()
    {
        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        button1.BackColor = theme.Light;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatAppearance.MouseDownBackColor = theme.Dark;
        button1.FlatAppearance.MouseOverBackColor = theme.Noon;

    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }
}
