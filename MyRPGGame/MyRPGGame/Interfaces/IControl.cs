using System.Windows.Forms;

namespace MyRPGGame
{
    public interface IControl
    {
        void TryAttack(object sender, KeyEventArgs e);
        //bool CheckUnitFront(int segmentStart, int segmentEnd);
        //Unit FindUnitInAttackRange();
        //int LaunchAttackBeam(int direction);
        void MoveUnit(object sender, KeyEventArgs e);
        //bool IsOnMap(Vector point);
        //bool CheckCorrectMove(Vector direction);
        //void MoveBorder(Vector direction);
    }
}
