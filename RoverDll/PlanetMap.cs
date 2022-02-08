using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public class PlanetMap
    {
        // |0.0|0.1|0.2|
        // |1.0|1.1|1.2|
        // |2.0|2.1|2.2|
        public (int NS, int WE) Size { get; }
        private readonly Dictionary<(int, int), IBlocker> blockers = new Dictionary<(int, int), IBlocker>();
        public PlanetMap(int sizeNS, int sizeWE, bool fillRandomly = false, double fillFrequency = 0)
        {
            if (sizeNS < 1 || sizeWE < 1) throw new ArgumentOutOfRangeException("Map sizes must be greater than 1");
            Size = (sizeNS, sizeWE);
            if (fillRandomly) FillWithRandomObstacles(fillFrequency);
        }
        public PlanetMap(int size, bool fillRandomly = false, double fillFrequency = 0)
        {
            if (size < 1) throw new ArgumentOutOfRangeException("Map sizes must be greater than 1");
            Size = (size, size);
            if (fillRandomly) FillWithRandomObstacles(fillFrequency);
        }

        public bool TryLandRover(out Rover r, int ns, int we, Direction dir, string name, IInput controller, IOutput logger)
        {
            if (blockers.ContainsKey((ns, we)))
            {
                UniversalLogger.LogMessage($"Could not land rover, something alread exists at coords {new Coords(ns, we)}");
                r = null;
                return false;
            }
            else
            {
                r = new Rover(this, ns, we, dir, name, controller, logger);
                blockers.Add(r.Position, r);
                return true;
            }
        }

        public bool TryAddObstacle(int ns, int we)
        {
            Obstacle newBlocker = new Obstacle(this, ns, we);
            if (blockers.TryAdd(newBlocker.Position, newBlocker))
            {
                return true;
            }
            else
            {
                UniversalLogger.LogMessage($"Could not add obstacle, something alread exists at coords {newBlocker.Position}");
                return false;
            }
        }

        public Coords WrapAround(Coords pos)
        {
            //Horrible reimplementation of modulo operator
            int ns = pos.NS % Size.NS;
            if (ns < 0) ns += Size.NS;
            int we = pos.WE % Size.WE;
            if (we < 0) we += Size.WE;
            return new Coords(ns, we);
        }

        public Coords DesiredMovementResult(Coords startPos, Direction dir)
        {
            return WrapAround(startPos + dir.DirectionVector());
        }

        public Coords PerformMovement(IMovingThing m, Direction dir, out bool succesful)
        {
            Coords oldPosition = m.Position;
            Coords desiredPosition = DesiredMovementResult(oldPosition, dir);
            Coords endPos;
            //We can't move if the destination is blocked, unless the destination is the same place we're already in
            if (blockers.ContainsKey(desiredPosition) && !desiredPosition.Equals(oldPosition))
            {
                endPos = oldPosition;
                succesful = false;
            }
            else
            {
                endPos = desiredPosition;
                succesful = true;
            }
            //Update blocker position, even if it didn't move
            if (m is IBlocker b)
            {
                blockers.Remove(oldPosition);
                blockers.Add(endPos, b);
            }
            return endPos;
        }

        public void FillWithRandomObstacles(double obstacleFrequency)
        {
            if (obstacleFrequency < 0 || obstacleFrequency > 1) throw new ArgumentOutOfRangeException("Obstacle frequency cannot be smaller than 0 nor greater than 1");
            bool[] randomObstacles = new bool[Size.NS * Size.WE];
            int numObstacles = (int)Math.Round((double)randomObstacles.Length * obstacleFrequency);
            for (int i = 0; i < randomObstacles.Length; i++)
            {
                randomObstacles[i] = false;
            }
            for (int i = 0; i < numObstacles; i++)
            {
                randomObstacles[i] = true;
            }
            randomObstacles = Shuffle.PerformShuffle(randomObstacles);
            for (int i = 0; i < randomObstacles.Length; i++)
            {
                if (randomObstacles[i]) TryAddObstacle(i / Size.NS, i % Size.NS);
            }
        }

        public Rover LandRover(string name, IInput controller, IOutput logger)
        {
            for (int ns = 0; ns < Size.NS; ns++)
            {
                for (int we = 0; we < Size.WE; we++)
                {
                    bool success = TryLandRover(out Rover r, ns, we, Direction.North, name, controller, logger);
                    if (success)
                    {
                        UniversalLogger.LogMessage($"Succesfully landed rover {r.Name} at coordinates {r.Position}");
                        return r;
                    }
                }
            }
            throw new Exception("The whole planet is full of obstacles!");
        }

        public override string ToString()
        {
            //Per row: N elements, N + 1 separators, 4 characters extra for newline
            //Also 4 extra characters for the starting newline
            var minimap = new StringBuilder(Size.NS * (Size.WE * 2 + 1 + 4) + 4);
            minimap.AppendLine();
            for (int ns = 0; ns < Size.NS; ns++)
            {
                for (int we = 0; we < Size.WE; we++)
                {
                    minimap.Append("|");
                    if (blockers.TryGetValue((ns, we), out IBlocker blocker))
                    {
                        minimap.Append(blocker.MapIcon);
                    }
                    else
                    {
                        minimap.Append(" ");
                    }
                }
                minimap.AppendLine("|");
            }
            return minimap.ToString();
        }
        /*
        public static PlanetMap GenerateRandom(uint width, uint height, uint obstacles)
        {
            var rand = new Random();
            var map = new PlanetMap(width, height);
            while (map.blockers.Count < obstacles)
            {
                var obstacle = new Obstacle(map, rand.Next(width - 1), rand.Next(height - 1));
                map.blockers.TryAdd(obstacle.Position, obstacle);
            }
            return map;
        }
        */
    }
}
