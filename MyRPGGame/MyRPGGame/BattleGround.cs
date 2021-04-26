using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class BattleGround : Form
    {
        private readonly Map map;

        public BattleGround(int width, int height)
        {
            InitializeComponent();

            map = new Map(width / 10, height / 10);

            SetAllTimers();

            Paint += DrawAllUnits;
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
            map.Player.Model.DrawUnit(e.Graphics);
            map.Player.Model.DrawHpBar(e.Graphics);
            foreach (var unit in map.Units)
            {
                unit.Model.DrawUnit(e.Graphics);
                unit.Model.DrawHpBar(e.Graphics);
            }

            DrawMap(e.Graphics);
            DrawTestUnits(e.Graphics);
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

        #region ForTestMyGame
        private void DrawMap(Graphics window) // Для тестирования игры
        {
            var pen = new Pen(Color.Aqua, 1);
            for (var i = 0; i <= map.MapCountCells.Height; i++)
                window.DrawLine(pen, new Point(0, i * Map.CellSize), new Point(map.MapCountCells.Width * Map.CellSize, i * Map.CellSize));
            for (var j = 0; j <= map.MapCountCells.Width; j++)
                window.DrawLine(pen, new Point(j * Map.CellSize, 0), new Point(j * Map.CellSize, map.MapCountCells.Height * Map.CellSize));
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
