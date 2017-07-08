using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Highland64CourseImporter.Data.CourseBlocks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.IO;

namespace Highland64CourseImporter
{
    public partial class Objecteditor : Form
    {
        public Objecteditor()
        {
            InitializeComponent();
            
        }

        private void glControl1_paint(object sender, EventArgs e)
        {
            GL.ClearColor(Color.DeepSkyBlue);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.CadetBlue);
            GL.Vertex2(-0.5, 0);
            GL.Color3(Color.Red);
            GL.Vertex2(-1.0f, 0);
            GL.Color3(Color.Yellow);
            GL.Vertex2(-0.5, 0);
            GL.Color3(Color.Green);
            GL.Vertex2(-1.0f, 0);
            
            GL.End();
            glControl1.SwapBuffers();
            glControl1.Refresh();

        }
    }
}
