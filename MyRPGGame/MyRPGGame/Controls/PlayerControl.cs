using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class PlayerControl : IControl
    {
        private readonly Map map;
        private readonly int speed = Map.CellSize;

        public PlayerControl(Map map)
        {
            this.map = map;
        }

        public void TryAttack(Keys key)
        {
            if (!map.Player.UnitClass.IsAlive)
                return;
            var skillKeys = DefineSkillKeys();
            if (!skillKeys.ContainsKey(key))
                return;
            if (!CheckUnitFront(-2, 2))
                return;
            var enemy = FindUnitInAttackRange();
            if (enemy == null) return;

            if (skillKeys[key](enemy))
            {
                map.Player.Model.Sprite.CurrentAnimationState = AnimationState.Attack;
                map.Player.Model.Sprite.CurrentFrame = 0;
            }
            if (!enemy.UnitClass.IsAlive)
            {
                enemy.Model.Sprite.CurrentAnimationState = AnimationState.Death;
                enemy.Model.Sprite.CurrentFrame = 0;
                enemy.SetUnitBorderState(map, false);
            }
        }

        private bool CheckUnitFront(int segmentStart, int segmentEnd)
        {
            var rangeCellX = map.Player.UnitClass.CurrentDirection == Direction.Right ?
                map.Player.Model.TopRight.X / Map.CellSize + map.Player.UnitClass.Attributes.AttackRange :
                map.Player.Model.TopLeft.X / Map.CellSize - map.Player.UnitClass.Attributes.AttackRange;

            if (!map.UnitIsOnMap(new Vector(rangeCellX*10, map.Player.UnitClass.Location.Y)))
                return false;

            for (var i = segmentStart; i <= segmentEnd; i++)
                if (!map.CellMap[rangeCellX, map.Player.UnitClass.Location.Y / Map.CellSize + i])
                    return false;

            return true;
        }

        private Unit FindUnitInAttackRange()
        {
            var currentDirection = map.Player.UnitClass.CurrentDirection == Direction.Right ? 1 : -1;
            var distance = LaunchAttackBeam(currentDirection);
            if (distance == -1)
                return null;

            foreach (var enemy in map.Units)
            {
                if (map.Player.UnitClass.Location.X + currentDirection *
                    (distance * Map.CellSize + (UnitView.UnitSize.Width / 20) * Map.CellSize) ==
                    enemy.UnitClass.Location.X - currentDirection * (UnitView.UnitSize.Width / 20) * Map.CellSize)
                    return enemy;
            }

            return null;
        }

        private int LaunchAttackBeam(int direction)
        {
            var distanceToObject = 1;
            while (distanceToObject <= map.Player.UnitClass.Attributes.AttackRange &&
                   !map.CellMap[
                       map.Player.UnitClass.Location.X / Map.CellSize + direction * (UnitView.UnitSize.Width / 20 + distanceToObject),
                       map.Player.UnitClass.Location.Y / Map.CellSize])
                distanceToObject++;

            return distanceToObject <= map.Player.UnitClass.Attributes.AttackRange ? distanceToObject : -1;
        }

        public void MoveUnit(Keys key)
        {
            if (!map.Player.UnitClass.IsAlive)
                return;
            var moveTo = DefineMoveKeys();
            if (!moveTo.ContainsKey(key))
                return;

            map.Player.UnitClass.CurrentDirection = moveTo[key].X > 0 ?
                Direction.Right :
                moveTo[key].X < 0 ? Direction.Left :
                    map.Player.UnitClass.CurrentDirection;

            map.Player.Model.Sprite.CurrentAnimationState = AnimationState.Walk;
            if (!(map.UnitIsOnMap(map.Player.UnitClass.Location + moveTo[key]) && CheckCorrectMove(moveTo[key])))
                return;

            map.Player.MoveBorder(map, moveTo[key]);
            map.Player.UnitClass.Location.Move(moveTo[key]);

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

        private Dictionary<Keys, Func<Unit, bool>> DefineSkillKeys()
        {
            return new Dictionary<Keys, Func<Unit, bool>>
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
                    map.Player.UnitClass.Location.X / Map.CellSize + sign * (UnitView.UnitSize.Width / (2 * Map.CellSize)) + sign :
                    map.Player.UnitClass.Location.X / Map.CellSize - (UnitView.UnitSize.Width / (2 * Map.CellSize)) + i;
                var y = direction.Y == 0 ?
                    map.Player.UnitClass.Location.Y / Map.CellSize - (UnitView.UnitSize.Height / (2 * Map.CellSize)) + i :
                    map.Player.UnitClass.Location.Y / Map.CellSize + sign * (UnitView.UnitSize.Height / (2 * Map.CellSize)) + sign;
                if (map.CellMap[x, y])
                    return false;
            }

            return true;
        }
    }
}
