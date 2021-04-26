using System.Collections.Generic;
using System.Drawing;

namespace MyRPGGame
{
    public class Map
    {
        public static readonly int CellSize = 10;

        public readonly Unit Player;
        public readonly List<Unit> Units;
        public readonly bool[,] CellMap;

        public readonly Size MapCountCells;

        public Map(int countCellsWidth, int countCellsHeight)
        {
            Player = new Unit(new Swordsman(14, new Vector(200, 200)), null, new PlayerControl(this));
            Units = new List<Unit>();

            CellMap = new bool[countCellsWidth, countCellsHeight];
            MapCountCells = new Size(countCellsWidth, countCellsHeight);
            CreateEnemy();
            InitializeMap();
        }

        public void CreateEnemy()
        {
            for (var i = 0; i < 2; i++)
            {
                var enemy = new Swordsman(2, new Vector(100 + (UnitView.UnitSize.Width + 200)*i, 100));
                Units.Add(new Unit(enemy, null, new AI(this, enemy)));
            }
        }

        private void InitializeMap()
        {
            Player.SetUnitBorderState(this, true);
            foreach (var unit in Units)
                unit.SetUnitBorderState(this, true);
        }

        public bool IsOnMap(Vector point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= MapCountCells.Width * CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= MapCountCells.Height * CellSize - UnitView.UnitSize.Height / 2;
        }
    }
}
