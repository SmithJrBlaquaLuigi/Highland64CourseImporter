using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

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
public Matrix4 GetViewMatrix()
{
    Vector3 lookat = new Vector3();

    lookat.X = (float)(Math.Sin((float)Orientation.X) *
    lookat.Y = (float)
    lookat.Z = (float)
}