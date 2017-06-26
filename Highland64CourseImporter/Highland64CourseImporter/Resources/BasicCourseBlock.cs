using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter.Data.CourseBlocks
{
    /// <summary>
    /// A simple CourseBlock that stores all its data as a byte array. Almost non-interactable,
    ///  but it's a placeholder for now.
    /// </summary>
    public class BasicCourseBlock : CourseBlock
    {
        public byte[] Data { get; private set; }

        public BasicCourseBlock(byte[] data, int offset, int length)
            : base(data, offset, length)
        {
            Data = data;
        }

        public override byte[] GetAsBytes()
        {
            return Data;
        }
    }
}
