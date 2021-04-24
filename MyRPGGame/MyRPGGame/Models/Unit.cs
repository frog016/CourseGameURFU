using System;
using System.Drawing;

namespace MyRPGGame
{
    public class Unit
    {
        public readonly Swordsman UnitType;
        public readonly UnitView UnitModel;
        
        public Unit(Swordsman unitType, Image unitModel)
        {
            UnitType = unitType;
            UnitModel = new UnitView(unitType, unitModel);
        }

        public void SetUnitBorderState(Map map, bool state)
        {
            for (var i = 0; i < UnitView.UnitSize.Height / 10; i++)
            for (var j = 0; j < UnitView.UnitSize.Width / 10; j++)
                map.CellMap[UnitModel.TopLeft.X / 10 + j, UnitModel.TopLeft.Y / 10 + i] = state;
        }

        public void MoveBorder(Map map, Vector direction)
        {
            var border = direction.X != 0 ? UnitView.UnitSize.Height / 10 : UnitView.UnitSize.Width / 10;
            var sign = new Point(direction.X != 0 ? Math.Sign(direction.X) : 0, direction.Y != 0 ? Math.Sign(direction.Y) : 0);
            var backSide = direction.X != 0 ? sign.X == 1 ? UnitModel.TopLeft : UnitModel.TopRight : sign.Y == 1 ? UnitModel.TopLeft : UnitModel.BottomLeft;
            var frontSide = direction.X != 0 ? sign.X == -1 ? UnitModel.TopLeft : UnitModel.TopRight : sign.Y == 1 ? UnitModel.BottomLeft : UnitModel.TopLeft;

            for (var i = 0; i < border; i++)
            {
                var side = new Point(direction.X != 0 ? 0 : i, direction.Y != 0 ? 0 : i);
                map.CellMap[backSide.X / Map.CellSize + side.X * sign.X + side.X, backSide.Y / Map.CellSize + side.Y * sign.Y + side.Y] = false;
                map.CellMap[frontSide.X / Map.CellSize + sign.X + side.X, frontSide.Y / Map.CellSize + sign.Y + side.Y] = true;
            }

        }
    }
}
