 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.IO;

 namespace Highland64CourseImporter.Data
 {
     /// <summary>
     /// MG64RomFile will hold all the data and information about the MG64 rom in general.
    /// </summary>
     public class MG64RomFile
     {
         public byte[] RomData { get; private set; }
 
         public const int LEVEL_COUNT = 145;
         public const int COURSE_TABLE_OFFSET = 0xE473F0;
 
          /// <summary>
         /// Offset in the rom where new data could be inserted without overwriting other data
         /// </summary>
         public int NewDataOffset { get; private set; }
 
         public CourseTable CourseTable { get; private set; }
 
        private MG64RomFile(byte[] romData)
         {
             RomData = romData;
 
             DeserializeRom();
         }
 
         public static MG64RomFile LoadRom(string filePath)
         {
             byte[] romData = File.ReadAllBytes(filePath);
             if (romData == null || romData.Length == 0)
                 return null;
 
             return new MG64RomFile(romData);
         }
 
         public bool SaveRom(string filePath)
         {
             SerializeRom();
             File.WriteAllBytes(filePath, RomData);
             return true;
         }
 
         /// <summary>
         /// Deserializes the rom. For now it only finds the new data offset and
         ///  pulls out the course table and the data that it points to.
         /// </summary>
        private void DeserializeRom()
         {
             //Course table
             int courseDataCount = MG64RomFile.LEVEL_COUNT * 7 * 8;
 
             byte[] courseTableData = new byte[courseDataCount];
             Array.Copy(RomData, COURSE_TABLE_OFFSET, courseTableData, 0, courseDataCount);
 
             CourseTable = new CourseTable(courseTableData, this);
 
             //New data offset - NOT GUARANTEED TO BE 100% ACCURATE
             for (int i = RomData.Length - 1; i >= 0; i--)
             {
                 if (RomData[i] != 0xFF)
                 {
                     NewDataOffset = i + 1;
                     break;
                 }
             }
         }
 
         /// <summary>
         /// Serializes the data back into the rom. Currently only saves changes to course data/pointers
         /// </summary>
         private void SerializeRom()
         {
             //Course levels
             CourseTable.SerializeData(this);
         }
 
         /// <summary>
         /// Advances the NewDataOffset forward a set length. Used when adding new data to the new data offset.
         /// </summary>
         public void AdvanceNewData(int length)
         {
             NewDataOffset += length;
         }
     }
 }