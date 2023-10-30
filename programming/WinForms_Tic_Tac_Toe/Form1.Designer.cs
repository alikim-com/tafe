namespace WinFormsApp1;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tpl = new TableLayoutPanel();
        tbox = new TextBox();
        SuspendLayout();
        // 
        // tpl
        // 
        tpl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tpl.BackColor = SystemColors.Desktop;
        tpl.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tpl.ColumnCount = 3;
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.Location = new Point(40, 40);
        tpl.Name = "tpl";
        tpl.RowCount = 3;
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.Size = new Size(324, 320);
        tpl.TabIndex = 0;
        tpl.SizeChanged += tpl_SizeChanged;
        // 
        // tbox
        // 
        tbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tbox.BackColor = SystemColors.Desktop;
        tbox.Location = new Point(40, 540);
        tbox.Name = "tbox";
        tbox.Size = new Size(324, 23);
        tbox.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 600);
        Controls.Add(tbox);
        Controls.Add(tpl);
        MaximizeBox = false;
        Name = "Form1";
        Text = "Tic-tac-toe";
        Load += Form1_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tpl;
    private TextBox tbox;
}