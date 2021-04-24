using System.Windows.Forms;

namespace MyRPGGame
{
    public static class BindKeyboardKeys
    {
        public static Keys MoveUp { get; set; }
        public static Keys MoveDown { get; set; }
        public static Keys MoveLeft { get; set; }
        public static Keys MoveRight { get; set; }

        public static Keys Attack { get; set; }

        static BindKeyboardKeys()
        {
            MoveUp = Keys.W;
            MoveDown = Keys.S;
            MoveLeft = Keys.A;
            MoveRight = Keys.D;
            Attack = Keys.F;
        }
    }
}
