﻿#region --- MIT License ---
/* 
 * This file is part of CSat - small C# 3D-library
 * 
 * Copyright (c) 2008 mjt[matola@sci.fi]
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 ***
 * -mjt,  
 * email: matola@sci.fi
 */
#endregion

using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Collections.Generic;

namespace CSat
{
    public class Path : ObjModel
    {
        Vector3[] path = null;
        public bool Looping = true;
        public float Time = 0;

        /// <summary>
        /// mitä objektia liikutetaan
        /// </summary>
        Node attachedObj;

        public Path(string name)
            : base(name)
        {
        }
        public Path(string name, string fileName, float sx, float sy, float sz)
            : base(name, fileName, sx, sy, sz)
        {
            pathData.RemoveAt(0); // !!
            path = pathData.ToArray();
        }

        public void MakeCurve(int lod)
        {
            if (path == null) return;

            for (int c = 0; c < lod; c++)
            {
                List<Vector3> tmpv = new List<Vector3>();
                tmpv.Add(path[0]); // eka vertex talteen

                for (int q = 0; q < path.Length - 1; q++)
                {
                    Vector3 p0 = path[q];
                    Vector3 p1 = path[q + 1];
                    Vector3 Q, R;

                    // average the 2 original points to create 2 new points. For each
                    // CV, another 2 verts are created.
                    Q.X = 0.75f * p0.X + 0.25f * p1.X;
                    Q.Y = 0.75f * p0.Y + 0.25f * p1.Y;
                    Q.Z = 0.75f * p0.Z + 0.25f * p1.Z;

                    R.X = 0.25f * p0.X + 0.75f * p1.X;
                    R.Y = 0.25f * p0.Y + 0.75f * p1.Y;
                    R.Z = 0.25f * p0.Z + 0.75f * p1.Z;

                    tmpv.Add(Q);
                    tmpv.Add(R);
                }

                tmpv.Add(path[path.Length - 1]); // vika vertex
                if(Looping)  tmpv.Add(path[0]); // eka vertex 

                // korvataan alkuperäinen reitti uudella reitillä
                path = null;
                path = new Vector3[tmpv.Count];
                for (int q = 0; q < path.Length; q++) path[q] = tmpv[q];
            }
            Log.WriteDebugLine("NewPath: " + path.Length);
        }

        /// <summary>
        /// aseta obj seuraamaan pathia. jos obj on Mesh ja lookAtNextPoint==true, objekti kääntyy pathin suuntaan
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="loop"></param>
        /// <param name="lookAtNextPoint"></param>
        public void FollowPath(Node obj, bool loop, bool lookAtNextPoint)
        {
            attachedObj = obj;
            obj.Position = path[0];
            this.Looping = loop;
            this.LookAtNextPoint = lookAtNextPoint;
        }

        public void UpdatePath(float updateTime)
        {
            Time += updateTime;

            int v1 = (int)Time;
            int v2 = v1 + 1;
            if ((v1 >= path.Length || v2 >= path.Length) && Looping == false) return;

            v1 %= path.Length;
            v2 %= path.Length;

            // laske Position reitillä
            Vector3 p1 = path[v1];
            Vector3 p2 = path[v2];
            Vector3 p = p2 - p1;
            float d = Time - (int)Time;
            p *= d;
            attachedObj.Position = p1 + p;

            Vector3 to;

            // laske kohta johon katsotaan
            if (LookAtNextPoint)
            {
                to = (path[(v2 + 1) % path.Length]) - p2;
                to = p2 + (to * d);
                attachedObj.Front = to;
            }
            else to = Front;

            // kamera asetetaan heti
            if (attachedObj is Camera)
            {
                GL.LoadIdentity();
                MathExt.LookAt(attachedObj.Position, to, attachedObj.Up);
                GL.GetFloat(GetPName.ModelviewMatrix, Util.ModelMatrix);
                Util.CopyArray(ref Util.ModelMatrix, ref attachedObj.Matrix);
            }
            else
            {
                if (LookAtNextPoint)
                {
                    // otetaan käännetyn objektin matriisi talteen
                    GL.PushMatrix();
                    GL.LoadIdentity();
                    MathExt.LookAt(attachedObj.Position, to, attachedObj.Up);
                    GL.GetFloat(GetPName.ModelviewMatrix, Util.ModelMatrix);

                    //Util.CopyArray(ref Util.ModelMatrix, ref attachedObj.Matrix);
                    //Util.CopyArray(ref Util.ModelMatrix, ref attachedObj.WMatrix);
                    // paikkatiedot nollaks
                    //Matrix[13] = Matrix[14] = Matrix[15] = 0;
                    //WMatrix[13] = WMatrix[14] = WMatrix[15] = 0;

                    GL.PopMatrix();

                    float heading, attitude, bank;
                    MathExt.MatrixToEuler(ref Util.ModelMatrix, out heading, out attitude, out bank);
                    attachedObj.Rotation.Y = heading * MathExt.RadToDeg;
                    attachedObj.Rotation.X = bank * MathExt.RadToDeg;
                    attachedObj.Rotation.Z = attitude * MathExt.RadToDeg;
                }
            }
        }

        /// <summary>
        /// käydään path läpi, joka vertexin kohdalla (xz) etsitään y ja lisätään siihen yp.
        /// </summary>
        /// <param name="yp"></param>
        /// <param name="obj"></param>
        public void FixPathY(int yp, ref Mesh obj)
        {
            Vector3 v;
            for (int q = 0; q < path.Length; q++)
            {
                v = path[q];
                v.Y = -10000;  // vektorin toinen pää kaukana alhaalla

                if (Intersection.CheckIntersection(ref path[q], ref v, ref obj))
                {
                    path[q].Y = Intersection.IntersectionPoint.Y + yp;
                }
            }
        }

        public new void Render() { }
        public new void RenderMesh() { }
    }
}
