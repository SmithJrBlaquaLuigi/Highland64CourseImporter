using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace Highland64CourseImporter

{
    public partial class VisualEditor : Form
    {
        public VisualEditor()
        {
            InitializeComponent();
        }

        private void glControl1_paint(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Red);
            GL.Vertex2(-0.5, 0);
            GL.Color3(Color.Yellow);
            GL.Vertex2(0.5, -1.0f);
            GL.Color3(Color.Green);
            GL.Vertex2(-1.0f, 0);
            GL.Color3(Color.Blue);
            GL.Vertex2(-0.5, 0);

            GL.End();
            glControl1.SwapBuffers();
            glControl1.Refresh();
            
        }
    }
}
