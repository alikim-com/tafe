
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
        tLayout = new TableLayoutPanel();
        info = new Label();
        menuStrip1 = new MenuStrip();
        menuLoad = new ToolStripMenuItem();
        menuLoadOpen = new ToolStripMenuItem();
        menuLoadCollection = new ToolStripMenuItem();
        menuSave = new ToolStripMenuItem();
        menuSaveAs = new ToolStripMenuItem();
        menuHelp = new ToolStripMenuItem();
        menuHelpAbout = new ToolStripMenuItem();
        menuLabel = new ToolStripLabel();
        menuDummy = new ToolStripTextBox();
        menuLayout = new ToolStripTextBox();
        tLayout.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // tLayout
        // 
        tLayout.BackColor = Color.Transparent;
        tLayout.ColumnCount = 3;
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
        tLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
        tLayout.Controls.Add(info, 0, 3);
        tLayout.Dock = DockStyle.Fill;
        tLayout.Location = new Point(0, 27);
        tLayout.Margin = new Padding(0);
        tLayout.Name = "tLayout";
        tLayout.RowCount = 5;
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
        tLayout.Size = new Size(500, 723);
        tLayout.TabIndex = 0;
        // 
        // info
        // 
        tLayout.SetColumnSpan(info, 3);
        info.Dock = DockStyle.Fill;
        info.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        info.ForeColor = Color.FromArgb(0, 255, 255, 255);
        info.Location = new Point(0, 477);
        info.Margin = new Padding(0);
        info.Name = "info";
        info.Size = new Size(500, 43);
        info.TabIndex = 3;
        info.TextAlign = ContentAlignment.TopCenter;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuLoad, menuSave, menuHelp, menuLabel, menuDummy, menuLayout });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(500, 27);
        menuStrip1.TabIndex = 2;
        menuStrip1.Text = "menuMain";
        // 
        // menuLoad
        // 
        menuLoad.DropDownItems.AddRange(new ToolStripItem[] { menuLoadOpen, menuLoadCollection });
        menuLoad.Name = "menuLoad";
        menuLoad.Size = new Size(45, 23);
        menuLoad.Text = "Load";
        // 
        // menuLoadOpen
        // 
        menuLoadOpen.Name = "menuLoadOpen";
        menuLoadOpen.Size = new Size(165, 22);
        menuLoadOpen.Text = "Open layout...";
        // 
        // menuLoadCollection
        // 
        menuLoadCollection.Name = "menuLoadCollection";
        menuLoadCollection.Size = new Size(165, 22);
        menuLoadCollection.Text = "Layout collection";
        // 
        // menuSave
        // 
        menuSave.DropDownItems.AddRange(new ToolStripItem[] { menuSaveAs });
        menuSave.Name = "menuSave";
        menuSave.Size = new Size(43, 23);
        menuSave.Text = "Save";
        // 
        // menuSaveAs
        // 
        menuSaveAs.Name = "menuSaveAs";
        menuSaveAs.Size = new Size(157, 22);
        menuSaveAs.Text = "Save layout as...";
        // 
        // menuHelp
        // 
        menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuHelpAbout });
        menuHelp.Name = "menuHelp";
        menuHelp.Size = new Size(44, 23);
        menuHelp.Text = "Help";
        // 
        // menuHelpAbout
        // 
        menuHelpAbout.Name = "menuHelpAbout";
        menuHelpAbout.Size = new Size(107, 22);
        menuHelpAbout.Text = "About";
        // 
        // menuLabel
        // 
        menuLabel.Margin = new Padding(50, 1, 5, 2);
        menuLabel.Name = "menuLabel";
        menuLabel.Size = new Size(46, 20);
        menuLabel.Text = "Layout:";
        // 
        // menuDummy
        // 
        menuDummy.Enabled = false;
        menuDummy.Margin = new Padding(1, 0, 0, 0);
        menuDummy.Name = "menuDummy";
        menuDummy.Size = new Size(7, 23);
        // 
        // menuLayout
        // 
        menuLayout.Margin = new Padding(0, 0, 1, 0);
        menuLayout.Name = "menuLayout";
        menuLayout.Size = new Size(100, 23);
        menuLayout.Text = "Default";
        // 
        // AppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 0);
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(500, 750);
        Controls.Add(tLayout);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        MaximizeBox = false;
        Name = "AppForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tic-Tac-Toe";
        Load += FormAspect_Load;
        tLayout.ResumeLayout(false);
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tLayout;
    private Panel[,] cells;
    private Label info;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem menuSave;
    private ToolStripMenuItem menuLoad;
    private ToolStripMenuItem menuHelp;
    private ToolStripMenuItem menuLoadOpen;
    private ToolStripMenuItem menuLoadCollection;
    private ToolStripMenuItem menuSaveAs;
    private ToolStripMenuItem menuHelpAbout;
    private ToolStripLabel menuLabel;
    private ToolStripTextBox menuLayout;
    private ToolStripTextBox menuDummy;
}