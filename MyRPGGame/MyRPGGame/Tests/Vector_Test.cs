using NUnit.Framework;

namespace MyRPGGame
{
    [TestFixture]
    public class Vector_Test
    {
        [TestCase(1, 2)]
        [TestCase(3, 3)]
        [TestCase(-1, -2)]
        public void CreateVector(int x, int y)
        {
            var actual = new Vector(x, y);
            Assert.AreEqual(x, actual.X);
            Assert.AreEqual(y, actual.Y);
        }

        [TestCase(1, 2, 2, 1)]
        [TestCase(3, 3, 0, 0)]
        [TestCase(-1, -2, 1, 2)]
        public void MoveVector(int x, int y, int dx, int dy)
        {
            var vector = new Vector(x, y);
            vector.Move(new Vector(dx, dy));
            Assert.AreEqual(x+dx, vector.X);
            Assert.AreEqual(y+dy, vector.Y);
        }

        [TestCase(0, 0, 0)]
        [TestCase(4, 3, 5)]
        public void LengthVector(int x, int y, double length)
        {
            var actual = new Vector(x, y).Length();

            Assert.AreEqual(length, actual, 1e-12);
        }

        [TestCase(1, 1, 2, 2, false)]
        [TestCase(2, 2, 2, 2, true)]
        public void EqualsVectors(int x1, int y1, int x2, int y2, bool result)
        {
            var first = new Vector(x1, y1);
            var second = new Vector(x2, y2);

            Assert.AreEqual(result, first.Equals(second));
        }

        [TestCase(1, 1, 2, 2)]
        [TestCase(2, 2, 2, 2)]
        public void OperatorVectors(int x1, int y1, int x2, int y2)
        {
            var first = new Vector(x1, y1);
            var second = new Vector(x2, y2);

            Assert.AreEqual(new Vector(x1 - x2, y1 - y2), first - second);
            Assert.AreEqual(new Vector(x2 - x1, y2 - y1), second - first);
            Assert.AreEqual(new Vector(x1 + x2, y1 + y2), first + second);
        }
    }
}
