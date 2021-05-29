using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MyRPGGame
{
    public class UnitView
    {
        public AnimationState CurrentAnimationState { get; set; }

        public static readonly Size UnitSize = new Size(5 * Map.CellSize, 7 * Map.CellSize);

        public Point TopLeft => new Point(unit.Location.X - (UnitSize.Width / 20) * 10,
            unit.Location.Y - (UnitSize.Height / 20) * 10);

        public Point TopRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10,
            unit.Location.Y - (UnitSize.Height / 20) * 10);

        public Point BottomLeft => new Point(unit.Location.X - (UnitSize.Width / 20) * 10,
            unit.Location.Y + (UnitSize.Height / 20) * 10);

        public Point BottomRight => new Point(unit.Location.X + (UnitSize.Width / 20) * 10,
            unit.Location.Y + (UnitSize.Height / 20) * 10);

        public int CurrentFrame
        {
            get => currentFrame;
            set => currentFrame = value < 4 ? value : 0;
        }

        private int currentFrame;
        public bool CanUpdateFrame;
        private readonly UnitClass unit;
        public Image currentSprite { get; private set; }
        private object locker = new object();

        public UnitView(UnitClass unit, Image currentSprite)
        {
            CanUpdateFrame = true;
            this.unit = unit;
            this.currentSprite = currentSprite;
            CurrentAnimationState = AnimationState.Stand;
        }

        public void DrawUnit(Graphics window, Point screenCenter, Rectangle focusScreen, Size mapSize)
        {
            lock (locker)
            {
                var position = ToPointInWindow(focusScreen, screenCenter);
                if (currentSprite != null)
                        window.DrawImage(currentSprite, position);
            }
        }

        public void DrawHpBar(Graphics window)
        {
            window.DrawString(unit.Attributes.Health.ToString(), new Font(new FontFamily("Arial"), 10), new HatchBrush(HatchStyle.Cross, Color.Black), new PointF(TopLeft.X, TopLeft.Y - 10));
        }

        public void PlayAnimation(object sender, ElapsedEventArgs e)
        {
            lock (locker)
            {
                currentSprite = Sprites.GetCopyWarriorSprite(CurrentAnimationState, CurrentFrame);
                UpdateFrame();
                if (!unit.IsAlive)
                {
                    CurrentAnimationState = AnimationState.Death;
                    if (CurrentFrame == 3)
                        CanUpdateFrame = false;
                }
                else
                {
                    if (unit.CurrentDirection == Direction.Left)
                        currentSprite =
                            Sprites.RotateImage(Sprites.GetCopyWarriorSprite(CurrentAnimationState, CurrentFrame));
                    if (CurrentAnimationState == AnimationState.Attack && CurrentFrame == 3)
                        CurrentAnimationState = AnimationState.Stand;
                }
            }
        }

        private void UpdateFrame()
        {
            lock (locker)
            {
                if (CanUpdateFrame) CurrentFrame++;
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
                return new Point(screenCenter.X - distanceToUnit.X - 6 * currentSprite.Width / 10 - 5,
                    screenCenter.Y - distanceToUnit.Y - 6 * currentSprite.Height / 10 - 20);
            }
        }
    }
}
