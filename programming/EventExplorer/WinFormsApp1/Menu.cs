
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
    public override Color ToolStripDropDownBackground => Color.FromArgb(69, 69, 77);
}

partial class UML_Events
{
    AboutForm? aboutForm;

    List<Profile> profiles = new(); 

    private void menuHelpAbout_Click(object sender, EventArgs e)
    {
        aboutForm ??= new AboutForm();

        if (aboutForm.ShowDialog(this) == DialogResult.OK) return;
    }

    private void menuSaveAs_Click(object sender, EventArgs e)
    {
        string layoutName = menuLayout.Text.Trim();
        layoutName = string.IsNullOrEmpty(layoutName) ? "Default" : layoutName;
        var prof = new Profile(layoutName, boxes);
        Utils.SaveProfile(profPath, layoutName, prof);
        AddProfile(prof);
    }

    void ApplyProfile(string pname)
    {
        var prof = profiles.Find(p => p.Name == pname);
        if (prof == null)
        {
            Utils.Msg($"UML_events.ApplyProfile : profile '{pname}' not found");
            return;
        }

        // update boxes from profile
        menuLayout.Text = prof.Name;
        foreach(var pbox in prof.Boxes)
        {
            var box = boxes.Find(b => b.name == pbox.Name);
            if (box == null) continue;
            
            box.pos = pbox.Pos;
            box.size = pbox.Size;
        }

        // force repaint, no grid positioning
        PaintCheckAfter(1);
        UpdatePaintCheck(true, false);
    }

    void AddProfile(Profile prof)
    {
        if(menuLoadCollection.DropDownItems.OfType<ToolStripMenuItem>().Any(itm => itm.Text == prof.Name))
        {
            Utils.Msg($"UML_Events.AddProfile : layout '{prof.Name}' menu item already exists");
            return;
        }

        // add to menu
        int ind = menuLoadCollection.DropDownItems.Count;
        ToolStripMenuItem menuItem = new()
        {
            BackColor = Color.FromArgb(46, 46, 51),
            ForeColor = Color.LightGray,
            Name = $"menuLoadCollection{ind}",
            Text = prof.Name,
        };
        menuItem.Click += (object? sender, EventArgs e) => { ApplyProfile(prof.Name); };

        menuLoadCollection.DropDownItems.Add(menuItem);

        // add to profiles
        profiles.Add(prof);
    }

    void RebuildProfileListAndMenu()
    {
        profiles.Clear();
        menuLoadCollection.DropDownItems.Clear();

        var pfiles = Utils.ReadFolder(profPath);

        foreach (var file in pfiles)
        {
            var prof = Utils.LoadProfile<Profile>(file, profPath);
            if (prof == null) continue;

            AddProfile(prof);
        }        
    }
}

public class BoxInfo
{
    public string Name { get; set; } = "";
    public Point Pos { get; set; } = new();
    public Size Size { get; set; } = new();

    public BoxInfo() { }

    public BoxInfo(string _name, Point _pos, Size _size)
    {
        Name = _name;
        Pos = _pos;
        Size = _size;
    }
}

public class Profile
{
    public string Name { get; set; } = "";
    public List<BoxInfo> Boxes { get; set; } = new();

    public Profile(string _name, List<ClassBox> _boxes)
    {
        Name = _name;
        foreach (var _box in _boxes)
            Boxes.Add(new BoxInfo(_box.name, _box.pos, _box.size));
    }

    public Profile()
    {

    }

    public override string ToString()
    {
        string outp = $"name: {Name}\n";
        foreach (var box in Boxes) outp += $"box: {box.Name}, pos: {box.Pos}, size: {box.Size}\n";
        return outp;
    }
}



