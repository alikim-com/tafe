
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
        panelLeft = new Panel();
        panelRight = new Panel();
        panelLeft.SuspendLayout();
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
        headerLeft.Location = new Point(23, 184);
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
        allegLeft.Size = new Size(40, 15);
        allegLeft.TabIndex = 2;
        allegLeft.Text = "Origin";
        // 
        // identityLeft
        // 
        identityLeft.AutoSize = true;
        identityLeft.Location = new Point(85, 239);
        identityLeft.Margin = new Padding(10, 0, 3, 0);
        identityLeft.Name = "identityLeft";
        identityLeft.Size = new Size(47, 15);
        identityLeft.TabIndex = 2;
        identityLeft.Text = "Identity";
        // 
        // panelLeft
        // 
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
        panelRight.Location = new Point(220, 0);
        panelRight.Name = "panelRight";
        panelRight.Size = new Size(220, 490);
        panelRight.TabIndex = 1;
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
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button button1;
    private Label headerLeft;
    private Label allegLeft;
    private Label identityLeft;
    private Panel panelLeft;
    private Panel panelRight;
}