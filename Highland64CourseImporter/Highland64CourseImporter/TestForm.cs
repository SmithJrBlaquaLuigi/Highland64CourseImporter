using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Highland64CourseImporter.Data;

namespace Highland64CourseImporter
{
    public partial class TestForm : Form
    {
        private MG64RomFile _rom;

        public TestForm(MG64RomFile rom)
        {
            InitializeComponent();

            _rom = rom;

            Initialize();
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
            pbPreview.Image = null;

            if (_rom == null || cbCourse.SelectedIndex == -1)
                return;

            CourseTableEntry course = _rom.CourseTable.Entries[CourseNames.OrderedIndices[cbCourse.SelectedIndex]];

            pbPreview.Image = course.SurfaceMap.GetAsBitmap();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {

        }
    }
}
