
namespace WinformsUMLEvents;

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

    private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
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

    private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
        if (sender is PictureBox pbox) 
            pbox.Cursor = DetectBox(e.Location, out ClassBox? _) ? Cursors.Hand : Cursors.Default;

        if (mState == MState.Up || mBox == null) return; // "|| mBox == null" to supress CS8602

        Point diff = e.Location.Sub(mPos);
        mBox.pos = mBox.pos.Add(diff);
        mPos = e.Location;
        pictureBox1.Invalidate();

    }

    private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            mState = MState.Up;
        }
    }

    private void PictureBox1_MouseLeave(object sender, EventArgs e)
    {
        mState = MState.Up;
    }

}