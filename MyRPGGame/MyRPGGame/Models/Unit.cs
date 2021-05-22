using System;
using System.Drawing;

namespace MyRPGGame
{
    public class Unit
    {
        public readonly UnitClass UnitClass;
        public readonly UnitView Model;
        public readonly IControl Control;
        
        public Unit(UnitClass unitClass, Image unitModel, IControl control)
        {
            UnitClass = unitClass;
            Model = new UnitView((UnitClass)unitClass, unitModel);
            Control = control;
        }

        public void SetUnitBorderState(Map map, bool state)
        {
            for (var i = 0; i < UnitView.UnitSize.Height / 10; i++)
            for (var j = 0; j < UnitView.UnitSize.Width / 10; j++)
                map.CellMap[Model.TopLeft.X / 10 + j, Model.TopLeft.Y / 10 + i] = state;
        }

        public void MoveBorder(Map map, Vector direction)
        {
            var border = direction.X != 0 ? UnitView.UnitSize.Height / 10 : UnitView.UnitSize.Width / 10;
            var sign = new Point(direction.X != 0 ? Math.Sign(direction.X) : 0, direction.Y != 0 ? Math.Sign(direction.Y) : 0);
            var backSide = direction.X != 0 ? sign.X == 1 ? Model.TopLeft : Model.TopRight : sign.Y == 1 ? Model.TopLeft : Model.BottomLeft;
            var frontSide = direction.X != 0 ? sign.X == -1 ? Model.TopLeft : Model.TopRight : sign.Y == 1 ? Model.BottomLeft : Model.TopLeft;

            for (var i = 0; i < border; i++)
            {
                var side = new Point(direction.X != 0 ? 0 : i, direction.Y != 0 ? 0 : i);
                map.CellMap[backSide.X / Map.CellSize + side.X * sign.X + side.X, backSide.Y / Map.CellSize + side.Y * sign.Y + side.Y] = false;
                map.CellMap[frontSide.X / Map.CellSize + sign.X + side.X, frontSide.Y / Map.CellSize + sign.Y + side.Y] = true;
            }

        }
    }
}
