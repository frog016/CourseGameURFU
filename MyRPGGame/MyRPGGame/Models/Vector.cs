using System;
using System.Drawing;

namespace MyRPGGame
{
    public class Vector
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Takes an angle in radians and returns a rotated vector.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        //public Vector Rotate(double angle)
        //{
        //    var x = X * Math.Cos(angle) - Y * Math.Sin(angle);
        //    var y = X * Math.Sin(angle) + Y * Math.Cos(angle);

        //    return new Vector(x, y);
        //}
        
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
    }
}
