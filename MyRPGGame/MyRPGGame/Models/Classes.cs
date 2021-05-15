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
    }

    public class Swordsman : UnitClass, IWarrior
    {
        public Swordsman(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(10, 1, 4, 3);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public void UseSkill(int skillNumber, Unit unit)
        {
            if (Skills[skillNumber].Cooldown.IsReady())
                Skills[skillNumber].UseSkill((UnitClass)unit.UnitClass);
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("bash", target => target.Attributes.Health -= Attributes.Damage - target.Attributes.Armor, new Cooldown(3)),
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

    public class Rogue : UnitClass, IWarrior
    {
        public Rogue(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(7, 0, 3, 1);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public void UseSkill(int skillNumber, Unit unit)
        {
            if (Skills[skillNumber].Cooldown.IsReady())
                Skills[skillNumber].UseSkill((UnitClass)unit.UnitClass);
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

    public class Guard : UnitClass, IWarrior
    {
        public Guard(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(14, 2, 2, 2);
            Location = location;
            CurrentDirection = Direction.Right;
        }

        public void UseSkill(int skillNumber, Unit unit)
        {
            if (Skills[skillNumber].Cooldown.IsReady())
                Skills[skillNumber].UseSkill((UnitClass)unit.UnitClass);
        }

        private List<Skill> InitializationSkills()
        {
            return new List<Skill>
            {
                new Skill("strike", target => target.Attributes.Health -= Attributes.Damage - target.Attributes.Armor, new Cooldown(2)),
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
