using NUnit.Framework;

namespace MyRPGGame.Tests
{
    [TestFixture]
    public class Unit_Test
    {
        [Test]
        public void CreateUnitClass()
        {
            var unit = new Swordsman(Vector.Zero);
            Assert.True(unit.IsAlive);
            Assert.True(unit.Skills.Count > 0);
        }

        [Test]
        public void TestDifferentClasses()
        {
            var first = new Swordsman(new Vector(1, 1)); //По логике игры в одном месте не могут быть несколько юнитов
            var second = new Guard(Vector.Zero);
            var third = new Swordsman(new Vector(-1, -1));
            Assert.False(first.Equals(second));
            Assert.False(first.Equals(third));
        }
    }
}
