using System.Windows.Forms;

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
        tLayout.SuspendLayout();
        tSplit.SuspendLayout();
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
        tLayout.Dock = DockStyle.Fill;
        tLayout.Location = new Point(0, 0);
        tLayout.Margin = new Padding(0);
        tLayout.Name = "tLayout";
        tLayout.RowCount = 5;
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20.19608F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 9.803922F));
        tLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 29.6078033F));
        tLayout.Size = new Size(400, 600);
        tLayout.TabIndex = 0;
        // 
        // pLeft
        // 
        pLeft.BackgroundImageLayout = ImageLayout.Stretch;
        pLeft.Cursor = Cursors.Hand;
        pLeft.Dock = DockStyle.Fill;
        pLeft.Location = new Point(0, 421);
        pLeft.Margin = new Padding(0);
        pLeft.Name = "pLeft";
        pLeft.Size = new Size(133, 179);
        pLeft.TabIndex = 0;
        pLeft.Click += pLeft_Click;
        // 
        // pRight
        // 
        pRight.BackgroundImageLayout = ImageLayout.Stretch;
        pRight.Cursor = Cursors.Hand;
        pRight.Dock = DockStyle.Fill;
        pRight.Location = new Point(266, 421);
        pRight.Margin = new Padding(0);
        pRight.Name = "pRight";
        pRight.Size = new Size(134, 179);
        pRight.TabIndex = 1;
        pRight.Click += pRight_Click;
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
        tSplit.Location = new Point(133, 421);
        tSplit.Margin = new Padding(0);
        tSplit.Name = "tSplit";
        tSplit.RowCount = 3;
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        tSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        tSplit.Size = new Size(133, 179);
        tSplit.TabIndex = 2;
        // 
        // sTL
        // 
        sTL.Dock = DockStyle.Fill;
        sTL.Location = new Point(0, 0);
        sTL.Margin = new Padding(0);
        sTL.Name = "sTL";
        sTL.Size = new Size(66, 59);
        sTL.TabIndex = 0;
        // 
        // sTR
        // 
        sTR.Dock = DockStyle.Fill;
        sTR.Location = new Point(66, 0);
        sTR.Margin = new Padding(0);
        sTR.Name = "sTR";
        sTR.Size = new Size(67, 59);
        sTR.TabIndex = 1;
        // 
        // sBL
        // 
        sBL.Dock = DockStyle.Fill;
        sBL.Location = new Point(0, 0);
        sBL.Margin = new Padding(0);
        sBL.Name = "sTL";
        sBL.Size = new Size(66, 59);
        sBL.TabIndex = 2;
        // 
        // sBR
        // 
        sBR.Dock = DockStyle.Fill;
        sBR.Location = new Point(66, 0);
        sBR.Margin = new Padding(0);
        sBR.Name = "sTR";
        sBR.Size = new Size(67, 59);
        sBR.TabIndex = 3;
        // 
        // choice
        // 
        choice.AutoSize = true;
        choice.BackColor = Color.FromArgb(128, 0, 0, 0);
        tSplit.SetColumnSpan(choice, 2);
        choice.Dock = DockStyle.Fill;
        choice.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        choice.ForeColor = Color.FromArgb(0, 255, 255, 255);
        choice.Location = new Point(0, 59);
        choice.Margin = new Padding(0);
        choice.Name = "choice";
        choice.Size = new Size(133, 59);
        choice.TabIndex = 2;
        choice.Text = "CHOOSE\nYOUR\nSIDE";
        choice.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // AppForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(0, 0, 0);
        BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
        BackgroundImageLayout = ImageLayout.Stretch;
        ClientSize = new Size(400, 600);
        Controls.Add(tLayout);
        MaximizeBox = false;
        Name = "AppForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tic-Tac-Toe";
        Load += FormAspect_Load;
        tLayout.ResumeLayout(false);
        tSplit.ResumeLayout(false);
        tSplit.PerformLayout();
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
}