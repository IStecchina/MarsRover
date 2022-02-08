using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public class Rover : MapThing, IMovingThing, IBlocker
    {
        public string Name { get; }
        public Direction Orientation { get; protected set; }

        public IInput Controller { get; }
        public IOutput Logger { get; }

        public Rover(PlanetMap map, int startNS, int startWE, Direction startOrientation, string name, IInput controller, IOutput logger) : base(map, startNS, startWE)
        {
            Orientation = startOrientation;
            Name = name;
            Controller = controller;
            Logger = logger;
        }

        public bool Move(Movement m)
        {
            Direction movementDirection = m switch
            {
                Movement.Forwards => Orientation,
                Movement.Backwards => Orientation.Backwards(),
                _ => throw new InvalidOperationException($"Invalid value for Movement enum: {m}"),
            };
            //Position is unchanged if movement is unsuccesful
            Position = ThisMap.PerformMovement(this, movementDirection, out bool succesful);
            if (succesful)
            {
                UniversalLogger.LogMessage($"Rover {Name} moved {movementDirection} to {Position}");
            }
            else
            {
                UniversalLogger.LogMessage($"Rover {Name} could not move to {ThisMap.DesiredMovementResult(Position, movementDirection)}, something blocks the path");
            }
            return succesful;
        }

        public bool Rotate(Rotation r)
        {
            Orientation = r switch
            {
                Rotation.Right => Orientation.RotateRight(),
                Rotation.Left => Orientation.RotateLeft(),
                _ => throw new InvalidOperationException($"Invalid value for Rotation enum: {r}"),
            };
            UniversalLogger.LogMessage($"Rover {Name} rotated {r} to {Orientation}");
            return true;
        }
        public override char MapIcon
        {
            get => Orientation switch
            {
                Direction.North => '^',
                Direction.West => '<',
                Direction.South => 'v',
                Direction.East => '>',
                _ => throw new InvalidOperationException($"Invalid value for Orientation enum: {Orientation}"),
            };
        }

        public void ProcessCommands(List<string> commands)
        {
            foreach (string cmd in commands)
            {
                char[] inputs = cmd.ToUpper().ToCharArray();
                if (ProcessCommand(inputs))
                {
                    UniversalLogger.LogMessage($"Command {cmd.ToUpper()} completed succesfully");
                }
                else
                {
                    UniversalLogger.LogMessage($"Command {cmd.ToUpper()} unsuccesful");
                }
                UniversalLogger.LogMessage(ThisMap);
            }
            UniversalLogger.LogMessage("Done processing commands, logged position to file");
            Logger.WriteState(Position, Orientation);
        }

        public bool ProcessCommand(char[] inputs)
        {
            foreach (char input in inputs)
            {
                UniversalLogger.LogMessage($"Executing input {input}");
                if (!ProcessInput(input)) return false;
            }
            return true;
        }

        public bool ProcessInput(char input)
        {
            return input switch
            {
                'F' => Move(Movement.Forwards),
                'B' => Move(Movement.Backwards),
                'R' => Rotate(Rotation.Right),
                'L' => Rotate(Rotation.Left),
                _ => throw new ArgumentException("INVALID INPUT; ACCEPTED CHARACTERS ARE: F B R L")
            };
        }
    }
}
