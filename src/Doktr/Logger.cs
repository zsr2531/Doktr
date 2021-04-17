using System;

namespace Doktr
{
    public static class Logger
    {
        public static void Verbose(string message) => Log($"VERBOSE: {message}");

        public static void Info(string message) => Log($"INFO: {message}");

        public static void Warning(string message) => Log($"WARNING: {message}");

        private static void Log(string message) => Console.WriteLine(message);
    }
}