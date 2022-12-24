using System;

namespace ZLL2.Unpacker
{
    class PakEntry
    {
        public UInt32 dwOffset { get; set; }
        public UInt32 dwHash { get; set; }
        public Int32 dwCompressedSize1 { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwUnknown1 { get; set; } // Block Size ??? -> 1024
        public Int32 dwCompressedSize2 { get; set; }
        public Int32 dwUnknown2 { get; set; } // 0
        public String m_FileName { get; set; }
    }
}
