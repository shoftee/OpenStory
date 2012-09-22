using System;

namespace OpenStory.Redirector
{
    [Serializable]
    internal enum LogMessageType
    {
        Error = 0,
        Warning,
        Info,
        Connection,
        DataLoad,
        Exception,
    }
}