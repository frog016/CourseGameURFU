using System.Drawing;
using System.Drawing.Text;

namespace MyRPGGame
{
    public static class GameSettings
    {
        public static Size GameWindowSize;

        public static Color TextColor;
        public static Color BackgroundControlsColor;

        public static FontFamily GameFontFamily;
        public static Font ButtonsFont => new Font(GameFontFamily, 20);
        public static Font LabelFont => new Font(GameFontFamily, 50);

        static GameSettings()
        {
            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(@"..\..\Fonts\LifeCraft_Font.ttf");
            GameFontFamily = new FontFamily("LifeCraft", fontCollection);
            TextColor = Color.FromArgb(152, 61, 17);
            BackgroundControlsColor = Color.FromArgb(198, 146, 87); //252, 252, 251

            GameWindowSize = new Size(800, 600);
        }
    }
}
