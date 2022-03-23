using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Pickup : Sprite
    {
        public Pickup(Texture2D image, int gridX, int gridY)
            : base(image, gridX * Game1.TILE_SIZE, gridY * Game1.TILE_SIZE, (int)Game1.Layers.Middle)
        { }
    }

    public class ExtraLife : Pickup
    {
        public ExtraLife(int gridX, int gridY)
            : base(Images.EXTRA_LIFE, gridX, gridY)
        { }
    }

    public class Clock : Pickup
    {
        public Clock(int gridX, int gridY)
            : base(Images.CLOCK, gridX, gridY)
        { }
    }

    public class HyperSpeed : Pickup
    {
        public HyperSpeed(int gridX, int gridY)
            : base(Images.EXCLAMATION, gridX, gridY)
        { color = Color.Orange; }
    }

    public class Points : Pickup
    {
        public readonly uint Value;

        public Points(Texture2D image, int gridX, int gridY, uint value)
            : base(image, gridX, gridY)
        { Value = value; }
    }

    public class Points100 : Points
    {
        public Points100(int gridX, int gridY)
            : base(Images.POINTS_100, gridX, gridY, 100)
        { }
    }

    public class Points250 : Points
    {
        public Points250(int gridX, int gridY)
            : base(Images.POINTS_250, gridX, gridY, 250)
        { }
    }

    public class Points500 : Points
    {
        public Points500(int gridX, int gridY)
            : base(Images.POINTS_500, gridX, gridY, 500)
        { }
    }
}
