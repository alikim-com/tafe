
namespace WinformsUMLEvents;

partial class UML_Events
{
    AboutForm? aboutForm;

    List<Profile> profiles = new();

    private void MenuHelpAbout_Click(object sender, EventArgs e)
    {
        aboutForm ??= new AboutForm();

        if (aboutForm.ShowDialog(this) == DialogResult.OK) return;
    }

    private void MenuSaveAs_Click(object sender, EventArgs e)
    {
        string layoutName = menuLayout.Text.Trim();
        layoutName = string.IsNullOrEmpty(layoutName) ? "Default" : layoutName;
        var prof = new Profile(layoutName, boxes);
        Utils.SaveProfile(profPath, layoutName, prof);
        AddProfile(prof);
    }

    void MenuLoadOpen_Click(object? sender, EventArgs e)
    {
        var input = Utils.OpenLoadFileDialog(profPath);
        if (string.IsNullOrEmpty(input)) return;

        var prof = Utils.LoadProfileFromString<Profile>(input);
        if (prof == default(Profile))
        {
            Utils.Msg($"Menu.MenuLoadOpen_Click : profile from string '{input}' produces empty game");
            return;
        }

        ApplyProfile(prof);
    }

    void ApplyProfile(Profile prof)
    {
        // update boxes from profile
        menuLayout.Text = prof.Name;
        foreach (var pbox in prof.Boxes)
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
        foreach (var pbox in prof.Boxes)
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
        foreach (var obj in menuLoadCollection.DropDownItems)
        {
            if (obj is ToolStripMenuItem item && item.Text == prof.Name)
            {

                menuLoadCollection.DropDownItems.Remove(item);

                var existingProfile = profiles.Find(p => p.Name == prof.Name);
                if (existingProfile != null) profiles.Remove(existingProfile);

                break;
            }
        }

        // add to menu
        int ind = menuLoadCollection.DropDownItems.Count;
        ToolStripMenuItem menuItem = new()
        {
            ForeColor = theme.Text,
            BackColor = theme.Light,
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



