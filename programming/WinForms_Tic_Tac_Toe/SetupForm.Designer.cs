
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
        headerLeft = new Label();
        allegLeft = new Label();
        identityLeft = new Label();
        panelLeft = new Panel();
        toolStrip = new ToolStrip();
        toolStripLabel = new ToolStripLabel();
        panelRight = new Panel();
        panelLeft.SuspendLayout();
        toolStrip.SuspendLayout();
        SuspendLayout();
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
        // toolStrip
        // 
        toolStrip.Anchor = AnchorStyles.None;
        toolStrip.Dock = DockStyle.None;
        toolStrip.GripMargin = new Padding(0);
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.Items.AddRange(new ToolStripItem[] { toolStripLabel });
        toolStrip.Location = new Point(140, 420);
        toolStrip.Name = "toolStripButton";
        toolStrip.Padding = new Padding(10);
        toolStrip.Size = new Size(163, 39);
        toolStrip.TabIndex = 3;
        toolStrip.MouseEnter += ToolStrip_MouseEnter;
        toolStrip.MouseLeave += ToolStrip_MouseLeave;
        toolStrip.Click += ToolStrip_Click;
        toolStrip.SizeChanged += ToolStrip_SizeChanged;
        // 
        // toolStripButton1
        // 
        toolStripLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
        toolStripLabel.Margin = new Padding(0);
        toolStripLabel.Name = "toolStripButtonLabel";
        toolStripLabel.Size = new Size(110, 19);
        toolStripLabel.Text = "...";
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
        Controls.Add(toolStrip);
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
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Label headerLeft;
    private Label allegLeft;
    private Label identityLeft;
    private Panel panelLeft;
    private Panel panelRight;
    private ToolStrip toolStrip;
    private ToolStripLabel toolStripLabel;
}