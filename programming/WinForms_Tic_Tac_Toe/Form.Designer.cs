
using System.Windows.Forms;
using static WinFormsApp1.LabelManager;

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
        tSplit = new TableLayoutPanel();
        sTL = new Panel();
        sTR = new Panel();
        sBL = new Panel();
        sBR = new Panel();
        choice = new Label();
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
        tSplit.SuspendLayout();
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
        tLayout.Controls.Add(pLeft, 0, 4);
        tLayout.Controls.Add(pRight, 2, 4);
        tLayout.Controls.Add(tSplit, 1, 4);
        tLayout.Controls.Add(info, 0, 3);
        tLayout.Dock = DockStyle.Fill;
        tLayout.Location = new Point(0, 0);
        tLayout.Margin = new Padding(0);
        tLayout.Name = "tLayout";
        tLayout.RowCount = 5;
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
        tLayout.Size = new Size(400, 600);
        tLayout.TabIndex = 0;
        // 
        // pLeft
        // 
        pLeft.BackgroundImageLayout = ImageLayout.Stretch;
        pLeft.Dock = DockStyle.Fill;
        pLeft.Location = new Point(0, 432);
        pLeft.Margin = new Padding(0);
        pLeft.Name = "pLeft";
        pLeft.Size = new Size(133, 168);
        pLeft.TabIndex = 0;
        // 
        // pRight
        // 
        pRight.BackgroundImageLayout = ImageLayout.Stretch;
        pRight.Dock = DockStyle.Fill;
        pRight.Location = new Point(266, 432);
        pRight.Margin = new Padding(0);
        pRight.Name = "pRight";
        pRight.Size = new Size(134, 168);
        pRight.TabIndex = 1;
        // 
        // tSplit
        // 
        tSplit.ColumnCount = 2;
        tSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tSplit.Controls.Add(sTL, 0, 0);
        tSplit.Controls.Add(sTR, 1, 0);
        tSplit.Controls.Add(sBL, 0, 2);
        tSplit.Controls.Add(sBR, 1, 2);
        tSplit.Controls.Add(choice, 0, 1);
        tSplit.Dock = DockStyle.Fill;
        tSplit.Location = new Point(133, 432);
        tSplit.Margin = new Padding(0);
        tSplit.Name = "tSplit";
        tSplit.RowCount = 3;
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 44F));
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
        tSplit.Size = new Size(133, 168);
        tSplit.TabIndex = 2;
        // 
        // sTL
        // 
        sTL.Dock = DockStyle.Fill;
        sTL.Location = new Point(0, 0);
        sTL.Margin = new Padding(0);
        sTL.Name = "sTL";
        sTL.Size = new Size(66, 47);
        sTL.TabIndex = 0;
        // 
        // sTR
        // 
        sTR.Dock = DockStyle.Fill;
        sTR.Location = new Point(66, 0);
        sTR.Margin = new Padding(0);
        sTR.Name = "sTR";
        sTR.Size = new Size(67, 47);
        sTR.TabIndex = 1;
        // 
        // sBL
        // 
        sBL.Dock = DockStyle.Fill;
        sBL.Location = new Point(0, 120);
        sBL.Margin = new Padding(0);
        sBL.Name = "sBL";
        sBL.Size = new Size(66, 48);
        sBL.TabIndex = 2;
        // 
        // sBR
        // 
        sBR.Dock = DockStyle.Fill;
        sBR.Location = new Point(66, 120);
        sBR.Margin = new Padding(0);
        sBR.Name = "sBR";
        sBR.Size = new Size(67, 48);
        sBR.TabIndex = 3;
        // 
        // choice
        // 
        choice.BackColor = Color.FromArgb(128, 0, 0, 0);
        tSplit.SetColumnSpan(choice, 2);
        choice.Dock = DockStyle.Fill;
        choice.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        choice.ForeColor = Color.FromArgb(0, 255, 255, 255);
        choice.Location = new Point(0, 47);
        choice.Margin = new Padding(0);
        choice.Name = "choice";
        choice.Size = new Size(133, 73);
        choice.TabIndex = 2;
        choice.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // info
        // 
        tLayout.SetColumnSpan(info, 3);
        info.Dock = DockStyle.Fill;
        info.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        info.ForeColor = Color.FromArgb(0, 255, 255, 255);
        info.Location = new Point(0, 396);
        info.Margin = new Padding(0);
        info.Name = "info";
        info.Size = new Size(400, 36);
        info.TabIndex = 3;
        info.TextAlign = ContentAlignment.TopCenter;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuLoad, menuSave, menuHelp, menuLabel, menuDummy, menuLayout });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1500, 24);
        menuStrip1.TabIndex = 2;
        menuStrip1.Text = "menuMain";
        // 
        // menuLoad
        // 
        menuLoad.DropDownItems.AddRange(new ToolStripItem[] { menuLoadOpen, menuLoadCollection });
        menuLoad.Name = "menuLoad";
        menuLoad.Size = new Size(45, 20);
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
        menuSave.Size = new Size(43, 20);
        menuSave.Text = "Save";
        // 
        // menuSaveAs
        // 
        menuSaveAs.Name = "menuSaveAs";
        menuSaveAs.Size = new Size(157, 22);
        menuSaveAs.Text = "Save layout as...";
        //menuSaveAs.Click += MenuSaveAs_Click;
        // 
        // menuHelp
        // 
        menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuHelpAbout });
        menuHelp.Name = "menuHelp";
        menuHelp.Size = new Size(44, 20);
        menuHelp.Text = "Help";
        // 
        // menuHelpAbout
        // 
        menuHelpAbout.Name = "menuHelpAbout";
        menuHelpAbout.Size = new Size(107, 22);
        menuHelpAbout.Text = "About";
        //menuHelpAbout.Click += MenuHelpAbout_Click;
        // 
        // menuLabel
        // 
        menuLabel.Margin = new Padding(50, 1, 5, 2);
        menuLabel.Name = "menuLabel";
        menuLabel.Size = new Size(46, 17);
        menuLabel.Text = "Layout:";
        // 
        // menuDummy
        // 
        menuDummy.Enabled = false;
        menuDummy.Margin = new Padding(1, 0, 0, 0);
        menuDummy.Name = "menuDummy";
        menuDummy.Size = new Size(7, 20);
        // 
        // menuLayout
        // 
        menuLayout.Margin = new Padding(0, 0, 1, 0);
        menuLayout.Name = "menuLayout";
        menuLayout.Size = new Size(100, 20);
        menuLayout.Text = "Default";
        // 
        // AppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 0);
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
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
        tSplit.ResumeLayout(false);
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tLayout;
    private Panel pLeft;
    private Panel pRight;
    private TableLayoutPanel tSplit;
    private Panel sTL;
    private Panel sTR;
    private Panel sBL;
    private Panel sBR;
    private Label choice;
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