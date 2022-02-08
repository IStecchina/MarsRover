using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public interface IInput
    {
        public List<string> GetCommands();
    }
}
