using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            var table = new TableLayoutPanel();
            table.Size = this.Size;
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));

            var nameLabel = new Label
            {
                Text = "MyRPGGame",
                Dock = DockStyle.Fill
            };
            var playButton = new Button();
            playButton.Dock = DockStyle.Fill;
            var settingsButton = new Button();
            var exitButton = new Button();

            table.Controls.Add(nameLabel, 1, 0);
            table.Controls.Add(playButton, 1, 1);
            table.Controls.Add(settingsButton, 1, 2);
            table.Controls.Add(exitButton, 1, 3);

            table.Dock = DockStyle.Fill;

            Controls.Add(table);
        }
    }
}
