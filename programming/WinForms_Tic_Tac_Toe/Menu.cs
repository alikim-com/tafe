
namespace WinFormsApp1;

partial class AppForm
{
    static readonly string curDir = Directory.GetCurrentDirectory();
    static readonly string profPath = Path.GetFullPath(Path.Combine(curDir, "../../../savegame/"));

    AboutForm? aboutForm;

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
            TurnWheel.Head,
            chosenArr
        );
        Utils.SaveProfile(profPath, layoutName, prof);
        AddProfileToMenu(prof);
    }

    void MenuLoadCollection_Click(string pname)
    {
        var prof = Utils.LoadProfileByName<SaveGame>(pname, profPath);
        if (prof == default(SaveGame))
        {
            Utils.Msg($"Menu.LoadProfile : profile '{pname}' appears to be empty");
            return;
        }

        if(LoadGame(prof))
            menuLayout.Text = pname;
        else
            Utils.Msg($"Menu.LoadProfile : profile '{pname}' appears to be empty");

    }

    void AddProfileToMenu(SaveGame prof)
    {
        foreach (var obj in menuLoadCollection.DropDownItems)
            if (obj is ToolStripMenuItem item && item.Text == prof.Name) return;

        // add to menu
        int ind = menuLoadCollection.DropDownItems.Count;
        ToolStripMenuItem menuItem = new()
        {
            ForeColor = theme.Text,
            BackColor = theme.Light,
            Name = $"menuLoadCollection{ind}",
            Text = prof.Name,
        };
        menuItem.Click += (object? sender, EventArgs e) => { MenuLoadCollection_Click(prof.Name); };

        menuLoadCollection.DropDownItems.Add(menuItem);
    }

    void RebuildProfileListAndMenu()
    {
        //profiles.Clear();
        //menuLoadCollection.DropDownItems.Clear();

        //var pfiles = Utils.ReadFolder(profPath);

        //foreach (var file in pfiles)
        //{
        //    var prof = Utils.LoadProfile<Profile>(file, profPath);
        //    if (prof == null) continue;

        //    AddProfile(prof);
        //}
    }
}

class SaveGame : Utils.INamedProfile
{
    public string Name { get; set; } = "";
    public Game.Roster[] Board { get; set; } = Array.Empty<Game.Roster>();
    public Game.Roster[] TurnList { get; set; } = Array.Empty<Game.Roster>();
    public Game.State State { get; set; }
    public int TurnWheelHead { get; set; }

    public IEnumerable<ChoiceItem> chosen = Enumerable.Empty<ChoiceItem>();

    internal SaveGame(
        string _name,
        Game.Roster[] _board,
        Game.Roster[] _turnList,
        Game.State _state,
        int _turnWheelHead,
        IEnumerable<ChoiceItem> _chosen
    )
    {
        Name = _name;

        var len = _board.Length;
        Board = new Game.Roster[len];
        Array.Copy(_board, Board, len);

        len = _turnList.Length;
        TurnList = new Game.Roster[len];
        Array.Copy(_turnList, TurnList, len);

        State = _state;
        TurnWheelHead = _turnWheelHead;

        chosen = _chosen;
    }

    internal SaveGame()
    {

    }
}



