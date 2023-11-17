
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

    static bool DetectBox(Point pos, out ClassBox? box)
    {
        box = boxes[0];
        return true;
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
        mBox.pos = mBox.pos.Add(diff); Msg($"{mBox.pos.Add(diff).X}->{mBox.pos.X} ");
        PositionBoxes();
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