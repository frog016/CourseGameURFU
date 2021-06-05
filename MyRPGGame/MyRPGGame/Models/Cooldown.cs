using System;

namespace MyRPGGame
{
    public class Cooldown
    {
        public readonly double CooldownTime;
        public DateTime LastTimeUse { get; private set; }

        /// <param name="cooldownTime">Time in seconds.</param>
        public Cooldown(double cooldownTime)
        {
            if (cooldownTime < 0)
                throw new ArgumentException();
            CooldownTime = cooldownTime;
            LastTimeUse = new DateTime();
        }

        public bool IsReady()
        {
            var currentTime = DateTime.Now;
            if (currentTime.Subtract(LastTimeUse).TotalSeconds > CooldownTime)
            {
                LastTimeUse = currentTime;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Cooldown time = {CooldownTime}";
        }
    }
}
