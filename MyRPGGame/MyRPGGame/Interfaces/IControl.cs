using System.Windows.Forms;

namespace MyRPGGame
{
    public interface IControl
    {
        void TryAttack(Keys key);
        void MoveUnit(Keys key);
    }
}
