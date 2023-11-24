
namespace uiRenderer;

public class UIFonts
{
    static readonly public Font Default = new("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);

    static readonly public Font header = new("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
    static readonly public Font title = new("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
    static readonly public Font regular = new("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point);
}

public class UIColors
{
    static readonly public Color Black = Color.FromArgb(0, 0, 0);
    static readonly public Color Transparent = Color.FromArgb(0, 0, 0, 0);

    public struct ColorTheme
    {
        public Color Pitch;
        public Color Dark;
        public Color Prime;
        public Color Dawn;
        public Color Light;
        public Color Noon;
        public Color Accent;
        public Color Text;
    }

    static public readonly ColorTheme Steel = new()
    {
        Pitch = Color.FromArgb(23, 23, 26),
        Dark = Color.FromArgb(34, 34, 38),
        Prime = Color.FromArgb(46, 46, 51),
        Dawn = Color.FromArgb(57, 57, 64),
        Light = Color.FromArgb(69, 69, 77),
        Noon = Color.FromArgb(80, 80, 89),
        Accent = Color.FromArgb(115, 115, 128),
        Text = Color.FromArgb(200, 200, 200),
    };
}

public class ColorTableMain : ProfessionalColorTable
{
    UIColors.ColorTheme theme;

    // menu item mouseOver border color
    public override Color MenuItemBorder => theme.Dark;

    // menu items mouseOver bg color
    public override Color MenuItemSelected => theme.Light;
    public override Color MenuItemSelectedGradientBegin => theme.Light;
    public override Color MenuItemSelectedGradientEnd => theme.Light;

    // menu items pressed (dropdown opened) color
    public override Color MenuItemPressedGradientBegin => theme.Light;
    public override Color MenuItemPressedGradientEnd => theme.Light;

    // menu dropdown bg color
    public override Color ToolStripDropDownBackground => theme.Dawn;

    // menu items pressed(dropdown opened) border color AND dropdown border color
    public override Color MenuBorder => theme.Accent;

    public ColorTableMain(UIColors.ColorTheme _theme)
    {
        theme = _theme;
    }
}

public class ToolStripRendererOverride : ToolStripProfessionalRenderer
{
    UIColors.ColorTheme theme;

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        e.ArrowColor = theme.Text;  
        base.OnRenderArrow(e);
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        ToolStripItem item = e.Item;
        Graphics g = e.Graphics;

        if (!item.IsOnDropDown || !item.Selected)
        {
            base.OnRenderMenuItemBackground(e);
            return;

        } else {
            using var brush = new SolidBrush(theme.Noon);
            Rectangle rc = new(Point.Empty, item.Size);
            g.FillRectangle(brush, rc);
            // g.DrawRectangle(Pens.Black, 1, 0, rc.Width - 2, rc.Height - 1);
        }
    }

    public ToolStripRendererOverride(ProfessionalColorTable colorTable, UIColors.ColorTheme _theme) : base(colorTable)
    {
        theme = _theme;
    }
}

public class UIRenderer
{
    static public ToolStripRendererOverride TSRenderer(UIColors.ColorTheme _theme, string colorTableName)
    {
        ProfessionalColorTable colorTable = colorTableName switch
        {
            "ColorTableMain" => new ColorTableMain(_theme),
            _ => throw new NotImplementedException($"UIRenderer.ToolStripRendererOverride : color table '{colorTableName}' does not exist"),
        };
        return new ToolStripRendererOverride(colorTable, _theme);
    }

}
