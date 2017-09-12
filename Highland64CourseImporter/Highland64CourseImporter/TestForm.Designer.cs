namespace Highland64CourseImporter
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCourse = new System.Windows.Forms.Label();
            this.cbCourse = new System.Windows.Forms.ComboBox();
            this.pnlSetings = new System.Windows.Forms.Panel();
            this.pnlViewer = new System.Windows.Forms.Panel();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.spatialElementRenderer = new Highland64CourseImporter.SpatialElementRenderer();
            this.pnlObjects = new System.Windows.Forms.Panel();
            this.gbObjects = new System.Windows.Forms.GroupBox();
            this.gbObjectSelection = new System.Windows.Forms.GroupBox();
            this.btnRemoveOld = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.lblZ = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblObjectZ = new System.Windows.Forms.Label();
            this.lblObjectX = new System.Windows.Forms.Label();
            this.lblWidthScale = new System.Windows.Forms.Label();
            this.nudWidthScale = new System.Windows.Forms.NumericUpDown();
            this.lblHeightScale = new System.Windows.Forms.Label();
            this.nudHeightScale = new System.Windows.Forms.NumericUpDown();
            this.lblHeight = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.cbObjectList = new System.Windows.Forms.ComboBox();
            this.lblObjectType = new System.Windows.Forms.Label();
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.cbObjects = new System.Windows.Forms.CheckBox();
            this.pnlSetings.SuspendLayout();
            this.pnlViewer.SuspendLayout();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.pbPreview.SuspendLayout();
            this.pnlObjects.SuspendLayout();
            this.gbObjects.SuspendLayout();
            this.gbObjectSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCourse
            // 
            this.lblCourse.AutoSize = true;
            this.lblCourse.Location = new System.Drawing.Point(12, 11);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new System.Drawing.Size(43, 13);
            this.lblCourse.TabIndex = 1;
            this.lblCourse.Text = "Course:";
            // 
            // cbCourse
            // 
            this.cbCourse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCourse.FormattingEnabled = true;
            this.cbCourse.Location = new System.Drawing.Point(61, 8);
            this.cbCourse.Name = "cbCourse";
            this.cbCourse.Size = new System.Drawing.Size(200, 21);
            this.cbCourse.TabIndex = 2;
            this.cbCourse.SelectedIndexChanged += new System.EventHandler(this.cbCourse_SelectedIndexChanged);
            // 
            // pnlSetings
            // 
            this.pnlSetings.Controls.Add(this.cbCourse);
            this.pnlSetings.Controls.Add(this.lblCourse);
            this.pnlSetings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSetings.Location = new System.Drawing.Point(0, 0);
            this.pnlSetings.Name = "pnlSetings";
            this.pnlSetings.Size = new System.Drawing.Size(273, 42);
            this.pnlSetings.TabIndex = 3;
            // 
            // pnlViewer
            // 
            this.pnlViewer.Controls.Add(this.pnlImage);
            this.pnlViewer.Controls.Add(this.pnlSetings);
            this.pnlViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlViewer.Location = new System.Drawing.Point(0, 0);
            this.pnlViewer.Name = "pnlViewer";
            this.pnlViewer.Size = new System.Drawing.Size(273, 482);
            this.pnlViewer.TabIndex = 4;
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.pbPreview);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 42);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(273, 440);
            this.pnlImage.TabIndex = 4;
            // 
            // pbPreview
            // 
            this.pbPreview.Controls.Add(this.spatialElementRenderer);
            this.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPreview.Location = new System.Drawing.Point(0, 0);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(273, 440);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // spatialElementRenderer
            // 
            this.spatialElementRenderer.BackColor = System.Drawing.Color.Transparent;
            this.spatialElementRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spatialElementRenderer.Location = new System.Drawing.Point(0, 0);
            this.spatialElementRenderer.MaxX = 1D;
            this.spatialElementRenderer.MaxY = 0D;
            this.spatialElementRenderer.MinX = -1D;
            this.spatialElementRenderer.MinY = -1D;
            this.spatialElementRenderer.Name = "spatialElementRenderer";
            this.spatialElementRenderer.Size = new System.Drawing.Size(273, 440);
            this.spatialElementRenderer.TabIndex = 1;
            // 
            // pnlObjects
            // 
            this.pnlObjects.Controls.Add(this.gbObjects);
            this.pnlObjects.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlObjects.Location = new System.Drawing.Point(273, 0);
            this.pnlObjects.Name = "pnlObjects";
            this.pnlObjects.Size = new System.Drawing.Size(146, 482);
            this.pnlObjects.TabIndex = 5;
            // 
            // gbObjects
            // 
            this.gbObjects.Controls.Add(this.gbObjectSelection);
            this.gbObjects.Controls.Add(this.cbObjects);
            this.gbObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbObjects.Location = new System.Drawing.Point(0, 0);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(146, 482);
            this.gbObjects.TabIndex = 0;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // gbObjectSelection
            // 
            this.gbObjectSelection.Controls.Add(this.btnRemoveOld);
            this.gbObjectSelection.Controls.Add(this.btnAddNew);
            this.gbObjectSelection.Controls.Add(this.lblZ);
            this.gbObjectSelection.Controls.Add(this.lblX);
            this.gbObjectSelection.Controls.Add(this.lblObjectZ);
            this.gbObjectSelection.Controls.Add(this.lblObjectX);
            this.gbObjectSelection.Controls.Add(this.lblWidthScale);
            this.gbObjectSelection.Controls.Add(this.nudWidthScale);
            this.gbObjectSelection.Controls.Add(this.lblHeightScale);
            this.gbObjectSelection.Controls.Add(this.nudHeightScale);
            this.gbObjectSelection.Controls.Add(this.lblHeight);
            this.gbObjectSelection.Controls.Add(this.nudHeight);
            this.gbObjectSelection.Controls.Add(this.cbObjectList);
            this.gbObjectSelection.Controls.Add(this.lblObjectType);
            this.gbObjectSelection.Controls.Add(this.cbObjectType);
            this.gbObjectSelection.Enabled = false;
            this.gbObjectSelection.Location = new System.Drawing.Point(10, 42);
            this.gbObjectSelection.Name = "gbObjectSelection";
            this.gbObjectSelection.Size = new System.Drawing.Size(130, 349);
            this.gbObjectSelection.TabIndex = 6;
            this.gbObjectSelection.TabStop = false;
            this.gbObjectSelection.Text = "Object Select";
            // 
            // btnRemoveOld
            // 
            this.btnRemoveOld.Enabled = false;
            this.btnRemoveOld.Location = new System.Drawing.Point(17, 315);
            this.btnRemoveOld.Name = "btnRemoveOld";
            this.btnRemoveOld.Size = new System.Drawing.Size(102, 23);
            this.btnRemoveOld.TabIndex = 17;
            this.btnRemoveOld.Text = "Remove Object";
            this.btnRemoveOld.UseVisualStyleBackColor = true;
            this.btnRemoveOld.Click += new System.EventHandler(this.btnRemoveOld_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(17, 286);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(102, 23);
            this.btnAddNew.TabIndex = 16;
            this.btnAddNew.Text = "Add New Object";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(43, 260);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(0, 13);
            this.lblZ.TabIndex = 15;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(43, 237);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(0, 13);
            this.lblX.TabIndex = 14;
            // 
            // lblObjectZ
            // 
            this.lblObjectZ.AutoSize = true;
            this.lblObjectZ.Location = new System.Drawing.Point(16, 260);
            this.lblObjectZ.Name = "lblObjectZ";
            this.lblObjectZ.Size = new System.Drawing.Size(17, 13);
            this.lblObjectZ.TabIndex = 13;
            this.lblObjectZ.Text = "Z:";
            // 
            // lblObjectX
            // 
            this.lblObjectX.AutoSize = true;
            this.lblObjectX.Location = new System.Drawing.Point(16, 237);
            this.lblObjectX.Name = "lblObjectX";
            this.lblObjectX.Size = new System.Drawing.Size(17, 13);
            this.lblObjectX.TabIndex = 12;
            this.lblObjectX.Text = "X:";
            // 
            // lblWidthScale
            // 
            this.lblWidthScale.AutoSize = true;
            this.lblWidthScale.Location = new System.Drawing.Point(27, 187);
            this.lblWidthScale.Name = "lblWidthScale";
            this.lblWidthScale.Size = new System.Drawing.Size(65, 13);
            this.lblWidthScale.TabIndex = 11;
            this.lblWidthScale.Text = "Width Scale";
            // 
            // nudWidthScale
            // 
            this.nudWidthScale.Location = new System.Drawing.Point(10, 203);
            this.nudWidthScale.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudWidthScale.Name = "nudWidthScale";
            this.nudWidthScale.Size = new System.Drawing.Size(98, 20);
            this.nudWidthScale.TabIndex = 10;
            this.nudWidthScale.ValueChanged += new System.EventHandler(this.nudWidthScale_ValueChanged);
            // 
            // lblHeightScale
            // 
            this.lblHeightScale.AutoSize = true;
            this.lblHeightScale.Location = new System.Drawing.Point(27, 143);
            this.lblHeightScale.Name = "lblHeightScale";
            this.lblHeightScale.Size = new System.Drawing.Size(68, 13);
            this.lblHeightScale.TabIndex = 9;
            this.lblHeightScale.Text = "Height Scale";
            // 
            // nudHeightScale
            // 
            this.nudHeightScale.Location = new System.Drawing.Point(10, 159);
            this.nudHeightScale.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudHeightScale.Name = "nudHeightScale";
            this.nudHeightScale.Size = new System.Drawing.Size(98, 20);
            this.nudHeightScale.TabIndex = 8;
            this.nudHeightScale.ValueChanged += new System.EventHandler(this.nudHeightScale_ValueChanged);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(40, 104);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(38, 13);
            this.lblHeight.TabIndex = 7;
            this.lblHeight.Text = "Height";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(10, 120);
            this.nudHeight.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(98, 20);
            this.nudHeight.TabIndex = 6;
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // cbObjectList
            // 
            this.cbObjectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbObjectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjectList.FormattingEnabled = true;
            this.cbObjectList.Location = new System.Drawing.Point(6, 19);
            this.cbObjectList.Name = "cbObjectList";
            this.cbObjectList.Size = new System.Drawing.Size(113, 21);
            this.cbObjectList.TabIndex = 3;
            this.cbObjectList.SelectedIndexChanged += new System.EventHandler(this.cbObjectList_SelectedIndexChanged);
            // 
            // lblObjectType
            // 
            this.lblObjectType.AutoSize = true;
            this.lblObjectType.Location = new System.Drawing.Point(40, 50);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(31, 13);
            this.lblObjectType.TabIndex = 5;
            this.lblObjectType.Text = "Type";
            // 
            // cbObjectType
            // 
            this.cbObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjectType.FormattingEnabled = true;
            this.cbObjectType.Location = new System.Drawing.Point(6, 69);
            this.cbObjectType.Name = "cbObjectType";
            this.cbObjectType.Size = new System.Drawing.Size(113, 21);
            this.cbObjectType.TabIndex = 4;
            this.cbObjectType.SelectedIndexChanged += new System.EventHandler(this.cbObjectType_SelectedIndexChanged);
            // 
            // cbObjects
            // 
            this.cbObjects.AutoSize = true;
            this.cbObjects.Location = new System.Drawing.Point(20, 19);
            this.cbObjects.Name = "cbObjects";
            this.cbObjects.Size = new System.Drawing.Size(98, 17);
            this.cbObjects.TabIndex = 0;
            this.cbObjects.Text = "Enable Objects";
            this.cbObjects.UseVisualStyleBackColor = true;
            this.cbObjects.CheckedChanged += new System.EventHandler(this.cbObjects_CheckedChanged);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 482);
            this.Controls.Add(this.pnlViewer);
            this.Controls.Add(this.pnlObjects);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.pnlSetings.ResumeLayout(false);
            this.pnlSetings.PerformLayout();
            this.pnlViewer.ResumeLayout(false);
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.pbPreview.ResumeLayout(false);
            this.pnlObjects.ResumeLayout(false);
            this.gbObjects.ResumeLayout(false);
            this.gbObjects.PerformLayout();
            this.gbObjectSelection.ResumeLayout(false);
            this.gbObjectSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.ComboBox cbCourse;
        private System.Windows.Forms.Panel pnlSetings;
        private System.Windows.Forms.Panel pnlViewer;
        private System.Windows.Forms.Panel pnlObjects;
        private System.Windows.Forms.GroupBox gbObjects;
        private System.Windows.Forms.CheckBox cbObjects;
        private System.Windows.Forms.Panel pnlImage;
        private SpatialElementRenderer spatialElementRenderer;
        private System.Windows.Forms.Label lblObjectType;
        private System.Windows.Forms.ComboBox cbObjectType;
        private System.Windows.Forms.ComboBox cbObjectList;
        private System.Windows.Forms.GroupBox gbObjectSelection;
        private System.Windows.Forms.Label lblHeightScale;
        private System.Windows.Forms.NumericUpDown nudHeightScale;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label lblWidthScale;
        private System.Windows.Forms.NumericUpDown nudWidthScale;
        private System.Windows.Forms.Label lblObjectX;
        private System.Windows.Forms.Label lblObjectZ;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Button btnRemoveOld;
        private System.Windows.Forms.Button btnAddNew;
    }
}