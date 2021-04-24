namespace MyRPGGame
{
    public class Swordsman
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }
        public Vector Location { get; set; }
        public Direction CurrentDirection { get; set; }
        public readonly Cooldown AttackCooldown;

        public Swordsman(int health, Vector location)
        {
            Health = health;
            Location = location;
            AttackCooldown = new Cooldown(3);
            CurrentDirection = Direction.Right;
            Damage = 1;
            AttackRange = 3;
        }

        public void Attack(Swordsman enemy) => enemy.Health -= Damage;
    }
}
