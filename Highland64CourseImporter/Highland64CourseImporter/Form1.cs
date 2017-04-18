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
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Stream mystream = null;
            OpenFileDialog openfiledialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Mario Golf 64 ROM |*.z64";
            openFileDialog1.FilterIndex = 1;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((mystream = openFileDialog1.OpenFile()) != null)
                    {
                        using (mystream)
                        {
                            //Insert code here. Blah borrrrrrrrrrrrrrrgy
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sorry: please select a correct format. Error " + ex.Message);
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}

