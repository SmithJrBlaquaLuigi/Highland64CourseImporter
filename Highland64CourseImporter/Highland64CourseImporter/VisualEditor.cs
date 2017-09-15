using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Highland64CourseImporter.Data.CourseBlocks;
using Highland64CourseImporter.Data;


namespace Highland64CourseImporter

{
    public partial class VisualEditor : Form
    {
        
        
        public VisualEditor(Data.MG64RomFile romFile)

        {
            InitializeComponent();
            

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            
            int w = glControl1.Width;
            int h = glControl1.Height;
            glControl1.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(-0.5, 0);
            GL.Color3(Color.Blue);
            GL.Vertex2(0.5f, -1.0f);
            GL.Color3(Color.Red);
            GL.Vertex2(-1.0f, 0);
            GL.Color3(Color.Yellow);
            GL.Vertex2(-0.5f, 0);
            GL.Color3(Color.Green);
            GL.LoadIdentity();
            GL.ClearColor(Color.FromArgb(2, 4, 20));
            GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -1, 1);
            GL.Viewport(0, 0, w, h);
            GL.End();
            glControl1.SwapBuffers();



        }

        private void glControl1_Load_1(object sender, EventArgs e)
        {
            
        }
    }
}
