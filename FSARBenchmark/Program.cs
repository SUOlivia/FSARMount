using System;
using System.IO;
using FSARLib;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FSARMount
{
    [InProcessAttribute]
    public class Bench
    {
        // FSARMount file.far Z
        // string FarPath = "MetaData.far"/* args[0] */;
        // char DriveLetter = 'Z' /* args[1] */;

        FSARArchive FARch = new FSARArchive();
        FSARFileEntryInfo[] FileHeaders;
        Byte[] FSARData = File.ReadAllBytes("MetaData.far");
        Byte[] Header = new Byte[0x20];
        Byte[] FileTable;
        Byte[] FileData;


        public Bench()
        {
            // Read header
            FSARHelper.fastCopyBlock(FSARData, 0, Header, 0, Header.Length);
            FARch.Header = FSARRead.ParseHeader(Header);

            // Parse the file headers
            FileTable = new Byte[FARch.Header.FileTableEnd];
            FSARHelper.fastCopyBlock(FSARData, 0x20, FileTable, 0, FileTable.Length);
            FileHeaders = FSARRead.GetFileEntries(FileTable, FARch.Header.FileTableObjects);

            // Copy the files' data to the right byte array for parsing and get the files array ready
            FARch.Files = new FSARFile[FARch.Header.FileTableObjects];
            FileData = new Byte[FSARData.Length - FARch.Header.FileTableEnd];
            FSARHelper.fastCopyBlock(FSARData, FARch.Header.FileTableEnd, FileData, 0, FileData.Length);
        }
            
         // Read the files
        [Benchmark]
        public void ReadFilesNoDecomp() => readFilesNoDecomp(FARch, FileHeaders, FileData);
            
        [Benchmark]
        public void ReadFilesDeflateStream() => readFilesDeflateStream(FARch, FileHeaders, FileData);
            
        [Benchmark]
        public void ReadFilesSpreads() => readFilesSpreads(FARch, FileHeaders, FileData);
        
        public void readFilesNoDecomp(FSARArchive FARch, FSARFileEntryInfo[] FileHeaders, byte[] FileData)
        {
            for(int i = 0; i < FARch.Header.FileTableObjects; i++)
            {
                FARch.Files[i] = FileHeaders[i].GetFileNoDecomp(FileData);
            }
        }
        public void readFilesDeflateStream(FSARArchive FARch, FSARFileEntryInfo[] FileHeaders, byte[] FileData)
        {
            for(int i = 0; i < FARch.Header.FileTableObjects; i++)
            {
                FARch.Files[i] = FileHeaders[i].GetFileDeflateStream(FileData);
            }
        }
        public void readFilesSpreads(FSARArchive FARch, FSARFileEntryInfo[] FileHeaders, byte[] FileData)
        {
            for(int i = 0; i < FARch.Header.FileTableObjects; i++)
            {
                FARch.Files[i] = FileHeaders[i].GetFileSpreads(FileData);
            }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
        }
        }
    }
}
