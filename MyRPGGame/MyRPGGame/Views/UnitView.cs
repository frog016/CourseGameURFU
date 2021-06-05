using System.Drawing;
using System.Timers;

namespace MyRPGGame
{
    public class UnitView
    {
        public static readonly Size UnitSize = new Size(5 * Map.CellSize, 7 * Map.CellSize);

        public Point TopLeft => new Point(unit.Location.X - (UnitSize.Width / 20) * 10, unit.Location.Y - (UnitSize.Height / 20) * 10);
        public Point TopRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10, unit.Location.Y - (UnitSize.Height / 20) * 10);
        public Point BottomLeft => new Point(unit.Location.X - (UnitSize.Width / 20) * 10, unit.Location.Y + (UnitSize.Height / 20) * 10);
        public Point BottomRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10, unit.Location.Y + (UnitSize.Height / 20) * 10);

        public readonly Sprite Sprite;

        private readonly UnitClass unit;
        private object locker = new object();

        public UnitView(UnitClass unit, string path)
        {
            this.unit = unit;
            Sprite = new Sprite(path);
        }

        public void DrawUnit(Graphics window, Point screenCenter, Rectangle focusScreen)
        {
            lock (locker)
            {
                var position = ToPointInWindow(focusScreen, screenCenter);
                window.DrawImage(Sprite.CurrentSprite, position);
                if (unit.IsAlive)
                    DrawHpBar(window, screenCenter, focusScreen);
            }
        }

        private void DrawHpBar(Graphics window, Point screenCenter, Rectangle focusScreen)
        {
            var position = ToPointInWindow(focusScreen, screenCenter);
            position = new Point(position.X + 2*Sprite.CurrentSprite.Width / 10,
                position.Y + 2 * Sprite.CurrentSprite.Height / 10);
            window.DrawString(unit.Attributes.Health.ToString(), GameSettings.ButtonsFont, Brushes.Black, position);
        }

        public void PlayAnimation(object sender, ElapsedEventArgs e)
        {
            lock (locker)
            {
                Sprite.UpdateSprite();
                if (!unit.IsAlive)
                {
                    Sprite.CurrentAnimationState = AnimationState.Death;
                    if (Sprite.CurrentFrame == 3)
                        Sprite.CanUpdateFrame = false;
                }
                else
                {
                    if (unit.CurrentDirection == Direction.Left)
                        Sprite.RotateSprite();
                    if (Sprite.CurrentAnimationState == AnimationState.Attack && Sprite.CurrentFrame == 3)
                        Sprite.CurrentAnimationState = AnimationState.Stand;
                }
            }
        }

        private Point ToPointInWindow(Rectangle focusScreen, Point screenCenter)
        {
            lock (locker)
            {
                var focusScreenCenter = new Point(focusScreen.Right - focusScreen.Width / 2,
                    focusScreen.Bottom - focusScreen.Height / 2);
                var distanceToUnit = new Point(focusScreenCenter.X - unit.Location.X,
                    focusScreenCenter.Y - unit.Location.Y);
                return new Point(screenCenter.X - distanceToUnit.X - 6 * Sprite.CurrentSprite.Width / 10 - 5,
                    screenCenter.Y - distanceToUnit.Y - 6 * Sprite.CurrentSprite.Height / 10 - 20);
            }
        }
    }
}
