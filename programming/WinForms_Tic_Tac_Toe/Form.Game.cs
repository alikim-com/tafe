namespace WinFormsApp1;

partial class AppForm
{
    struct LabelTexts
    {
        public string left;
        public string right;
        public string choice;
    }

    readonly Dictionary<string, LabelTexts> cfg = new()
    {
        { "", new LabelTexts { left = "", right = "", choice = "CHOOSE\nYOUR\nSIDE" } },
        { "playerLeft", new LabelTexts { left = "YOU", right = "AI", choice = "YOU | AI" } },
        { "playerRight", new LabelTexts { left = "AI", right = "YOU", choice = "AI YOU" } },
    };

    string _player = "";
    string Player
    {
        get => _player;
        set
        {
            _player = value;
            SetLabels(_player);
        }
    }

    void SetLabels(string player)
    {
        //labelChoice.Text = cfg[player].choice;
    }

}
