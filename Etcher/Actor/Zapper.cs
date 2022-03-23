using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Zapper : Tile
    {
        private const uint DURATION = 120;
        private const uint HALF_DURATION = DURATION / 2;
            
        private readonly bool _offset;
        public static readonly Rectangle[] _zapperHitBox =
        {
            new Rectangle(1, 1, Game1.TILE_SIZE - 2, Game1.TILE_SIZE - 2)
        };

        public static uint Timer { get; private set; } = 0;
        public bool Potent
        {
            get
            {
                if (_offset)

                    return Timer % DURATION < HALF_DURATION;

                else return Timer % DURATION >= HALF_DURATION;
            }
        }

        public Zapper(int gridX, int gridY, bool offset = false)
            : base(
                  new SpriteInfo(Images.ZAPPER, gridX, gridY, (int)Game1.Layers.Back),
                  new SpriteExtraInfo(null, Color.Green, SpriteEffects.None),
                  new CollisionInfo(_zapperHitBox, null),
                  new AnimationInfo(1, 3, 0))
        { _offset = offset; }

        protected override void Animate()
        {
            const uint GRADATION = HALF_DURATION / 30;
            uint elapsedTime = Timer % HALF_DURATION;

            if (elapsedTime < (1 * GRADATION) || elapsedTime >= HALF_DURATION - (1 * GRADATION))
                tileSelection.Y = 2;
            else if (elapsedTime < (2 * GRADATION) || elapsedTime >= HALF_DURATION - (2 * GRADATION))
                tileSelection.Y = 1;
            else tileSelection.Y = 0;

            SetFrame();
        }

        public override void Update()
        {
            if (Potent)
            {
                render = true;

                if (Timer % 2 == 0)
                    orientation = SpriteEffects.FlipHorizontally;
                else orientation = SpriteEffects.None;
            }
            else render = false;

            base.Update();
        }

        public static void ResetTimer() { Timer = 0; }
        public static void UpdateTimer() { Timer++; }
    }
}
