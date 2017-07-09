using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;
using System.IO;

namespace Highland64CourseImporter
{
    class camera
 
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float Movespeed = 0.5f;
        public float MouseSensitvity = 0.03f;

    }
}
static class Matrix4
{
    internal static void LookAt(Vector3 position, Vector3 vector3, Vector3 unitY)
    {
       
    }
}
//Must have a return type.. Please help on line 32.
 public static GetViewMatrix()
{
    Vector3 lookat = new Vector3();

    lookat.X = (float)(Math.Sin((float)Orientation.Vertical) * Math.Cos((float)Orientation.Horizontal));
    lookat.Y = (float)Math.Sin((float)Orientation.Horizontal);
    lookat.Z = (float)(Math.Cos((float)Orientation.Vertical) * Math.Cos((float)Orientation.Horizontal));

    Vector3 Position = default(Vector3);
     Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
}