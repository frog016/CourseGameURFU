using System;
using System.Collections.Generic;

namespace MyRPGGame
{
    public class Swordsman : UnitClass
    {
        public Swordsman(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(10, 1, 4, 3);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public override bool Equals(object obj)
        {
            var unit = obj as UnitClass;
            return unit.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Attributes.Health * 321 + Attributes.Armor * 32 + Attributes.Damage * 3 + Attributes.AttackRange + Location.X*94 + Location.Y*17;
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("bash", target => target.Attributes.Health -= Math.Max(Attributes.Damage - target.Attributes.Armor, 0), new Cooldown(3)),
                new Skill("berserk", target =>
                {
                    Attributes.Damage *= 2;
                    var timeOfAction = new System.Timers.Timer(5000);
                    timeOfAction.Elapsed += (s, e) =>
                    {
                        Attributes.Damage /= 2;
                        timeOfAction.Dispose();
                    };
                    timeOfAction.Start();
                }, new Cooldown(20))
            };
        }
    }
}
