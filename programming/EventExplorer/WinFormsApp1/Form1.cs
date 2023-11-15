using System.Drawing;
using System.Net;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // disposed by the system

            DrawArrow(g, Color.White, 1, new Point(50, 50), new Point(150, 150), 10);

            DrawRectangle(g, Color.White, 1, new Point(200, 200), 100, 150);

            Font font = new("Arial", 12);
            DrawText(g, Color.Yellow, font, "Hello, world!", new Point(350, 250));

        }

        private void DrawText(Graphics g, Color color, Font font, string text, Point location)
        {
            using Brush brush = new SolidBrush(color);
            g.DrawString(text, font, brush, location);
        }

        static private void DrawRectangle(Graphics g, Color color, int thick, Point location, int width, int height)
        {
            using Pen pen = new(color, thick);
            g.DrawRectangle(pen, location.X, location.Y, width, height);
        }

        static private void DrawArrow(Graphics g, Color color, int thick, Point start, Point end, int head)
        {
            using Pen pen = new(color, thick);
            using Brush brush = new SolidBrush(color);

            PaintArrow(g, pen, brush, start, end, head);
        }

        static private void PaintArrow(Graphics g, Pen pen, Brush brush, Point start, Point end, int size)
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
}