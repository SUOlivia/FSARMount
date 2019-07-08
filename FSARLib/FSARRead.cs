using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using NoFrill.Common;

// Deflate testing
using Spreads.Native;
using ICSharpCode;

namespace FSARLib
{
    public static class FSARRead
    {
        public static FSARInfo ParseHeader(Byte[] Header)
        {
            FSARInfo CurHeader = new FSARInfo();
            String MagicWord = "FSAR";
            
            if(Header.Length == 0x20 || Header.Length == 0x10) // Verify the lenght of the given header (lenght it 0x20 if it includes the 0x10 bytes of padding)
            {
                if(Encoding.UTF8.GetString(Header, 0, 4) == MagicWord) // Verify if the magic word is right
                {
                    CurHeader.FileTableEnd = Header.ReadInt32BE(0x08);
                    CurHeader.FileTableObjects = Header.ReadInt32BE(0x0C);
                }
            }
            return CurHeader;
        }

        public static FSARFileEntryInfo[] GetFileEntries(Byte[] FileTable, int FileNum)
        {
            int ArrPos;
            FSARFileEntryInfo CurFileEntry;
            FSARFileEntryInfo[] FileEntries = new FSARFileEntryInfo[FileNum];

            for(int i=0; i < FileNum; i++)
            {
                CurFileEntry = new FSARFileEntryInfo();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static FSARFile GetFile(this FSARFileEntryInfo FileEntry, Byte[] FilesData)
        {
            FSARFile CurFile = new FSARFile();
            CurFile.FileHeader = FileEntry;
            CurFile.UncompressedData = new Byte[FileEntry.UncompressedSize];
            
            if(FileEntry.Compressed)
            {
                CurFile.CompressedData = new Byte[FileEntry.CompressedSize - 2];
                CurFile.UncompressedData = new Byte[FileEntry.UncompressedSize];
                FSARHelper.fastCopyBlock(FilesData, (int) FileEntry.DataPos + 2, CurFile.CompressedData, 0, CurFile.CompressedData.Length);
                byte* CompressedData = (byte*) Unsafe.AsPointer(ref CurFile.CompressedData[0]);
                byte* UncompressedData = (byte*) Unsafe.AsPointer(ref CurFile.UncompressedData[0]);
                Compression.decompress_deflate(CompressedData, (IntPtr) CurFile.CompressedData.Length, UncompressedData, (IntPtr) CurFile.UncompressedData.Length);
            }
            else
            {
                FSARHelper.fastCopyBlock(FilesData, (int) FileEntry.DataPos, CurFile.UncompressedData, 0, (int) FileEntry.UncompressedSize);
            }
            return CurFile;
        }

        public static FSARFile GetFile(Byte[] FilesData, FSARFileEntryInfo FileEntry)
        {
            FSARFile CurFile = new FSARFile();
            CurFile.FileHeader = FileEntry;
            CurFile.UncompressedData = new Byte[FileEntry.UncompressedSize];
            
            if(FileEntry.Compressed)
            {
                CurFile.CompressedData = new Byte[FileEntry.CompressedSize - 2];
                FSARHelper.fastCopyBlock(FilesData, (int) FileEntry.DataPos + 2, CurFile.CompressedData, 0, CurFile.CompressedData.Length);
                MemoryStream CompressedData = new MemoryStream(CurFile.CompressedData);
                MemoryStream DecompressedData = new MemoryStream();
                DeflateStream DecompFile = new DeflateStream(CompressedData, CompressionMode.Decompress);
                DecompFile.CopyTo(DecompressedData);
                CurFile.UncompressedData = DecompressedData.ToArray();

                DecompressedData.Close();
                DecompFile.Close();
                CompressedData.Close();
            }
            else
            {
                FSARHelper.fastCopyBlock(FilesData, (int) FileEntry.DataPos, CurFile.UncompressedData, 0, (int) FileEntry.UncompressedSize);
            }
            return CurFile;
        }
    }
}
