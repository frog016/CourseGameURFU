using System;
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
            Player = new Unit(new Swordsman(1, new Vector(200, 200)), null);
            Units = new List<Unit>();
            CellMap = new bool[countCellsWidth, countCellsHeight];
            MapCountCells = new Size(countCellsWidth, countCellsHeight);
            CreateEnemy();
            InitializeMap();
        }

        public void CreateEnemy()
        {
            for (var i = 0; i < 1; i++)
            {
                var enemy = new Swordsman(3, new Vector(270, 200));
                Units.Add(new Unit(enemy, null));
            }
        }

        private void InitializeMap()
        {
            Player.SetUnitBorderState(this, true);
            foreach (var enemy in Units)
                enemy.SetUnitBorderState(this, true);
        }
    }
}
