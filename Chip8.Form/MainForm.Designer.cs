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
            this.chip8Panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // chip8Panel
            // 
            this.chip8Panel.BackColor = System.Drawing.Color.Black;
            this.chip8Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chip8Panel.Location = new System.Drawing.Point(0, 0);
            this.chip8Panel.Name = "chip8Panel";
            this.chip8Panel.Size = new System.Drawing.Size(1010, 680);
            this.chip8Panel.TabIndex = 0;
            this.chip8Panel.Click += new System.EventHandler(this.RefreshPanel);
            this.chip8Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintChip8Panel);
            this.chip8Panel.Resize += new System.EventHandler(this.RefreshPanel);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 680);
            this.Controls.Add(this.chip8Panel);
            this.Name = "MainForm";
            this.Text = "Chip8";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel chip8Panel;
    }
}