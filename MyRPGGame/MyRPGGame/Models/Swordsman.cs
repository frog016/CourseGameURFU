namespace MyRPGGame
{
    public class Swordsman
    {
        public int Health { get; set; } // Выделить в класс статов
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public Vector Location { get; set; }
        public Direction CurrentDirection { get; set; }
        private readonly Cooldown attackCooldown;
        public bool IsAlive => Health > 0;

        public Swordsman(int health, Vector location)
        {
            Health = health;
            Location = location;
            attackCooldown = new Cooldown(3);
            CurrentDirection = Direction.Right;
            Damage = 1;
            AttackRange = 3;
        }

        public void Attack(Swordsman enemy)
        {
            if (attackCooldown.IsReady())
                enemy.Health -= Damage;
        }
    }
}
