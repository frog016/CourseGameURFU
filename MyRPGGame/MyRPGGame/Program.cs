using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MyRPGGame
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BattleGround
            {
                Size = new Size(1280, 720),
                MinimumSize = new Size(450, 450)
        });
        }
    }
}
