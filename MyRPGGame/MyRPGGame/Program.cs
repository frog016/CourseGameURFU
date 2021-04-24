using System;
using System.Drawing;
using System.Windows.Forms;

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
            Application.Run(new BattleGround(600, 800)
            {
                Size = new Size(1280, 720)
            });
        }
    }
}
