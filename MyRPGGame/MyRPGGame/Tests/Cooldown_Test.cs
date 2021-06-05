using NUnit.Framework;

namespace MyRPGGame.Tests
{
    [TestFixture]
    public class Cooldown_Test
    {
        [TestCase(0)]
        [TestCase(3)]
        public void TestIsCooldownReady(double time)
        {
            Assert.Catch(() => new Cooldown(-10));
            var cooldown = new Cooldown(time);
            Assert.True(cooldown.IsReady());
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(8)]
        public void TestActivateCooldown(double time)
        {
            var cooldown = new Cooldown(time);
            cooldown.IsReady();
            Assert.False(cooldown.IsReady());
        }
    }
}
