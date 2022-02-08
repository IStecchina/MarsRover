using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public interface IMapThing
    {
        public PlanetMap ThisMap { get; }
        public abstract Coords Position { get; }
        public virtual char MapIcon { get => '?'; }
    }
}
