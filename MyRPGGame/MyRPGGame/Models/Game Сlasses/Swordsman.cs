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
}
