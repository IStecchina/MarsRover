using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public abstract class MapThing : IMapThing
    {
        public PlanetMap ThisMap { get; protected set; }
        public Coords Position { get; protected set; }
        public virtual char MapIcon { get => '?'; }

        public MapThing(PlanetMap map, int startNS, int startWE)
        {
            ThisMap = map;
            Position = ThisMap.WrapAround(new Coords(startNS, startWE));
        }

    }
}
