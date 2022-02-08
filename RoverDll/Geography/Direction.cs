using System;

namespace MarsRover
{
    public enum Direction
    {
        North,
        West,
        South,
        East
    }

    public static class DirectionExtensions
    {
        public static Coords DirectionVector(this Direction dir) => dir switch
        {
            Direction.North => new Coords(-1, 0),
            Direction.South => new Coords(1, 0),
            Direction.West => new Coords(0, -1),
            Direction.East => new Coords(0, 1),
            _ => throw new InvalidOperationException($"Invalid value for Direction enum: {dir}")
        };
        public static Direction Backwards(this Direction dir) => dir switch
        {
            Direction.North => Direction.South,
            Direction.West => Direction.East,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            _ => throw new Exception($"Invalid value for Direction enum: {dir}"),
        };

        public static Direction RotateRight(this Direction dir) => dir switch
        {
            Direction.North => Direction.East,
            Direction.West => Direction.North,
            Direction.South => Direction.West,
            Direction.East => Direction.South,
            _ => throw new Exception($"Invalid value for Direction enum: {dir}"),
        };

        public static Direction RotateLeft(this Direction dir) => dir switch
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => throw new Exception($"Invalid value for Direction enum: {dir}"),
        };
        public static string ToShortString(this Direction dir) => dir switch
        {
            Direction.North => "N",
            Direction.South => "S",
            Direction.West => "W",
            Direction.East => "E",
            _ => throw new InvalidOperationException($"Invalid value for Direction enum: {dir}")
        };
    }
}
