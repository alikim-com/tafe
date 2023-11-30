
namespace WinFormsApp1;

partial class AboutForm : Form
{
    UIColors.ColorTheme theme;

    internal AboutForm()
    {
        theme = UIColors.Steel;

        InitializeComponent();

        ForeColor = theme.Text;
        BackColor = theme.Prime;

        button1.BackColor = theme.Light;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatAppearance.MouseDownBackColor = theme.Dark;
        button1.FlatAppearance.MouseOverBackColor = theme.Noon;

    }

    private void Button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }
}
