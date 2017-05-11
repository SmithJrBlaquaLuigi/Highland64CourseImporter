using System;

namespace Highland64CourseImporter.Data
{
    public class CourseTable
    {
        private byte[] courseTableData;
        private MG64RomFile mG64RomFile;

        public CourseTable(byte[] courseTableData, MG64RomFile mG64RomFile)
        {
            this.courseTableData = courseTableData;
            this.mG64RomFile = mG64RomFile;
        }

        internal void SerializeData(MG64RomFile mG64RomFile)
        {
            throw new NotImplementedException();
        }
    }
}