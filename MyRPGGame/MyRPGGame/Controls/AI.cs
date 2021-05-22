using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class AI : IControl
    {
        private readonly UnitClass unit;
        private Unit Target { get; set; }
        private readonly Map map;
        private readonly int speed = Map.CellSize;

        public AI(Map map, UnitClass unit)
        {
            this.map = map;
            this.unit = unit;
        }

        public void TryAttack(Keys key)
        {
            FindNearestTarget();
            if (Target == null)
                return;

            if (CheckTargetInAttackRange())
            {
                unit.UseSkill(0, Target);
                if (!Target.UnitClass.IsAlive)
                {
                    Target.SetUnitBorderState(map, false);
                    map.Units.Remove(Target);
                }
                return;
            }
            MoveUnit(key);
        }

        public void MoveUnit(Keys key)
        {
            var leftPoint = new Vector(Target.UnitClass.Location.X - UnitView.UnitSize.Width - Map.CellSize,
                Target.UnitClass.Location.Y);
            var rightPoint = new Vector(Target.UnitClass.Location.X + UnitView.UnitSize.Width + Map.CellSize,
                Target.UnitClass.Location.Y);

            var target = (unit.Location - leftPoint).Length() < (unit.Location - rightPoint).Length() ? leftPoint : rightPoint;
            var neighbor = GetNeighbors(unit.Location)
                .OrderBy(s => ((target - s).Length(), s.X))
                .Where(v => CheckCorrectMove(v - unit.Location))
                .FirstOrDefault();
            var direction = neighbor - unit.Location;
            unit.CurrentDirection = direction.X > 0 ?
                Direction.Right :
                direction.X < 0 ? Direction.Left :
                    unit.CurrentDirection;
            if (map.UnitIsOnMap(neighbor) && CheckCorrectMove(direction))
            {
                map.Units.Find(u => u.UnitClass == unit).MoveBorder(map, direction);
                unit.Location.Move(direction);
            }
        }

        private void FindNearestTarget()
        {
            var units = map.Units.ToList();
            units.Add(map.Player);
            Target = units.Where(u => u.UnitClass != unit)
                .OrderBy(u =>
                { 
                    var unit = (UnitClass) u.UnitClass; 
                    return (unit.Location - this.unit.Location).Length();
                })
                .FirstOrDefault();
        }

        private bool CheckTargetInAttackRange()
        {
            var currentDirection = unit.CurrentDirection == Direction.Right ? 1 : -1;
            var distance = LaunchAttackBeam(currentDirection);
            if (distance == -1)
                return false;

            if (unit.Location.X + currentDirection * (distance * Map.CellSize + (UnitView.UnitSize.Width / 20) * Map.CellSize) ==
                Target.UnitClass.Location.X - currentDirection * (UnitView.UnitSize.Width / 20) * Map.CellSize && CheckUnitFront(-2, 2))
                return true;

            return false;
        }

        private bool CheckUnitFront(int segmentStart, int segmentEnd)
        {
            var rangeCellX = unit.CurrentDirection == Direction.Right ?
                (unit.Location.X + (UnitView.UnitSize.Width/20)*10) / Map.CellSize + unit.Attributes.AttackRange :
                (unit.Location.X - (UnitView.UnitSize.Width / 20) * 10) / Map.CellSize - unit.Attributes.AttackRange;

            if (!map.UnitIsOnMap(new Vector(rangeCellX * 10, unit.Location.Y)))
                return false;

            for (var i = segmentStart; i <= segmentEnd; i++)
                if (!map.CellMap[rangeCellX, unit.Location.Y / Map.CellSize + i])
                    return false;

            return true;
        }

        private int LaunchAttackBeam(int direction)
        {
            var distanceToObject = 1;
            while (distanceToObject <= unit.Attributes.AttackRange &&
                   map.UnitIsOnMap(new Vector(unit.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       unit.Location.Y / Map.CellSize)) &&
                   !map.CellMap[unit.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       unit.Location.Y / Map.CellSize])
                distanceToObject++;

            return distanceToObject <= unit.Attributes.AttackRange ? distanceToObject : -1;
        }

        private List<Vector> GetNeighbors(Vector point)
        {
            var result = new List<Vector>();

            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if ((dx == 0 || dy == 0) && dx + dy != 0)
                    result.Add(new Vector(point.X + dx*speed, point.Y + dy* speed));

            return result;
        }

        private bool CheckCorrectMove(Vector direction)
        {
            var border = direction.X != 0 ? UnitView.UnitSize.Height / 10 : UnitView.UnitSize.Width / 10;
            var sign = Math.Sign(direction.X != 0 ? direction.X : direction.Y);
            for (var i = 0; i < border; i++)
            {
                var x = direction.X != 0 ?
                    unit.Location.X / Map.CellSize + sign * (UnitView.UnitSize.Width / (2 * Map.CellSize)) + sign :
                    unit.Location.X / Map.CellSize - (UnitView.UnitSize.Width / (2 * Map.CellSize)) + i;
                var y = direction.Y == 0 ?
                    unit.Location.Y / Map.CellSize - (UnitView.UnitSize.Height / (2 * Map.CellSize)) + i :
                    unit.Location.Y / Map.CellSize + sign * (UnitView.UnitSize.Height / (2 * Map.CellSize)) + sign;
                if (map.CellMap[x, y])
                    return false;
            }

            return true;
        }
    }
}
