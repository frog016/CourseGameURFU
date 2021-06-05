using System.Collections.Generic;
using System.Windows.Forms;

namespace MyRPGGame
{
    public class TableRow
    {
        public int Index { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int Height { get; set; }
        public List<Control> Controls { get; set; }
        private bool isOneOnly { get; }

        public TableRow(int index, int startColumn, int endColumn, int height, List<Control> controls, bool isOneOnly = false)
        {
            Index = index;
            StartColumn = startColumn;
            EndColumn = endColumn;
            Height = height;
            Controls = controls;
            this.isOneOnly = isOneOnly;
        }

        public void CreateRow(TableLayoutPanel table)
        {
            table.RowStyles.Add(new RowStyle(SizeType.Percent, Height));
            for (var i = StartColumn; i <= (isOneOnly ? StartColumn : EndColumn); i++)
                table.Controls.Add(Controls[i - StartColumn], i, Index);
            if (isOneOnly)
                table.SetColumnSpan(Controls[0], EndColumn + 1);
        }
    }
}
