using NUnit.Framework;

namespace MyRPGGame.Tests
{
    [TestFixture]
    public class Sprite_Test
    {
        [Test]
        public void TestLoadSprites()
        {
            Assert.Catch(() => new Sprites(""));
            Assert.DoesNotThrow(() => new Sprites(@"..\..\Sprites\RogueSprites\"));
            Assert.NotNull(new Sprites(@"..\..\Sprites\RogueSprites\").GetSprite(AnimationState.Stand, 0));
            Assert.Catch(() => new Sprites(@"..\..\Sprites\RogueSprites\").GetSprite(AnimationState.Stand, 111));
        }

        [Test]
        public void TestDifferentSprites()
        {
            var sprites = new Sprites(@"..\..\Sprites\RogueSprites\");
            Assert.AreNotEqual(sprites.GetSprite(AnimationState.Death, 0), sprites.GetSprite(AnimationState.Death, 0));
            Assert.AreNotEqual(sprites.GetSprite(AnimationState.Death, 0), sprites.GetSprite(AnimationState.Death, 1));
            Assert.AreNotEqual(sprites.GetSprite(AnimationState.Death, 0), sprites.GetSprite(AnimationState.Stand, 0));
        }

        [Test]
        public void TestAnimationSprite()
        {
            Assert.Catch(() => new Sprite(""));
            var sprite = new Sprite(@"..\..\Sprites\RogueSprites\");
            var previousFrame = sprite.CurrentFrame;
            var previousSprite = sprite.CurrentSprite;
            sprite.UpdateSprite();
            Assert.AreNotEqual(sprite.CurrentSprite, previousFrame);
            Assert.AreNotEqual(sprite.CurrentSprite, previousSprite);
        }
    }
}
