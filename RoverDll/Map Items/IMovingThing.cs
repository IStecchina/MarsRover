using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public interface IMovingThing : IMapThing
    {
        public Direction Orientation { get; }
        public IInput Controller { get; }
        public bool Move(Movement m);
        public bool Rotate(Rotation r);
        public void ProcessCommands(List<string> commands);
    }
}
