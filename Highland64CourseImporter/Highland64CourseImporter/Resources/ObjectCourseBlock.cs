using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter.Data.CourseBlocks
{
    /// <summary>
    /// A CourseBlock that stores all objects on the map.
    /// </summary>
    public class ObjectCourseBlock : CourseBlock
    {
        public byte ObjectCount { get; private set; }

        public List<ObjectInfo> ObjectList { get; private set; }
        public object Items { get; internal set; }
        public int SelectedIndex { get; internal set; }
        public Action<object, EventArgs> SelectedIndexChanged { get; internal set; }
        public ushort Value { get; internal set; }
        public bool Enabled { get; internal set; }
        public bool Checked { get; internal set; }
        public string Text { get; internal set; }
        public object Clear { get; internal set; }

        public bool Add;

        public ObjectCourseBlock(byte[] data, int offset, int length)
            : base(data, offset, length)
        {
            ObjectCount = data[0];
            int dataOffset = 1;
            byte[] objectData = new byte[ObjectInfo.Length];
            ObjectList = new List<ObjectInfo>();
            for (; ObjectList.Count < ObjectCount; dataOffset += ObjectInfo.Length)
            {
                Array.Copy(data, dataOffset, objectData, 0, ObjectInfo.Length);
                ObjectList.Add(new ObjectInfo(objectData));
            }
        }

        public void OptimizeData()
        {
            ObjectCount = (byte)ObjectList.Count;
        }

        public override byte[] GetAsBytes()
        {
            if (ObjectCount < ObjectList.Count)
                OptimizeData();

            byte[] data = new byte[ObjectCount * ObjectInfo.Length + 1];

            data[0] = (byte)ObjectCount;
            for (int i = 0; i < ObjectList.Count; i++)
            {
                Array.Copy(ObjectList[i].GetAsBytes(), 0, data, 1 + i * ObjectInfo.Length, ObjectInfo.Length);
            }

            return data;
        }
    }

    public class ObjectInfo
    {
        public const int Length = 0xC;

        public enum ObjectType
        {
            Flower1 = 0x00,
            Flower2 = 0x01,
            Flower3 = 0x02,
            KoopaTroopaHedge = 0x03,
            BigGreenTurtleShell = 0x04,
            PineTree1 = 0x05,
            PineTree2 = 0x06,
            ThinTree = 0x07,
            ThickTree = 0x08,
            Hedge1 = 0x09,
            ClayishTree = 0x0A,
            SpikeyTree = 0x0B,
            GreenTurtleShell = 0x0C,
            RedTurtleShell = 0x0D,
            SpikeyRedTurtleShell = 0x0E,
            FloweringCactus = 0x0F,
            Cactus = 0x10,
            TallCactus = 0x11,
            ShyguyCactus = 0x12,
            ShyguyBrown = 0x13,
            GreenPillar = 0x14,
            PinkPillar = 0x15,
            BluePillar = 0x16,
            RedMushroom = 0x17,
            GreenMushroom = 0x18,
            PalmTree1 = 0x19,
            PalmTree2 = 0x1A,
            PalmTree3 = 0x1B,
            Orange = 0x1C,
            StrangeFruit = 0x1D,
            PiranhaPlant = 0x1E,
            RedTulip = 0x1F,
            Flower4 = 0x20,
            Flower5 = 0x21,
            Hedge2 = 0x22,
            Sign100 = 0x23,
            Sign200 = 0x24,
            Sign300 = 0x25,
            Sign400 = 0x26,
            SignPlus50 = 0x27,
            SignFlag = 0x28,
            Bobomb = 0x29,
            RedBobomb = 0x2A,
            PinkTree = 0x2B,
            BlueTree = 0x2C,
            GreenTree = 0x2D
        }

        public short X, Y, Z;
        public ObjectType Type;
        public ushort WidthFactor, HeightFactor;

        public ObjectInfo()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Type = ObjectType.Bobomb;
            HeightFactor = 349;
            WidthFactor = 104;
        }

        public ObjectInfo(byte[] data)
        {
            X = (short)((data[0] << 8) + data[1]);
            Y = (short)((data[2] << 8) + data[3]);
            Z = (short)((data[4] << 8) + data[5]);
            Type = (ObjectType)data[6];
            WidthFactor = (ushort)((data[0x8] << 8) + data[0x9]);
            HeightFactor = (ushort)((data[0xA] << 8) + data[0xB]);
        }

        public byte[] GetAsBytes()
        {
            byte[] data = new byte[Length];

            data[0x0] = (byte)((X >> 8) & 0xFF);
            data[0x1] = (byte)(X & 0xFF);
            data[0x2] = (byte)((Y >> 8) & 0xFF);
            data[0x3] = (byte)(Y & 0xFF);
            data[0x4] = (byte)((Z >> 8) & 0xFF);
            data[0x5] = (byte)(Z & 0xFF);
            data[0x6] = (byte)Type;
            data[0x7] = 0;
            data[0x8] = (byte)((WidthFactor >> 8) & 0xFF);
            data[0x9] = (byte)(WidthFactor & 0xFF);
            data[0xA] = (byte)((HeightFactor >> 8) & 0xFF);
            data[0xB] = (byte)(HeightFactor & 0xFF);

            return data;
        }
    }
}