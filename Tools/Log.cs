using System;
using System.IO;
using System.Text;

namespace OpenMaple.Tools
{
    class Log : ILogger
    {
        private TextWriter writer;

        public static ILogger Instance { get { return InternalInstance; } }
        private static readonly ILogger InternalInstance = new Log();
        private Log()
        {
            writer = Console.Out;
        }

        public static void WriteInfo(string info)
        {
            InternalInstance.WriteInfo(info);
        }

        public static void WriteInfo(string format, params object[] args)
        {
            InternalInstance.WriteInfo(format, args);
        }

        public static void WriteWarning(string warning)
        {
            InternalInstance.WriteWarning(warning);
        }

        public static void WriteWarning(string format, params object[] args)
        {
            InternalInstance.WriteWarning(format, args);
        }

        public static void WriteError(string error)
        {
            InternalInstance.WriteError(error);
        }

        public static void WriteError(string format, params object[] args)
        {
            InternalInstance.WriteError(format, args);
        }

        #region Explicit ILogger implementation

        void ILogger.WriteInfo(string info)
        {
            lock (writer) writer.WriteLine("[Info] {0}", info);
        }

        void ILogger.WriteInfo(string format, params object[] args)
        {
            lock (writer) writer.WriteLine("[Info] " + format, args);
        }

        void ILogger.WriteWarning(string warning)
        {
            lock (writer) writer.WriteLine("[Warning] {0}", warning);
        }

        void ILogger.WriteWarning(string format, params object[] args)
        {
            lock (writer) writer.WriteLine("[Warning] " + format, args);
        }

        void ILogger.WriteError(string error)
        {
            lock (writer) writer.WriteLine("[Warning] {0}", error);
        }

        void ILogger.WriteError(string format, params object[] args)
        {
            lock (writer) writer.WriteLine("[Warning] " + format, args);
        }

        #endregion
    }

    /// <summary>
    /// Provides methods for logging messages.
    /// </summary>
    interface ILogger
    {
        void WriteInfo(string info);
        void WriteInfo(string format, params object[] args);
        void WriteWarning(string warning);
        void WriteWarning(string format, params object[] args);
        void WriteError(string error);
        void WriteError(string format, params object[] args);
    }
}
