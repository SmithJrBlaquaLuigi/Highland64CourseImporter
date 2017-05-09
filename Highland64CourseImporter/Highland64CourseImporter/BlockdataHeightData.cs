using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Highland64CourseImporter
{
    class BlockdataHeightData
    {
        private string openfiledialog1;

        private object bw;

        private int intHeight;

        private void HeightData( int IntHeight)
        {
            for (int i = 0; i < intHeight; i++)
            {
                FileStream fs = new FileStream(openfiledialog1, FileMode.Open, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);
                BinaryReader br = new BinaryReader(fs);
                //find a height data by seeking through binary. The height data has been formed into clumps. In other words, height data makes the textures diffuse is dark due to elevated height data on each level.
                bw.Seek(0xEC7C20, SeekOrigin.Begin);
                
            }
        }
    }
}
