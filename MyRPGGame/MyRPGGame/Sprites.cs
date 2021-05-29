using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyRPGGame
{
    public static class Sprites
    {
        public static readonly Dictionary<AnimationState, List<Image>> WarriorSprites; // Поставить тут лок
        public static readonly Dictionary<AnimationState, List<Image>> RogueSprites;
        public static readonly Dictionary<AnimationState, List<Image>> GuardSprites;

        private static object locker = new object();

        static Sprites()
        {
            WarriorSprites = new Dictionary<AnimationState, List<Image>>();
            RogueSprites = new Dictionary<AnimationState, List<Image>>();
            GuardSprites = new Dictionary<AnimationState, List<Image>>();

            LoadSpritesForm(@"..\..\Sprites\WarriorSprites", WarriorSprites);
            //LoadSpritesForm(@"..\..\Sprites\", RogueSprites);
            //LoadSpritesForm(@"..\..\Sprites\", GuardSprites);
        }

        public static Image GetCopyWarriorSprite(AnimationState animationState, int index)
        {
            lock (locker)
            {
                return (Image)WarriorSprites[animationState][index].Clone();
            }
        }

        public static Image GetCopyRogueSprite(AnimationState animationState, int index)
        {
            lock (locker)
            {
                return (Image)RogueSprites[animationState][index].Clone();
            }
        }

        public static Image GetCopyGuardSprite(AnimationState animationState, int index)
        {
            lock (locker)
            {
                return (Image)GuardSprites[animationState][index].Clone();
            }
        }

        public static Image RotateImage(Image sprite)
        {
            lock (locker)
            {
                var copySprite = (Image) sprite.Clone();
                copySprite.RotateFlip(RotateFlipType.Rotate180FlipY);
                return copySprite;
            }
        }

        private static void LoadSpritesForm(string path, Dictionary<AnimationState, List<Image>> currentSprites)
        {
            var pathBuilder = new StringBuilder(path);
            var spriteStates = new List<AnimationState> { AnimationState.Stand, AnimationState.Walk, AnimationState.Attack, AnimationState.Death };
            foreach (var spriteState in spriteStates)
            {
                var nameOfState = spriteState.ToString();
                pathBuilder.Append(@"\" + nameOfState);
                var spritesImage = new List<Image>();
                for (var i = 1; i <= 4; i++)
                {
                    pathBuilder.Append(i + ".png");
                    spritesImage.Add(Image.FromFile(pathBuilder.ToString()));
                    pathBuilder.Remove(pathBuilder.Length - i / 10 - 5, i/10 + 5);
                }
                currentSprites.Add(spriteState, spritesImage);
                pathBuilder.Remove(pathBuilder.Length - nameOfState.Length - 1, nameOfState.Length + 1);
            }
        }
    }
}
