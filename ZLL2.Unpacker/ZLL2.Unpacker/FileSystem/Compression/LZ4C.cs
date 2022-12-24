using System;
using System.IO;

using LZ4Sharp;

namespace ZLL2.Unpacker
{
    class LZ4C
    {
        public static Byte[] iDecompress(Byte[] lpSrcBuffer, Int32 dwDecompressedSize)
        {
            Byte[] lpDstBuffer = new Byte[dwDecompressedSize];

            using (MemoryStream TMemoryStream = new MemoryStream(lpSrcBuffer))
            {
                LZ4Decompressor64 TLZ4Decompressor64 = new LZ4Decompressor64();
                TLZ4Decompressor64.Decompress(lpSrcBuffer, lpDstBuffer);
            }
            return lpDstBuffer;
        }
    }
}
