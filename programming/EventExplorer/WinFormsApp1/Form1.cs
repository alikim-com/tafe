
using System.Text.RegularExpressions;

namespace WinFormsApp1;

public class ClassBox // SAVE EVT WITH COLOR
{
    static readonly Font fntName = new("Arial", 11);
    static readonly Font fntEvt = new("Arial", 10);
    static readonly Color[] colors = new Color[]
    {
        Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Cyan, Color.Orange, 
        Color.Beige, Color.Purple, Color.LightGreen, Color.LightBlue, Color.LightCyan
    };

    Point pos;
    Size size;
    public readonly string fpath = "";
    public readonly string fname = "";
    public readonly string name = "";
    public List<string> events = new();

    public ClassBox(string _name, string _fpath, string _fname, Point _pos, Size _size) 
    {
        name = _name;
        pos = _pos;
        size = _size;
        fpath = _fpath;
        fname = _fname;
    }

    public void Draw(Graphics g)
    {
        Form1.DrawRectangle(g, Color.Gray, 1, pos, size.Width, size.Height);

        Form1.DrawText(g, Color.Cyan, fntName, name, new Point(pos.X + 10, pos.Y + 10));

        int ind = 0;
        foreach (var evt in events)
        {
            Form1.DrawText(g, colors[ind], fntEvt, evt, new Point(pos.X, pos.Y + 30 + 15 * ind));
            if((++ind) == events.Count) ind = 0;
        }
    }
}

public partial class Form1 : Form
{
    static void Msg(object obj) => MessageBox.Show(obj.ToString());

    static void Msg(object[] obj)
    {
        foreach (var o in obj)
            MessageBox.Show(o.ToString());
    }

    static public List<ClassBox> boxes = new();

    public Form1()
    {
        InitializeComponent();
    }

    void Form1_Load(object sender, EventArgs e)
    {
        string path = "../../../source/";

        boxes.Add(new ClassBox("EventManager", path, "EventManager.cs", new Point(50, 50), new Size(200, 300)));

        string eventPattern = @"enum\s+Evt\s*{(.*?)}";

        foreach (var box in boxes)
        {
            MatchCollection matches = 
                Regex.Matches(ReadFile(box.fpath, box.fname), eventPattern, RegexOptions.Singleline);

            string res = "";
            foreach (Match match in matches.Cast<Match>())
                if (match.Groups.Count > 1)
                    res += match.Groups[1].Value;

            box.events = res.Split(",").ToList<string>();
        }
        

    }

    static string ReadFile(string path, string name)
    {
        try
        {
            return File.ReadAllText(Path.Combine(path, name));
        }
        catch (Exception ex)
        {
            Msg($"An error occurred: {ex.Message}");
            return "";
        }
    }

    void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics; // disposed by the system

        foreach (var box in boxes) box.Draw(g);

        // test
        //DrawArrow(g, Color.White, 1, new Point(50, 50), new Point(150, 150), 10);
        //DrawRectangle(g, Color.White, 1, new Point(200, 200), 100, 150);
        //Font font = new("Arial", 12);
        //DrawText(g, Color.Yellow, font, "Hello, world!", new Point(350, 250));

    }

    static public Size DrawText(Graphics g, Color color, Font font, string text, Point location)
    {
        using Bitmap bitmap = new(1, 1);
        using Graphics gbmp = Graphics.FromImage(bitmap);
        SizeF textSize = gbmp.MeasureString(text, font);

        using Brush brush = new SolidBrush(color);
        g.DrawString(text, font, brush, location);

        return new Size((int)textSize.Width, (int)textSize.Height);
    }

    static public void DrawRectangle(Graphics g, Color color, int thick, Point location, int width, int height)
    {
        using Pen pen = new(color, thick);
        g.DrawRectangle(pen, location.X, location.Y, width, height);
    }

    static public void DrawArrow(Graphics g, Color color, int thick, Point start, Point end, int head)
    {
        using Pen pen = new(color, thick);
        using Brush brush = new SolidBrush(color);

        PaintArrow(g, pen, brush, start, end, head);
    }

    static void PaintArrow(Graphics g, Pen pen, Brush brush, Point start, Point end, int size)
    {
        g.DrawLine(pen, start, end);

        PointF[] arrowhead = new PointF[3];
        double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        double PIo6 = Math.PI / 6;
        double angMPI = angle - PIo6;
        double angPPI = angle + PIo6;
        float sinMPI = (float)Math.Sin(angMPI);
        float sinPPI = (float)Math.Sin(angPPI);
        float cosMPI = (float)Math.Cos(angMPI);
        float cosPPI = (float)Math.Cos(angPPI);
        arrowhead[0] = new PointF(end.X, end.Y);
        arrowhead[1] = new PointF(end.X - size * cosMPI, end.Y - size * sinMPI);
        arrowhead[2] = new PointF(end.X - size * cosPPI, end.Y - size * sinPPI);

        g.FillPolygon(brush, arrowhead);
    }
}