using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Vortex : Sprite
    {
        public const uint RADIUS_SMALL = Game1.TILE_SIZE;
        public const uint RADIUS_LARGE = Game1.TILE_SIZE * 5;

        private const uint INTERVAL = 6 * Game1.FRAME_RATE;

        public static uint Radius { get; private set; }
        public static bool Present { get; private set; }

        private static uint timer = 0;
        private static bool expanding = false;

        private static Rectangle[] _vortexHitBox =
        {
            new Rectangle(0, 0, 2 * (int)RADIUS_LARGE, 2 * (int)RADIUS_LARGE)
        };

        public Vortex(int x, int y) :
            base(
                new SpriteInfo(
                    Game1.vortexSurface, 
                    x * Game1.TILE_SIZE - (4 * Game1.TILE_SIZE), 
                    y * Game1.TILE_SIZE - (4 * Game1.TILE_SIZE), 
                    layer: (int)Game1.Layers.Back),
                new CollisionInfo(_vortexHitBox, null)) { }

        public bool Collides(Player player)
        {
            //Where 'c' is the hypotenuse of a right triangle...

            double a = player.Center.X - Center.X;
            double b = player.Center.Y - Center.Y;
            double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            return c <= Radius;
        }

        public static void Reset()
        {
            timer = 0;
            Radius = RADIUS_SMALL;
            Present = false;
            expanding = false;
        }

        public static void UpdateRadius()
        {
            if (timer++ % INTERVAL == 0)
                expanding = !expanding;

            if (expanding && Radius < RADIUS_LARGE)
                Radius++;
            else if (!expanding && Radius > RADIUS_SMALL)
                Radius--;
        }

        public static void DeterminePresence(List<IGameObject> spriteSet)
        {
            Present = false;

            foreach (IGameObject i in spriteSet)

                if (i is Vortex)
                {
                    Present = true;
                    break;
                }
        }
    }
}
