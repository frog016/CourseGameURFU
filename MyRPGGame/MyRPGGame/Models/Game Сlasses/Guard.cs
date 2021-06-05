using System;
using System.Collections.Generic;

namespace MyRPGGame
{
    public class Guard : UnitClass
    {
        public Guard(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(14, 2, 3, 2);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public override bool Equals(object obj)
        {
            var unit = obj as Guard;
            return unit.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Attributes.Health * 321 + Attributes.Armor*32 + Attributes.Damage * 3 + Attributes.AttackRange;
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("strike", target => target.Attributes.Health -= Math.Max(Attributes.Damage - target.Attributes.Armor, 0), new Cooldown(2)),
                new Skill("defence", target =>
                {
                    Attributes.Armor *= 2;
                    var timeOfAction = new System.Timers.Timer(4000);
                    timeOfAction.Elapsed += (s, e) =>
                    {
                        Attributes.Armor /= 2;
                        timeOfAction.Dispose();
                    };
                    timeOfAction.Start();
                }, new Cooldown(10))
            };
        }
    }
}
