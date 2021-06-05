using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class AI : IControl
    {
        private readonly UnitClass unitClass;
        private Unit unit => map.Units.Find(u => u.UnitClass == unitClass);
        private Unit Target { get; set; }
        private readonly Map map;
        private readonly int speed = Map.CellSize;

        public AI(Map map, UnitClass unitClass)
        {
            this.map = map;
            this.unitClass = unitClass;
        }

        public void TryAttack(Keys key)
        {
            if (!unit.UnitClass.IsAlive)
                return;
            FindNearestTarget();
            if (Target == null)
                return;

            if (CheckTargetInAttackRange())
            {
                if (unitClass.UseSkill(0, Target))
                    unit.Model.Sprite.CurrentAnimationState = AnimationState.Attack;
                if (!Target.UnitClass.IsAlive)
                {
                    Target.Model.Sprite.CurrentAnimationState = AnimationState.Death;
                    Target.Model.Sprite.CurrentFrame = 0;
                    Target.SetUnitBorderState(map, false);
                }
                return;
            }
            MoveUnit(key);
        }

        public void MoveUnit(Keys key)
        {
            if (unit != null)
                unit.Model.Sprite.CurrentAnimationState = AnimationState.Walk;
            var leftPoint = new Vector(Target.UnitClass.Location.X - UnitView.UnitSize.Width - Map.CellSize,
                Target.UnitClass.Location.Y);
            var rightPoint = new Vector(Target.UnitClass.Location.X + UnitView.UnitSize.Width + Map.CellSize,
                Target.UnitClass.Location.Y);

            var target = (unitClass.Location - leftPoint).Length() <= (unitClass.Location - rightPoint).Length() ? leftPoint : rightPoint;
            var neighbor = GetNeighbors(unitClass.Location)
                .OrderBy(s => ((target - s).Length(), s.X))
                .Where(v => CheckCorrectMove(v - unitClass.Location))
                .FirstOrDefault();
            var direction = neighbor - unitClass.Location;
            unitClass.CurrentDirection = direction.X > 0 ?
                Direction.Right :
                direction.X < 0 ? Direction.Left :
                    unitClass.CurrentDirection;
            if (map.UnitIsOnMap(neighbor) && CheckCorrectMove(direction))
            {
                unit.MoveBorder(map, direction);
                unitClass.Location.Move(direction);
            }
        }

        private void FindNearestTarget()
        {
            var units = map.Units.ToList();
            units.Add(map.Player);
            Target = units.Where(u => u.UnitClass != unitClass && u.UnitClass.IsAlive)
                .OrderBy(u =>
                { 
                    var unit = u.UnitClass; 
                    return (unit.Location - this.unitClass.Location).Length();
                })
                .FirstOrDefault();
        }

        private bool CheckTargetInAttackRange()
        {
            var currentDirection = unitClass.CurrentDirection == Direction.Right ? 1 : -1;
            var distance = LaunchAttackBeam(currentDirection);
            if (distance == -1)
                return false;

            if (unitClass.Location.X + currentDirection * (distance * Map.CellSize + (UnitView.UnitSize.Width / 20) * Map.CellSize) ==
                Target.UnitClass.Location.X - currentDirection * (UnitView.UnitSize.Width / 20) * Map.CellSize && CheckUnitFront(-2, 2))
                return true;

            return false;
        }

        private bool CheckUnitFront(int segmentStart, int segmentEnd)
        {
            var rangeCellX = unitClass.CurrentDirection == Direction.Right ?
                (unitClass.Location.X + (UnitView.UnitSize.Width/20)*10) / Map.CellSize + unitClass.Attributes.AttackRange :
                (unitClass.Location.X - (UnitView.UnitSize.Width / 20) * 10) / Map.CellSize - unitClass.Attributes.AttackRange;

            if (!map.UnitIsOnMap(new Vector(rangeCellX * 10, unitClass.Location.Y)))
                return false;

            for (var i = segmentStart; i <= segmentEnd; i++)
                if (!map.CellMap[rangeCellX, unitClass.Location.Y / Map.CellSize + i])
                    return false;

            return true;
        }

        private int LaunchAttackBeam(int direction)
        {
            var distanceToObject = 1;
            while (distanceToObject <= unitClass.Attributes.AttackRange &&
                   map.UnitIsOnMap(new Vector(unitClass.Location.X + direction * ((UnitView.UnitSize.Width / 20)*10 + distanceToObject),
                       unitClass.Location.Y)) &&
                   !map.CellMap[unitClass.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       unitClass.Location.Y / Map.CellSize])
                distanceToObject++;

            return distanceToObject <= unitClass.Attributes.AttackRange ? distanceToObject : -1;
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
                    unitClass.Location.X / Map.CellSize + sign * (UnitView.UnitSize.Width / (2 * Map.CellSize)) + sign :
                    unitClass.Location.X / Map.CellSize - (UnitView.UnitSize.Width / (2 * Map.CellSize)) + i;
                var y = direction.Y == 0 ?
                    unitClass.Location.Y / Map.CellSize - (UnitView.UnitSize.Height / (2 * Map.CellSize)) + i :
                    unitClass.Location.Y / Map.CellSize + sign * (UnitView.UnitSize.Height / (2 * Map.CellSize)) + sign;
                if (map.UnitIsOnMap(new Vector(x, y)) && map.CellMap[x, y])
                    return false;
            }

            return true;
        }
    }
}
