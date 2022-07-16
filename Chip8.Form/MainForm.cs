
using System.ComponentModel;
using Chip8.Core;

namespace Chip8.Forms
{
    public partial class MainForm : Form
    {
        private Chip8Core _chip8Core = new();
        private Color _pixelColor = Color.Green;
        private bool _running = false;

        public MainForm()
        {
            InitializeComponent();

            _chip8Core.DisplayChanged += RefreshChip8Panel;
            _chip8Core.DisplayChanged += PrintDisplayToFile;
        }

        public void StartUpdate()
        {
            updateTimer.Start();
            _running = true;
        }

        public void StopUpdate()
        {
            updateTimer.Stop();
            _running = false;
        }

        public void ToggleUpdate()
        {
            if (_running)
                StopUpdate();
            else
                StartUpdate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _chip8Core.Execute(0x71FF);
            _chip8Core.Execute(0x7215);

            _chip8Core.Execute(0x8124);
        }

        private void PrintDisplayToFile(object sender, EventArgs e)
        {
            using (var writer = new StreamWriter("display.txt"))
            {
                for (int y = 0; y < Chip8Core.DisplayHeight; y++)
                {
                    for (int x = 0; x < Chip8Core.DisplayWidth; x++)
                    {
                        writer.Write(_chip8Core.Display[y, x] ? "#" : " ");
                    }
                    writer.WriteLine();
                }
            }
        }


        private void PaintChip8Panel(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            var graphics = e.Graphics;

            var pixelBrush = new SolidBrush(_pixelColor);

            var pixelWidth = panel.Width / Chip8Core.DisplayWidth;
            var pixelHeight = panel.Height / Chip8Core.DisplayHeight;

            var marginX = (panel.Width % Chip8Core.DisplayWidth) / 2 + 1;
            var marginY = (panel.Height % Chip8Core.DisplayHeight) / 2 + 1;


            for (int y = 0; y < Chip8Core.DisplayHeight; y++)
            {
                for (int x = 0; x < Chip8Core.DisplayWidth; x++)
                {
                    if (_chip8Core.Display[y, x])
                        graphics.FillRectangle(pixelBrush, marginX + x * pixelWidth, marginY + y * pixelHeight, pixelWidth - 1, pixelHeight - 1);
                }
            }
        }

        private void RefreshChip8Panel(object sender, EventArgs e)
        {
            chip8Panel.Refresh();
        }

        private void loadRomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openRomDialog.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            _chip8Core.LoadRom(openRomDialog.FileName);
            chip8Panel.Refresh();
            StartUpdate();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            _chip8Core.Update();
        }

        private void chip8Panel_Click(object sender, EventArgs e)
        {
            ToggleUpdate();
        }
    }
}
