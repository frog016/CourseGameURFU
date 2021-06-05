using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class LoseForm : Form
    {
        public LoseForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            Font = GameSettings.ButtonsFont;
            MaximizeBox = false;
            Size = new Size(500, 250);
            BackgroundImage = new Bitmap(Image.FromFile(@"..\..\Sprites\Background\LoseBackground.png"), Size);

            var loseRows = new List<TableRow>
            {
                new TableRow(0, 1, 1, 60, new List<Control> {new Label { Text = "Game Over"}}, true),
                new TableRow(1, 1, 2, 25, new List<Control> {new Button { Text = "Menu"}, new Button { Text = "Exit" } }),
                new TableRow(2, 0, 3, 15, new List<Control> {new Panel()}, true),
            };
            var controlActions = new Dictionary<string, Action>
            {
                ["Menu"] = () =>
                {
                    var menu = new MainMenu();
                    Close();
                    menu.Show();
                },
                ["Exit"] = () => Application.Exit(),

            };
            var loseTable = new Table(loseRows, new List<int> {20, 40, 40, 20}, controlActions).TablePanel;

            Controls.Add(loseTable);
        }
    }
}
