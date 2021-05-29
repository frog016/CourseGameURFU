using System;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class BattleGround : Form
    {
        private readonly Map map;
        private readonly Size visibleArea = new Size(700, 500);
        private Point screenCenter => new Point(Width/2, Height/2);
        private Rectangle focusScreen => new Rectangle(new Point(map.Player.UnitClass.Location.X - visibleArea.Width/2, map.Player.UnitClass.Location.Y - visibleArea.Height / 2), visibleArea);

        private object locker = new object();

        private System.Timers.Timer animationTimer;
        private System.Timers.Timer deathPlayerTimer;

        public BattleGround()
        {
            map = new Map("Map1");
            InitializeComponent();
            SetAllTimers();

            Paint += DrawAllUnits;
            Paint += InitializeInterface;
            KeyDown += PressKeyDown;
            KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.W || args.KeyCode == Keys.A || args.KeyCode == Keys.S ||
                    args.KeyCode == Keys.D)
                    map.Player.Model.CurrentAnimationState = AnimationState.Stand;
            };
        }

        public void DrawAllUnits(object sender, PaintEventArgs e)
        {
            lock (locker)
            {
                if (!map.Player.UnitClass.IsAlive)
                    deathPlayerTimer.Start();

                DoubleBuffered = true;
                if (focusScreen.Contains(map.Player.UnitClass.Location.ToPoint()))
                    map.Player.Model.DrawUnit(e.Graphics, screenCenter, focusScreen, map.MapCountCells);
                map.Player.Model.DrawHpBar(e.Graphics);
                foreach (var unit in map.Units.ToList())
                {
                    if (focusScreen.Contains(unit.UnitClass.Location.ToPoint())) 
                        unit.Model.DrawUnit(e.Graphics, screenCenter, focusScreen, map.MapCountCells);
                }
                
                DrawMap(e.Graphics);
                DrawSegmentOfMap(e.Graphics);

                Invalidate();
            }
        }


        private void SetAllTimers()
        {
            var movePerSecond = new System.Timers.Timer(250);
            movePerSecond.AutoReset = true;
            movePerSecond.Elapsed += StartAllEnemies;
            movePerSecond.Start();

            animationTimer = new System.Timers.Timer(150);
            animationTimer.AutoReset = true;
            animationTimer.Elapsed += map.Player.Model.PlayAnimation;
            foreach (var unit in map.Units)
                animationTimer.Elapsed += unit.Model.PlayAnimation;
            animationTimer.Start();

            deathPlayerTimer = new System.Timers.Timer(4000);
            deathPlayerTimer.Elapsed += (sender, args) =>
            {
                BeginInvoke(new Action(() =>
                {
                    var loseForm = new LoseForm();
                    Hide();
                    loseForm.Show();
                }));
            };
        }

        private void StartAllEnemies(object sender, ElapsedEventArgs e)
        {
            var units = map.Units.Where(u => u.UnitClass.IsAlive).ToList();
            foreach (var unit in units)
                unit.Control.TryAttack(Keys.K);
            
        }

        private void PressKeyDown(object sender, KeyEventArgs e)
        {
            map.Player.Control.MoveUnit(e.KeyData);
            map.Player.Control.TryAttack(e.KeyData);
        }

        private void InitializeInterface(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var skillPanelSize = new Size(50, 50);
            var player = (UnitClass)map.Player.UnitClass;
            for (var i = 0; i < player.Skills.Count; i++)
            {
                var skillCell = new Rectangle(
                        new Point(Size.Width / 2 - skillPanelSize.Width + skillPanelSize.Width * i,
                            Size.Height - skillPanelSize.Height - 40), skillPanelSize);
                graphics.FillRectangle(Brushes.BurlyWood, skillCell);
                graphics.DrawRectangle(new Pen(Color.SaddleBrown, 3), skillCell);
                graphics.DrawString(player.Skills[i].Name, new Font("Arial", 10), Brushes.Black, skillCell);
            }
        }

        private void DrawSegmentOfMap(Graphics window)
        {
            var pen = new Pen(Color.Red, 1);
            var penForUnit = new Pen(Color.Black, 3);
            for (var i = focusScreen.Left; i <= focusScreen.Right; i += Map.CellSize)
                for (var j = focusScreen.Top; j <= focusScreen.Bottom; j += Map.CellSize)
                {
                    var pointInWindow = new Point(screenCenter.X - visibleArea.Width / 2 + (i - focusScreen.Left),
                        screenCenter.Y - visibleArea.Height / 2 + (j - focusScreen.Top));
                    if (map.PointIsOnMap(new Point(i, j)))
                    {
                        window.DrawEllipse(pen, pointInWindow.X, pointInWindow.Y, 1, 1);
                        if (map.CellMap[i / 10, j / 10])
                            window.DrawEllipse(penForUnit, pointInWindow.X, pointInWindow.Y, 1, 1);
                    }
                }
        }

        private Point ToPointInWindow(Rectangle focusScreen, Point screenCenter, Unit unit)
        {
            lock (locker)
            {
                var focusScreenCenter = new Point(focusScreen.Right - focusScreen.Width / 2,
                    focusScreen.Bottom - focusScreen.Height / 2);
                var distanceToUnit = new Point(focusScreenCenter.X - unit.UnitClass.Location.X,
                    focusScreenCenter.Y - unit.UnitClass.Location.Y);
                return new Point(screenCenter.X - distanceToUnit.X - 6 * unit.Model.currentSprite.Width / 10 - 5,
                    screenCenter.Y - distanceToUnit.Y - 6 * unit.Model.currentSprite.Height / 10 - 20);
            }
        }

        #region ForTestMyGame
        private void DrawMap(Graphics window) // Для тестирования игры
        {
            var pen = new Pen(Color.Aqua, 1);
            for (var i = 0; i <= visibleArea.Height / Map.CellSize; i++)
                window.DrawLine(pen, 
                    new Point(screenCenter.X - visibleArea.Width / 2, screenCenter.Y - visibleArea.Height / 2 + i * Map.CellSize),
                    new Point(screenCenter.X + visibleArea.Width / 2, screenCenter.Y - visibleArea.Height / 2 + i * Map.CellSize));
            for (var i = 0; i <= visibleArea.Width / Map.CellSize; i++)
                window.DrawLine(pen,
                    new Point(screenCenter.X - visibleArea.Width / 2 + i * Map.CellSize, screenCenter.Y - visibleArea.Height / 2),
                    new Point(screenCenter.X - visibleArea.Width / 2 + i * Map.CellSize, screenCenter.Y + visibleArea.Height / 2));
        }

        private void DrawTestUnits(Graphics window)
        {
            var pen1 = new Pen(Color.Black, 2);
            for (var i = 0; i < map.CellMap.GetLength(0); i++)
            {
                for (var j = 0; j < map.CellMap.GetLength(1); j++)
                {
                    if (map.CellMap[i, j])
                        window.DrawEllipse(pen1, i * 10, j * 10, 1, 1);
                }
            }
        }

        #endregion
    }
}
