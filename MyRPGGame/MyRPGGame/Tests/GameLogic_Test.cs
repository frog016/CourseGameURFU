using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;

namespace MyRPGGame.Tests
{
    [TestFixture]
    public class GameLogic_Test
    {
        private Map testMap { get; set; }
        private AI Bot { get; set; }

        private string firstTestMap = @"----
-P--
-E--
----";
        private string secondTestMap = @"----
----
-EP-
----";
        private string mapWithOnlyPlayer = @"---
-P-
---";

        [Test]
        public void TestInitializeAI()
        {
            CreateTestMap(firstTestMap);
            var aiType = typeof(AI);
            var findNearestTarget = aiType.GetMethod("FindNearestTarget", BindingFlags.Instance | BindingFlags.NonPublic);
            findNearestTarget?.Invoke(Bot, new object[]{});
            Assert.NotNull(aiType.GetProperty("Target", BindingFlags.Instance | BindingFlags.NonPublic));
        }

        [Test]
        public void TestMoveToTarget()
        {
            CreateTestMap(firstTestMap);
            var aiType = typeof(AI);
            var findNearestTarget = aiType.GetMethod("FindNearestTarget", BindingFlags.Instance | BindingFlags.NonPublic);
            findNearestTarget?.Invoke(Bot, new object[] { });
            Bot.MoveUnit(Keys.K);

            var unitClass = aiType.GetField("unitClass", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Bot) as UnitClass;
            Assert.AreEqual(new Vector(50, 130), unitClass.Location); // Ожидаемый результат посчитан исходя из тестовой карты

            Bot.MoveUnit(Keys.K);
            Assert.AreEqual(new Vector(40, 130), unitClass.Location);
        }

        [Test]
        public void TestAttack()
        {
            CreateTestMap(secondTestMap);
            var aiType = typeof(AI);
            var unitClass = aiType.GetField("unitClass", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Bot) as UnitClass;
            var findNearestTarget = aiType.GetMethod("FindNearestTarget", BindingFlags.Instance | BindingFlags.NonPublic);
            findNearestTarget?.Invoke(Bot, new object[] { });
            var target = aiType.GetProperty("Target", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Bot) as Unit;
            var previousHealth = target.UnitClass.Attributes.Health;
            Bot.TryAttack(Keys.K);
            Assert.AreEqual(previousHealth, target.UnitClass.Attributes.Health + unitClass.Attributes.Damage - target.UnitClass.Attributes.Armor);
        }

        [Test]
        public void TestMoveOutOfMap() //Тест будет пройден, если не вылетит эксепшена
        {
            CreateTestMap(mapWithOnlyPlayer);
            for (var i = 0; i < 100; i++)
                testMap.Player.Control.MoveUnit(Keys.D);
        }

        private void CreateTestMap(string map)
        {
            File.WriteAllText(@"..\..\Maps\TestMap.txt", map);
                testMap = new Map(new Swordsman(Vector.Zero), @"TestMap");
            Bot = testMap.Units.FirstOrDefault()?.Control as AI;
        }
    }
}
