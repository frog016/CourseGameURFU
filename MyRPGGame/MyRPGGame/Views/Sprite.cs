using System.Drawing;

namespace MyRPGGame
{
    public class Sprite
    {
        private int currentFrame;
        public int CurrentFrame
        {
            get => currentFrame;
            set => currentFrame = value < 4 ? value : 0;
        }
        public bool CanUpdateFrame;

        public Image CurrentSprite { get; private set; }
        public AnimationState CurrentAnimationState { get; set; }

        private readonly Sprites allUnitSprites;
        private readonly object locker;

        public Sprite(string path)
        {
            locker = new object();
            allUnitSprites = new Sprites(path);
            CanUpdateFrame = true;
            CurrentAnimationState = AnimationState.Stand;
            CurrentSprite = allUnitSprites.GetSprite(CurrentAnimationState, 0);
        }

        public void UpdateSprite()
        {
            lock (locker)
            {
                CurrentSprite = allUnitSprites.GetSprite(CurrentAnimationState, CurrentFrame); 
                if (CanUpdateFrame) 
                    CurrentFrame++;
            }
        }

        public void RotateSprite()
        {
            lock (locker)
            {
                CurrentSprite = allUnitSprites.RotateImage(CurrentSprite);
            }
        }
    }
}
