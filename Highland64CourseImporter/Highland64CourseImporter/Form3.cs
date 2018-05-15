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
        private List<_HeightDataEncoding> _HeightDataEncoding;
        public MG64RomFile RomFile { get; set; }
        public List<CourseTableEntry> CourseTable { get; private set; }
        
       

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void HeightMapData()
        {
            _HeightDataEncoding = new List<_HeightDataEncoding>();
            _HeightDataEncoding heightpointer = new _HeightDataEncoding();


            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MG64RomFile.LoadRom(openFileDialog2.FileName);

            TestForm form = new TestForm();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();

            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            VisualEditor form = new VisualEditor(RomFile);
            form.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            {

            }
        }
    }
}
