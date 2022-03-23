using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Sprite : IGameObject
    {
        protected readonly bool _animated;

        protected Rectangle sourceRect;

        protected Point sheetDimensions; //Measured in tiles.
        protected Point tileDimensions; //Measured in pixels.
        protected Point tileSelection;

        /*
            Frame-count mode: 
        
            'delayCount' increases until equal to 'interval' and frame is updated 
            before setting 'delayCount' back to zero. Timing is independent of other 
            sprite sheets. Is active when 'timeSync == false'.

            (1 frame per second at 'interval = 60'.)

            Time-sync mode: 
        
            Checks if timestamp from clock is divisible by 'interval' and updates
            frame accordingly. Timing is in sync with other sprite sheets. Is active 
            when 'timeSync == true'.

            (1 frame per second at 'interval = 1000'.)
        */

        protected uint interval;
        protected uint delayCount;
        protected bool timeSync;

        public bool Animated { get { return _animated; } }
        public Point SheetDimensions { get { return sheetDimensions; } }
        public Rectangle SourceRect
        {
            get
            {
                if (_animated)

                    return sourceRect;

                return new Rectangle(0, 0, image.Width, image.Height);
            }
        }

        public Sprite(
            Texture2D image,
            int x,
            int y,
            int sheetX,
            int sheetY,
            int layer = 0,
            uint interval = 10,
            bool timeSync = false)
        {
            this.image = image;
            this.layer = layer;
            this.x = x;
            this.y = y;
            width = image.Width;
            height = image.Height;

            _animated = true;

            sheetDimensions = new Point(sheetX, sheetY);

            if (sheetDimensions.X <= 0 || sheetDimensions.Y <= 0)
                throw new Exception("Error - Sprite sheet dimensions must be greater than zero.");

            tileDimensions = new Point(image.Width / sheetDimensions.X, image.Height / sheetDimensions.Y);
            tileSelection = new Point(0, 0);

            this.x = x;
            this.y = y;
            width = image.Width / sheetDimensions.X;
            height = image.Height / sheetDimensions.Y;

            sourceRect = new Rectangle(
                tileSelection.X * tileDimensions.X,
                tileSelection.Y * tileDimensions.Y,
                tileDimensions.X,
                tileDimensions.Y);

            this.interval = interval;
            delayCount = 0;

            this.timeSync = timeSync;
        }

        public Sprite(
            SpriteInfo spriteInfo,
            SpriteExtraInfo spriteExtraInfo,
            CollisionInfo collisionInfo,
            AnimationInfo animationInfo)
        {
            image = spriteInfo.image;
            layer = spriteInfo.layer;
            x = spriteInfo.x;
            y = spriteInfo.y;
            width = spriteInfo.image.Width;
            height = spriteInfo.image.Height;
            imageSet = spriteExtraInfo.imageSet;
            hitBox = collisionInfo.hitBox;
            collisionPoint = collisionInfo.collisionPoint;
            color = spriteExtraInfo.color;
            orientation = spriteExtraInfo.orientation;
            Alpha = spriteExtraInfo.alpha;

            _animated = true;

            sheetDimensions = new Point(animationInfo.sheetX, animationInfo.sheetY);

            if (sheetDimensions.X <= 0 || sheetDimensions.Y <= 0)
                throw new Exception("Error - Sprite sheet dimensions must be greater than zero.");

            tileDimensions = new Point(image.Width / sheetDimensions.X, image.Height / sheetDimensions.Y);
            tileSelection = new Point(0, 0);

            width = image.Width / sheetDimensions.X;
            height = image.Height / sheetDimensions.Y;

            sourceRect = new Rectangle(
                tileSelection.X * tileDimensions.X,
                tileSelection.Y * tileDimensions.Y,
                tileDimensions.X,
                tileDimensions.Y);

            interval = animationInfo.interval;
            delayCount = 0;

            timeSync = animationInfo.timeSync;
        }

        public Sprite(
            SpriteInfo spriteInfo,
            CollisionInfo collisionInfo,
            AnimationInfo animationInfo)
        {
            image = spriteInfo.image;
            layer = spriteInfo.layer;
            x = spriteInfo.x;
            y = spriteInfo.y;
            width = spriteInfo.image.Width;
            height = spriteInfo.image.Height;
            hitBox = collisionInfo.hitBox;
            collisionPoint = collisionInfo.collisionPoint;
            color = Color.White;
            orientation = SpriteEffects.None;
            Alpha = 1.0f;

            _animated = true;

            sheetDimensions = new Point(animationInfo.sheetX, animationInfo.sheetY);

            if (sheetDimensions.X <= 0 || sheetDimensions.Y <= 0)
                throw new Exception("Error - Sprite sheet dimensions must be greater than zero.");

            tileDimensions = new Point(image.Width / sheetDimensions.X, image.Height / sheetDimensions.Y);
            tileSelection = new Point(0, 0);

            width = image.Width / sheetDimensions.X;
            height = image.Height / sheetDimensions.Y;

            sourceRect = new Rectangle(
                tileSelection.X * tileDimensions.X,
                tileSelection.Y * tileDimensions.Y,
                tileDimensions.X,
                tileDimensions.Y);

            interval = animationInfo.interval;
            delayCount = 0;

            timeSync = animationInfo.timeSync;
        }

        protected virtual void Animate()
        {
            if (!timeSync)
            {
                //Frame-count Mode

                if (++delayCount >= interval)
                {
                    if (++tileSelection.X >= sheetDimensions.X)
                    {
                        tileSelection.X = 0;
                        if (++tileSelection.Y >= sheetDimensions.Y) tileSelection.Y = 0;
                    }
                    SetFrame();
                    delayCount = 0;
                }
            }
            else
            {
                //Time-sync Mode

                long milliseconds = TimeElapsed() % interval;
                long totalFrames = sheetDimensions.X * sheetDimensions.Y;
                long frame = (milliseconds * totalFrames) / interval;
                tileSelection.Y = (int)(frame / sheetDimensions.X);
                tileSelection.X = (int)(frame % sheetDimensions.X);

                SetFrame();
            }
        }

        protected long TimeElapsed()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        protected void SetFrame()
        {
            sourceRect = new Rectangle(
                tileSelection.X * tileDimensions.X,
                tileSelection.Y * tileDimensions.Y,
                tileDimensions.X,
                tileDimensions.Y);
        }

        public virtual void Update()
        {
            UpdatePosition();

            if (_animated) Animate();
        }
    }
}
