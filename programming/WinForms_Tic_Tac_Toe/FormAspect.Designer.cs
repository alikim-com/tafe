﻿namespace WinFormsApp1;

partial class FormAspect
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAspect));
        tpl = new TableLayoutPanel();
        button1 = new TokenButton();
        tbox = new TextBox();
        tpl.SuspendLayout();
        SuspendLayout();
        // 
        // tpl
        // 
        tpl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tpl.BackColor = Color.FromArgb(192, 0, 0, 0);
        tpl.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tpl.ColumnCount = 3;
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        tpl.Controls.Add(button1, 0, 0);
        tpl.Location = new Point(40, 40);
        tpl.Name = "tpl";
        tpl.RowCount = 3;
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
        tpl.Size = new Size(324, 320);
        tpl.TabIndex = 0;
        tpl.SizeChanged += tpl_SizeChanged;
        // 
        // button1
        // 
        button1.BackColor = Color.Transparent;
        button1.Cursor = Cursors.Hand;
        button1.Dock = DockStyle.Fill;
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Location = new Point(11, 11);
        button1.Margin = new Padding(10);
        button1.Name = "button1";
        button1.Size = new Size(86, 85);
        button1.TabIndex = 0;
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // tbox
        // 
        tbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tbox.BackColor = SystemColors.Desktop;
        tbox.Location = new Point(40, 540);
        tbox.Name = "tbox";
        tbox.Size = new Size(324, 23);
        tbox.TabIndex = 1;
        // 
        // FormAspect
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(400, 600);
        Controls.Add(tbox);
        Controls.Add(tpl);
        MaximizeBox = false;
        Name = "FormAspect";
        Text = "Tic-tac-toe";
        Load += FormAspect_Load;
        tpl.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TableLayoutPanel tpl;
    private TextBox tbox;
    private TokenButton button1;
}