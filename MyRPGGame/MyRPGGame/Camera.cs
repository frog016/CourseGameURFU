using System.Drawing;

namespace MyRPGGame
{
    public class Camera
    {
        public void FocusCameraOnPlayer(Unit player, Graphics graphics)
        {
            var startPosition = new Vector(200, 200);
            var playerUnitClass = (UnitClass) player.UnitClass;
            var delta = playerUnitClass.Location - startPosition;
        }
    }
}
