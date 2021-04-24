using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class PlayerControl : IControl
    {
        private readonly Unit player;
        private readonly Map map;
        private readonly int speed = Map.CellSize;

        public PlayerControl(Unit player, Map map)
        {
            this.player = player;
            this.map = map;
        }

        public void TryAttack(object sender, KeyEventArgs e)
        {
            if (e.KeyData != BindKeyboardKeys.Attack || !CheckUnitFront(-2, 2))
                return;
            var enemy = FindUnitInAttackRange();
            if (enemy == null) return;

            if (player.UnitType.AttackCooldown.IsReady())
            {
                player.UnitType.Attack(enemy.UnitType);
                if (enemy.UnitType.Health <= 0)
                {
                    map.Units.Remove(enemy);
                    enemy.SetUnitBorderState(map, false);
                }
            }
        }

        bool CheckUnitFront(int segmentStart, int segmentEnd)
        {
            var rangeCellX = player.UnitType.CurrentDirection == Direction.Right ?
                player.UnitModel.TopRight.X / Map.CellSize + player.UnitType.AttackRange :
                player.UnitModel.TopLeft.X / Map.CellSize - player.UnitType.AttackRange;

            if (!IsOnMap(new Vector(rangeCellX, player.UnitType.Location.Y)))
                return false;

            for (var i = segmentStart; i <= segmentEnd; i++)
                if (!map.CellMap[rangeCellX, player.UnitType.Location.Y / Map.CellSize + i])
                    return false;

            return true;
        }

        Unit FindUnitInAttackRange()
        {
            var currentDirection = player.UnitType.CurrentDirection == Direction.Right ? 1 : -1;
            var distance = LaunchAttackBeam(currentDirection);
            if (distance == -1)
                return null;

            foreach (var enemy in map.Units)
                if (player.UnitType.Location.X + currentDirection * (distance * Map.CellSize + (UnitView.UnitSize.Width / 20) * Map.CellSize) ==
                    enemy.UnitType.Location.X - currentDirection * (UnitView.UnitSize.Width / 20) * Map.CellSize)
                    return enemy;

            return null;
        }

        int LaunchAttackBeam(int direction)
        {
            var distanceToObject = 1;
            while (distanceToObject <= player.UnitType.AttackRange &&
                   !map.CellMap[
                       player.UnitType.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       player.UnitType.Location.Y / Map.CellSize])
                distanceToObject++;

            return distanceToObject <= player.UnitType.AttackRange ? distanceToObject : -1;
        }

        public void MoveUnit(object sender, KeyEventArgs e)
        {
            var moveTo = DefineMoveKeys();
            if (!moveTo.ContainsKey(e.KeyData))
                return;

            player.UnitType.CurrentDirection = moveTo[e.KeyData].X > 0 ?
                Direction.Right :
                moveTo[e.KeyData].X < 0 ? Direction.Left :
                    player.UnitType.CurrentDirection;

            if (!(IsOnMap(player.UnitType.Location + moveTo[e.KeyData]) && CheckCorrectMove(moveTo[e.KeyData])))
                return;

            player.MoveBorder(map, moveTo[e.KeyData]);
            player.UnitType.Location.Move(moveTo[e.KeyData]);

        }

        Dictionary<Keys, Vector> DefineMoveKeys()
        {
            return new Dictionary<Keys, Vector>
            {
                { BindKeyboardKeys.MoveUp, new Vector(0, -speed) },
                { BindKeyboardKeys.MoveDown, new Vector(0, speed) },
                { BindKeyboardKeys.MoveLeft, new Vector(-speed, 0) },
                { BindKeyboardKeys.MoveRight, new Vector(speed, 0) }
            };
        }

        bool IsOnMap(Vector point)
        {
            return point.X >= UnitView.UnitSize.Width / 2 &&
                   point.X <= map.MapCountCells.Width * Map.CellSize - UnitView.UnitSize.Width / 2 &&
                   point.Y >= UnitView.UnitSize.Height / 2 &&
                   point.Y <= map.MapCountCells.Height * Map.CellSize - UnitView.UnitSize.Height / 2;
        }

        bool CheckCorrectMove(Vector direction)
        {
            var border = direction.X != 0 ? UnitView.UnitSize.Height / 10 : UnitView.UnitSize.Width / 10;
            var sign = Math.Sign(direction.X != 0 ? direction.X : direction.Y);
            for (var i = 0; i < border; i++)
            {
                var x = direction.X != 0 ?
                    player.UnitType.Location.X / Map.CellSize + sign * (UnitView.UnitSize.Width / (2 * Map.CellSize)) + sign :
                    player.UnitType.Location.X / Map.CellSize - (UnitView.UnitSize.Width / (2 * Map.CellSize)) + i;
                var y = direction.Y == 0 ?
                    player.UnitType.Location.Y / Map.CellSize - (UnitView.UnitSize.Height / (2 * Map.CellSize)) + i :
                    player.UnitType.Location.Y / Map.CellSize + sign * (UnitView.UnitSize.Height / (2 * Map.CellSize)) + sign;
                if (map.CellMap[x, y])
                    return false;
            }

            return true;
        }
    }
}
