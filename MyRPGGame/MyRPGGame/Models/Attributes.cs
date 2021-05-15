namespace MyRPGGame
{
    public class Attributes
    {
        public int Health { get; set; }
        public int Armor { get; set; }
        public int Damage { get; set; }
        public int AttackRange { get; set; }

        public Attributes(int health, int armor, int damage, int attackRange)
        {
            Health = health;
            Armor = armor;
            Damage = damage;
            AttackRange = attackRange;
        }
    }
}
