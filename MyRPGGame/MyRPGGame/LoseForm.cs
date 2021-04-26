using System.Drawing;
using System.Windows.Forms;

namespace MyRPGGame
{
    public partial class LoseForm : Form
    {
        public LoseForm()
        {
            InitializeComponent();

            Paint += (sender, args) => args.Graphics.DrawString("You lose", new Font("Arial", 10), Brushes.Black, Size.Width/2, Size.Height / 2);
            FormClosed += (sender, args) => Application.Exit();
        }
    }
}
