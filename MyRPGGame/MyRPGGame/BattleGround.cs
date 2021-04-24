using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class BattleGround : Form
    {
        private readonly PlayerControl playerControl;
        private readonly Map map;
        //public readonly System.Timers.Timer MovePerSecond;
        //private readonly List<AI> ai;

        public BattleGround(int width, int height)
        {
            InitializeComponent();

            map = new Map(width/10, height/10);
            //ai = new List<AI>();

            //MovePerSecond = new System.Timers.Timer();
            //MovePerSecond.AutoReset = true;
            //MovePerSecond.Interval = 100;
            foreach (var m in map.Units)
            {
                var newAi = new AI(m, map);
                //ai.Add(newAi);
                //MovePerSecond.Elapsed += newAi.A;
            }
            playerControl = new PlayerControl(map.Player, map);
            //MovePerSecond.Start();

            Paint += DrawAllUnits;
            KeyDown += playerControl.MoveUnit;
            KeyDown += playerControl.TryAttack;
        }

        public void DrawAllUnits(object sender, PaintEventArgs e)
        {
            DoubleBuffered = true;
            map.Player.UnitModel.DrawUnit(e.Graphics);

            foreach (var unit in map.Units)
            {
                unit.UnitModel.DrawUnit(e.Graphics);
                unit.UnitModel.DrawHpBar(e.Graphics);
            }
            DrawMap(e.Graphics);
            DrawTestUnits(e.Graphics);

            Invalidate();
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
