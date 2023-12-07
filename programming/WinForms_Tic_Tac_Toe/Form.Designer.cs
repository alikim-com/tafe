
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
        labelVS = new Label();
        labelLeft = new Label();
        labelRight = new Label();
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
        toolStripButton = new ToolStrip();
        toolStripButtonLabel = new ToolStripLabel();
        tLayout.SuspendLayout();
        menuStrip1.SuspendLayout();
        toolStripButton.SuspendLayout();
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
        tLayout.Controls.Add(labelVS, 1, 4);
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
        info.Location = new Point(0, 477);
        info.Margin = new Padding(0);
        info.Name = "info";
        info.Size = new Size(500, 43);
        info.TabIndex = 3;
        info.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // labelVS
        // 
        labelVS.AutoSize = true;
        labelVS.Dock = DockStyle.Fill;
        labelVS.Location = new Point(166, 520);
        labelVS.Margin = new Padding(0);
        labelVS.Name = "labelVS";
        labelVS.Size = new Size(166, 203);
        labelVS.TabIndex = 4;
        labelVS.Text = "VS";
        labelVS.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // labelLeft
        // 
        labelLeft.AutoSize = true;
        labelLeft.Location = new Point(0, 520);
        labelLeft.Margin = new Padding(0);
        labelLeft.Name = "labelLeft";
        labelLeft.Size = new Size(52, 15);
        labelLeft.TabIndex = 4;
        labelLeft.Text = "labelLeft";
        // 
        // labelRight
        // 
        labelRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        labelRight.AutoSize = true;
        labelRight.Location = new Point(440, 708);
        labelRight.Margin = new Padding(0);
        labelRight.Name = "labelRight";
        labelRight.Size = new Size(60, 15);
        labelRight.TabIndex = 4;
        labelRight.Text = "labelRight";
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
        menuLoadOpen.Size = new Size(180, 22);
        menuLoadOpen.Text = "Open saved game...";
        menuLoadOpen.Click += MenuLoadOpen_Click;
        // 
        // menuLoadCollection
        // 
        menuLoadCollection.Name = "menuLoadCollection";
        menuLoadCollection.Size = new Size(180, 22);
        menuLoadCollection.Text = "Saved games";
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
        menuSaveAs.Size = new Size(180, 22);
        menuSaveAs.Text = "Save game as...";
        menuSaveAs.Click += MenuSaveAs_Click;
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
        menuHelpAbout.Size = new Size(180, 22);
        menuHelpAbout.Text = "About";
        menuHelpAbout.Click += MenuHelpAbout_Click;
        // 
        // menuLabel
        // 
        menuLabel.Margin = new Padding(50, 1, 5, 2);
        menuLabel.Name = "menuLabel";
        menuLabel.Size = new Size(46, 20);
        menuLabel.Text = "Game name:";
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
        // toolStripButton
        // 
        toolStripButton.Anchor = AnchorStyles.None;
        toolStripButton.AutoSize = false;
        toolStripButton.Dock = DockStyle.None;
        toolStripButton.GripMargin = new Padding(0);
        toolStripButton.GripStyle = ToolStripGripStyle.Hidden;
        toolStripButton.Items.AddRange(new ToolStripItem[] { toolStripButtonLabel });
        toolStripButton.Location = new Point(190, 500);
        toolStripButton.Name = "toolStripButton";
        toolStripButton.Padding = new Padding(0);
        toolStripButton.CanOverflow = false;
        toolStripButton.Size = new Size(100, 39);
        toolStripButton.TabIndex = 3;
        // 
        // toolStripButtonLabel
        // 
        toolStripButtonLabel.TextAlign = ContentAlignment.MiddleLeft;
        toolStripButtonLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
        toolStripButtonLabel.Margin = new Padding(12,7,0,7);
        toolStripButtonLabel.Name = "toolStripButtonLabel";
        //toolStripButtonLabel.Size = new Size(64, 39);
        toolStripButtonLabel.Text = "New game";
        // 
        // AppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(64, 0, 64);
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(500, 750);
        Controls.Add(toolStripButton);
        Controls.Add(labelLeft);
        Controls.Add(labelRight);
        Controls.Add(tLayout);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        MaximizeBox = false;
        Name = "AppForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tic-Tac-Toe";
        Load += FormAspect_Load;
        ClientSizeChanged += FormAspect_ClientSizeChanged;
        tLayout.ResumeLayout(false);
        tLayout.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        toolStripButton.ResumeLayout(false);
        toolStripButton.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private void ToolStripButton_MouseLeave(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ToolStripButton_MouseEnter(object sender, EventArgs e)
    {
        throw new NotImplementedException();
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
    private Label labelLeft;
    private Label labelRight;
    private Label labelVS;
    private ToolStrip toolStripButton;
    private ToolStripLabel toolStripButtonLabel;
}