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

        public Map(UnitClass player, string mapName)
        {
            Player = new Unit(player, @"..\..\Sprites\"+ player.GetType().Name +"Sprites", new PlayerControl(this));
            Units = new List<Unit>();
            LoadMapFromFile(mapName);
            CellMap = new bool[MapCountCells.Width, MapCountCells.Height];
            InitializeMap();
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
                        var player = Player.UnitClass;
                        player.Location = new Vector(j * 50, i * 70);
                        break;
                    case 'E':
                        var enemy = GenerateRandomUnit(new Vector(j * 50, i * 70));
                        Units.Add(new Unit(enemy, @"..\..\Sprites\" + enemy.GetType().Name + "Sprites", new AI(this, enemy)));
                        break;
                }
        }

        public bool UnitIsOnMap(Vector point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= MapCountCells.Width * CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= MapCountCells.Height * CellSize - UnitView.UnitSize.Height;
        }

        private void InitializeMap()
        {
            Player.SetUnitBorderState(this, true);
            foreach (var unit in Units)
                unit.SetUnitBorderState(this, true);
        }

        private UnitClass GenerateRandomUnit(Vector position)
        {
            var random = new Random();
            switch (random.Next(0, 3))
            {
                case 0: return new Swordsman(position);
                case 1: return new Guard(position);
                case 2: return new Rogue(position);
            }

            return null;
        }
    }
}
