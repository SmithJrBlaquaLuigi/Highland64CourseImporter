using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Highland64CourseImporter
{
    public partial class VisualEditor : Form
    {
        public VisualEditor()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            glControl1.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.ClearColor(Color.Blue);
            GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -1, 1);
            GL.Viewport(0, 0, w, h);
            GL.End();
            glControl1.SwapBuffers();
        }
    }
}
