using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter.Data.CourseBlocks
{
    /// <summary>
    /// Course block acts as a container for course data. This class is meant to be inherited and
    ///  expanded on for the different types of data (height data, object list, etc.). All you
    ///  have to do in your child class is convert the bytes in the constructor to meaningful data
    ///  and implement GetAsBytes() to turn it back into a byte array. The rest (loading/saving that
    ///  data to the rom) should already be handled.
    /// </summary>
    public abstract class CourseBlock
    {
        public int RomOffset { get; private set; }
        public int BlockLength { get; private set; }

        /// <summary>
        /// Converts the block data into a byte array to store it back in the rom file.
        /// </summary>
        public abstract byte[] GetAsBytes();
        
        byte[] CourseBlockData = new byte[0x04];
        byte[] Surfacedata = new byte[0x10];
        byte[] objectdata = new byte[0x2D];      


        /// <summary>
        /// Constructor. The byte[] data will be used by the child class.
        /// </summary>
        public CourseBlock(byte[] data, int offset, int length)
        {
            RomOffset = offset;
            BlockLength = length;
        }

        /// <summary>
        /// Update the RomOffset/BlockLength. Used when the block data is resized for whatever reason.
        /// </summary>
        public void Update(int offset, int length)
        {
            RomOffset = offset;
            BlockLength = length;
        }

        /// <summary>
        /// Converts the RomOffset/BlockLength into an 8 byte course pointer format that's
        ///  used in the CourseTable.
        /// </summary>
        public byte[] GetPointerAsBytes()
        {
            byte[] bytes = new byte[8];
            byte[] length = BitConverter.GetBytes(BlockLength).Reverse().ToArray();
            byte[] offset = BitConverter.GetBytes(RomOffset).Reverse().ToArray();

            Array.Copy(length, bytes, 4);
            Array.Copy(offset, 0, bytes, 4, 4);

            return bytes;
        }
    }
}
