using System;
using System.IO;

namespace OpenMaple.Tools
{
    class Log
    {
        private TextWriter writer;

        private static readonly Log Instance = new Log();
        private Log()
        {
            writer = Console.Out;
        }

        public static void WriteInfo(string info)
        {
            lock (Instance.writer) 
                Instance.writer.WriteLine("[Info] {0}", info);
        }

        public static void WriteInfo(string format, params object[] args)
        {
            lock (Instance.writer) 
                Instance.writer.WriteLine("[Info] " + format, args);
        }

        public static void WriteWarning(string warning)
        {
            lock (Instance.writer) 
                Instance.writer.WriteLine("[Warning] {0}", warning);
        }

        public static void WriteWarning(string format, params object[] args)
        {
            lock (Instance.writer) 
                Instance.writer.WriteLine("[Warning] " + format, args);
        }

        public static void WriteError(string error)
        {
            lock (Instance.writer) 
                Instance.writer.WriteLine("[Error] " + error);
        }

        public static void WriteError(string format, params object[] args)
        {
            lock (Instance.writer)
                Instance.writer.WriteLine("[Error] {0}" + format, args);
        }
    }
}
