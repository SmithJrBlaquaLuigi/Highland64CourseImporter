using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Highland64CourseImporter.Data;
using Highland64CourseImporter.Data.CourseBlocks;

namespace Highland64CourseImporter
{
    public partial class TestForm : Form
    {
        private MG64RomFile _rom;

        private List<ObjectInfo> _levelObjects;

        private Bitmap _underlyingLevelImage;

        private SpatialElementRenderer _spatialElementRenderer { get; set; }

        private ObjectCourseBlock _cbObjectList { get; set; }
        private ObjectCourseBlock _cbObjectType { get; set; }
        private ObjectCourseBlock _cbObjects { get; set; }
        private ObjectCourseBlock _nudWidthScale { get; set; }
        private ObjectCourseBlock _nudHeightScale { get; set; }
        private ObjectCourseBlock _gbObjectSelection { get; set; }
        private ObjectCourseBlock _nudHeight { get; set; }
        private ObjectCourseBlock lblX { get; set; }
        private ObjectCourseBlock lblZ { get; set; }
        private ObjectCourseBlock _btnRemoveOld { get; set; }
        public int SelectedIndex { get; internal set; }
        private object Items { get; set; }

        public TestForm(MG64RomFile rom)
            
        {
            InitializeComponent();

            _rom = rom;

            _spatialElementRenderer.MinX = 0;
            _spatialElementRenderer.MinY = 0;
            _spatialElementRenderer.MaxX = 0x1000;
            _spatialElementRenderer.MaxY = 0x2000;

            foreach (string s in Enum.GetNames(typeof(ObjectInfo.ObjectType)))
            {
                cbCourse.Items.Add(s);
            }

            _spatialElementRenderer.ObjectSelected += ObjectClicked;
            _spatialElementRenderer.ObjectXYChanged += ObjectXYChanged;
        }

        private void Initialize()
        {
            //Fill out the combo box
            cbCourse.Items.Clear();

            foreach (int index in CourseNames.OrderedIndices)
            {
                cbCourse.Items.Add(CourseNames.NameList[index]);
            }

            //Select first course
            cbCourse.SelectedIndex = 0;
        }

        private void cbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCourse();
        }

        private void UpdateCourse(int selectedObjectIndex = 0)
        {
            pbPreview.Image = null;

            if (_rom == null || cbCourse.SelectedIndex == -1)
                return;

            _underlyingLevelImage = SelectedCourse.SurfaceMap.GetAsBitmap();

            _levelObjects = SelectedCourse.ObjectList.ObjectList;

            UpdateImage();

            UpdateObjectList(selectedObjectIndex);
        }

        private void UpdateObjectList(int selectedObjectIndex = 0)
        {
            cbCourse.Items.Clear();

            for (int i = 0; i < _levelObjects.Count; i++)
            {
                cbCourse.Items.Add("Object " + (i + 1));
            }

            if (cbCourse.Items.Count > selectedObjectIndex)
                _cbObjectList.SelectedIndex = selectedObjectIndex;
            else if (cbCourse.Items.Count > 0)
                _cbObjectList.SelectedIndex = 0;
        }

        private void UpdateImage()
        {
            pbPreview.Image = _underlyingLevelImage;
            if (EnableObjects)
            {
                _spatialElementRenderer.ClearElements();

                AddLevelElements();

                _spatialElementRenderer.ReDraw();
            }
        }


        private void AddLevelElements()
        {
            foreach (ObjectInfo obj in _levelObjects)
            {
                _spatialElementRenderer.AddElement(obj, obj.X, obj.Z);
            }
        }

        private bool EnableObjects
        {
            get
            {
                return _cbObjects.Checked;
            }
        }

        private CourseTableEntry SelectedCourse
        {
            get
            {
                if (cbCourse.SelectedIndex < 0 || cbCourse.SelectedIndex >= CourseNames.OrderedIndices.Length)
                    return null;

                return _rom.CourseTable.Entries[CourseNames.OrderedIndices[cbCourse.SelectedIndex]];
            }
        }

        private ObjectInfo SelectedObject
        {
            get
            {
                if (_cbObjectList.SelectedIndex < 0 || _cbObjectList.SelectedIndex >= _levelObjects.Count)
                    return null;

                return _levelObjects[_cbObjectList.SelectedIndex];
            }
        }
        
        private void ObjectXYChanged(object obj, double newX, double newY)
        {
            ObjectInfo objI = (ObjectInfo)obj;
            objI.X = (short)Math.Round(newX);
            objI.Z = (short)Math.Round(newY);
            UpdateObjectUI(objI);
        }

        private void ObjectClicked(object obj)
        {
            _cbObjectList.SelectedIndexChanged -= cbObjectList_SelectedIndexChanged;

            _cbObjectList.SelectedIndex = _levelObjects.IndexOf((ObjectInfo)obj);

            _cbObjectList.SelectedIndexChanged += cbObjectList_SelectedIndexChanged;

            //Basically update the ui and silently set the dropdown box
            UpdateObjectUI((ObjectInfo)obj);
        }

        private void UpdateObjectUI(ObjectInfo obj)
        {
            
            _nudHeightScale.Value = obj.HeightFactor;
            _nudWidthScale.Value = obj.WidthFactor;

            _cbObjectType.SelectedIndex = (int)obj.Type;

            lblX.Text = obj.X.ToString();
            lblZ.Text = obj.Z.ToString();
        }

        private void cbObjects_CheckedChanged(object sender, EventArgs e)
        {
            _gbObjectSelection.Enabled = _cbObjects.Checked;

            if (_cbObjects.Checked)
            {
                AddLevelElements();
                _spatialElementRenderer.ReDraw();
            }
            else
            {
                _spatialElementRenderer.ClearElements();
            }
        }

        private void cbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _btnRemoveOld.Enabled = false;

            if (_cbObjectList.SelectedIndex >= _levelObjects.Count)
                return;

            ObjectInfo info = _levelObjects[_cbObjectList.SelectedIndex];

            if (SelectedObject == null) return;

            _btnRemoveOld.Enabled = true;

            UpdateObjectUI(SelectedObject);

            //Highlight it
            _spatialElementRenderer.Select(SelectedObject);
        }

        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.Type = (ObjectInfo.ObjectType)_cbObjectType.SelectedIndex;
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.Y = (short)_nudHeight.Value;
        }

        private void nudHeightScale_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.HeightFactor = (ushort)_nudHeightScale.Value;
        }

        private void nudWidthScale_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.WidthFactor = (ushort)_nudWidthScale.Value;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ObjectInfo info = new ObjectInfo();
            info.X = 100;
            info.Z = 100;
            _levelObjects.Add(info);
            UpdateImage();
            UpdateObjectList(_levelObjects.Count - 1);
        }

        private void btnRemoveOld_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                _levelObjects.Remove(SelectedObject);
                UpdateImage();
                UpdateObjectList();
            }
        }
    }
}