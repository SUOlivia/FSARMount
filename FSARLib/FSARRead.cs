using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using NoFrill.Common;

namespace FSARLib
{
    public class FSARRead
    {
        // Init vars
        FSARInfo CurHeader = new FSARInfo();
        FileEntryInfo CurFileEntry;
        String MagicWord = "FSAR";
        Byte[] FSARWord = new Byte[0x04];
        Byte[] FTEnd = new Byte[0x08];
        Byte[] FTO = new Byte[0x08];

        public FSARInfo ReadHeader(Byte[] Header)
        {
            if(Header.Length == 0x20 || Header.Length == 0x10) // Verify the lenght of the given header (lenght it 0x20 if it includes the 0x10 bytes of padding)
            {
                if(Encoding.UTF8.GetString(Header, 0, 4) == MagicWord) // Verify if the magic word is right
                {
                    CurHeader.FileTableEnd = (int) BinUtils.ReadUInt32BE(Header, 0x08);
                    CurHeader.FileTableObjects = (int) BinUtils.ReadUInt32BE(Header, 0x0C);
                }
            }
            return CurHeader;
        }

        public FileEntryInfo[] GetFileEntries(Byte[] FileTable, FSARInfo HeaderInfo)
        {
            int ArrPos;
            FileEntryInfo[] FileEntries = new FileEntryInfo[HeaderInfo.FileTableObjects];

            for(int i=0; i < HeaderInfo.FileTableObjects; i++)
            {
                CurFileEntry = new FileEntryInfo();
                ArrPos = i * 0x120; // Current file index * Lenght of the header for a single file
                CurFileEntry.Path = Encoding.UTF8.GetString(FileTable, ArrPos, 0x100).TrimEnd('\0');
                CurFileEntry.FileName = Path.GetFileName(CurFileEntry.Path);
                CurFileEntry.Directory = Path.GetDirectoryName(CurFileEntry.Path);
                ArrPos += 0x100;
                CurFileEntry.UncompressedSize = FileTable.ReadInt64BE(ref ArrPos);
                CurFileEntry.CompressedSize = FileTable.ReadInt64BE(ref ArrPos);
                CurFileEntry.DataPos = FileTable.ReadInt64BE(ref ArrPos);
                ArrPos += 0x03;
                CurFileEntry.Compressed = FileTable[ArrPos] - 1 != 0;
                FileEntries[i] = CurFileEntry;
            }
            return FileEntries;
        }
        public FSARFile GetFile(Byte[] FilesData, FileEntryInfo FileEntry)
        {
            FSARFile CurFile = new FSARFile();
            CurFile.FileHeader = FileEntry;
            CurFile.UncompressedData = new Byte[FileEntry.UncompressedSize];
            
            if(FileEntry.Compressed)
            {
                CurFile.CompressedData = new Byte[FileEntry.CompressedSize - 2];
                Array.Copy(FilesData, (int) FileEntry.DataPos + 2, CurFile.CompressedData, 0, CurFile.CompressedData.Length);
                MemoryStream WeirdShit = new MemoryStream(CurFile.CompressedData);
                MemoryStream WS2 = new MemoryStream();
                DeflateStream DecompFile = new DeflateStream(WeirdShit, CompressionMode.Decompress);
                DecompFile.CopyTo(WS2);
                CurFile.UncompressedData = WS2.ToArray();
            }
            else
            {
                Array.Copy(FilesData, (int) FileEntry.DataPos, CurFile.UncompressedData, 0, (int) FileEntry.UncompressedSize);
            }
            return CurFile;
        }
    }
}
