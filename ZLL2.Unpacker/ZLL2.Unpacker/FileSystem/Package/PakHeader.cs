using System;

namespace ZLL2.Unpacker
{
    class PakHeader
    {
        public UInt64 dwMagic { get; set; } //0x303037305950414B (KAPY0700)
        public Int32 dwHeaderSize { get; set; } // 64
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwEntryTableOffset { get; set; }
        public UInt32 dwNamesTableOffset { get; set; }
        public Int32 dwEntryTableCompressedSize { get; set; }
        public Int32 dwNamesTableCompressedSize { get; set; }
        public Int32 dwNamesTableDecompressedSize { get; set; }
        public Int32 dwUnknown { get; set; } // Block Size ??? -> 262144
    }
}
