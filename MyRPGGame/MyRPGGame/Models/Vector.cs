using System;
using System.Drawing;

namespace MyRPGGame
{
    public class Vector
    {
        public static Vector Zero = new Vector(0, 0);

        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector() { }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(Vector direction)
        {
            X += direction.X;
            Y += direction.Y;
        }

        public double Length()
        {
            return Math.Sqrt(X*X + Y*Y);
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public static Vector operator +(Vector first, Vector second) =>
            new Vector(first.X + second.X, first.Y + second.Y);

        public static Vector operator -(Vector first, Vector second) =>
            new Vector(first.X - second.X, first.Y - second.Y);

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public override bool Equals(object obj)
        {
            var vector = (Vector)obj;
            return X == vector.X && Y == vector.Y;
        }

        public override int GetHashCode()
        {
            return 397*X + 123*Y;
        }
    }
}
