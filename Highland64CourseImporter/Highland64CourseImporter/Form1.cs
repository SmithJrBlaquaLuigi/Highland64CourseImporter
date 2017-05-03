using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Highland64CourseImporter
{
    public partial class Form1 : Form
    {
        private const int Rom_offset = 0x1F00000;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openfiledialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Mario Golf 64 ROM |*.z64";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.ShowDialog();

            textBox1.Text = openFileDialog1.FileName;
        }
    }
}