using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class Table
    {
        public readonly TableLayoutPanel TablePanel;
        private readonly Dictionary<string, Action> controlActions;

        public Table(List<TableRow> rows, List<int> columnsWidth, Dictionary<string, Action> controlActions)
        {
            this.controlActions = controlActions;
            TablePanel = CreateTable(rows, columnsWidth);
        }

        private void AddActionsOnTable(List<Control> controls)
        {
            foreach (var button in controls.Where(b => controlActions.ContainsKey(b.Text)))
                button.Click += (sender, args) => controlActions[button.Text]();
        }

        private void InitializeStyleControls(List<Control> controls)
        {
            foreach (var control in controls)
            {
                control.BackColor = Color.Transparent;
                if (control is Button)
                {
                    control.Font = GameSettings.ButtonsFont;
                    control.BackColor = GameSettings.BackgroundControlsColor;
                }
                else if (control is Label)
                {
                    control.Font = GameSettings.LabelFont;
                    (control as Label).TextAlign = ContentAlignment.MiddleCenter;
                }

                control.Dock = DockStyle.Fill;
                control.ForeColor = GameSettings.TextColor;
            }
        }

        private TableLayoutPanel CreateTable(List<TableRow> rows, List<int> columnsWidth)
        {
            var table = new TableLayoutPanel();
            table.BackColor = Color.Transparent;
            table.Font = GameSettings.ButtonsFont;
            AddActionsOnTable(rows.SelectMany(row => row.Controls).ToList());

            foreach (var columnWidth in columnsWidth)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));

            foreach (var row in rows)
                row.CreateRow(table);

            InitializeStyleControls(rows.SelectMany(row => row.Controls).ToList());
            table.Dock = DockStyle.Fill;
            return table;
        }
    }
}
