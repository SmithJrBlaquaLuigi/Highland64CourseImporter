using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter
{
    public class _HeightDataEncoding
    {
        internal struct CommandByte
        {
            public bool UseDefaultZ;
            public bool UseDefaultX;
            public ByteReadingType ZFlags;
            public ByteReadingType YFlags;
            public ByteReadingType XFlags;

            public const byte EXIT_COMMAND = 0x09;

            public enum ByteReadingType
            {
                Read2Bytes = 0,
                Read1ByteNoShift = 1,
                Read1ByteShift1 = 2,
                Read1ByteShift2 = 3
            }

            public CommandByte(byte command)
            {
                UseDefaultZ = ((command & 0x80) != 0);
                UseDefaultX = ((command & 0x40) != 0);
                ZFlags = (ByteReadingType)((command >> 0x4) & 0x3);
                YFlags = (ByteReadingType)((command >> 0x2) & 0x3);
                XFlags = (ByteReadingType)(command & 0x3);
            }

            public byte GetAsByte()
            {
                byte command = 0;
                command |= (byte)XFlags;
                command |= (byte)((int)YFlags << 0x2);
                command |= (byte)((int)ZFlags << 0x4);
                if (UseDefaultX) command |= 0x40;
                if (UseDefaultZ) command |= 0x80;

                return command;
            }

            public bool IsQuitCommand
            {
                get
                {
                    return GetAsByte() == EXIT_COMMAND;
                }
            }
        }

        public class HeightMapVertex
        {
            public short X;
            public short Y;
            public short Z;

            public short S;
            public short T;

            public byte R;
            public byte G;
            public byte B;
            public byte A;

            public HeightMapVertex(short x, short y, short z)
            {
                X = x;
                Y = y;
                Z = z;

                S = (short)Math.Min(((X << 0x10) >> 0xe) + 0x20, 0x7FFF);
                T = (short)Math.Min(((Z << 0x10) >> 0xe) + 0x20, 0x7FFF);

                R = 0x80;
                G = 0x80;
                B = 0x80;
                A = 0xFF;
            }

            public HeightMapVertex(byte[] data)
            {
                X = (short)((data[0] << 8) + data[1]);
                Y = (short)((data[2] << 8) + data[3]);
                Z = (short)((data[4] << 8) + data[5]);

                S = (short)((data[8] << 8) + data[9]);
                T = (short)((data[10] << 8) + data[11]);

                R = data[12];
                G = data[13];
                B = data[14];
                A = data[15];
            }

            public byte[] GetAsBytes()
            {
                byte[] data = new byte[0x10];

                Array.Copy(GetShortBytes(X), 0, data, 0x0, 2);
                Array.Copy(GetShortBytes(Y), 0, data, 0x2, 2);
                Array.Copy(GetShortBytes(Z), 0, data, 0x4, 2);

                Array.Copy(GetShortBytes(S), 0, data, 0x8, 2);
                Array.Copy(GetShortBytes(T), 0, data, 0xa, 2);

                data[0xc] = R;
                data[0xd] = G;
                data[0xe] = B;
                data[0xf] = A;

                return data;
            }

            private byte[] GetShortBytes(short shrt)
            {
                byte[] shortHolder = BitConverter.GetBytes(shrt);
                if (BitConverter.IsLittleEndian)
                    shortHolder = shortHolder.Reverse().ToArray();

                return shortHolder;
            }
        }

        public static HeightMapVertex[,] CreateBlankMap()
        {
            HeightMapVertex[,] vertices = new HeightMapVertex[0x21, 0x41];

            for (int j = 0; j < 0x41; j++)
            {
                for (int i = 0; i < 0x21; i++)
                {
                    vertices[i, j] = new HeightMapVertex((short)(i * 0x80), 0, (short)(j * 0x80));
                }
            }

            return vertices;
        }

        public static void EncodeHeightMap(HeightMapVertex[,] heightMap,
            out byte[] heightData1, out byte[] heightData2)
        {
            if (heightMap.GetLength(0) != 0x21 || heightMap.GetLength(1) != 0x41)
            {
                throw new Exception();
            }

            List<byte> outputData = new List<byte>();

            //Decode heightmap 1
            for (int j = 0; j <= 0x20; j++)
            {
                for (int i = 0; i <= 0x10; i += 2)
                {
                    EncodeVertex(heightMap[i * 2, j * 2], outputData,
                        (short)(i * 0x100), (short)(j * 0x100));
                }

                for (int i = 1; i <= 0x10; i += 2)
                {
                    EncodeVertex(heightMap[i * 2, j * 2], outputData,
                        (short)(i * 0x100), (short)(j * 0x100));
                }
            }

            List<HeightMapVertex> UnusedVertices = new List<HeightMapVertex>(); //should be 33
            for (int i = 0; i <= 0x20; i++)
            {
                //Just push certain bytes to preserve the old pattern
                if (i == 0 || i == 17)
                {
                    outputData.Add(0x7C);
                    outputData.Add(0x00);
                    outputData.Add(0x00);
                }
                else
                {
                    outputData.Add(0x3F);
                    outputData.Add(0x00);
                    outputData.Add(0x00);
                    outputData.Add(0x00);
                }
            }

            outputData.Add(CommandByte.EXIT_COMMAND);
            while (outputData.Count % 16 != 0)
            {
                outputData.Add(0xFF);
            }

            heightData1 = outputData.ToArray();
            outputData.Clear();

            //Height data 2
            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        int x, y;
#pragma warning disable 0168 // variable declared but not used.

                        short defaultX, defaultZ;

                        GetHeightMap2Info(i, j, k, out x, out y);
                        //GetDefaultHD2Values(k, out defaultX, out defaultZ);
                        EncodeVertex(heightMap[x, y], outputData,
                            (short)(x * 0x80), (short)(y * 0x80));
                    }
                }
            }

            outputData.Add(CommandByte.EXIT_COMMAND);
            while (outputData.Count % 16 != 0)
            {
                outputData.Add(0xFF);
            }

            heightData2 = outputData.ToArray();
            outputData.Clear();

            return;
        }

        public static HeightMapVertex[,] DecodeHeightMap(byte[] HeightData1, byte[] HeightData2)
        {
            HeightMapVertex[,] heightMap = new HeightMapVertex[0x21, 0x41]; //0x21 blank spaces between the 2 vertex sets, unused?

            int offset = 0;

            //Decode heightmap 1
            for (int j = 0; j <= 0x20; j++)
            {
                for (int i = 0; i <= 0x10; i += 2)
                {
                    heightMap[i * 2, j * 2] = DecodeVertex(HeightData1, ref offset,
                        (short)(i * 0x100), (short)(j * 0x100));
                }

                for (int i = 1; i <= 0x10; i += 2)
                {
                    heightMap[i * 2, j * 2] = DecodeVertex(HeightData1, ref offset,
                        (short)(i * 0x100), (short)(j * 0x100));
                }
            }

            List<HeightMapVertex> UnusedVertices = new List<HeightMapVertex>(); //should be 33
            for (int i = 0; i <= 0x20; i++)
            {
                UnusedVertices.Add(DecodeVertex(HeightData1, ref offset, 0, 0));
            }

            //Decode heightmap 2
            offset = 0;

            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        int x, y;
                        short defaultS, defaultT;

                        GetHeightMap2Info(i, j, k, out x, out y);
                        GetDefaultHD2Values(k, out defaultS, out defaultT);
                        HeightMapVertex vertex = DecodeVertex(HeightData2, ref offset,
                            (short)(x * 0x80), (short)(y * 0x80));
                        vertex.S = defaultS;
                        vertex.T = defaultT;
                        heightMap[x, y] = vertex;
                    }
                }
            }

            return heightMap;
        }

        private static void GetDefaultHD2Values(int index, out short s, out short t)
        {
            switch (index)
            {
                case 0:
                    s = 0x1100;
                    t = 0x0100;
                    break;
                case 1:
                    s = 0x3100;
                    t = 0x0100;
                    break;
                case 2:
                    s = 0x0100;
                    t = 0x1100;
                    break;
                case 3:
                    s = 0x4100;
                    t = 0x1100;
                    break;
                case 4:
                    s = 0x0100;
                    t = 0x3100;
                    break;
                case 5:
                    s = 0x4100;
                    t = 0x3100;
                    break;
                case 6:
                    s = 0x1100;
                    t = 0x4100;
                    break;
                case 7:
                    s = 0x3100;
                    t = 0x4100;
                    break;
                case 8:
                    s = 0x1100;
                    t = 0x1100;
                    break;
                case 9:
                    s = 0x2100;
                    t = 0x1100;
                    break;
                case 10:
                    s = 0x3100;
                    t = 0x1100;
                    break;
                case 11:
                    s = 0x1100;
                    t = 0x2100;
                    break;
                case 12:
                    s = 0x3100;
                    t = 0x2100;
                    break;
                case 13:
                    s = 0x1100;
                    t = 0x3100;
                    break;
                case 14:
                    s = 0x2100;
                    t = 0x3100;
                    break;
                case 15:
                default:
                    s = 0x3100;
                    t = 0x3100;
                    break;
            }
        }

        private static void GetHeightMap2Info(int squareX, int squareY, int squareIndex,
            out int vertexX, out int vertexY)
        {
            switch (squareIndex)
            {
                case 0:
                    vertexX = 1;
                    vertexY = 0;
                    break;
                case 1:
                    vertexX = 3;
                    vertexY = 0;
                    break;
                case 2:
                    vertexX = 0;
                    vertexY = 1;
                    break;
                case 3:
                    vertexX = 4;
                    vertexY = 1;
                    break;
                case 4:
                    vertexX = 0;
                    vertexY = 3;
                    break;
                case 5:
                    vertexX = 4;
                    vertexY = 3;
                    break;
                case 6:
                    vertexX = 1;
                    vertexY = 4;
                    break;
                case 7:
                    vertexX = 3;
                    vertexY = 4;
                    break;
                case 8:
                    vertexX = 1;
                    vertexY = 1;
                    break;
                case 9:
                    vertexX = 2;
                    vertexY = 1;
                    break;
                case 10:
                    vertexX = 3;
                    vertexY = 1;
                    break;
                case 11:
                    vertexX = 1;
                    vertexY = 2;
                    break;
                case 12:
                    vertexX = 3;
                    vertexY = 2;
                    break;
                case 13:
                    vertexX = 1;
                    vertexY = 3;
                    break;
                case 14:
                    vertexX = 2;
                    vertexY = 3;
                    break;
                case 15:
                default:
                    vertexX = 3;
                    vertexY = 3;
                    break;
            }

            vertexX += squareX * 4;
            vertexY += squareY * 4;
        }

        private static void EncodeVertex(HeightMapVertex vertex, List<byte> output,
            short defaultX, short defaultZ)
        {
            CommandByte command = new CommandByte();
            command.UseDefaultX = (defaultX == vertex.X);
            command.UseDefaultZ = (defaultZ == vertex.Z);

            List<byte> bytes = new List<byte>();

            if (!command.UseDefaultX)
            {
                //Determine x flags
                if (((ushort)(vertex.X << 8) >> 8) == vertex.X)
                {
                    command.XFlags = CommandByte.ByteReadingType.Read1ByteNoShift;
                    bytes.Add((byte)(vertex.X & 0xff));
                }
                else if ((vertex.X & 0x3fc0) == vertex.X && ((ushort)(vertex.X << 2) >> 2) == vertex.X)
                {
                    command.XFlags = CommandByte.ByteReadingType.Read1ByteShift2;
                    bytes.Add((byte)((vertex.X >> 6) & 0xff));
                }
                else if ((vertex.X & 0x1fe0) == vertex.X && ((ushort)(vertex.X << 3) >> 3) == vertex.X)
                {
                    command.XFlags = CommandByte.ByteReadingType.Read1ByteShift1;
                    bytes.Add((byte)((vertex.X >> 5) & 0xff));
                }
                else
                {
                    command.XFlags = CommandByte.ByteReadingType.Read2Bytes;
                    bytes.Add((byte)((vertex.X >> 8) & 0xff));
                    bytes.Add((byte)(vertex.X & 0xff));
                }
            }

            //Determine y flags
            if (((ushort)(vertex.Y << 8) >> 8) == vertex.Y)
            {
                command.YFlags = CommandByte.ByteReadingType.Read1ByteNoShift;
                bytes.Add((byte)(vertex.Y & 0xff));
            }
            else if ((vertex.Y & 0x03fc) == vertex.Y && ((ushort)(vertex.Y << 6) >> 6) == vertex.Y)
            {
                command.YFlags = CommandByte.ByteReadingType.Read1ByteShift2;
                bytes.Add((byte)((vertex.Y >> 2) & 0xff));
            }
            else if ((vertex.Y & 0x01fe) == vertex.Y && ((ushort)(vertex.Y << 7) >> 7) == vertex.Y)
            {
                command.YFlags = CommandByte.ByteReadingType.Read1ByteShift1;
                bytes.Add((byte)((vertex.Y >> 1) & 0xff));
            }
            else
            {
                command.YFlags = CommandByte.ByteReadingType.Read2Bytes;
                bytes.Add((byte)((vertex.Y >> 8) & 0xff));
                bytes.Add((byte)(vertex.Y & 0xff));
            }

            if (!command.UseDefaultZ)
            {
                //Determine z flags
                if (((ushort)(vertex.Z << 8) >> 8) == vertex.Z)
                {
                    command.ZFlags = CommandByte.ByteReadingType.Read1ByteNoShift;
                    bytes.Add((byte)(vertex.Z & 0xff));
                }
                else if ((vertex.Z & 0x3fc0) == vertex.Z && ((ushort)(vertex.Z << 2) >> 2) == vertex.Z)
                {
                    command.ZFlags = CommandByte.ByteReadingType.Read1ByteShift2;
                    bytes.Add((byte)((vertex.Z >> 6) & 0xff));
                }
                else if ((vertex.Z & 0x1fe0) == vertex.Z && ((ushort)(vertex.Z << 3) >> 3) == vertex.Z)
                {
                    command.ZFlags = CommandByte.ByteReadingType.Read1ByteShift1;
                    bytes.Add((byte)((vertex.Z >> 5) & 0xff));
                }
                else
                {
                    command.ZFlags = CommandByte.ByteReadingType.Read2Bytes;
                    bytes.Add((byte)((vertex.Z >> 8) & 0xff));
                    bytes.Add((byte)(vertex.Z & 0xff));
                }
            }

            bytes.Insert(0, command.GetAsByte());

            output.AddRange(bytes);

            return;
        }

        private static HeightMapVertex DecodeVertex(byte[] data, ref int index, short defaultX, short defaultZ)
        {
            short x = defaultX;
            short y = 0;
            short z = defaultZ;

            CommandByte command = new CommandByte(data[index]);
            index++;

            if (command.IsQuitCommand)
                return null;

            //Obtain the X
            if (!command.UseDefaultX)
            {
                switch (command.XFlags)
                {
                    case CommandByte.ByteReadingType.Read2Bytes:
                        x = (short)((short)(data[index] << 8) + data[index + 1]);
                        index += 2;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteNoShift:
                        x = (short)data[index];
                        index++;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteShift1:
                        x = (short)(data[index] << 5);
                        index++;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteShift2:
                        x = (short)(data[index] << 6);
                        index++;
                        break;
                }
            }

            //Then obtain the Y
            switch (command.YFlags)
            {
                case CommandByte.ByteReadingType.Read2Bytes:
                    y = (short)((short)(data[index] << 8) + data[index + 1]);
                    index += 2;
                    break;
                case CommandByte.ByteReadingType.Read1ByteNoShift:
                    y = (short)data[index];
                    index++;
                    break;
                case CommandByte.ByteReadingType.Read1ByteShift1:
                    y = (short)(data[index] << 1);
                    index++;
                    break;
                case CommandByte.ByteReadingType.Read1ByteShift2:
                    y = (short)(data[index] << 2);
                    index++;
                    break;
            }

            //And finally Z
            if (!command.UseDefaultZ)
            {
                switch (command.ZFlags)
                {
                    case CommandByte.ByteReadingType.Read2Bytes:
                        z = (short)((short)(data[index] << 8) + data[index + 1]);
                        index += 2;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteNoShift:
                        z = (short)data[index];
                        index++;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteShift1:
                        z = (short)(data[index] << 5);
                        index++;
                        break;
                    case CommandByte.ByteReadingType.Read1ByteShift2:
                        z = (short)(data[index] << 6);
                        index++;
                        break;
                }
            }

            return new HeightMapVertex(x, y, z);
        }

    }
}
