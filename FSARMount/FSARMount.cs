using System;
using System.IO;
using FSARLib;
using DokanNet;

namespace FSARMount
{
    class FSARMount
    {
        static void Main(string[] args)
        {
            // FSARMount file.far Z
            string FarPath = args[0];
            string DriveLetter = args[1];

            FSARArchive FARch = new FSARArchive();
            FSARFileEntryInfo[] FileHeaders;
            Byte[] FSARData = File.ReadAllBytes(FarPath);
            Byte[] Header = new Byte[0x20];
            Byte[] FileTable;
            Byte[] FileData;
 
            // Read header
            Buffer.BlockCopy(FSARData, 0, Header, 0, Header.Length);
            FARch.Header = FSARRead.ParseHeader(Header);

            // Parse the file headers
            FileTable = new Byte[FARch.Header.FileTableEnd];
            Buffer.BlockCopy(FSARData, 0x20, FileTable, 0, FileTable.Length);
            FileHeaders = FSARRead.GetFileEntries(FileTable, FARch.Header.FileTableObjects);

            // Copy the files' data to the right byte array for parsing and get the files array ready
            FARch.Files = new FSARFile[FARch.Header.FileTableObjects];
            FileData = new Byte[FSARData.Length - FARch.Header.FileTableEnd];
            Buffer.BlockCopy(FSARData, FARch.Header.FileTableEnd, FileHeaders, 0, FileHeaders.Length);
            
            // Read the files
            for(int i = 0; i < FARch.Header.FileTableObjects; i++)
            {
                FARch.Files[i] = FileHeaders[i].GetFile(FileData);
            }
        }
    }
}
