using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chip8.Core;

namespace Chip8.Forms
{
    public partial class MainForm : Form
    {
        private Chip8Core _chip8Core = new();

        private Color _pixelColor = Color.Green;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _chip8Core.Execute(0x71FF);
            _chip8Core.Execute(0x7215);

            _chip8Core.Execute(0x8124);
        }

        private void PaintChip8Panel(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var graphics = e.Graphics;

            var pixelBrush = new SolidBrush(_pixelColor);

            var pixelWidth = panel.Width / Chip8Core.DisplayWidth;
            var pixelHeight = panel.Height / Chip8Core.DisplayHeight;

            var marginX = (panel.Width % Chip8Core.DisplayWidth) / 2;
            var marginY = (panel.Height % Chip8Core.DisplayHeight) / 2;

            for (int y = 0; y < Chip8Core.DisplayHeight; y++)
            {
                for (int x = 0; x < Chip8Core.DisplayWidth; x++)
                {
                    if (_chip8Core.Display[y, x])
                        graphics.FillRectangle(pixelBrush, marginX + x * pixelWidth, marginY + y * pixelHeight, pixelWidth, pixelHeight);
                }
            }
        }

        private void RefreshPanel(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            panel.Refresh();
        }
    }
}
