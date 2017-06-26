using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Collections.ObjectModel;
 using Highland64CourseImporter.Data.CourseBlocks;
 
 namespace Highland64CourseImporter.Data
 {
     public class CourseTable
     {
         public ReadOnlyCollection<CourseTableEntry> Entries { get { return _entries.AsReadOnly(); } }
         private List<CourseTableEntry> _entries;
 
         public CourseTable(byte[] tableData, MG64RomFile rom)
         {
             //There MUST be the correct amount of data for the course table.
             int courseDataCount = MG64RomFile.LEVEL_COUNT * 7 * 8;
             if (tableData.Length != courseDataCount)
                 return;
 
             _entries = new List<CourseTableEntry>();
             int index = 0;
             List<CourseBlock> blocks = new List<CourseBlock>();
 
             //Each course has 7 different 8-byte data pointers, for a total of 0x38 bytes per level
             for (int i = 0; i<tableData.Length; i += 7 * 8)
             {
                 blocks.Clear();
                 byte[] tempData = new byte[4];
                 for (int j = 0; j< 7; j++)
                 {
                    Array.Copy(tableData, i + j* 0x8, tempData, 0, 4);
                     uint length = BitConverter.ToUInt32(tempData.Reverse().ToArray(), 0);
                     Array.Copy(tableData, i + j* 0x8 + 4, tempData, 0, 4);
                     uint offset = BitConverter.ToUInt32(tempData.Reverse().ToArray(), 0);
 
                     byte[] blockData = new byte[length];
                     Array.Copy(rom.RomData, MG64RomFile.COURSE_TABLE_OFFSET + offset, blockData, 0, length);
                     if (j == 2) //Objects
                         blocks.Add(new ObjectCourseBlock(blockData, (int) offset, (int) length));
                     else if (j == 6) //Surface
                         blocks.Add(new SurfaceCourseBlock(blockData, (int) offset, (int) length));
                     else
                         blocks.Add(new BasicCourseBlock(blockData, (int) offset, (int) length));
                 }
 
                 _entries.Add(new CourseTableEntry(CourseNames.NameList[index], index, blocks[0], blocks[1], (ObjectCourseBlock) blocks[2], blocks[3], blocks[4], blocks[5], (SurfaceCourseBlock) blocks[6]));
                 index++;
             }
         }
 
         /// <summary>
         /// Serializes the table data back into the rom. Used when saving.
         /// </summary>
         public void SerializeData(MG64RomFile file)
         {
             foreach (CourseTableEntry entry in _entries)
             {
                 //First encode the height map data
                 entry.PrepForSaving(file);
 
                 //Then serialize all the block data
                 SerializeData(file, entry.HeightData1);
                 SerializeData(file, entry.HeightData2);
                 SerializeData(file, entry.ObjectList);
                 SerializeData(file, entry.Pointer4);
                 SerializeData(file, entry.Pointer5);
                 SerializeData(file, entry.Pointer6);
                 SerializeData(file, entry.SurfaceMap);
 
                 //Then serialize the table entries
                 SerializeTableEntry(file, entry);
             }
         }
 
         private void SerializeData(MG64RomFile file, CourseBlock block)
         {
             byte[] blockData = block.GetAsBytes();
 
             if (blockData.Length != block.BlockLength) //size has changed
             {
                 if (blockData.Length<block.BlockLength) //shrunk in size
                 {
                     block.Update(block.RomOffset, blockData.Length);
                 }
                 else //grown in size, needs to be appended to the rom
                 {
                     block.Update(file.NewDataOffset, blockData.Length);
                     file.AdvanceNewData(block.BlockLength);
                 }
            }
 
             Array.Copy(blockData, 0, file.RomData, block.RomOffset + MG64RomFile.COURSE_TABLE_OFFSET, block.BlockLength);
         }
 
         private void SerializeTableEntry(MG64RomFile file, CourseTableEntry entry)
         {
             byte[] entryData = entry.HeightData1.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8, entryData.Length);
             entryData = entry.HeightData2.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x8, entryData.Length);
             entryData = entry.ObjectList.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x10, entryData.Length);
             entryData = entry.Pointer4.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x18, entryData.Length);
             entryData = entry.Pointer5.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x20, entryData.Length);
             entryData = entry.Pointer6.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x28, entryData.Length);
             entryData = entry.SurfaceMap.GetPointerAsBytes();
             Array.Copy(entryData, 0, file.RomData, MG64RomFile.COURSE_TABLE_OFFSET + entry.Index* 7 * 8 + 0x30, entryData.Length);
         }
 
         private void EncodeHeightMapData(MG64RomFile file, BasicCourseBlock hd1, BasicCourseBlock hd2)
         {
 
         }
     }
 
     /// <summary>
     /// A single entry in the CourseTable. Should have a course name, index, and 7 pointers to different course data
     /// </summary>
     public class CourseTableEntry
     {
         public string CourseName;
         public int Index;
         
         public CourseBlock HeightData1;
         public CourseBlock HeightData2;
         public ObjectCourseBlock ObjectList;
         public CourseBlock Pointer4;
         public CourseBlock Pointer5;
         public CourseBlock Pointer6;
         public SurfaceCourseBlock SurfaceMap;
 
         public HeightDataEncoding.HeightMapVertex[,] HeightMapData;
 
         public CourseTableEntry(string courseName, int index, CourseBlock p1, CourseBlock p2, ObjectCourseBlock p3, CourseBlock p4, CourseBlock p5, CourseBlock p6, SurfaceCourseBlock p7)
         {
             CourseName = courseName;
             Index = index;
             HeightData1 = p1;
             HeightData2 = p2;
             ObjectList = p3;
             Pointer4 = p4;
             Pointer5 = p5;
             Pointer6 = p6;
             SurfaceMap = p7;
 
             HeightMapData = HeightDataEncoding.DecodeHeightMap(p1.GetAsBytes(), p2.GetAsBytes());
         }
 
         public void PrepForSaving(MG64RomFile file)
         {
             //Convert height map data back to encoded formats
             byte[] heightMap1, heightMap2;
 
             HeightDataEncoding.EncodeHeightMap(HeightMapData, out heightMap1, out heightMap2);
 
             if (heightMap1.Length <= HeightData1.BlockLength)
                 HeightData1 = new BasicCourseBlock(heightMap1, HeightData1.RomOffset, heightMap1.Length);
             else
             {
                 HeightData1 = new BasicCourseBlock(heightMap1, file.NewDataOffset, heightMap1.Length);
                 file.AdvanceNewData(heightMap1.Length);
             }
 
             if (heightMap2.Length <= HeightData2.BlockLength)
                 HeightData2 = new BasicCourseBlock(heightMap2, HeightData2.RomOffset, heightMap2.Length);
             else
             {
                 HeightData2 = new BasicCourseBlock(heightMap2, file.NewDataOffset, heightMap2.Length);
                 file.AdvanceNewData(heightMap2.Length);
             }
         }
 
         public override string ToString()
         {
             return string.Format("{0} - {1}", Index, CourseName);
         }
     }
     
 }