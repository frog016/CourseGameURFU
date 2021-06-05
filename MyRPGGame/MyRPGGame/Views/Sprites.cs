using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyRPGGame
{
    public class Sprites
    {
        private readonly Dictionary<AnimationState, List<Image>> UnitSprites;
        private readonly object locker = new object();

        public Sprites(string path)
        {
            UnitSprites = new Dictionary<AnimationState, List<Image>>();
            LoadSpritesForm(path);
        }

        public Image GetSprite(AnimationState animationState, int index)
        {
            lock (locker)
            {
                if (!UnitSprites.ContainsKey(animationState) || UnitSprites[animationState].Count < index || index < 0)
                    throw new ArgumentException();
                return (Image)UnitSprites[animationState][index].Clone();
            }
        }

        public Image RotateImage(Image sprite)
        {
            lock (locker)
            {
                var copySprite = (Image) sprite.Clone();
                copySprite.RotateFlip(RotateFlipType.Rotate180FlipY);
                return copySprite;
            }
        }

        private void LoadSpritesForm(string path)
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
                UnitSprites.Add(spriteState, spritesImage);
                pathBuilder.Remove(pathBuilder.Length - nameOfState.Length - 1, nameOfState.Length + 1);
            }
        }
    }
}
