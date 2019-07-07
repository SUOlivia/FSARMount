using System;

namespace FSARLib
{
    
    public class FSARInfo
    {
        public Int32 FileTableEnd;
        public Int32 FileTableObjects;
    }

    public class FSARFileEntryInfo
    {
        public string Path;
        public string FileName;
        public string Directory;
        public Int64 UncompressedSize;
        public Int64 CompressedSize;
        public Int64 DataPos;
        public Boolean Compressed;
    }

    public class FSARFile
    {
        public FSARFileEntryInfo FileHeader;
        public Byte[] CompressedData; // Compressed data without 2 bytes of Zlib header
        public Byte[] UncompressedData;

    }

    public class FSARArchive
    {
        public FSARInfo Header;
        public FSARFile[] Files;
    }
}