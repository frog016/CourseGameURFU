using System;
using System.Collections.Generic;

namespace MyRPGGame
{
    public class Rogue : UnitClass
    {
        public Rogue(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(8, 0, 3, 2);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public override bool Equals(object obj)
        {
            var unit = obj as Rogue;
            return unit.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Attributes.Health * 321 + Attributes.Armor * 32 + Attributes.Damage * 3 + Attributes.AttackRange;
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("quick strike", target => target.Attributes.Health -= Math.Max(Attributes.Damage - target.Attributes.Armor, 0), new Cooldown(1)),
                new Skill("backstab", target =>
                {
                    var damageMultiplier = target.CurrentDirection == CurrentDirection ? 3 : 1;
                    target.Attributes.Health -= damageMultiplier * Attributes.Damage - target.Attributes.Armor;
                }, new Cooldown(5))
            };
        }
    }
}
