using System;
using System.IO;

namespace OpenStory.Common.Tools
{
    public class Log : ILogger
    {
        private static readonly ILogger InternalInstance = new Log();
        private TextWriter writer;

        private Log()
        {
            this.writer = Console.Out;
        }

        public static ILogger Instance
        {
            get { return InternalInstance; }
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
            lock (this.writer) this.writer.WriteLine("[Info] {0}", info);
        }

        void ILogger.WriteInfo(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Info] " + format, args);
        }

        void ILogger.WriteWarning(string warning)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] {0}", warning);
        }

        void ILogger.WriteWarning(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] " + format, args);
        }

        void ILogger.WriteError(string error)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] {0}", error);
        }

        void ILogger.WriteError(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] " + format, args);
        }

        #endregion
    }

    /// <summary>
    /// Provides methods for logging messages.
    /// </summary>
    public interface ILogger
    {
        void WriteInfo(string info);
        void WriteInfo(string format, params object[] args);
        void WriteWarning(string warning);
        void WriteWarning(string format, params object[] args);
        void WriteError(string error);
        void WriteError(string format, params object[] args);
    }
}