using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class MainMenu : Form
    {
        private readonly List<Image> Logos;

        private readonly TableLayoutPanel menuTable;
        private readonly TableLayoutPanel characterTable;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ExStyle |= 0x02000000;
                return param;
            }
        }

        public MainMenu()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ClientSize = GameSettings.GameWindowSize;
            BackgroundImage = new Bitmap(Image.FromFile(@"..\..\Sprites\Background\MenuBackground.png"), GameSettings.GameWindowSize);

            Logos = LoadLogos();
            var menuRows = new List<TableRow>
            {
                new TableRow(0, 0, 2, 25, new List<Control> {new Label {Text = "Arena Battle"}}, true),
                new TableRow(1, 0, 2, 9, new List<Control> {new Panel()}, true),
                new TableRow(2, 1, 1, 10, new List<Control> { new Button { Text = "Play"}}),
                new TableRow(3, 1, 1, 10, new List<Control> { new Button { Text = "Exit"}}),
                new TableRow(4, 0, 2, 26, new List<Control> {new Panel()}, true),
            };

            var characterRows = new List<TableRow>
            {
                new TableRow(0, 0, 2, 11, new List<Control> {new Panel()}, true),
                new TableRow(1, 0, 2, 52, new List<Control> {new PictureBox(), new PictureBox(), new PictureBox()}),
                new TableRow(2, 0, 2, 10, new List<Control>
                {
                    new Button {Text = nameof(Swordsman) }, 
                    new Button {Text = nameof(Guard) }, 
                    new Button {Text = nameof(Rogue) }
                }),
                new TableRow(3, 0, 2, 5, new List<Control> {new Panel()}, true),
                new TableRow(4, 1, 1, 10, new List<Control> {new Button {Text = "Back"}}),
                new TableRow(5, 0, 2, 12, new List<Control> {new Panel()}, true),
            };

            menuTable = new Table(menuRows, new List<int> { 40, 20, 40 }, new Dictionary<string, Action>
            {
                ["Play"] = () => Show(menuTable, characterTable),
                ["Exit"] = () => Application.Exit(),
            }).TablePanel;

            characterTable = new Table(characterRows, Enumerable.Repeat(33, 3).ToList(), new Dictionary<string, Action>
            {
                ["Back"] = () => Show(characterTable, menuTable),
                [nameof(Swordsman)] = () => StartGame(new Swordsman(Vector.Zero)),
                [nameof(Guard)] = () => StartGame(new Guard(Vector.Zero)),
                [nameof(Rogue)] = () => StartGame(new Rogue(Vector.Zero)),
            }).TablePanel;


            Controls.Add(characterTable);
            Controls.Add(menuTable);
            characterTable.Hide();

            SetDoubleBuffered(characterTable);
            SetDoubleBuffered(menuTable);

            Load += (sender, args) => OnClientSizeChanged(args);
            ClientSizeChanged += (sender, args) =>
            {
                for (var i = 0; i < 3; i++)
                {
                    var pictureBox = characterRows[1].Controls[i] as PictureBox;
                    pictureBox.Image = new Bitmap(Logos[i], pictureBox.Size);
                }
            };

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
        }

        private void StartGame(UnitClass player)
        {
            var game = new BattleGround(player);
            Close();
            game.Show();
        }

        private void Show(TableLayoutPanel closingTable, TableLayoutPanel openingTable)
        {
            closingTable.Hide();
            openingTable.Show();
        }


        public void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;
            PropertyInfo property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(control, true, null);
        }
        
        private List<Image> LoadLogos()
        {
            var images = new List<Image>();
            var characters = new List<UnitClass> { new Swordsman(Vector.Zero), new Guard(Vector.Zero), new Rogue(Vector.Zero) };
            foreach (var character in characters)
            {
                var path = @"..\..\Sprites\ClassLogo\" + character.GetType().Name + "Logo.png";
                images.Add(Image.FromFile(path));
            }

            return images;
        }
    }
}
