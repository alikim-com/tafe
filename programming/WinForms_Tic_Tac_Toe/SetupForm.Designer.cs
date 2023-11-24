
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
        comboLeft = new FlatCombo();
        allegRight = new Label();
        identityRight = new Label();
        nameRight = new TextBox();
        comboRight = new ComboBox();
        panelLeft = new Panel();
        panelRight = new Panel();
        headerRight = new Label();
        panelLeft.SuspendLayout();
        panelRight.SuspendLayout();
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
        headerLeft.Location = new Point(23, 25);
        headerLeft.Name = "headerLeft";
        headerLeft.Size = new Size(62, 15);
        headerLeft.TabIndex = 2;
        headerLeft.Text = "Left Player";
        // 
        // allegLeft
        // 
        allegLeft.AutoSize = true;
        allegLeft.Location = new Point(23, 239);
        allegLeft.Name = "allegLeft";
        allegLeft.Size = new Size(62, 15);
        allegLeft.TabIndex = 2;
        allegLeft.Text = "Allegiance";
        // 
        // identityLeft
        // 
        identityLeft.AutoSize = true;
        identityLeft.Location = new Point(23, 352);
        identityLeft.Name = "identityLeft";
        identityLeft.Size = new Size(47, 15);
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
        comboLeft.BorderColor = Color.Yellow;
        comboLeft.FormattingEnabled = true;
        comboLeft.Location = new Point(23, 269);
        comboLeft.Name = "comboLeft";
        comboLeft.Size = new Size(121, 23);
        comboLeft.TabIndex = 4;
        // 
        // allegRight
        // 
        allegRight.AutoSize = true;
        allegRight.Location = new Point(139, 269);
        allegRight.Name = "allegRight";
        allegRight.Size = new Size(62, 15);
        allegRight.TabIndex = 2;
        allegRight.Text = "Allegiance";
        // 
        // identityRight
        // 
        identityRight.AutoSize = true;
        identityRight.Location = new Point(164, 352);
        identityRight.Name = "identityRight";
        identityRight.Size = new Size(47, 15);
        identityRight.TabIndex = 2;
        identityRight.Text = "Identity";
        // 
        // nameRight
        // 
        nameRight.Location = new Point(120, 382);
        nameRight.Name = "nameRight";
        nameRight.Size = new Size(100, 23);
        nameRight.TabIndex = 3;
        // 
        // comboRight
        // 
        comboRight.FormattingEnabled = true;
        comboRight.Location = new Point(99, 299);
        comboRight.Name = "comboRight";
        comboRight.Size = new Size(121, 23);
        comboRight.TabIndex = 4;
        // 
        // panelLeft
        // 
        panelLeft.BackgroundImage = Resource.AILeft;
        panelLeft.Controls.Add(comboLeft);
        panelLeft.Controls.Add(nameLeft);
        panelLeft.Controls.Add(identityLeft);
        panelLeft.Controls.Add(allegLeft);
        panelLeft.Controls.Add(headerLeft);
        panelLeft.Location = new Point(0, 0);
        panelLeft.Name = "panelLeft";
        panelLeft.Size = new Size(220, 490);
        panelLeft.TabIndex = 0;
        // 
        // panelRight
        // 
        panelRight.Controls.Add(comboRight);
        panelRight.Controls.Add(nameRight);
        panelRight.Controls.Add(identityRight);
        panelRight.Controls.Add(allegRight);
        panelRight.Controls.Add(headerRight);
        panelRight.Location = new Point(220, 0);
        panelRight.Name = "panelRight";
        panelRight.Size = new Size(220, 490);
        panelRight.TabIndex = 1;
        // 
        // headerRight
        // 
        headerRight.AutoSize = true;
        headerRight.Location = new Point(96, 25);
        headerRight.Name = "headerRight";
        headerRight.Size = new Size(70, 15);
        headerRight.TabIndex = 2;
        headerRight.Text = "Right Player";
        // 
        // SetupForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = Resource.SetupBackImg;
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(440, 490);
        Controls.Add(button1);
        Controls.Add(panelRight);
        Controls.Add(panelLeft);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SetupForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Player setup";
        panelLeft.ResumeLayout(false);
        panelLeft.PerformLayout();
        panelRight.ResumeLayout(false);
        panelRight.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button button1;
    private Label headerLeft;
    private Label allegLeft;
    private Label identityLeft;
    private TextBox nameLeft;
    private Label allegRight;
    private Label identityRight;
    private TextBox nameRight;
    private ComboBox comboRight;
    private Panel panelLeft;
    private Panel panelRight;
    private Label headerRight;
    private FlatCombo comboLeft;
}