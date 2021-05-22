using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class BattleGround : Form
    {
        private readonly Map map;
        private readonly Size visibleArea = new Size(400, 400);
        private Point screenCenter => new Point(Width/2, Height/2);
        private Rectangle focusScreen => new Rectangle(new Point(map.Player.UnitClass.Location.X - visibleArea.Width/2, map.Player.UnitClass.Location.Y - visibleArea.Height / 2), visibleArea);

        public BattleGround()
        {
            map = new Map("Map1");

            InitializeComponent();
            SetAllTimers();

            Paint += DrawAllUnits;
            Paint += InitializeInterface;
            KeyDown += PressKeyDown;
        }

        public void DrawAllUnits(object sender, PaintEventArgs e)
        {
            if (!map.Player.UnitClass.IsAlive)
            {
                var loseForm = new LoseForm();
                Hide();
                loseForm.Show();
            }

            DoubleBuffered = true;
            map.Player.Model.DrawUnit(e.Graphics, screenCenter);
            map.Player.Model.DrawHpBar(e.Graphics);
            foreach (var unit in map.Units)
            {
                unit.Model.DrawUnit(e.Graphics, screenCenter);
                unit.Model.DrawHpBar(e.Graphics);
            }

            DrawMap(e.Graphics);
            //DrawTestUnits(e.Graphics);
            DrawSegmentOfMap(e.Graphics);
            Invalidate();
        }

        private void SetAllTimers()
        {
            var movePerSecond = new System.Timers.Timer(500);
            movePerSecond.AutoReset = true;
            movePerSecond.Elapsed += StartAllEnemies;
            movePerSecond.Start();
        }

        private void StartAllEnemies(object sender, ElapsedEventArgs e)
        {
            var units = map.Units.ToList();
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
