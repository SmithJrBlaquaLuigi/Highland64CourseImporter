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
using Highland64CourseImporter.Data.CourseBlocks;
using Highland64CourseImporter.Data;

namespace Highland64CourseImporter
{
    public partial class Form3 : Form
    {
        public MG64RomFile RomFile { get; set; } 


        public Form3(MG64RomFile mGRomFile)
        {
           
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RomFile = MG64RomFile.LoadRom(openFileDialog1.FileName);

            TestForm form = new TestForm(RomFile);
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            VisualEditor form = new VisualEditor(RomFile);
            form.Show();
        }
    }
}
