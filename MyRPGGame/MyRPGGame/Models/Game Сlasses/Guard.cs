using System.Collections.Generic;

namespace MyRPGGame
{
    public class Guard : UnitClass
    {
        public Guard(Vector location)
        {
            Skills = InitializationSkills();
            Attributes = new Attributes(14, 2, 2, 2);
            Location = location;
            CurrentDirection = Direction.Right;
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
