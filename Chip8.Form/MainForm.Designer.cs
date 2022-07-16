namespace Chip8.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.chip8Panel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRomDialog = new System.Windows.Forms.OpenFileDialog();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.chip8Panel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chip8Panel
            // 
            this.chip8Panel.BackColor = System.Drawing.Color.Black;
            this.chip8Panel.Controls.Add(this.menuStrip1);
            this.chip8Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chip8Panel.Location = new System.Drawing.Point(0, 0);
            this.chip8Panel.Name = "chip8Panel";
            this.chip8Panel.Size = new System.Drawing.Size(1010, 680);
            this.chip8Panel.TabIndex = 0;
            this.chip8Panel.Click += new System.EventHandler(this.chip8Panel_Click);
            this.chip8Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintChip8Panel);
            this.chip8Panel.Resize += new System.EventHandler(this.RefreshChip8Panel);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1010, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRomToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadRomToolStripMenuItem
            // 
            this.loadRomToolStripMenuItem.Name = "loadRomToolStripMenuItem";
            this.loadRomToolStripMenuItem.Size = new System.Drawing.Size(160, 26);
            this.loadRomToolStripMenuItem.Text = "&Load Rom";
            this.loadRomToolStripMenuItem.Click += new System.EventHandler(this.loadRomToolStripMenuItem_Click);
            // 
            // openRomDialog
            // 
            this.openRomDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 2;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 680);
            this.Controls.Add(this.chip8Panel);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Chip8";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.chip8Panel.ResumeLayout(false);
            this.chip8Panel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel chip8Panel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadRomToolStripMenuItem;
        private OpenFileDialog openRomDialog;
        private System.Windows.Forms.Timer updateTimer;
    }
}