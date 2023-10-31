namespace WinFormsApp1;

internal class TokenButton : Button
{
    protected override void OnPaint(PaintEventArgs e)
    {
        // Fill the button with a transparent color
        using (SolidBrush brush = new SolidBrush(BackColor))
        {
            e.Graphics.FillRectangle(brush, ClientRectangle);
        }

        // Draw the button's text
        //TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
    }
}


