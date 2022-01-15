using System;

namespace UdpRule.Test.Objects
{
    class Vector3
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public Vector3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
