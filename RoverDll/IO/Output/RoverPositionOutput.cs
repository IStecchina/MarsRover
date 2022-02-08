using System.IO;

namespace MarsRover
{
    public class RoverPositionOutput : IOutput
    {
        private readonly string filepath;

        public RoverPositionOutput(string filename)
        {
            var f = File.Create(filename);
            f.Dispose();
            filepath = filename;
        }

        public void WriteState(Coords position, Direction orientation)
        {
            File.AppendAllText(filepath, $"({position.NS},{position.WE},{orientation.ToShortString()})\n");
        }
    }
}