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

        public TestForm(MG64RomFile rom)
        {
            InitializeComponent();

            _rom = rom;

            spatialElementRenderer.MinX = 0;
            spatialElementRenderer.MinY = 0;
            spatialElementRenderer.MaxX = 0x1000;
            spatialElementRenderer.MaxY = 0x2000;

            foreach (string s in Enum.GetNames(typeof(ObjectInfo.ObjectType)))
            {
                cbObjectType.Items.Add(s);
            }

            Initialize();

            spatialElementRenderer.ObjectSelected += ObjectClicked;
            spatialElementRenderer.ObjectXYChanged += ObjectXYChanged;
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
            cbObjectList.Items.Clear();

            for (int i = 0; i < _levelObjects.Count; i++)
            {
                cbObjectList.Items.Add("Object " + (i + 1));
            }

            if (cbObjectList.Items.Count > selectedObjectIndex)
                cbObjectList.SelectedIndex = selectedObjectIndex;
            else if(cbObjectList.Items.Count > 0)
                cbObjectList.SelectedIndex = 0;
        }

        private void UpdateImage()
        {
            pbPreview.Image = _underlyingLevelImage;
            if (EnableObjects)
            {
                spatialElementRenderer.ClearElements();

                AddLevelElements();

                spatialElementRenderer.ReDraw();
            }
        }

        
        private void AddLevelElements()
        {
            foreach (ObjectInfo obj in _levelObjects)
            {
                spatialElementRenderer.AddElement(obj, obj.X, obj.Z);
            }
        }

        private bool EnableObjects
        {
            get
            {
                return cbObjects.Checked;
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
                if (cbObjectList.SelectedIndex < 0 || cbObjectList.SelectedIndex >= _levelObjects.Count)
                    return null;

                return _levelObjects[cbObjectList.SelectedIndex];
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
            cbObjectList.SelectedIndexChanged -= cbObjectList_SelectedIndexChanged;

            cbObjectList.SelectedIndex = _levelObjects.IndexOf((ObjectInfo)obj);

            cbObjectList.SelectedIndexChanged += cbObjectList_SelectedIndexChanged;

            //Basically update the ui and silently set the dropdown box
            UpdateObjectUI((ObjectInfo)obj);
        }

        private void UpdateObjectUI(ObjectInfo obj)
        {
            nudHeight.Value = obj.Y;
            nudHeightScale.Value = obj.HeightFactor;
            nudWidthScale.Value = obj.WidthFactor;

            cbObjectType.SelectedIndex = (int)obj.Type;

            lblX.Text = obj.X.ToString();
            lblZ.Text = obj.Z.ToString();
        }

        private void cbObjects_CheckedChanged(object sender, EventArgs e)
        {
            gbObjectSelection.Enabled = cbObjects.Checked;

            if (cbObjects.Checked)
            {
                AddLevelElements();
                spatialElementRenderer.ReDraw();
            }
            else
            {
                spatialElementRenderer.ClearElements();
            }
        }

        private void cbObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveOld.Enabled = false;

            if (cbObjectList.SelectedIndex >= _levelObjects.Count)
                return;

            ObjectInfo info = _levelObjects[cbObjectList.SelectedIndex];

            if (SelectedObject == null) return;

            btnRemoveOld.Enabled = true;

            UpdateObjectUI(SelectedObject);

            //Highlight it
            spatialElementRenderer.Select(SelectedObject);
        }

        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.Type = (ObjectInfo.ObjectType)cbObjectType.SelectedIndex;
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.Y = (short)nudHeight.Value;
        }

        private void nudHeightScale_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
                SelectedObject.HeightFactor = (ushort)nudHeightScale.Value;
        }

        private void nudWidthScale_ValueChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null) 
                SelectedObject.WidthFactor = (ushort)nudWidthScale.Value;
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
