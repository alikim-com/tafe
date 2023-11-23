
using System.ComponentModel;

namespace WinFormsApp1;
partial class SetupForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        button1 = new Button();
        headerLeft = new Label();
        allegLeft = new Label();
        identityLeft = new Label();
        nameLeft = new TextBox();
        comboLeft = new ComboBox();
        headerRight = new Label();
        allegRight = new Label();
        identityRight = new Label();
        nameRight = new TextBox();
        comboRight = new ComboBox();
        panel1 = new Panel();
        panelLeft = new Panel();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        button1.AutoSize = true;
        button1.Cursor = Cursors.Hand;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Location = new Point(160, 445);
        button1.Margin = new Padding(0);
        button1.Name = "button1";
        button1.Size = new Size(120, 27);
        button1.TabIndex = 1;
        button1.Text = "Leave the narrative";
        button1.UseVisualStyleBackColor = false;
        button1.Click += button1_Click;
        // 
        // headerLeft
        // 
        headerLeft.AutoSize = true;
        headerLeft.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
        headerLeft.Location = new Point(23, 25);
        headerLeft.Name = "headerLeft";
        headerLeft.Size = new Size(112, 24);
        headerLeft.TabIndex = 2;
        headerLeft.Text = "Left Player";
        // 
        // allegLeft
        // 
        allegLeft.AutoSize = true;
        allegLeft.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
        allegLeft.Location = new Point(23, 269);
        allegLeft.Name = "allegLeft";
        allegLeft.Size = new Size(81, 18);
        allegLeft.TabIndex = 2;
        allegLeft.Text = "Allegiance";
        // 
        // identityLeft
        // 
        identityLeft.AutoSize = true;
        identityLeft.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
        identityLeft.Location = new Point(23, 352);
        identityLeft.Name = "identityLeft";
        identityLeft.Size = new Size(56, 18);
        identityLeft.TabIndex = 2;
        identityLeft.Text = "Identity";
        // 
        // nameLeft
        // 
        nameLeft.Location = new Point(23, 382);
        nameLeft.Name = "nameLeft";
        nameLeft.Size = new Size(121, 23);
        nameLeft.TabIndex = 3;
        // 
        // comboLeft
        // 
        comboLeft.FormattingEnabled = true;
        comboLeft.Location = new Point(23, 299);
        comboLeft.Name = "comboLeft";
        comboLeft.Size = new Size(121, 23);
        comboLeft.TabIndex = 4;
        // 
        // headerRight
        // 
        headerRight.AutoSize = true;
        headerRight.Font = new Font("Arial", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
        headerRight.Location = new Point(295, 25);
        headerRight.Name = "headerRight";
        headerRight.Size = new Size(124, 24);
        headerRight.TabIndex = 2;
        headerRight.Text = "Right Player";
        // 
        // allegRight
        // 
        allegRight.AutoSize = true;
        allegRight.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
        allegRight.Location = new Point(321, 269);
        allegRight.Name = "allegRight";
        allegRight.Size = new Size(81, 18);
        allegRight.TabIndex = 2;
        allegRight.Text = "Allegiance";
        // 
        // identityRight
        // 
        identityRight.AutoSize = true;
        identityRight.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point);
        identityRight.Location = new Point(348, 352);
        identityRight.Name = "identityRight";
        identityRight.Size = new Size(56, 18);
        identityRight.TabIndex = 2;
        identityRight.Text = "Identity";
        // 
        // nameRight
        // 
        nameRight.Location = new Point(316, 382);
        nameRight.Name = "nameRight";
        nameRight.Size = new Size(100, 23);
        nameRight.TabIndex = 3;
        // 
        // comboRight
        // 
        comboRight.FormattingEnabled = true;
        comboRight.Location = new Point(295, 299);
        comboRight.Name = "comboRight";
        comboRight.Size = new Size(121, 23);
        comboRight.TabIndex = 4;
        // 
        // panel1
        // 
        panel1.BackgroundImage = Resource.SetupBackImg;
        panel1.BackgroundImageLayout = ImageLayout.Stretch;
        panel1.Controls.Add(panelLeft);
        panel1.Dock = DockStyle.Fill;
        panel1.Location = new Point(0, 0);
        panel1.Name = "panel1";
        panel1.Size = new Size(440, 490);
        panel1.TabIndex = 5;
        // 
        // panelLeft
        // 
        panelLeft.BackgroundImage = Resource.AILeft;
        panelLeft.Location = new Point(0, 0);
        panelLeft.Name = "panelLeft";
        panelLeft.Size = new Size(220, 490);
        panelLeft.TabIndex = 0;
        // 
        // SetupForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(440, 490);
        Controls.Add(comboRight);
        Controls.Add(comboLeft);
        Controls.Add(nameRight);
        Controls.Add(nameLeft);
        Controls.Add(identityRight);
        Controls.Add(identityLeft);
        Controls.Add(allegRight);
        Controls.Add(allegLeft);
        Controls.Add(headerRight);
        Controls.Add(headerLeft);
        Controls.Add(button1);
        Controls.Add(panel1);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SetupForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Player setup";
        panel1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button button1;
    private Label headerLeft;
    private Label allegLeft;
    private Label identityLeft;
    private TextBox nameLeft;
    private ComboBox comboLeft;
    private Label headerRight;
    private Label allegRight;
    private Label identityRight;
    private TextBox nameRight;
    private ComboBox comboRight;
    private Panel panel1;
    private Panel panelLeft;
}