using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class BattleGround : Form
    {
        private readonly Map map;
        private readonly Bitmap background;
        private readonly List<Image> playerIcons;

        private readonly Size visibleArea;
        private object locker = new object();

        private System.Timers.Timer animationTimer;
        private System.Timers.Timer deathPlayerTimer;

        private Point screenCenter => new Point(Width/2, Height/2);
        private Rectangle focusScreen => new Rectangle(new Point(map.Player.UnitClass.Location.X - visibleArea.Width / 2, map.Player.UnitClass.Location.Y - visibleArea.Height / 2), visibleArea);

        public BattleGround(UnitClass player)
        {
            InitializeComponent();
            ClientSize = GameSettings.GameWindowSize;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;

            map = new Map(player,"BigMap");
            visibleArea = Size;
            background = new Bitmap(Image.FromFile(@"..\..\Sprites\Background\GameBackground.png"), new Size(map.MapCountCells.Width * 10, map.MapCountCells.Height * 10));
            playerIcons = LoadIcons(@"..\..\Sprites\Icons\");

            SetAllTimers();
            Paint += DrawAllUnits;
            Paint += InitializeInterface;
            KeyDown += PressKeyDown;
            KeyUp += (sender, args) =>
            {
                if (args.KeyCode == Keys.W || args.KeyCode == Keys.A || args.KeyCode == Keys.S ||
                    args.KeyCode == Keys.D)
                    map.Player.Model.Sprite.CurrentAnimationState = AnimationState.Stand;
            };
        }

        public void DrawAllUnits(object sender, PaintEventArgs e)
        {
            lock (locker)
            {
                DoubleBuffered = true;
                if (!map.Player.UnitClass.IsAlive)
                    deathPlayerTimer.Start();

                DrawSegmentOfMap(e.Graphics);
                foreach (var unit in map.Units.OrderBy(u => u.UnitClass.IsAlive).ToList())
                    if (focusScreen.Contains(unit.UnitClass.Location.ToPoint())) 
                        unit.Model.DrawUnit(e.Graphics, screenCenter, focusScreen);

                if (focusScreen.Contains(map.Player.UnitClass.Location.ToPoint()))
                    map.Player.Model.DrawUnit(e.Graphics, screenCenter, focusScreen);

                Invalidate();
            }
        }


        private void SetAllTimers()
        {
            var movePerSecond = new System.Timers.Timer(200);
            movePerSecond.AutoReset = true;
            movePerSecond.Elapsed += StartAllEnemies;
            movePerSecond.Start();

            animationTimer = new System.Timers.Timer(100);
            animationTimer.AutoReset = true;
            animationTimer.Elapsed += map.Player.Model.PlayAnimation;
            foreach (var unit in map.Units)
                animationTimer.Elapsed += unit.Model.PlayAnimation;
            animationTimer.Start();

            deathPlayerTimer = new System.Timers.Timer(4000);
            deathPlayerTimer.AutoReset = false;
            deathPlayerTimer.Elapsed += (sender, args) =>
            {
                BeginInvoke(new Action(() =>
                {
                    var loseForm = new LoseForm();
                    Close();
                    loseForm.Show();
                    deathPlayerTimer.Dispose();
                }));
            };
        }

        private void StartAllEnemies(object sender, ElapsedEventArgs e)
        {
            var units = map.Units.Where(u => u.UnitClass.IsAlive);
            foreach (var unit in units)
                unit.Control.TryAttack(Keys.K);
            
        }

        private void PressKeyDown(object sender, KeyEventArgs e)
        {
            map.Player.Control.MoveUnit(e.KeyData);
            map.Player.Control.TryAttack(e.KeyData);
        }

        private List<Image> LoadIcons(string pathToFolder)
        {
            var pathBuilder = new StringBuilder(pathToFolder);
            pathBuilder.Append(@"\" + map.Player.UnitClass.GetType().Name);
            var spritesImage = new List<Image>();
            for (var i = 1; i <= map.Player.UnitClass.Skills.Count; i++)
            {
                pathBuilder.Append(i + ".png");
                spritesImage.Add(Image.FromFile(pathBuilder.ToString()));
                pathBuilder.Remove(pathBuilder.Length - i / 10 - 5, i / 10 + 5);
            }

            return spritesImage;
        }

        private void InitializeInterface(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var currentTime = DateTime.Now;
            var skillPanelSize = new Size(60, 60);
            var player = map.Player.UnitClass;
            for (var i = 0; i < player.Skills.Count; i++)
            {
                var skillCell = new Rectangle(
                        new Point(Size.Width / 2 - skillPanelSize.Width + skillPanelSize.Width * i,
                            Size.Height - skillPanelSize.Height), skillPanelSize);
                var lastUsed = player.Skills[i].Cooldown.LastTimeUse;
                var cooldown = player.Skills[i].Cooldown.CooldownTime - currentTime.Subtract(lastUsed).Seconds;
                if (cooldown > player.Skills[i].Cooldown.CooldownTime || cooldown < 0)
                    cooldown = 0;
                graphics.DrawImage(playerIcons[i], skillCell);
                graphics.DrawString(cooldown < 1e-10 ? "" : Convert.ToString(cooldown, new NumberFormatInfo()), GameSettings.ButtonsFont, Brushes.Goldenrod, 
                    new Point(Size.Width / 2 - skillPanelSize.Width + skillPanelSize.Width * i - 25 + 88*i,
                    Size.Height - skillPanelSize.Height));
            }
        }

        private void DrawSegmentOfMap(Graphics window)
        {
            window.DrawImage(background,
               new Rectangle(new Point(screenCenter.X - visibleArea.Width/2, screenCenter.Y - visibleArea.Height / 2), visibleArea), focusScreen, GraphicsUnit.Pixel);
        }
    }
}
