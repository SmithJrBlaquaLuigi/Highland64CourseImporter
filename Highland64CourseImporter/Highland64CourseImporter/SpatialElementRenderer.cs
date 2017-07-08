using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Highland64CourseImporter
{
    public partial class SpatialElementRenderer : UserControl
    {
        //Thank you StackOverflow:
        //https://stackoverflow.com/questions/487661/how-do-i-suspend-painting-for-a-control-and-its-children

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        //Events to tell TestForm when an object is selected/dragged
        public delegate void ObjectSelectedEvent(object obj);

        public ObjectSelectedEvent ObjectSelected = delegate { };

        public delegate void ObjectXYChangedEvent(object obj, double newX, double newY);

        public ObjectXYChangedEvent ObjectXYChanged = delegate { };

        //A dot on the screen, representing one X/Y object
        public class SpatialObject
        {
            public double X;
            public double Y;
            public PictureBox PBReference;
            public object Tag;

            public void SetHighlight(bool highlight)
            {
                if (highlight)
                {
                    PBReference.Image = Highland64CourseImporter.Properties.Resources.blipSelect;
                    PBReference.BringToFront();
                }
                else
                {
                    PBReference.Image = Highland64CourseImporter.Properties.Resources.blip;
                }
            }
        }

        public List<SpatialObject> Elements { get; private set; }

        public Dictionary<object, SpatialElementRenderer.SpatialObject> ObjectToDot { get; private set; }

        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MinY { get; set; }
        public double MaxY { get; set; }

        private object _selectedObject;

        private SpatialObject _movingObject = null;

        private int _mouseX, _mouseY;

        public SpatialElementRenderer()
        {
            InitializeComponent();

            MinX = -1;
            MaxX = 1;
            MinY = -1;
            MaxX = 1;

            Elements = new List<SpatialObject>();
            ObjectToDot = new Dictionary<object, SpatialObject>();
        }

        public void ClearElements()
        {
            SuspendDrawing(this);
            while (Elements.Count > 0)
            {
                this.Controls.Remove(Elements[0].PBReference);
                Elements[0].PBReference.Dispose();
                Elements.RemoveAt(0);
            }
            ObjectToDot.Clear();
            _selectedObject = null;

            ReDraw();
            ResumeDrawing(this);
        }

        public void AddElement(object tag, double x, double y, bool redraw = false)
        {
            PictureBox picture = new PictureBox();
            picture.Image = Highland64CourseImporter.Properties.Resources.blip;
            picture.MouseDown += new MouseEventHandler(picture_MouseDown);
            picture.MouseUp += new MouseEventHandler(picture_MouseUp);
            picture.MouseMove += new MouseEventHandler(picture_MouseMove);
            SpatialObject obj = new SpatialObject();
            obj.X = x;// (x - MinX) / (MaxX - MinX) * this.Width;
            obj.Y = y;// (y - MinY) / (MaxY - MinY) * this.Height;
            obj.PBReference = picture;
            obj.Tag = tag;
            Elements.Add(obj);
            ObjectToDot.Add(tag, obj);

            if (redraw)
                ReDraw();
        }

        public void ReDraw()
        {
            SuspendDrawing(this);
            this.Controls.Clear();

            foreach (SpatialObject obj in Elements)
            {
                int newX = (int)Math.Round(this.Width * (obj.X - MinX) / (MaxX - MinX));
                int newY = (int)Math.Round(this.Height * (obj.Y - MinY) / (MaxY - MinY));

                //inverse the number here?
                newX = (int)Math.Round((this.Width / 2.0) - (newX - this.Width / 2.0));
                //newY = (int)Math.Round((this.Height / 2.0) - (newY - this.Height / 2.0));

                //Offset for the image size
                newX -= 2;
                newY -= 2;

                obj.PBReference.SetBounds(newX, newY, Highland64CourseImporter.Properties.Resources.blip.Width,
                    Highland64CourseImporter.Properties.Resources.blip.Height);
                this.Controls.Add(obj.PBReference);
            }
            ResumeDrawing(this);
        }
        
        public void Select(object obj)
        {
            if (ObjectToDot.ContainsKey(obj) && _selectedObject != obj)
            {
                if (_selectedObject != null)
                    ObjectToDot[_selectedObject].SetHighlight(false);

                ObjectToDot[obj].SetHighlight(true);
                _selectedObject = obj;
            }
        }

        private SpatialObject GetSpatialObjectFromPicture(PictureBox pb)
        {
            foreach (SpatialObject obj in Elements)
            {
                if (obj.PBReference == pb)
                    return obj;
            }
            return null;
        }

        private void SpatialElementRenderer_Resize(object sender, EventArgs e)
        {
            ReDraw();
        }

        private void picture_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_movingObject != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _movingObject.PBReference.Left += (e.X - _mouseX);
                _movingObject.PBReference.Top += (e.Y - _mouseY);

                //_mouseX = e.X;
                //_mouseY = e.Y;
            }
        }

        private void picture_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_movingObject != null)
            {
                //Release & update the X/Y of the object
                int newX = _movingObject.PBReference.Left;
                int newY = _movingObject.PBReference.Top;

                //Offset from center
                newX += 2;
                newY += 2;

                //Inverse the X direction
                newX = (int)Math.Round((this.Width / 2.0) - (newX - this.Width / 2.0));

                //Convert
                _movingObject.X = newX * (MaxX - MinX) / this.Width + MinX;
                _movingObject.Y = newY * (MaxY - MinY) / this.Height + MinY;

                ObjectXYChanged(_movingObject.Tag, +_movingObject.X, _movingObject.Y);

                _movingObject = null;
            }
        }
        private void picture_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Get the Spatial Image it belongs to
            SpatialObject obj = GetSpatialObjectFromPicture((PictureBox)sender);
            if(obj == null) return;

            _mouseX = e.X;
            _mouseY = e.Y;
            _movingObject = obj;

            Select(obj.Tag);
            ObjectSelected(obj.Tag);
        }
    }
}
