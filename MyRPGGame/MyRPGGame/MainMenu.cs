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

            var nameLabel = new Label
            {
                Text = "MyRPGGame",
                Dock = DockStyle.Fill
            };
            var playButton = new Button
            {
                Dock = DockStyle.Fill
            };
            var settingsButton = new Button
            {
                Dock = DockStyle.Fill
            };
            var exitButton = new Button
            {
                Dock = DockStyle.Fill
            };

            var table = new TableLayoutPanel();
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 40));

            table.Controls.Add(playButton, 1, 1);
            table.Controls.Add(settingsButton, 1, 2);
            table.Controls.Add(exitButton, 1, 3);

            table.Dock = DockStyle.Fill;
            Controls.Add(table);
        }
    }
}
