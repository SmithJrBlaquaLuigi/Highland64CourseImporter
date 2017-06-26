using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Highland64CourseImporter.Data.CourseBlocks
{
    /// <summary>
    /// A CourseBlock that stores all surface map data for the course.
    /// </summary>
    public class SurfaceCourseBlock : CourseBlock
    {
        //Used for preview image
        public Color[] SurfaceColors = new Color[]
        {
            Color.Green,
            Color.DarkGreen,
            Color.Beige,
            Color.DeepSkyBlue,
            Color.Gray,
            Color.DarkOliveGreen,
            Color.LawnGreen,
            Color.Black,
            Color.LimeGreen,
            Color.SaddleBrown,
            Color.Orange,
            Color.SlateGray,
            Color.Pink,
            Color.Purple,
            Color.OrangeRed,
            Color.Maroon,
            Color.SandyBrown
        };

        public enum SurfaceType
        {
            Fairway = 0x00,
            Rough = 0x01,
            Bunker = 0x02,
            GroundUnderwater = 0x03,
            Cartway = 0x04,
            DeepRough = 0x05,
            OnGreen = 0x06,
            OutOfBounds1 = 0x07,
            TeeGround = 0x08,
            Rock = 0x09,
            OutOfBounds2 = 0x0A,
            OutOfBounds3 = 0x0B,
            OutOfBounds4 = 0x0C,
            OutOfBounds5 = 0x0D,
            OutOfBounds6 = 0x0E,
            OutOfBounds7 = 0x0F,
            FairwaySand = 0x10
            //Others?
        }

        public SurfaceType[,] SurfaceMap { get; private set; }

        public SurfaceCourseBlock(byte[] data, int offset, int length)
            : base(data, offset, length)
        {
            //Need to decompress the data
            byte[] decompressedData = MarioGolf64Compression.DecompressGolfHalfword(data, 0, 0);

            SurfaceMap = new SurfaceType[0x100, 0x200];

            for (int j = 0; j < 0x200; j++)
            {
                for (int i = 0; i < 0x100; i++)
                {
                    SurfaceMap[i, j] = (SurfaceType)decompressedData[(0xFF - i) + j * 0x100];
                }
            }
        }

        public override byte[] GetAsBytes()
        {
            byte[] decompressedData = new byte[0x100 * 0x200];

            for (int j = 0; j < 0x200; j++)
            {
                for (int i = 0; i < 0x100; i++)
                {
                    decompressedData[(0xFF - i) + j * 0x100] = (byte)SurfaceMap[i, j];
                }
            }

            //compress the data
            return MarioGolf64Compression.CompressGolfHalfword(decompressedData);
        }

        public Bitmap GetAsBitmap()
        {
            //Make a new bitmap (CAN BE OPTIMIZED)
            Bitmap bmp = new Bitmap(0x100, 0x200);

            BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, 0x100, 0x200), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* srcPointer = (byte*)srcData.Scan0;

                for (int j = 0; j < 0x200; j++)
                {
                    for (int i = 0; i < 0x100; i++)
                    {
                        srcPointer[0] = SurfaceColors[(int)SurfaceMap[i, j]].B; // Blue
                        srcPointer[1] = SurfaceColors[(int)SurfaceMap[i, j]].G; // Green
                        srcPointer[2] = SurfaceColors[(int)SurfaceMap[i, j]].R; // Red
                        srcPointer[3] = SurfaceColors[(int)SurfaceMap[i, j]].A; // Alpha

                        srcPointer += 4;
                    }
                    //srcPointer += srcStrideOffset + srcTileOffset;
                }
            }

            bmp.UnlockBits(srcData);

            //for (int j = 0; j < 0x200; j++)
            //{
            //    for (int i = 0; i < 0x100; i++)
            //    {
            //        bmp.SetPixel(i, j, SurfaceColors[(int)SurfaceMap[i, j]]);
            //    }
            //}

            return bmp;
        }
    }
}
