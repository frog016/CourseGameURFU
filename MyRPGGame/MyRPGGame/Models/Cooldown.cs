using System;

namespace MyRPGGame
{
    public class Cooldown
    {
        public readonly double CooldownTime;
        private DateTime LastTimeUse { get; set; }

        /// <param name="cooldownTime">Time in seconds.</param>
        public Cooldown(double cooldownTime)
        {
            CooldownTime = cooldownTime;
            LastTimeUse = DateTime.Now;
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
