using System;

namespace WPR.Common
{
    public static class Log
    {
        private static void Write(LogCategory category, String content)
        {
            Console.WriteLine($"[{category}] {content}");
        }

        public static void Error(LogCategory category, String content)
        {
            Write(category, content);
        }
        public static void Warn(LogCategory category, String content)
        {
            Write(category, content);
        }
    }
}