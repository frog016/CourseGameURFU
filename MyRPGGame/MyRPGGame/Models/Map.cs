using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MyRPGGame
{
    public class Map
    {
        public static readonly int CellSize = 10;

        public readonly Unit Player;
        public readonly List<Unit> Units;
        public readonly bool[,] CellMap;

        public Size MapCountCells { get; private set; }

        public Map(string mapName)
        {
            Player = new Unit(new Swordsman(new Vector(0, 0)), null, new PlayerControl(this));
            Units = new List<Unit>();
            LoadMapFromFile(mapName);

            CellMap = new bool[MapCountCells.Width, MapCountCells.Height];
            InitializeMap();
        }

        private void InitializeMap()
        {
            Player.SetUnitBorderState(this, true);
            foreach (var unit in Units)
                unit.SetUnitBorderState(this, true);
        }

        public void LoadMapFromFile(string mapName)
        {
            var mapFromFile = File.ReadAllText(@"..\..\Maps\" + mapName + ".txt").Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            MapCountCells = new Size(mapFromFile[0].Length * 5, mapFromFile.GetLength(0) * 7);

            for (var i = 0; i < MapCountCells.Height / 7; i++)
            for (var j = 0; j < MapCountCells.Width / 5; j++)
                switch (mapFromFile[i][j])
                {
                    case 'P':
                        var player = (UnitClass)Player.UnitClass;
                        player.Location = new Vector(j * 50, i * 70);
                        break;
                    case 'E':
                        var enemy = new Swordsman(new Vector(j * 50, i * 70));
                        Units.Add(new Unit(enemy, null, new AI(this, enemy)));
                        break;
                }
        }

        public bool UnitIsOnMap(Vector point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= MapCountCells.Width * CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= MapCountCells.Height * CellSize - UnitView.UnitSize.Height / 2;
        }

        public bool UnitIsOnMap(Point point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= MapCountCells.Width * CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= MapCountCells.Height * CellSize - UnitView.UnitSize.Height / 2;
        }

        public bool PointIsOnMap(Point point)
        {
            return point.X > 0 &&
                   point.X < MapCountCells.Width * CellSize &&
                   point.Y > 0 &&
                   point.Y < MapCountCells.Height * CellSize;
        }
    }
}
