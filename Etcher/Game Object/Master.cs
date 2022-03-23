using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public static class Master
    {
        public static GraphicsDevice TheGraphicsDevice;
        public static GraphicsDeviceManager TheGraphics;
        public static SpriteBatch TheSpriteBatch;
        public static Texture2D DefaultImage;
        public static uint TileSize = 50;

        public static Point CurrentResolution
        {
            get
            {
                DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

                return new Point(displayMode.Width, displayMode.Height);
            }
        }

        public static List<Point> GetSupportedResolutions()
        {
            List<Point> supportedResolutions = new List<Point>();

            foreach (DisplayMode displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                bool duplicateFound = false;

                foreach (Point resolution in supportedResolutions)

                    if (displayMode.Width == resolution.X && displayMode.Height == resolution.Y)
                    {
                        duplicateFound = true;
                        break;
                    }

                if (!duplicateFound) supportedResolutions.Add(new Point(displayMode.Width, displayMode.Height));
            }

            return supportedResolutions;
        }
    }
}
