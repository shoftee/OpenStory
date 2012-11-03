using System;
using System.Collections.Generic;

namespace OpenStory.Redirector
{
    internal static class Logger
    {
        private static readonly Dictionary<LogMessageType, ConsoleColor> LogColors =
            new Dictionary<LogMessageType, ConsoleColor>
                {
                    {LogMessageType.Error, ConsoleColor.Red},
                    {LogMessageType.Warning, ConsoleColor.Yellow},
                    {LogMessageType.Info, ConsoleColor.Blue},
                    {LogMessageType.Connection, ConsoleColor.Green},
                    {LogMessageType.DataLoad, ConsoleColor.Cyan},
                    {LogMessageType.Exception, ConsoleColor.Magenta},
                };

        public static void Write(LogMessageType messageType, string message, params object[] args)
        {
            string formatted = string.Format(message, args);
            string type = string.Format("[{0}]", messageType);

            var lastColor = Console.ForegroundColor;

            Console.ForegroundColor = LogColors[messageType];
            Console.Write(type);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" " + formatted);

            Console.ForegroundColor = lastColor;

            Console.WriteLine();
        }
    }
}
