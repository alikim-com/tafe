namespace WinformsUMLEvents;

partial class UML_Events
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

        if(aboutForm != null) aboutForm.Dispose();
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        pictureBox1 = new PictureBox();
        label1 = new Label();
        menuStrip1 = new MenuStrip();
        menuLoad = new ToolStripMenuItem();
        menuLoadOpen = new ToolStripMenuItem();
        menuLoadCollection = new ToolStripMenuItem();
        menuSave = new ToolStripMenuItem();
        menuSaveAs = new ToolStripMenuItem();
        menuHelp = new ToolStripMenuItem();
        menuHelpAbout = new ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // pictureBox1
        // 
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Location = new Point(0, 24);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(1200, 876);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        pictureBox1.Paint += pictureBox1_Paint;
        pictureBox1.MouseDown += pictureBox1_MouseDown;
        pictureBox1.MouseLeave += pictureBox1_MouseLeave;
        pictureBox1.MouseMove += pictureBox1_MouseMove;
        pictureBox1.MouseUp += pictureBox1_MouseUp;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.ForeColor = Color.Gray;
        label1.Location = new Point(609, 871);
        label1.Name = "label1";
        label1.Size = new Size(591, 29);
        label1.TabIndex = 1;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuLoad, menuSave, menuHelp });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(1200, 24);
        menuStrip1.TabIndex = 2;
        menuStrip1.Text = "menuStrip1";
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
        menuLoadOpen.Size = new Size(168, 22);
        menuLoadOpen.Text = "Open profile...";
        // 
        // menuLoadCollection
        // 
        menuLoadCollection.Name = "menuLoadCollection";
        menuLoadCollection.Size = new Size(168, 22);
        menuLoadCollection.Text = "Profiles collection";
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
        menuSaveAs.Size = new Size(158, 22);
        menuSaveAs.Text = "Save profile as...";
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
        menuHelpAbout.Size = new Size(180, 22);
        menuHelpAbout.Text = "About";
        menuHelpAbout.Click += menuHelpAbout_Click;
        // 
        // UML_Events
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Black;
        ClientSize = new Size(1200, 900);
        Controls.Add(label1);
        Controls.Add(pictureBox1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "UML_Events";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "UML_Events";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private Label label1;
    private MenuStrip menuStrip1;
    private ToolStripComboBox menuAbout;
    private ToolStripMenuItem menuSave;
    private ToolStripMenuItem menuLoad;
    private ToolStripMenuItem menuHelp;
    private ToolStripMenuItem menuLoadOpen;
    private ToolStripMenuItem menuLoadCollection;
    private ToolStripMenuItem menuSaveAs;
    private ToolStripMenuItem menuHelpAbout;
    private ToolStripTextBox menuText;
}