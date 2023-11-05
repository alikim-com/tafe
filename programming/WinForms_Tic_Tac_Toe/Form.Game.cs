namespace WinFormsApp1;

static class Game
{

    static string player = "";

    struct LabelTexts
    {
        public string left;
        public string right;
        public string choice;
    }
    // ꙲
    static readonly Dictionary<string, LabelTexts> cfgInfo = new()
    {
        { "start", new LabelTexts { left = "", right = "", choice = "CHOOSE\nYOUR\nSIDE" } },
        { "playerLeft", new LabelTexts { left = "YOU", right = "AI", choice = "YOU  -vs-  AI   " } },
        { "playerRight", new LabelTexts { left = "AI", right = "YOU", choice = "   AI  -vs-  YOU" } },
    };

    static readonly Dictionary<string, Control> cfgCtrl = new();

    static public void Init(Control left, Control right, Control choice)
    {
        cfgCtrl.Add("left", left);
        cfgCtrl.Add("right", right);
        cfgCtrl.Add("choice", choice);
        State = "start";
    }

    static public string State
    {
        set
        {

            if (value == "start") 
            { 
                
            }
            else if (value == "playerLeft")
            {
                player = "left";
            }
            else if (value == "playerRight")
            {
                player = "right";
            }
            SetLabels(value);

        }
    }

    static void SetLabels(string state)
    {
        cfgCtrl["left"].Text = cfgInfo[state].left;
        cfgCtrl["right"].Text = cfgInfo[state].right;
        cfgCtrl["choice"].Text = cfgInfo[state].choice;
    }

}
