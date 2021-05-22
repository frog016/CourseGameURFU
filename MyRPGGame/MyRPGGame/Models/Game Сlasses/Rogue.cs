using System.Collections.Generic;

namespace MyRPGGame
{
    public class Rogue : UnitClass
    {
        public Rogue(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(7, 0, 3, 1);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("quick strike", target => target.Attributes.Health -= Attributes.Damage - target.Attributes.Armor, new Cooldown(1)),
                new Skill("backstab", target =>
                {
                    var damageMultiplier = target.CurrentDirection == CurrentDirection ? 3 : 1;
                    target.Attributes.Health -= damageMultiplier * Attributes.Damage - target.Attributes.Armor;
                }, new Cooldown(5))
            };
        }
    }
}
