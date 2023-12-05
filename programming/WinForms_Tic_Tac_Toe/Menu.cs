
namespace WinFormsApp1;

partial class AppForm
{
    static readonly string curDir = Directory.GetCurrentDirectory();
    static readonly string profPath = Path.GetFullPath(Path.Combine(curDir, "../../../savegame/"));

    AboutForm? aboutForm;

    readonly List<SaveGame> profiles = new();

    private void MenuHelpAbout_Click(object sender, EventArgs e)
    {
        aboutForm ??= new AboutForm();

        if (aboutForm.ShowDialog(this) == DialogResult.OK) return;
    }

    private void MenuSaveAs_Click(object? sender, EventArgs e)
    {
        string layoutName = menuLayout.Text.Trim();
        layoutName = string.IsNullOrEmpty(layoutName) ? "Default" : layoutName;
        var prof = new SaveGame(
            layoutName,
            Game.board.ToArray(),
            Game.TurnList,
            Game.state,
            TurnWheel.Head
        );
        Utils.SaveProfile(profPath, layoutName, prof);
       // AddProfile(prof);
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
        //foreach (var pbox in prof.Boxes)
       // {
           // var box = boxes.Find(b => b.name == pbox.Name);
            //if (box == null) continue;

            //box.pos = pbox.Pos;
            //box.size = pbox.Size;
        //}

        // force repaint, no grid positioning
     //   PaintCheckAfter(1);
     //   UpdatePaintCheck(true, false);
    }

    void AddProfile(SaveGame prof)
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

        //var pfiles = Utils.ReadFolder(profPath);

        //foreach (var file in pfiles)
        //{
        //    var prof = Utils.LoadProfile<Profile>(file, profPath);
        //    if (prof == null) continue;

        //    AddProfile(prof);
        //}
    }
}

class SaveGame
{
    public string Name { get; set; } = "";
    public Game.Roster[] Board { get; set; } = Array.Empty<Game.Roster>();
    public Game.Roster[] TurnList { get; set; } = Array.Empty<Game.Roster>();
    public Game.State State { get; set; }
    public int TurnWheelHead { get; set; }

    internal SaveGame(
        string _name,
        Game.Roster[] _board, 
        Game.Roster[] _turnList, 
        Game.State _state,
        int _turnWheelHead
    ){
        Name = _name;

        var len = _board.Length;
        Board = new Game.Roster[len];
        Array.Copy(_board, Board, len);

        len = _turnList.Length;
        TurnList = new Game.Roster[len];
        Array.Copy(_turnList, TurnList, len);

        State = _state;
        TurnWheelHead = _turnWheelHead;
    }

    internal SaveGame()
    {
        
    }
}



