using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarsRover
{
    public static class UniversalLogger
    {
        private const string filepath = "log.txt";
        static UniversalLogger()
        {
            var f = File.Create(filepath);
            f.Dispose();
        }

        public static void LogMessage(object message)
        {
            File.AppendAllText(filepath, message.ToString() + Environment.NewLine);
        }
    }
}
