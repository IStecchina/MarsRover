using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover
{
    public class Coords
    {
        public int NS { get; set; }
        public int WE { get; set; }
        public Coords(int ns, int we)
        {
            NS = ns;
            WE = we;
        }

        public static Coords operator +(Coords a, Coords b) => new Coords(a.NS + b.NS, a.WE + b.WE);
        public static implicit operator (int, int)(Coords c) => (c.NS, c.WE);

        public override bool Equals(object obj)
        {
            if (obj is Coords c)
            {
                return NS == c.NS && WE == c.WE;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return (NS,WE).GetHashCode();
        }
        public override string ToString()
        {
            return $"N:{NS}/W:{WE}";
        }
    }
}
