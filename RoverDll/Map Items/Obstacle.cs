using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public class Obstacle : MapThing, IBlocker
    {
        public override char MapIcon { get => 'X'; }
        public Obstacle(PlanetMap map, int startNS, int startWE) : base(map, startNS, startWE)
        {
            UniversalLogger.LogMessage($"Obstacle created at coordinates {Position}");
        }
    }
}
