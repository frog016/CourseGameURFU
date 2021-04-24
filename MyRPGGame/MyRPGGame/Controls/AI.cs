using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class AI : IControl
    {
        private readonly Unit unit;
        private Vector target { get; set; }
        private readonly Map map;
        private readonly int speed = Map.CellSize;

        public AI(Unit unit, Map map)
        {
            this.unit = unit;
            this.map = map;
        }

        public void TryAttack(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void MoveUnit(object sender, KeyEventArgs e)
        {
            foreach (var nearestTarget in FindPathToNearestTarget())
                unit.UnitType.Location.Move(nearestTarget - unit.UnitType.Location);
        }

        public void A(object sender, EventArgs e)
        {
            foreach (var nearestTarget in FindPathToNearestTarget())
                unit.UnitType.Location.Move(nearestTarget - unit.UnitType.Location);
        }

        List<Vector> FindPathToNearestTarget()
        {
            var path = new Dictionary<Vector, Vector> {{unit.UnitType.Location, null }};
            var visited = new HashSet<Vector> { unit.UnitType.Location };
            var queue = new Queue<Vector>();
            var otherUnit = new HashSet<Vector> {map.Player.UnitType.Location};
            foreach (var u in map.Units)
                if (u.UnitType.Location != unit.UnitType.Location)
                {
                    var leftPoint = new Vector(u.UnitType.Location.X - UnitView.UnitSize.Width - Map.CellSize,
                        u.UnitType.Location.Y);
                    var rightPoint = new Vector(u.UnitType.Location.X + UnitView.UnitSize.Width + Map.CellSize,
                        u.UnitType.Location.Y);
                    if (IsOnMap(leftPoint))
                        otherUnit.Add(leftPoint);
                    if (IsOnMap(rightPoint))
                        otherUnit.Add(rightPoint);
                }


            queue.Enqueue(unit.UnitType.Location);
            while (queue.Count != 0)
            {
                var currentPoint = queue.Dequeue();
                if (otherUnit.Contains(currentPoint))
                {
                    target = currentPoint;
                    break;
                }

                var neighbors = GetNeighbors(currentPoint)
                    .Where(point => point.X >= 0 && point.X < map.MapCountCells.Width &&
                                    point.Y >= 0 && point.Y < map.MapCountCells.Height &&
                                    !visited.Contains(point) && !map.CellMap[point.X, point.Y]);
                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    path[neighbor] = currentPoint;
                }
            }

            var pathItem = target; 
            var result = new List<Vector>();
            while (pathItem != null)
            {
                result.Add(pathItem);
                pathItem = path[pathItem];
            }
            result.Reverse();
            return result;
        }

        IEnumerable<Vector> GetNeighbors(Vector point)
        {
            var result = new List<Vector>();

            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if ((dx == 0 || dy == 0) && dx + dy != 0)
                    yield return new Vector(point.X + dx*speed, point.Y + dy* speed);
        }

        bool IsOnMap(Vector point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= map.MapCountCells.Width * Map.CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= map.MapCountCells.Height * Map.CellSize - UnitView.UnitSize.Height / 2;
        }
    }
}
