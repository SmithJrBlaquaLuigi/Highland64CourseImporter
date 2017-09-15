/*
Copyright (C) 2017 SmithJrBlaquaLuigi
Highland64 Course Importer is a free, copyleft license for
software and other kinds of works.
Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.
*/





using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Highland64CourseImporter.Data;
using Highland64CourseImporter.Data.CourseBlocks;
using System.Threading;
using OpenTK.Graphics.OpenGL;

namespace Highland64CourseImporter
{
    public partial class Form1 : Form
    {
        public MG64RomFile RomFile;

        public MG64RomFile MGRomFile;

        public bool Objectlist { get; set; }

        int[] objectlist = {
          0x00,
          0x01,
          0x02,
          0x03,
          0x04,
          0x05,
          0x06,
          0x07,
          0x08,
          0x09,
          0x0A,
          0x0B,
          0x0C,
          0x0D,
          0x0E,
          0x0F,
          0x10,
          0x11,
          0x12,
          0x13,
          0x14,
          0x15,
          0x16,
          0x17,
          0x18,
          0x19,
          0x1A,
          0x1B,
          0x1C,
          0x1D,
          0x1E,
          0x1F,
          0x20,
          0x21,
          0x22,
          0x23,
          0x24,
          0x25,
          0x26,
          0x27,
          0x28,
          0x29,
          0x2A,
          0x2B,
          0x2C,
          0x2D, //*object list trees, etc..... Gotta go dig deeper down through objects. Shit. I'm kinda lost for a moment.
        };

        public bool GetLeveldata()
        {
            FileStream fs = new FileStream(textBox1.Text, FileMode.Open, FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);
            BinaryReader br = new BinaryReader(fs);

            bw.Seek(0x0080830, SeekOrigin.Begin);
            {
                return false;
            }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = "Mario Golf 64 ROM |*.z64";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)

                textBox1.Text = openFileDialog1.FileName;

            else
                return;
            
            {
                RomFile = MG64RomFile.LoadRom(openFileDialog1.FileName);

                TestForm form = new TestForm(RomFile);
                form.Show();
            }
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text != "Please load a ROM before you proceed to do anything else.")
            //{
            //    if (!GetCheckROM() == false)
            //    {
            //        MessageBox.Show("Please select a correct ROM before proceeding.", "Wrong ROM!");
            //        textBox1.Text = "No File Loaded";
            //    }
            //    else
            //    {
            //        return;

            //    }
            //    }
            levelImporterToolStripMenuItem.Enabled = true;
        }

        private void ToolStripTextBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void VisualEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisualEditor form = new VisualEditor(RomFile);
            form.Show();
        }

        private void objectEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Objecteditor form = new Objecteditor();
            form.Show();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void levelImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3(MGRomFile);
            form.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            levelImporterToolStripMenuItem.Enabled = false;
        }
    }
}
