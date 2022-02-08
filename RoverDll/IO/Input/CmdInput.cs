using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarsRover
{
    public class CmdInput : IInput
    {
        private readonly string filepath;

        public CmdInput(string filename)
        {
            if (!File.Exists(filename))
            {
                var f = File.Create(filename);
                f.Dispose();
            }
            filepath = filename;
        }
        public List<string> GetCommands()
        {
            var commandList = new List<string>(File.ReadLines(filepath));
            return commandList;
        }
    }
}
