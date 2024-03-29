﻿using System;
using System.Diagnostics;
using System.IO;
using FSARLib;

namespace FSARTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch Time = new Stopwatch();
            
            Time.Start();
            Byte[] Header = new Byte[0x20];
            FSARFile[] Files;
            Byte[] FFArray = File.ReadAllBytes("test.far");

            // Read the header
            Buffer.BlockCopy(FFArray, 0, Header, 0, 0x20);
            FSARInfo HeaderInfo = FSARRead.ParseHeader(Header);

            // Parse the File entries
            Byte[] FARTable = new Byte[HeaderInfo.FileTableEnd - 0x20];
            Buffer.BlockCopy(FFArray, 0x20, FARTable, 0, FARTable.Length);
            FSARFileEntryInfo[] FARFiles = FSARRead.GetFileEntries(FARTable, HeaderInfo.FileTableObjects);

            // Read the data of all the files
            Files = new FSARFile[HeaderInfo.FileTableObjects];
            Byte[] FilesData = new Byte[FFArray.Length - HeaderInfo.FileTableEnd]; 
            Buffer.BlockCopy(FFArray, HeaderInfo.FileTableEnd, FilesData, 0, FilesData.Length);
            for(int i = 0; i < HeaderInfo.FileTableObjects; i++)
            {
                Console.WriteLine(FARFiles[i].Path);
                Files[i] = FARFiles[i].GetFile(FilesData);
            }
            FSARArchive FARArch = new FSARArchive();
            FARArch.Files = Files;
            FARArch.Header = HeaderInfo;
            Time.Stop();
            UInt64 TS = (UInt64) Time.ElapsedMilliseconds;
            Console.WriteLine("It took {0}ms to extract the files from Test.far", TS);
            foreach(FSARFile CurrentFile in FARArch.Files)
            {
                string OutStr = String.Format("test_far\\{0}", CurrentFile.FileHeader.Path);
                Directory.CreateDirectory(String.Format("test_far\\{0}", CurrentFile.FileHeader.Directory));
                File.WriteAllBytes(OutStr, CurrentFile.UncompressedData);
            }
        }
    }
}
