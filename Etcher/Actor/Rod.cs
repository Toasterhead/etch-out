using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Rod : Sprite
    {
        private const int SPEED = 2;

        private static readonly Rectangle[] _rodHitBox =
        {
        new Rectangle(1, 2, 2 * Game1.TILE_SIZE - 2, Game1.TILE_SIZE - 4)
    };

        public Rod(int gridX, int gridY, bool upward = false)
            : base(
                new SpriteInfo(Images.ROD, gridX * Game1.TILE_SIZE, gridY * Game1.TILE_SIZE, (int)Game1.Layers.Middle),
                new SpriteExtraInfo(null, Color.Pink, SpriteEffects.None),
                new CollisionInfo(_rodHitBox, null))
        { velocity = upward ? new Vector2(0.0f, -SPEED) : new Vector2(0.0f, SPEED); }

        public void ProcessCollision(List<IGameObject> spriteSet)
        {
            foreach (IGameObject i in spriteSet)

                if (i is Block && GetHitBox(0).Intersects(i.Rect))
                {
                    if (velocity.Y > 0.0f && Bottom >= i.Rect.Top)
                    {
                        SetHitBoxBottom(0, i.Rect.Top, absolute: true);
                        velocity.Y = -SPEED;
                    }
                    else if (velocity.Y < 0.0f && Top <= i.Rect.Bottom)
                    {
                        SetHitBoxTop(0, i.Rect.Bottom, absolute: true);
                        velocity.Y = SPEED;
                    }
                }
        }

        public override void Update()
        {
            orientation = orientation == SpriteEffects.None ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            base.Update();
        }
    }
}
