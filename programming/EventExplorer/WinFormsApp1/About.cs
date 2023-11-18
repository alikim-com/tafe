
namespace WinformsUMLEvents;

partial class UML_Events
{
    AboutForm? aboutForm;

    private void menuHelpAbout_Click(object sender, EventArgs e)
    {
        aboutForm ??= new AboutForm();

        if (aboutForm.ShowDialog(this) == DialogResult.OK) 
            return;
    }
}
