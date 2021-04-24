using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class UnitView
    {
        private readonly Swordsman unit;

        public Point TopLeft => new Point(unit.Location.X - (UnitSize.Width / 20)*10, unit.Location.Y - (UnitSize.Height / 20) * 10);
        public Point TopRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10, unit.Location.Y - (UnitSize.Height / 20) * 10);
        public Point BottomLeft => new Point(unit.Location.X - (UnitSize.Width / 20) * 10, unit.Location.Y + (UnitSize.Height / 20) * 10);
        public Point BottomRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10, unit.Location.Y + (UnitSize.Height / 20) * 10);

        public static readonly Size UnitSize = new Size(5 * Map.CellSize, 7 * Map.CellSize);
        //private readonly Image sprite;

        public UnitView(Swordsman unit, Image sprite)
        {
            this.unit = unit;
            //this.sprite = sprite;
        }

        public void DrawUnit(Graphics window)
        {
            var dir = unit.CurrentDirection == Direction.Left? -1 : 1;
            var res = new Point(unit.Location.X + dir * (UnitSize.Width / 20) * 10, unit.Location.Y);
            window.DrawEllipse(new Pen(Color.Blue, 8), res.X, res.Y, 1, 1);
        }

        public void DrawCooldownBar(Graphics window)
        {
            
        }

        public void DrawHpBar(Graphics window)
        {
            window.DrawString(unit.Health.ToString(), new Font(new FontFamily("Arial"), 10), new HatchBrush(HatchStyle.Cross, Color.Black), new PointF(TopLeft.X, TopLeft.Y - 10));
        }
    }
}
