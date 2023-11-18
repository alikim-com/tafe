
namespace WinformsUMLEvents;

public class CustomRenderer : ToolStripProfessionalRenderer
{
    public CustomRenderer() : base(new CustomColors()) { }
}

public class CustomColors : ProfessionalColorTable
{
    public override Color MenuItemBorder => Color.Transparent;
    public override Color MenuItemSelected => Color.FromArgb(69, 69, 77);
    public override Color MenuItemSelectedGradientBegin => Color.FromArgb(69, 69, 77);
    public override Color MenuItemSelectedGradientEnd => Color.FromArgb(69, 69, 77);
    public override Color MenuItemPressedGradientBegin => Color.FromArgb(69, 69, 77);
    public override Color MenuItemPressedGradientEnd => Color.FromArgb(69, 69, 77);
}

partial class UML_Events
{
    AboutForm? aboutForm;

    private void menuHelpAbout_Click(object sender, EventArgs e)
    {
        aboutForm ??= new AboutForm();

        if (aboutForm.ShowDialog(this) == DialogResult.OK) return;
    }
}


