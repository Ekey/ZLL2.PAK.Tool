using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ZLL2.Unpacker
{
    class PakUnpack
    {
        private static List<PakEntry> m_EntryTable = new List<PakEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TPakStream = File.OpenRead(m_Archive))
            {
                var m_Header = new PakHeader();

                m_Header.dwMagic = TPakStream.ReadUInt64();
                m_Header.dwHeaderSize = TPakStream.ReadInt32();
                m_Header.dwTotalFiles = TPakStream.ReadInt32();
                m_Header.dwEntryTableOffset = TPakStream.ReadUInt32();
                m_Header.dwNamesTableOffset = TPakStream.ReadUInt32();
                m_Header.dwEntryTableCompressedSize = TPakStream.ReadInt32();
                m_Header.dwNamesTableCompressedSize = TPakStream.ReadInt32();
                m_Header.dwNamesTableDecompressedSize = TPakStream.ReadInt32();
                m_Header.dwUnknown = TPakStream.ReadInt32();

                if (m_Header.dwMagic != 0x303037305950414B)
                {
                    throw new Exception("[ERROR]: Invalid magic of PAK archive file!");
                }

                TPakStream.Seek(m_Header.dwEntryTableOffset, SeekOrigin.Begin);

                var lpEntryTableTemp = TPakStream.ReadBytes(m_Header.dwEntryTableCompressedSize);
                var lpEntryTable = ZLIB.iDecompress(lpEntryTableTemp);

                //File.WriteAllBytes("ENTRY_C3DPATCH", lpEntryTable);

                TPakStream.Seek(m_Header.dwNamesTableOffset, SeekOrigin.Begin);

                var lpNamesTableTemp = TPakStream.ReadBytes(m_Header.dwNamesTableCompressedSize);
                var lpNamesTable = ZLIB.iDecompress(lpNamesTableTemp);

                m_EntryTable.Clear();
                using (MemoryStream TEntryReader = new MemoryStream(lpEntryTable))
                {
                    using (MemoryStream TNamesReader = new MemoryStream(lpNamesTable))
                    {
                        for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                        {
                            var m_Entry = new PakEntry();

                            m_Entry.dwOffset = TEntryReader.ReadUInt32();
                            m_Entry.dwHash = TEntryReader.ReadUInt32();
                            m_Entry.dwCompressedSize1 = TEntryReader.ReadInt32();
                            m_Entry.dwDecompressedSize = TEntryReader.ReadInt32();
                            m_Entry.dwFlags = TEntryReader.ReadInt32();
                            m_Entry.dwCompressedSize2 = TEntryReader.ReadInt32();
                            m_Entry.dwUnknown2 = TEntryReader.ReadInt32();
                            m_Entry.m_FileName = TNamesReader.ReadString(Encoding.GetEncoding("gb2312"));

                            m_EntryTable.Add(m_Entry);
                        }

                        TNamesReader.Dispose();
                    }

                    TEntryReader.Dispose();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TPakStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    if (m_Entry.m_FileName == "level7")
                    {
                        Console.WriteLine();
                    }

                    if (m_Entry.dwFlags == 0x400)
                    {
                        var lpBuffer = TPakStream.ReadBytes(m_Entry.dwCompressedSize1);
                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                    else
                    {
                        var lpSrcBuffer = TPakStream.ReadBytes(m_Entry.dwCompressedSize1);
                        var lpDstBuffer = LZ4C.iDecompress(lpSrcBuffer, m_Entry.dwDecompressedSize);

                        File.WriteAllBytes(m_FullPath, lpDstBuffer);
                    }
                }

                TPakStream.Dispose();
            }
        }
    }
}
