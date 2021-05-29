using System.Collections.Generic;

namespace MyRPGGame
{
    public abstract class UnitClass
    {
        public Direction CurrentDirection { get; set; }
        public Vector Location { get; set; }
        public Attributes Attributes { get; protected set; }
        public List<Skill> Skills { get; protected set; }

        public bool IsAlive => Attributes.Health > 0;

        public bool UseSkill(int skillNumber, Unit unit)
        {
            if (Skills[skillNumber].Cooldown.IsReady())
            {
                Skills[skillNumber].UseSkill(unit.UnitClass);
                return true;
            }

            return false;
        }
    }
}
