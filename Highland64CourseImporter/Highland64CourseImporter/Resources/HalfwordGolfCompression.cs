using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Highland64CourseImporter
{
    public static class MarioGolf64Compression
    {
        /// <summary>
        /// Decompress the data given the raw data and the pointer to the start of the data. Length is optional and unused
        /// </summary>
        /// <param name="data"></param>
        /// <param name="inputPointer"></param>
        /// <param name="inputLength"></param>
        /// <returns></returns>
        public static byte[] DecompressGolfHalfword(byte[] data, int inputPointer, int inputLength)
        {
            //You skip forward 4 bytes at the start, dunno what it is
            inputPointer += 4;
            int inputEnd = inputPointer + inputLength;

            int dataPointer = inputPointer;

            List<byte> output = new List<byte>();

            bool quitFlag = false; //Don't quit until you hit the copy-forward command that's 0x0000

            while (!quitFlag)
            {
                //Grab the command halfword
                short dataFlag = (short)((data[dataPointer] << 8) + data[dataPointer + 1]);
                dataPointer += 2;

                if (dataFlag == 0) //If it's all uncompressed
                {
                    //just copy 20 bytes
                    for (int i = 0; i < 0x20; i++)
                    {
                        output.Add(data[dataPointer]);
                        dataPointer++;
                    }
                }
                else
                {
                    uint flagCheck = 0x8000; //Check one bit at a time, going left to right

                    while (flagCheck != 0)//Quit out when all flag bits have been checked
                    {
                        bool flag = (flagCheck & dataFlag) != 0;
                        //Grab next 2-byte clump: 0xffe * 2 is dist back to read, 0x1f * 2 + 4 is the length

                        if (flag) //1 = copy-forward, 0 = uncompressed
                        {
                            //Read from previously made data
                            ushort copyForwardInfo = (ushort)((data[dataPointer] << 8) + data[dataPointer + 1]);
                            dataPointer += 2;

                            if (copyForwardInfo == 0) //full 0 means it's the end of the file
                            {
                                //QUIT!
                                quitFlag = true;
                                break;
                            }

                            int prevDataPointer = output.Count - ((copyForwardInfo >> 5) << 1); //Point to copy data from
                            int prevReadEndPointer = prevDataPointer + ((copyForwardInfo & 0x1f) << 1) + 4; //Bytes to copy

                            //Copy forward the bytes
                            while (prevDataPointer < prevReadEndPointer)
                            {
                                output.Add(output[prevDataPointer]);
                                prevDataPointer++;
                            }
                        }
                        else
                        {
                            //Copy next 2 bytes uncompressed!
                            output.Add(data[dataPointer]);
                            dataPointer++;
                            output.Add(data[dataPointer]);
                            dataPointer++;
                        }

                        flagCheck >>= 1; //Advance to next bit
                    }
                }

            }

            return output.ToArray();
        }

        /// <summary>
        /// Compresses the data in the Mario Golf format. Ignores first 4 bytes, may be incorrect, 
        ///  but when I change those first 4 bytes I see no changes.
        /// </summary>
        public static byte[] CompressGolfHalfword(byte[] data)
        {
            int minByteClump = 4; //Minimum of 4 same bytes for a copy-forward command
            int maxByteClump = 66; //Maximum of 66 same bytes for a copy-forward command
            int maxReadBack = 4094; //Max read back of 4094 bytes

            //We optimize this compression by finding clumps of data behind the current data pointer where we can do the copy-forward
            // command to save space. The more consecutive halfwords we can copy, the more space we save. We do this check in FindBestClump()

            ClumpInfo currentBestClump;

            //All clumps that will be used in the compression
            List<ClumpInfo> clumpInfo = new List<ClumpInfo>();

            //Start after the first halfword, can't copy-forward from nothing. (technically you can, but shh)
            for (int offset = 2; offset < data.Length;)
            {
                //Find the best clump
                currentBestClump = FindBestClumpOrig(data, offset, maxReadBack, maxByteClump, minByteClump);

                //If the clump is large enough, go ahead and save it. Special case: if the length is 4 and the offset 0, 
                // that's a signal to end the decompression, so we can't allow that use case.
                if (currentBestClump.Length >= minByteClump && (currentBestClump.Length != 4 && currentBestClump.CopyOffset != 0))
                {
                    clumpInfo.Add(currentBestClump);
                    offset += currentBestClump.Length;
                }
                else
                {
                    //Uncompressed, move on one byte
                    offset += 2;
                }
            }

            ////Optimization code
            //for (int i = 0; i < clumpInfo.Count; i++)
            //{
            //    if (FindBestAlternativeClumps(data, maxReadBack, maxByteClump, minByteClump,
            //        clumpInfo, i))
            //    {
            //        //to try to further optimize?
            //    }

            //}

            int commandByteOffset; //Point at the command half word
            bool quitFlag = false; //True if the process is complete
            int commandBit; //The current bit used by the command byte
            int currentClump = 0; //current clump index in the list

            int inputPointer = 0; //points at the current data point in the input

            List<byte> output = new List<byte>();

            //First 4 unknown bytes (length perhaps???)
            output.Add(0);
            output.Add(0);
            output.Add(0);
            output.Add(0);

            while (!quitFlag)
            {
                commandByteOffset = output.Count;
                output.Add(0);//command byte 1
                output.Add(0);//command byte 2

                //Step through each command bit. If it's copy-forward, set it to 1. If uncompressed, set to 0.
                for (commandBit = 0; commandBit < 0x10; commandBit++)
                {
                    int commandPointer = (commandBit > 7 ? commandByteOffset + 1 : commandByteOffset);
                    byte commandFlag = (byte)(0x80 >> (commandBit % 8));

                    //If copy-forward
                    if (clumpInfo.Count > currentClump && clumpInfo[currentClump].Offset == inputPointer)
                    {
                        //Set the command bit
                        output[commandPointer] |= commandFlag;

                        //Add the clump halfword
                        ushort clumpHalfword = clumpInfo[currentClump].ClumpHalfword;
                        output.Add((byte)(clumpHalfword >> 8));
                        output.Add((byte)(clumpHalfword));

                        //Move up the input pointer!
                        inputPointer += clumpInfo[currentClump].Length;
                        currentClump++;
                    }
                    else if (inputPointer < data.Length - 1) //if uncompressed
                    {
                        //straight up copy!
                        output.Add(data[inputPointer]);
                        output.Add(data[inputPointer + 1]);
                        inputPointer += 2;
                    }
                    else //if it's the end of the data
                    {
                        //Time to end!
                        output[commandPointer] |= commandFlag;
                        output.Add(0);
                        output.Add(0);
                        quitFlag = true;
                    }
                }


            }

            return output.ToArray();
        }

        private static bool FindBestAlternativeClumps(byte[] rawData, int maxReadBack, int maxByteClump, int minByteClump,
            List<ClumpInfo> origClumps, int clumpOffset)
        {
            //The idea is to shave off a byte at the beginning of the clump. If it improves the # of bytes used, then goodie goodie!

            //To do: look into shaving the end as well!
            int oldLayoutBitCount = 1;
            int oldByteCount = 2;
            bool isInOldClump = true;
            int oldClumpOffset = clumpOffset;

            int newLayoutBitCount = 1;
            int newByteCount = 1;
            bool isInNewClump = false;
            int newClumpOffset = 0;

            List<ClumpInfo> newClumps = new List<ClumpInfo>();

            int byteOffset = origClumps[clumpOffset].Offset + 1;

            while (byteOffset < rawData.Length)
            {
                if ((!isInNewClump || newClumps[newClumpOffset].FollowingByte == byteOffset) &&
                    (!isInOldClump || origClumps[oldClumpOffset].FollowingByte == byteOffset))
                {
                    break; //We are synced again, make sure to increment the oldClumpOffset so you don't overwrite a clump
                }

                //If we're not in a clump, try to generate a clump. If successful, add it in and jump in.
                if (isInNewClump)
                {
                    //Check if we need to exit
                    if (newClumps[newClumpOffset].FollowingByte == byteOffset)
                    {
                        newClumpOffset++;
                        //Try to generate new clump
                        ClumpInfo newClump = FindBestClump(rawData, byteOffset, maxReadBack, maxByteClump, minByteClump);
                        if (newClump.Offset != -1)
                        {
                            //jump into new clump
                            newClumps.Add(newClump);
                            newLayoutBitCount++;
                            newByteCount += 2;
                        }
                        else
                        {
                            isInNewClump = false;
                            newLayoutBitCount++;
                            newByteCount++;
                        }
                    }
                }
                else
                {
                    //Try to generate.
                    ClumpInfo newClump = FindBestClump(rawData, byteOffset, maxReadBack, maxByteClump, minByteClump);
                    if (newClump.Offset != -1)
                    {
                        //jump into new clump
                        newClumps.Add(newClump);
                        isInNewClump = true;
                        newLayoutBitCount++;
                        newByteCount += 2;
                    }
                    else
                    {
                        newLayoutBitCount++;
                        newByteCount++;
                    }
                }

                //Handle the old info
                if (isInOldClump)
                {
                    //Check if we need to exit
                    if (origClumps[oldClumpOffset].FollowingByte == byteOffset)
                    {
                        oldClumpOffset++;
                        if (oldClumpOffset < origClumps.Count && origClumps[oldClumpOffset].Offset == byteOffset)
                        {
                            //jump into new clump
                            oldLayoutBitCount++;
                            oldByteCount += 2;
                        }
                        else
                        {
                            isInOldClump = false;
                            oldLayoutBitCount++;
                            oldByteCount++;
                        }
                    }
                }
                else
                {
                    //Check if we enter
                    if (oldClumpOffset < origClumps.Count && origClumps[oldClumpOffset].Offset == byteOffset)
                    {
                        isInOldClump = true;
                        oldLayoutBitCount++;
                        oldByteCount += 2;
                    }
                    else
                    {
                        oldLayoutBitCount++;
                        oldByteCount++;
                    }
                }

                //if (isInNewClump && isInOldClump && origClumps[oldClumpOffset].LastByte == newClumps[newClumpOffset].LastByte)
                //    break; //We are synced again

                //if (!isInOldClump && !isInNewClump)
                //    break; //We are synced again

                byteOffset++;
            }

            if (newByteCount > oldByteCount || (newByteCount == oldByteCount && newLayoutBitCount >= oldLayoutBitCount))
                return false;

            //make sure to go back an index if we're not in a clump
            if (!isInOldClump)
                oldClumpOffset--;

            //Apply
            int j;
            for (j = oldClumpOffset; j > clumpOffset; j--)
            {
                origClumps.RemoveAt(j);
            }
            origClumps.RemoveAt(j);

            while (newClumps.Count > 0)
            {
                origClumps.Insert(j, newClumps.Last());
                newClumps.RemoveAt(newClumps.Count - 1);
            }

            return true;

            //Idea: have an algorithm that'll take a given clump, bump one byte off the start and step through bytes, incrementing
            // the old and new clumps until they match up again. Remember to try to create clumps before ending the algorithm. If
            // you can't drop it, consider recursively running the alogritm over it again, allowing for one more extra raw byte to be
            // injected somewhere. If that doesn't work, failure!
        }

        /// <summary>
        /// I'm too lazy to comment this one. It basically finds the biggest previous clump of bytes that can replicate the current bytes
        /// </summary>
        private static ClumpInfo FindBestClump(byte[] rawData, int offset, int maxReadBack, int maxByteClump, int minByteClump)
        {
            ClumpInfo currentBestClump = new ClumpInfo(-1, -1, -1);

            int startRef = Math.Max(offset - 2, 0);

            for (int refOffset = startRef; refOffset >= 0 && refOffset < rawData.Length; refOffset -= 2)
            {
                int matchCount = 0;
                while (matchCount < maxByteClump && offset + matchCount < rawData.Length && rawData[refOffset + matchCount] == rawData[offset + matchCount])
                    matchCount++;

                if (matchCount % 2 == 1)
                    matchCount--;

                if (matchCount >= minByteClump && matchCount > currentBestClump.Length)
                {
                    currentBestClump = new ClumpInfo(offset, offset - refOffset, matchCount);
                    if (matchCount == maxByteClump)
                        break;
                }
            }

            return currentBestClump;
        }

        /// <summary>
        /// I'm too lazy to comment this one. It basically finds the biggest previous clump of bytes that can replicate the current bytes
        /// </summary>
        private static ClumpInfo FindBestClumpOrig(byte[] rawData, int offset, int maxReadBack, int maxByteClump, int minByteClump)
        {
            ClumpInfo currentBestClump = new ClumpInfo(-1, -1, -1);

            int startRef = Math.Max(offset - maxReadBack, 0);

            for (int refOffset = startRef; refOffset < offset && refOffset < rawData.Length; refOffset += 2)
            {
                int matchCount = 0;
                while (matchCount < maxByteClump && offset + matchCount < rawData.Length && rawData[refOffset + matchCount] == rawData[offset + matchCount])
                    matchCount++;

                if (matchCount % 2 == 1)
                    matchCount--;

                if (matchCount >= minByteClump && matchCount > currentBestClump.Length)
                {
                    currentBestClump = new ClumpInfo(offset, offset - refOffset, matchCount);
                    if (matchCount == maxByteClump)
                        break;
                }
            }

            return currentBestClump;
        }

        internal class ClumpInfo
        {
            public int Offset; //Offset in the output data
            public int CopyOffset; //Offset back from the current data pointer
            public int Length; //Length of the data to copy

            public ClumpInfo(int offset, int copyOffset, int length)
            {
                Offset = offset;
                CopyOffset = copyOffset;
                Length = length;
            }

            public int FollowingByte
            { get { return Offset + Length; } }

            public int LastByte
            { get { return FollowingByte - 1; } }

            public ushort ClumpHalfword //Halfword used to store the copy-forward information in the compressed data
            { get { return (ushort)((((CopyOffset >> 1) << 5) & 0xFFE0) | (((Length - 4) >> 1)) & 0x1F); } }
        }

    }
}