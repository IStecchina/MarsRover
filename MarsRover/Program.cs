using System;
namespace MarsRover
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize(out PlanetMap myPlanetMap, out Rover myRover);
            myRover.ProcessCommands(myRover.Controller.GetCommands());
        }

        static void Initialize(out PlanetMap map, out Rover rv)
        {
            IInput newInput = new CmdInput("input.txt");
            IOutput newOutput = new RoverPositionOutput("output.txt");
            map = new PlanetMap(6, 6, true, 0.25);
            rv = map.LandRover("R-1", newInput, newOutput);
            UniversalLogger.LogMessage(map);
        }
    }
}
