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
        private Chip8Core _chip8Core = new Chip8Core();

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
    }
}
