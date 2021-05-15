using System;

namespace MyRPGGame
{
    public class Skill
    {
        public readonly string Name;
        public readonly Action<UnitClass> UseSkill;
        public readonly Cooldown Cooldown;

        public Skill(string name, Action<UnitClass> useSkill, Cooldown cooldown)
        {
            Name = name;
            UseSkill = useSkill;
            Cooldown = cooldown;
        }
    }
}
