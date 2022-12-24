using System;
using System.IO;

namespace ZLL2.Unpacker
{
    class Program
    {
        //com.indie.zll2.google.kr
        private static String m_Title = "강림2:제천대성 PAK Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ZLL2 PAK Unpacker");
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    ZLL2.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of PAK file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    ZLL2.Unpacker E:\\Games\\ZLL2\\c3d.pak D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_PakFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_PakFile))
            {
                Utils.iSetError("[ERROR]: Input PAK file -> " + m_PakFile + " <- does not exist");
                return;
            }

            PakUnpack.iDoIt(m_PakFile, m_Output);
        }
    }
}
