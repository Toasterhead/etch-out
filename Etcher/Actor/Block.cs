using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Block : Tile
    {
        public static readonly Rectangle[] _blockHitBox =
        {
            new Rectangle(1, 1, Game1.TILE_SIZE - 2, Game1.TILE_SIZE - 2)
        };

        public Block(Texture2D image, int gridX, int gridY, bool borderColor = true)
            : base(
                  new SpriteInfo(image, gridX, gridY, (int)Game1.Layers.Back),
                  new SpriteExtraInfo(null, borderColor ? Game1.borderColor : Color.Purple, SpriteEffects.None),
                  new CollisionInfo(_blockHitBox, null))
        { }
    }

    public class Gate : Block
    {
        private const uint OPENING_DURATION = 60;

        private uint? count;

        public Gate(int gridX, int gridY)
            : base(Images.BALL, gridX, gridY)
        {
            color = Color.Red;
            count = null;
        }

        public void InitiateOpening() { count = 0; }

        public override void Update()
        {
            if (count != null)
            {
                count++;

                if (count > OPENING_DURATION / 2 && count % 2 == 0)
                    render = false;
                else if (count % 3 == 0)
                    render = false;
                else render = true;

                if (count >= OPENING_DURATION)
                    remove = true;
            }

            base.Update();
        }
    }
}
