namespace WinFormsApp1;

partial class AppForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppForm));
        tLayout = new TableLayoutPanel();
        pLeft = new Panel();
        pRight = new Panel();
        tLayout.SuspendLayout();
        SuspendLayout();
        // 
        // tLayout
        // 
        tLayout.BackColor = Color.Transparent;
        tLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tLayout.ColumnCount = 3;
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
        tLayout.Controls.Add(pLeft, 0, 4);
        tLayout.Controls.Add(pRight, 2, 4);
        tLayout.Dock = DockStyle.Fill;
        tLayout.Location = new Point(0, 0);
        tLayout.Margin = new Padding(0);
        tLayout.Name = "tLayout";
        tLayout.RowCount = 6;
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 9.803922F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 24.5098038F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 5.098039F));
        tLayout.Size = new Size(400, 600);
        tLayout.TabIndex = 0;
        // 
        // pLeft
        // 
        pLeft.BackgroundImageLayout = ImageLayout.Stretch;
        pLeft.Cursor = Cursors.Hand;
        pLeft.Dock = DockStyle.Fill;
        pLeft.Location = new Point(1, 420);
        pLeft.Margin = new Padding(0);
        pLeft.Name = "pLeft";
        pLeft.Size = new Size(131, 145);
        pLeft.TabIndex = 0;
        pLeft.Click += pLeft_Click;
        // 
        // pRight
        // 
        pRight.BackgroundImageLayout = ImageLayout.Stretch;
        pRight.Cursor = Cursors.Hand;
        pRight.Dock = DockStyle.Fill;
        pRight.Location = new Point(266, 420);
        pRight.Margin = new Padding(0);
        pRight.Name = "pRight";
        pRight.Size = new Size(133, 145);
        pRight.TabIndex = 1;
        pRight.Click += pRight_Click;
        // 
        // AppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 0);
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(400, 600);
        Controls.Add(tLayout);
        MaximizeBox = false;
        Name = "AppForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tic-Tac-Toe";
        Load += FormAspect_Load;
        tLayout.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tLayout;
    private Panel pLeft;
    private Panel pRight;
}