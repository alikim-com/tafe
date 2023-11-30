
using System.ComponentModel;

namespace WinFormsApp1;
partial class AboutForm
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
        ComponentResourceManager resources = new ComponentResourceManager(typeof(AboutForm));
        label1 = new Label();
        button1 = new Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(0, 0);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Padding = new Padding(20, 0, 20, 45);
        label1.Size = new Size(500, 541);
        label1.TabIndex = 0;
        label1.Text = resources.GetString("label1.Text");
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // button1
        // 
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        button1.AutoSize = true;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Location = new Point(192, 492);
        button1.Margin = new Padding(0);
        button1.Name = "button1";
        button1.Size = new Size(116, 25);
        button1.TabIndex = 1;
        button1.Text = "Leave the narrative";
        button1.UseVisualStyleBackColor = false;
        button1.Click += Button1_Click;
        button1.Cursor = Cursors.Hand;
        // 
        // AboutForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 541);
        Controls.Add(button1);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "About";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button button1;
}