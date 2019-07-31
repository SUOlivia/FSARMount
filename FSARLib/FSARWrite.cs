using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using NoFrill.Common;

namespace FSARLib
{
    public static class FSARWrite
    {
        public static byte[] MakeHeader(FSARInfo Header)
        {
            byte[] OutHeader = new byte[0x20]
            {
                (byte) 'F', (byte) 'S', (byte) 'A', (byte) 'R', // Magic word
                0x00, 0x00, 0x00, 0x02,                         // Compressed flag?
                0x00, 0x00, 0x00, 0x00,                         // FileTable end
                0x00, 0x00, 0x00, 0x00,                         // Number of files 
                (byte) 'x', (byte) 'x', (byte) 'x', (byte) 'x', // 0x10 bytes long padding
                (byte) 'x', (byte) 'x', (byte) 'x', (byte) 'x', // 0x10 bytes long padding
                (byte) 'x', (byte) 'x', (byte) 'x', (byte) 'x', // 0x10 bytes long padding
                (byte) 'x', (byte) 'x', (byte) 'x', (byte) 'x', // 0x10 bytes long padding
            };
            OutHeader.WriteInt32BE(0x08, Header.FileTableEnd);
            OutHeader.WriteInt32BE(0x0C, Header.FileTableObjects);
            return OutHeader;
        }
        
        public static byte[] MakeFileHeaders(FSARFile[] Files, int size)
        {
            int Pos = 0;
            Byte[] FileHeaders = new Byte[size];
            FSARFileEntryInfo CurHeader;
            
            for(int f = 0; f < Files.Length; f++)
            {
                CurHeader = Files[f].FileHeader;
                Pos = f * 0x120;

                FSARHelper.fastCopyBlock(Encoding.UTF8.GetBytes(CurHeader.Path), 0, FileHeaders, Pos, Encoding.UTF8.GetByteCount(CurHeader.Path));

                Pos += 0x100;

                FileHeaders.WriteInt64BE(ref Pos, CurHeader.UncompressedSize);
                FileHeaders.WriteInt64BE(ref Pos, CurHeader.CompressedSize);
                FileHeaders.WriteInt64BE(ref Pos, CurHeader.DataPos);

                Pos += 0x03;
                
                FileHeaders[Pos] = CurHeader.Compressed ? (byte) 0x02 : (byte) 0x01;
            }

            return FileHeaders;
        }
        
        public static byte[] MakeFileHeaders(FSARFileEntryInfo[] FilesHeaders, int size)
        {
            int Pos = 0;
            Byte[] FileHeaders = new Byte[size];
            FSARFileEntryInfo CurHeader;
            
            for(int f = 0; f < FilesHeaders.Length; f++)
            {
                CurHeader = FilesHeaders[f];
                Pos = f * 0x120;

                FSARHelper.fastCopyBlock(Encoding.UTF8.GetBytes(CurHeader.Path), 0, FileHeaders, Pos, Encoding.UTF8.GetByteCount(CurHeader.Path));

                Pos += 0x100;

                FileHeaders.WriteInt64BE(ref Pos, CurHeader.UncompressedSize);
                FileHeaders.WriteInt64BE(ref Pos, CurHeader.CompressedSize);
                FileHeaders.WriteInt64BE(ref Pos, CurHeader.DataPos);

                Pos += 0x03;
                
                FileHeaders[Pos] = CurHeader.Compressed ? (byte) 0x02 : (byte) 0x01;
            }

            return FileHeaders;
        }

        public static byte[] MakeFilesData(FSARFile[] Files, Boolean UseChecks = false)
        {
            long FilesData_Lenght = Files[Files.Length-1].FileHeader.Compressed ?
                                    Files[Files.Length-1].FileHeader.DataPos + Files[Files.Length-1].FileHeader.CompressedSize : 
                                    Files[Files.Length-1].FileHeader.DataPos + Files[Files.Length-1].FileHeader.UncompressedSize;

            Byte[] FilesData = new Byte[FilesData_Lenght];
            Byte[] CurFileData, padding;
            FSARFile CurFile; 
            for(int f = 0; f < Files.Length; f++)
            {
                CurFile = Files[f];
                Console.WriteLine(CurFile.FileHeader.Path);
                if(CurFile.FileHeader.Compressed)
                {
                    if(f + 1 != Files.Length && Files[f + 1].FileHeader.DataPos - CurFile.FileHeader.DataPos != CurFile.FileHeader.CompressedSize)
                    {
                        CurFileData = new Byte[Files[f + 1].FileHeader.DataPos - CurFile.FileHeader.DataPos];
                        padding = new Byte[CurFileData.Length - CurFile.FileHeader.CompressedSize];
                        for(int p = 0; p > CurFileData.Length - CurFile.FileHeader.CompressedSize; p++)
                            padding[p] = (byte) '-';
                        
                        FSARHelper.fastCopyBlock(padding, 0, CurFileData, CurFile.CompressedData.Length, padding.Length);
                        FSARHelper.fastCopyBlock(new byte[2]{0x78, 0xDA}, 0, CurFileData, 0, 2);
                        FSARHelper.fastCopyBlock(CurFile.CompressedData, 0, CurFileData, 2, CurFile.CompressedData.Length);
                        FSARHelper.fastCopyBlock(CurFileData, 0, FilesData, (int) CurFile.FileHeader.DataPos, CurFileData.Length);
                    }
                    else
                    {
                        FSARHelper.fastCopyBlock(new byte[2]{0x78, 0xDA}, 0, FilesData, (int) CurFile.FileHeader.DataPos, 2);
                        FSARHelper.fastCopyBlock(CurFile.CompressedData, 0, FilesData, (int) CurFile.FileHeader.DataPos + 2, CurFile.CompressedData.Length);
                    }
                }
                else
                {
                    if(f + 1 != Files.Length && Files[f + 1].FileHeader.DataPos - CurFile.FileHeader.DataPos != CurFile.FileHeader.UncompressedSize)
                    {
                        CurFileData = new Byte[f != Files.Length ? Files[f].FileHeader.DataPos - CurFile.FileHeader.DataPos : CurFile.FileHeader.UncompressedSize];
                        padding = new Byte[CurFileData.Length - CurFile.FileHeader.UncompressedSize];
                        for(int p = 0; p > CurFileData.Length - CurFile.FileHeader.UncompressedSize; p++)
                        {
                            padding[p] = (byte) '-';
                        }
                        FSARHelper.fastCopyBlock(CurFile.UncompressedData, 0, CurFileData, 0, CurFileData.Length);
                        FSARHelper.fastCopyBlock(padding, 0, CurFileData, CurFile.UncompressedData.Length, padding.Length);
                        FSARHelper.fastCopyBlock(CurFileData, 0, FilesData, (int) CurFile.FileHeader.DataPos, CurFileData.Length);
                    }
                    else FSARHelper.fastCopyBlock(CurFile.UncompressedData, 0, FilesData, (int) CurFile.FileHeader.DataPos, CurFile.UncompressedData.Length);
                }
            }
            return FilesData;
        }

        public static byte[] MakeFSARArchive(this FSARArchive Archive)
        {
            Byte[] Header = MakeHeader(Archive.Header);
            Byte[] FileHeaders = MakeFileHeaders(Archive.Files, Archive.Header.FileTableEnd - 0x20);
            Byte[] Files = MakeFilesData(Archive.Files);
            
            Byte[] OutFile = new Byte[Header.Length + FileHeaders.Length + Files.Length];
            FSARHelper.fastCopyBlock(Header, 0, OutFile, 0, Header.Length);
            FSARHelper.fastCopyBlock(FileHeaders, 0, OutFile, Header.Length, FileHeaders.Length);
            FSARHelper.fastCopyBlock(Files, 0, OutFile, Header.Length + FileHeaders.Length, Files.Length);
            
            return OutFile;
        }
    }
}