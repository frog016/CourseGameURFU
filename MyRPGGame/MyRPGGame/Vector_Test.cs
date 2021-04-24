//using System;
//using System.Drawing;
//using NUnit.Framework;

//namespace MyRPGGame
//{
//    [TestFixture]
//    public class Vector_Test
//    {
//        [TestCase(1, 2)]
//        [TestCase(3, 3)]
//        [TestCase(-1.0, -2)]
//        [TestCase(0.646, 1.778)]
//        public void CreateVector(double x, double y)
//        {
//            var actual = new Vector(x, y);
//            Assert.AreEqual(x, actual.X);
//            Assert.AreEqual(y, actual.Y);
//        }

//        [TestCase(1, 2, 0.5, 0.5)]
//        [TestCase(3, 3, 0, 0)]
//        [TestCase(-1.0, -2, 1, 2)]
//        [TestCase(0.646, 1.778, -1.646, -1.778)]
//        public void MoveVector(double x, double y, double dx, double dy)
//        {
//            var vector = new Vector(x, y);
//            var actual = vector.Move(dx, dy);
//            Assert.AreEqual(x, vector.X);
//            Assert.AreEqual(y, vector.Y);

//            Assert.AreEqual(x + dx, actual.X);
//            Assert.AreEqual(y + dy, actual.Y);
//        }

//        [TestCase(1, 0, Math.PI/2, 0, 1)]
//        [TestCase(0, -2, Math.PI, 0, 2)]
//        public void RotateVector(double x, double y, double angle, double exX, double exY)
//        {
//            var actual = new Vector(x, y).Rotate(angle);

//            Assert.AreEqual(exX, actual.X, 1e-12);
//            Assert.AreEqual(exY, actual.Y, 1e-12);
//        }

//        [TestCase(0, 0, 0)]
//        [TestCase(4, 3, 5)]
//        public void LengthVector(double x, double y, double length)
//        {
//            var actual = new Vector(x, y).Length();

//            Assert.AreEqual(length, actual, 1e-12);
//        }
//    }
//}
