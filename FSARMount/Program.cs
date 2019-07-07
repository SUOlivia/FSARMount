using System;
using System.IO;
using System.Diagnostics;
using FSARLib;
// using DokanNet;

namespace FSARMount
{
    class Program
    {
        static void Main(string[] args)
        {
            var Timer = new Stopwatch();
            Timer.Start();

            // FSARMount file.far Z
            string FarPath = "MetaData.far"/* args[0] */;
            // char DriveLetter = 'Z' /* args[1] */;

            FSARArchive FARch = new FSARArchive();
            FSARFileEntryInfo[] FileHeaders;
            Byte[] FSARData = File.ReadAllBytes(FarPath);
            Byte[] Header = new Byte[0x20];
            Byte[] FileTable;
            Byte[] FileData;
 
            // Read header
            FSARRead.fastCopyBlock(FSARData, 0, Header, 0, Header.Length);
            FARch.Header = FSARRead.ParseHeader(Header);

            // Parse the file headers
            FileTable = new Byte[FARch.Header.FileTableEnd];
            FSARRead.fastCopyBlock(FSARData, 0x20, FileTable, 0, FileTable.Length);
            FileHeaders = FSARRead.GetFileEntries(FileTable, FARch.Header.FileTableObjects);

            // Copy the files' data to the right byte array for parsing and get the files array ready
            FARch.Files = new FSARFile[FARch.Header.FileTableObjects];
            FileData = new Byte[FSARData.Length - FARch.Header.FileTableEnd];
            FSARRead.fastCopyBlock(FSARData, FARch.Header.FileTableEnd, FileData, 0, FileData.Length);
            
            // Read the files
            for(int i = 0; i < FARch.Header.FileTableObjects; i++)
            {
                FARch.Files[i] = FileHeaders[i].GetFile(FileData);
            }
            Timer.Stop();
            var FinalTime = Timer.ElapsedTicks;
            Console.WriteLine(string.Format("Parsed files: {0}", FARch.Header.FileTableObjects));
            Console.WriteLine(string.Format("Completed FSARArchive class in {0}ms", FinalTime / 10000));
        }
    }
}
