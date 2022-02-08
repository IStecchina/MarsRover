using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public static class Shuffle
    {
        private static readonly Random rng;
        static Shuffle()
        {
            rng = new Random();
        }
        public static bool[] PerformShuffle(bool[] input)
        {
            int length = input.Length;
            bool[] output = input;
            bool temp;
            int swapIndex;
            for (int i = 0; i < length; i++)
            {
                swapIndex = rng.Next(i, length - 1);
                temp = output[i];
                output[i] = output[swapIndex];
                output[swapIndex] = temp;
            }
            return output;
        }
    }
}
