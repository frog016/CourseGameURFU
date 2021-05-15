using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class PlayerControl : IControl
    {
        private readonly Map map;
        private UnitClass player => (UnitClass)map.Player.UnitClass;
        private readonly int speed = Map.CellSize;

        public PlayerControl(Map map)
        {
            this.map = map;
        }

        public void TryAttack(Keys key)
        {
            var skillKeys = DefineSkillKeys();
            if (!skillKeys.ContainsKey(key) || !CheckUnitFront(-2, 2))
                return;
            var enemy = FindUnitInAttackRange();
            if (enemy == null) return;

            var enemyUnitClass = (UnitClass) enemy.UnitClass;
            skillKeys[key](enemy);
            if (enemyUnitClass.Attributes.Health <= 0)
            {
                map.Units.Remove(enemy);
                enemy.SetUnitBorderState(map, false);
            }
        }

        private bool CheckUnitFront(int segmentStart, int segmentEnd)
        {
            var rangeCellX = player.CurrentDirection == Direction.Right ?
                map.Player.Model.TopRight.X / Map.CellSize + player.Attributes.AttackRange :
                map.Player.Model.TopLeft.X / Map.CellSize - player.Attributes.AttackRange;

            if (!map.IsOnMap(new Vector(rangeCellX*10, player.Location.Y)))
                return false;

            for (var i = segmentStart; i <= segmentEnd; i++)
                if (!map.CellMap[rangeCellX, player.Location.Y / Map.CellSize + i])
                    return false;

            return true;
        }

        private Unit FindUnitInAttackRange()
        {
            var currentDirection = player.CurrentDirection == Direction.Right ? 1 : -1;
            var distance = LaunchAttackBeam(currentDirection);
            if (distance == -1)
                return null;

            foreach (var enemy in map.Units)
            {
                var enemyUnitClass = (UnitClass) enemy.UnitClass;
                if (player.Location.X + currentDirection *
                    (distance * Map.CellSize + (UnitView.UnitSize.Width / 20) * Map.CellSize) ==
                    enemyUnitClass.Location.X - currentDirection * (UnitView.UnitSize.Width / 20) * Map.CellSize)
                    return enemy;
            }

            return null;
        }

        private int LaunchAttackBeam(int direction)
        {
            var distanceToObject = 1;
            while (distanceToObject <= player.Attributes.AttackRange &&
                   !map.CellMap[
                       player.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       player.Location.Y / Map.CellSize])
                distanceToObject++;

            return distanceToObject <= player.Attributes.AttackRange ? distanceToObject : -1;
        }

        public void MoveUnit(Keys key)
        {
            var moveTo = DefineMoveKeys();
            if (!moveTo.ContainsKey(key))
                return;

            player.CurrentDirection = moveTo[key].X > 0 ?
                Direction.Right :
                moveTo[key].X < 0 ? Direction.Left :
                    player.CurrentDirection;

            if (!(map.IsOnMap(player.Location + moveTo[key]) && CheckCorrectMove(moveTo[key])))
                return;

            map.Player.MoveBorder(map, moveTo[key]);
            player.Location.Move(moveTo[key]);

        }

        private Dictionary<Keys, Vector> DefineMoveKeys()
        {
            return new Dictionary<Keys, Vector>
            {
                { BindKeyboardKeys.MoveUp, new Vector(0, -speed) },
                { BindKeyboardKeys.MoveDown, new Vector(0, speed) },
                { BindKeyboardKeys.MoveLeft, new Vector(-speed, 0) },
                { BindKeyboardKeys.MoveRight, new Vector(speed, 0) }
            };
        }

        private Dictionary<Keys, Action<Unit>> DefineSkillKeys()
        {
            return new Dictionary<Keys, Action<Unit>>
            {
                { BindKeyboardKeys.FirstSkill, unit => map.Player.UnitClass.UseSkill(0, unit) },
                { BindKeyboardKeys.SecondSkill, unit => map.Player.UnitClass.UseSkill(1, unit) }
            };
        }

        private bool CheckCorrectMove(Vector direction)
        {
            var border = direction.X != 0 ? UnitView.UnitSize.Height / 10 : UnitView.UnitSize.Width / 10;
            var sign = Math.Sign(direction.X != 0 ? direction.X : direction.Y);
            for (var i = 0; i < border; i++)
            {
                var x = direction.X != 0 ?
                    player.Location.X / Map.CellSize + sign * (UnitView.UnitSize.Width / (2 * Map.CellSize)) + sign :
                    player.Location.X / Map.CellSize - (UnitView.UnitSize.Width / (2 * Map.CellSize)) + i;
                var y = direction.Y == 0 ?
                    player.Location.Y / Map.CellSize - (UnitView.UnitSize.Height / (2 * Map.CellSize)) + i :
                    player.Location.Y / Map.CellSize + sign * (UnitView.UnitSize.Height / (2 * Map.CellSize)) + sign;
                if (map.CellMap[x, y])
                    return false;
            }

            return true;
        }
    }
}
