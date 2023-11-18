
namespace WinFormsApp1;

public partial class UML_Events
{

    enum MState
    {
        Up,
        Down
    }

    MState mState = MState.Up;
    Point mPos = new(0, 0);

    ClassBox? mBox;

    static bool DetectBox(Point pos, out ClassBox? boxOver)
    {
        var x = pos.X;
        var y = pos.Y;
        foreach(var box in boxes)
        {
            var bx = box.pos.X;
            var by = box.pos.Y;
            if (
            x > bx && x < bx + box.size.Width &&
            y > by && y < by + box.size.Height
            ) {
                boxOver = box;
                return true;
            }
        }
        boxOver = null;
        return false;
    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            mPos = e.Location;
            if (!DetectBox(mPos, out ClassBox? box)) return;
            // clicked on a box
            mBox = box;
            mState = MState.Down;
        }
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        if (mState == MState.Up || mBox == null) return;
        Point diff = e.Location.Sub(mPos);
        mBox.pos = mBox.pos.Add(diff);
        mPos = e.Location;
        pictureBox1.Invalidate();

    }

    private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) mState = MState.Up;
    }

    private void pictureBox1_MouseLeave(object sender, EventArgs e)
    {
        mState = MState.Up;
    }

}