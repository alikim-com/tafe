namespace WinFormsApp1;

partial class FormAspect
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAspect));
        tpl = new TableLayoutPanel();
        button1 = new Button();
        tbox = new TextBox();
        panel = new Panel();
        pLeft = new PictureBox();
        pRight = new PictureBox();
        panel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pLeft).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pRight).BeginInit();
        SuspendLayout();
        // 
        // tpl
        // 
        tpl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tpl.BackColor = Color.FromArgb(180, 0, 0, 0);
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
        // 
        // button1
        // 
        button1.BackColor = Color.Transparent;
        button1.Cursor = Cursors.Hand;
        button1.Dock = DockStyle.Fill;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Location = new Point(11, 11);
        button1.Margin = new Padding(10);
        button1.Name = "button1";
        button1.Size = new Size(86, 85);
        button1.TabIndex = 0;
        button1.UseVisualStyleBackColor = true;
        // 
        // tbox
        // 
        tbox.Anchor = AnchorStyles.Top;
        tbox.BackColor = SystemColors.Desktop;
        tbox.Location = new Point(40, 11);
        tbox.Name = "tbox";
        tbox.Size = new Size(324, 23);
        tbox.TabIndex = 1;
        // 
        // panel
        // 
        panel.BackColor = Color.FromArgb(196, 0, 0, 0);
        panel.Controls.Add(pLeft);
        panel.Controls.Add(pRight);
        panel.Dock = DockStyle.Bottom;
        panel.Location = new Point(0, 390);
        panel.Name = "panel";
        panel.Size = new Size(400, 210);
        panel.TabIndex = 2;
        // 
        // pLeft
        // 
        pLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        pLeft.BackColor = Color.FromArgb(0, 0, 0, 0);
        pLeft.BackgroundImageLayout = ImageLayout.None;
        pLeft.Cursor = Cursors.Hand;
        pLeft.Location = new Point(0, 0);
        pLeft.Name = "pLeft";
        pLeft.Size = new Size(200, 210);
        pLeft.TabIndex = 0;
        pLeft.TabStop = false;
        // 
        // pRight
        // 
        pRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        pRight.BackColor = Color.FromArgb(0, 0, 0, 0);
        pRight.BackgroundImageLayout = ImageLayout.None;
        pRight.Cursor = Cursors.Hand;
        pRight.Location = new Point(200, 0);
        pRight.Name = "pRight";
        pRight.Size = new Size(200, 210);
        pRight.TabIndex = 1;
        pRight.TabStop = false;
        // 
        // FormAspect
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(400, 600);
        Controls.Add(tbox);
        Controls.Add(tpl);
        Controls.Add(panel);
        MaximizeBox = false;
        Name = "FormAspect";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tic-tac-toe";
        Load += FormAspect_Load;
        panel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pLeft).EndInit();
        ((System.ComponentModel.ISupportInitialize)pRight).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tpl;
    private TextBox tbox;
    private Button button1;
    private Panel panel;
    private PictureBox pLeft;
    private PictureBox pRight;
}