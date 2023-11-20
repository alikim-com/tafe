
using System.Text.RegularExpressions;

namespace WinformsUMLEvents;

public partial class UML_Events : Form
{
    static string curDir = Directory.GetCurrentDirectory();
    static string srcPath = Path.GetFullPath(Path.Combine(curDir, "../../../source/"));
    static string profPath = Path.GetFullPath(Path.Combine(curDir, "../../../profiles/"));

    static public readonly List<ClassBox> boxes = new();

    static Point topLeft = new(10, 10);
    static Size boxClientDef = new(170, 180);
    static Size boxMargin = new(20, 20);

    public UML_Events()
    {
        BackColor = Color.Black;
        InitializeComponent();

        // read profiles from profPath and add to the menu
        RebuildProfileListAndMenu();

        // menu appearance
        menuStrip1.Renderer = new CustomRenderer();
        menuLoad.DropDown.Renderer = new CustomRenderer();
        ((ToolStripDropDownMenu)menuLoad.DropDown).ShowImageMargin = false;
        ((ToolStripDropDownMenu)menuSave.DropDown).ShowImageMargin = false;
        ((ToolStripDropDownMenu)menuHelp.DropDown).ShowImageMargin = false;
    }

    int countdown = 0;
    bool invalidated = false;

    /// <summary>
    /// Ensures the image control is invalidated only once after<br/>
    /// a round of box Draw calls, if needed
    /// </summary>
    /// <param name="invalid">If set true by at least one caller, repaint will happen at zero countdown</param>
    public void UpdatePaintCheck(bool invalid, bool grid = true)
    {
        invalidated |= invalid;
        countdown--;
        if (countdown == 0 && invalidated)
        {
            if (grid) ArrangeBoxesOnGrid();
            pictureBox1.Invalidate();
        }
    }

    /// <summary>
    /// Will check if image control needs enforced painting after<br/>
    /// a number of UpdatePaintCheck calls
    /// </summary>
    /// <param name="cnt">Number of UpdatePaintCheck calls before checking "invalidated" value</param>
    /// <param name="force">Initial "invalidated" value, to force Paint call after countdown</param>
    void PaintCheckAfter(int cnt, bool force = false)
    {
        countdown = cnt;
        invalidated = force;
    }

    void ArrangeBoxesOnGrid()
    {
        int rowHeight = int.MinValue;
        Point curPos = topLeft.Add(boxMargin);
        for (var i = 0; i < boxes.Count; i++)
        {
            var box = boxes[i];
            box.pos = curPos;
            if (i == boxes.Count - 1) break;
            if (rowHeight < box.size.Height) rowHeight = box.size.Height;
            curPos.X += box.size.Width + 2 * boxMargin.Width;
            var next = boxes[i + 1];
            if (curPos.X + next.size.Width + boxMargin.Width + topLeft.X > ClientSize.Width)
            {
                curPos.X = topLeft.X + boxMargin.Width;
                curPos.Y += rowHeight + 2 * boxMargin.Height;
                rowHeight = int.MinValue;
            }
        }
    }

    void Form1_Load(object sender, EventArgs e)
    {
        // read source files

        string[] files = Utils.ReadFolder(srcPath);

        // create boxes

        foreach (var fname in files)
        {
            string boxName = fname[0..^3];

            boxes.Add(new ClassBox(boxName, srcPath, fname, topLeft, boxClientDef, this));
        }

        // update boxes content

        foreach (var box in boxes)
        {
            string code = Utils.ReadFile(box.fpath, box.fname);

            string cleanCode = StripComments(code);

            // find events
            box.events = FindEvents(cleanCode);

            // find events
            box.triggers = FindTriggers(cleanCode);

            // find subscriptions 
            box.subs = FindSubscriptions(cleanCode);

            box.cls = FindClasses(cleanCode);
            if (box.name == "PanelWrapper")
            {
                box.cls.Add(new Item("pwLeft", ""));
                box.cls.Add(new Item("pwRight", ""));

            }
            else if (box.name == "CellWrapper")
            {
                box.cls.Add(new Item("cw", ""));
            }
        }

        // repaint image component if at least one box in DrawFirst is resized
        PaintCheckAfter(boxes.Count);
    }

    static List<Item> FindClasses(string inp)
    {
        string clsPattern = @"class\s+(\w+)";
        MatchCollection matches = Regex.Matches(StripComments(inp), clsPattern, RegexOptions.Singleline);

        List<Item> cls = new();

        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 2)
            {
                cls.Add(new Item(match.Groups[1].Value, ""));
            }
        }
        return cls;
    }

    static Dictionary<Item, Item> FindTriggers(string inp)
    {
        // EM.Raise(EM.Evt.ConfigFinished, new { }, new EventArgs());
        // EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { cfgEndedLabel, Info.None });
        // EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { e }));

        string triggerPattern = @"Raise\((\w+)\.(\w+)\.(\w+),(.*?),\s*(.*?[^,\s])\s*\);";

        MatchCollection matches = Regex.Matches(StripComments(inp), triggerPattern, RegexOptions.Singleline);

        Dictionary<Item, Item> trig = new();
        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 4)
                trig.Add(
                    new(match.Groups[1].Value, match.Groups[2].Value + match.Groups[3].Value),
                    new(match.Groups[1].Value, match.Groups[2].Value + match.Groups[3].Value)
                );
        }
        return trig;
    }

    static Dictionary<Item, List<Item>> FindSubscriptions(string inp)
    {
        //EM.Subscribe(EM.Evt.UpdateLabels, LabelManager.UpdateLabelsHandler);

        string subPattern = @"Subscribe\((\w+)\.(\w+)\.(\w+)\s*,\s*(\w+)\.(\w+)\);";

        MatchCollection matches = Regex.Matches(StripComments(inp), subPattern, RegexOptions.Singleline);

        Dictionary<Item, List<Item>> subs = new();
        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 6)
            {
                var key = new Item(match.Groups[1].Value, match.Groups[2].Value + match.Groups[3].Value);

                if (!subs.ContainsKey(key)) subs.Add(key, new List<Item>());

                var val = new Item(match.Groups[4].Value, match.Groups[5].Value);

                subs[key].Add(val);
            }
        }
        return subs;
    }

    static List<Item> FindEvents(string inp)
    {
        // static event EventHandler<Dictionary<Point, Game.Roster>> EvtSyncBoard
        // static event EventHandler EvtReset = delegate { };

        string eventTypePattern = @"event\s+(EventHandler<.*?>+)\s*(\w+)";
        string eventPattern = @"event\s+(EventHandler)\s+(\w+)";

        MatchCollection matches = Regex.Matches(StripComments(inp), @$"(?:{eventTypePattern})|(?:{eventPattern})", RegexOptions.Singleline);

        List<Item> evt = new();
        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups.Count == 5)
            {
                evt.Add(new Item(match.Groups[2].Value + match.Groups[4].Value, match.Groups[1].Value + match.Groups[3].Value));
                evt[^1].color = ClassBox.NextColor;
            }

        }
        return evt;
    }

    void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics; // disposed by the system

        foreach (var box in boxes) box.Draw(g);

        // connect subs to events
        foreach (var boxFrom in boxes)
        {
            if (boxFrom.firstDraw) return;

            foreach (var sub in boxFrom.subs)
            {
                foreach (var itm in sub.Value)
                {
                    var classTo = itm.name;
                    foreach (var boxTo in boxes)
                    {
                        if (boxTo.cls.FindIndex(itm => itm.name == classTo) != -1)
                        {
                            DrawLineItemToBox(g, boxFrom.pos.Add(itm.offName).Add(-3, 6), boxTo.Anchor, itm.color, 1);
                        }
                    }
                }
            }
        }
    }

    static public Point ClosestAnchor(Point start, ClassBox._anchor ends)
    {
        Point res = new(0, 0);
        int minSQDist = int.MaxValue;
        var endPoints = new Point[] { ends.top, ends.bottom, ends.left, ends.right };
        foreach (var end in endPoints)
        {
            int sq = end.SquaredDistanceTo(start);
            if (sq < minSQDist)
            {
                minSQDist = sq;
                res = end;
            }
        }
        return res;
    }

    static public void DrawLineItemToBox(Graphics g, Point start, ClassBox._anchor ends, Color color, int thick)
    {
        Point end = ClosestAnchor(start, ends);
        DrawLine(g, color, thick, start, end);
        //Msg($"{start} -> {end}");
    }

    static public void DrawText(Graphics g, Color color, Font font, string text, Point location)
    {
        using Brush brush = new SolidBrush(color);
        g.DrawString(text, font, brush, location);
    }

    static public Size MeasureText(Font font, string text)
    {
        using Bitmap bitmap = new(1, 1);
        using Graphics gbmp = Graphics.FromImage(bitmap);
        SizeF textSize = gbmp.MeasureString(text, font);

        return new Size((int)textSize.Width, (int)textSize.Height);
    }

    static public void DrawRectangle(Graphics g, Color color, int thick, Point location, int width, int height)
    {
        using Pen pen = new(color, thick);
        g.DrawRectangle(pen, location.X, location.Y, width, height);
    }

    static public void DrawLine(Graphics g, Color color, int thick, Point start, Point end)
    {
        using Pen pen = new(color, thick);
        using Brush brush = new SolidBrush(color);

        g.DrawLine(pen, start, end);
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

    static string StripComments(string inp)
    {
        var blockComments = @"/\*(.*?)\*/";
        var lineComments = @"//(.*?)\r?\n";
        var strings = @"""((\\[^\n]|[^""\n])*)""";
        var verbatimStrings = @"@(""[^""]*"")+";

        return
        Regex.Replace
        (
            inp,
            blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
            me =>
            {
                if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                    return me.Value.StartsWith("//") ? Environment.NewLine : "";

                return me.Value;
            },
            RegexOptions.Singleline
        );
    }
}


static public class PointExtensions
{
    static public Point Add(this Point pnt, int x, int y) => new(pnt.X + x, pnt.Y + y);
    static public Point Add(this Point pnt, Point add) => new(pnt.X + add.X, pnt.Y + add.Y);
    static public Point Add(this Point pnt, Size add) => new(pnt.X + add.Width, pnt.Y + add.Height);
    static public Point Sub(this Point pnt, int x, int y) => new(pnt.X - x, pnt.Y - y);
    static public Point Sub(this Point pnt, Point add) => new(pnt.X - add.X, pnt.Y - add.Y);
    static public Point Sub(this Point pnt, Size add) => new(pnt.X - add.Width, pnt.Y - add.Height);
    static public int SquaredDistanceTo(this Point pnt, Point dst)
    {
        Point diff = dst.Sub(pnt);
        return diff.X * diff.X + diff.Y * diff.Y;
    }
}

public class Item
{
    readonly public string name;
    readonly public string subName;
    public Color color = Color.Black;

    // offsets from the client top-left for drawing lines
    public Point offName = new(-1, -1);
    public Point offSubname = new(-1, -1);

    public Item(string _name, string _subName)
    {
        name = _name;
        subName = _subName;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Item compareTo) return name == compareTo.name && subName == compareTo.subName;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(name, subName);

    public override string ToString() => $"name: {name}, subName: {subName}";

}

public class ClassBox
{
    static readonly Font fntName = new("Arial", 11);
    static readonly Font fntEvt = new("Arial", 10);
    static readonly Font fntEType = new("Arial", 8);

    static int _ind = -1;
    static readonly Color[] _colors = new Color[]
    {
        Color.Red, Color.Green, Color.MediumSlateBlue, Color.Yellow, Color.Cyan, Color.Orange,
        Color.CadetBlue, Color.Magenta, Color.LightGreen, Color.LightBlue, Color.LightCyan
    };

    static public Color NextColor
    {
        get
        {
            _ind = (++_ind) % _colors.Length;
            return _colors[_ind];
        }
    }

    public Point pos;
    public Size size;

    public struct _anchor
    {
        public Point top;
        public Point bottom;
        public Point left;
        public Point right;
    }

    public _anchor Anchor
    {
        get
        {
            int hw = size.Width / 2;
            int hh = size.Height / 2;
            return new _anchor
            {
                top = pos.Add(hw, 0),
                bottom = pos.Add(hw, size.Height),
                left = pos.Add(0, hh),
                right = pos.Add(size.Width, hh)
            };
        }
    }

    public readonly string fpath;
    public readonly string fname;
    public readonly string name;

    public List<Item> cls = new();

    public List<Item> events = new();

    public Dictionary<Item, List<Item>> subs = new();

    public Dictionary<Item, Item> triggers = new();

    public Action<Graphics> Draw;

    readonly bool autoSize = true;

    readonly UML_Events parent;

    public bool firstDraw = true;

    public string PrintSubs()
    {
        string outp = "";
        foreach (var s in subs)
        {
            outp += s.Key.ToString() + "\n\r";
            foreach (var e in s.Value)
                outp += e.ToString() + "\n\r";
            outp += "\n\r";
        }
        return outp;
    }

    public ClassBox(string _name, string _fpath, string _fname, Point _pos, Size _size, UML_Events _parent)
    {
        name = _name;
        pos = _pos;
        size = _size;
        fpath = _fpath;
        fname = _fname;

        parent = _parent;

        Draw = FirstDraw;
    }

    void FirstDraw(Graphics g)
    {
        Size maxSize = size;

        void UpdateSize(Size sz, Point pos)
        {
            if (!autoSize) return;

            if (pos.X + sz.Width > maxSize.Width) maxSize.Width = sz.Width + pos.X;
            if (pos.Y + sz.Height > maxSize.Height) maxSize.Height = sz.Height + pos.Y;
        }

        // events
        Size sz = new(0, 0);
        Point curPos = pos.Add(20, 30);
        foreach (var evt in events)
        {
            curPos.Y += sz.Height + 5;
            evt.offName = curPos.Sub(pos);

            sz = UML_Events.MeasureText(fntEvt, evt.name);
            UpdateSize(sz, evt.offName);

            curPos.Y += sz.Height;
            evt.offSubname = curPos.Sub(pos);

            sz = UML_Events.MeasureText(fntEType, evt.subName);
            UpdateSize(sz, evt.offSubname);
        }

        // subscriptions
        sz = new(0, 0);
        if (events.Count > 0) curPos.Y += 15;

        foreach (var sub in subs)
        {
            Item? evt = null;
            for (int i = 0; i < UML_Events.boxes.Count; i++)
            {
                var events = UML_Events.boxes[i].events;
                evt = events.Find(evt => evt.name == sub.Key.subName);
                if (evt == null) continue;

                sub.Key.color = evt.color;
                curPos.Y += sz.Height;
                sub.Key.offName = curPos.Sub(pos);

                sz = UML_Events.MeasureText(fntEvt, $"SUB {sub.Key.name}.{sub.Key.subName}");
                UpdateSize(sz, sub.Key.offName);

                curPos.X += 5;
                curPos.Y += sz.Height;

                foreach (var s in sub.Value)
                {
                    s.color = evt.color;
                    s.offName = curPos.Sub(pos);

                    sz = UML_Events.MeasureText(fntEType, $"{s.name}.{s.subName}");
                    UpdateSize(sz, s.offName);

                    curPos.Y += sz.Height;
                }
                curPos.X -= 5;

                break;
            }
            if (evt == null)
            {
                Utils.Msg($"ClassBox.FirstDraw : SUB for unknown event '{sub.Key}'");
            }
        }

        // triggers
        sz = new(0, 0);
        if (subs.Count > 0) curPos.Y += 15;

        foreach (var trig in triggers)
        {
            Item? evt = null;
            for (int i = 0; i < UML_Events.boxes.Count; i++)
            {
                var events = UML_Events.boxes[i].events;
                evt = events.Find(evt => evt.name == trig.subName);
                if (evt == null) continue;

                trig.color = evt.color;
                trig.offName = curPos.Sub(pos);

                sz = UML_Events.MeasureText(fntEvt, $"{trig.name}.{trig.subName}");
                UpdateSize(sz, trig.offName);

                curPos.Y += sz.Height;
                break;
            }
            if (evt == null)
            {
                Utils.Msg($"ClassBox.FirstDraw : trigger for unknown event '{trig.subName}'");
            }
        }

        // schedule repaint
        bool updatePaintCheck = false;
        if (autoSize)
        {
            if (maxSize.Width > size.Width)
            {
                size.Width = maxSize.Width + 10;
                updatePaintCheck = true;
            }
            if (maxSize.Height > size.Height)
            {
                size.Height = maxSize.Height + 10;
                updatePaintCheck = true;
            }
        }
        parent.UpdatePaintCheck(updatePaintCheck);

        Draw = FastDraw;
    }

    void FastDraw(Graphics g)
    {
        firstDraw = false;

        // box
        UML_Events.DrawRectangle(g, Color.Gray, 1, pos, size.Width, size.Height);

        // box name
        UML_Events.DrawText(g, Color.LightGray, fntName, name, pos.Add(10, 10));

        // events
        foreach (var evt in events)
        {
            UML_Events.DrawText(g, evt.color, fntEvt, evt.name, pos.Add(evt.offName));

            UML_Events.DrawText(g, evt.color, fntEType, evt.subName, pos.Add(evt.offSubname));
        }

        // subscriptions
        foreach (var sub in subs)
        {
            Item? evt = null;
            for (int i = 0; i < UML_Events.boxes.Count; i++)
            {
                var events = UML_Events.boxes[i].events;
                evt = events.Find(evt => evt.name == sub.Key.subName);
                if (evt == null) continue;

                UML_Events.DrawText(g, sub.Key.color, fntEvt, $"SUB {sub.Key.name}.{sub.Key.subName}", pos.Add(sub.Key.offName));

                foreach (var s in sub.Value)
                    UML_Events.DrawText(g, s.color, fntEType, $"{s.name}.{s.subName}", pos.Add(s.offName));

                break;
            }
        }

        // triggers
        foreach(var trig in triggers)
            UML_Events.DrawText(g, trig.color, fntEvt, $"{trig.name}.{trig.subName}", pos.Add(trig.offName));
    }
}