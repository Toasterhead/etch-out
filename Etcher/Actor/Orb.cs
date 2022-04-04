using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Etcher
{
    public class Orb : Sprite
    {
        public const int MIDPOINT_X = 21;  //Note that the inner playfield is 38 x 26 tiles with a border size of 2 tiles.
        public const int MIDPOINT_Y = 15;  //The midpoint is the result of dividing the dimensions in half and adding the border size.
        public const uint M = 38 / 2;      //Half of playfield width in tiles.
        public const uint N = 26 / 2;      //Half of playfield height in tiles.

        private const int ADJUSTMENT_LEVEL = 8;

        public enum Phases { Top, Right, Bottom, Left, EnumSize }
        public enum Speeds { Slow = 2, Fast = 4 }

        private const uint ACTIVE_DURATION = 20 * Game1.FRAME_RATE;
        private const uint NUM_LEVELS = 7;

        public readonly uint Level;

        private readonly bool _clockwise;
        private readonly Speeds _speed;
        private readonly int _boundsLeft;
        private readonly int _boundsRight;
        private readonly int _boundsTop;
        private readonly int _boundsBottom;

        private static readonly Rectangle[] _orbHitBox =
        {
            new Rectangle(1, 1, 2 * Game1.TILE_SIZE - 2, 2 * Game1.TILE_SIZE - 2)
        };

        private uint activeCount;
        private Phases phase;

        public Phases CurrentPhase { get { return phase; } }

        public Orb(bool clockwise, Speeds speed)
            : base(
                new SpriteInfo(Images.ORB, 0, 0, (int)Game1.Layers.Front),
                new SpriteExtraInfo(null, clockwise ? Color.Red : Color.Green, SpriteEffects.None),
                new CollisionInfo(_orbHitBox, null),
                new AnimationInfo(5, 1, (uint)(speed == Speeds.Slow ? 2 : 0)))
        {
            Level = (uint)Game1.rand.Next(1, (int)NUM_LEVELS);
            phase = (Phases)Game1.rand.Next((int)Phases.EnumSize);

            _clockwise = clockwise;
            _speed = speed;
            _boundsLeft = CalculateBoundsLeft(Level);
            _boundsRight = CalculateBoundsRight(Level);
            _boundsTop = CalculateBoundsTop(Level);
            _boundsBottom = CalculateBoundsBottom(Level);
            activeCount = ACTIVE_DURATION;
            velocity = new Vector2();

            /*Debug.WriteLine("l: " + Level);
            Debug.WriteLine("m: " + M);
            Debug.WriteLine("n: " + N);
            Debug.WriteLine("mid: " + MIDPOINT_X + ", " + MIDPOINT_Y);
            Debug.WriteLine("Upper-left: " + _boundsUpperLeft);
            Debug.WriteLine("Upper-right: " + _boundsUpperRight);
            Debug.WriteLine("Lower-left: " + _boundsLowerLeft);
            Debug.WriteLine("Lower-right: " + _boundsLowerRight);*/
        }

        public void InitializeLocation(OrbAppearing orbAppearing)
        {
            x = orbAppearing.X;
            y = orbAppearing.Y;
        }

        public override void Update()
        {
            if (--activeCount == 0)
                Remove = true;

            if (_clockwise)
            {
                switch (phase)
                {
                    case Phases.Top:
                        velocity = new Vector2((int)_speed, 0.0f);
                        if (x > _boundsRight * Game1.TILE_SIZE)
                        {
                            x = _boundsRight * Game1.TILE_SIZE;
                            phase = Phases.Right;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Right:
                        velocity = new Vector2(0.0f, (int)_speed);
                        if (y > _boundsBottom * Game1.TILE_SIZE)
                        {
                            y = _boundsBottom * Game1.TILE_SIZE;
                            phase = Phases.Bottom;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Bottom:
                        velocity = new Vector2(-(int)_speed, 0.0f);
                        if (x < _boundsLeft * Game1.TILE_SIZE)
                        {
                            x = _boundsLeft * Game1.TILE_SIZE;
                            phase = Phases.Left;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Left:
                        velocity = new Vector2(0.0f, -(int)_speed);
                        if (y < _boundsTop * Game1.TILE_SIZE)
                        {
                            y = _boundsTop * Game1.TILE_SIZE;
                            phase = Phases.Top;
                            velocity = new Vector2();
                        }
                        break;
                }
            }
            else
            {
                switch (phase)
                {
                    case Phases.Top:
                        velocity = new Vector2(-(int)_speed, 0.0f);
                        if (x < _boundsLeft * Game1.TILE_SIZE)
                        {
                            x = _boundsLeft * Game1.TILE_SIZE;
                            phase = Phases.Left;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Right:
                        velocity = new Vector2(0.0f, -(int)_speed);
                        if (y < _boundsTop * Game1.TILE_SIZE)
                        {
                            y = _boundsTop * Game1.TILE_SIZE;
                            phase = Phases.Top;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Bottom:
                        velocity = new Vector2((int)_speed, 0.0f);
                        if (x > _boundsRight * Game1.TILE_SIZE)
                        {
                            x = _boundsRight * Game1.TILE_SIZE;
                            phase = Phases.Right;
                            velocity = new Vector2();
                        }
                        break;
                    case Phases.Left:
                        velocity = new Vector2(0.0f, (int)_speed);
                        if (y > _boundsBottom * Game1.TILE_SIZE)
                        {
                            y = _boundsBottom * Game1.TILE_SIZE;
                            phase = Phases.Bottom;
                            velocity = new Vector2();
                        }
                        break;
                }
            }

            base.Update();
        }

        public static int CalculateBoundsLeft(uint l)
        {
            return (int)((l * (M - 1)) / (NUM_LEVELS - 1) * (-1) - 1) + MIDPOINT_X;
        }

        public static int CalculateBoundsRight(uint l)
        {
            return (int)((l * (M - 1)) / (NUM_LEVELS - 1)) + MIDPOINT_X - 1;
        }

        public static int CalculateBoundsTop(uint l)
        {
            return (int)((l * (N - 1)) / (NUM_LEVELS - 1) * (-1) - 1) + MIDPOINT_Y;
        }

        public static int CalculateBoundsBottom(uint l)
        {
            return (int)((l * (N - 1)) / (NUM_LEVELS - 1)) + MIDPOINT_Y - 1;
        }

        public static bool Appears(uint orbTimer, uint level)
        {         
            const int INITIATE = 20 * (int)Game1.FRAME_RATE;
            const int INTERVAL = 10 * (int)Game1.FRAME_RATE;
            const int SUBTRACTION_INTERVAL = (int)Game1.FRAME_RATE / 3;
            const uint B_TYPE_LEVEL = ADJUSTMENT_LEVEL - 1;

            if (Game1.bType)
            {
                level = B_TYPE_LEVEL;
                if (Game1.rand.Next(2) == 0) return false;
            }

            level = level % ADJUSTMENT_LEVEL;

            int adjustedInitiation = INITIATE - ((int)level * SUBTRACTION_INTERVAL);
            int adjustedInterval = INTERVAL - ((int)level * (int)Game1.FRAME_RATE);

            adjustedInitiation = adjustedInitiation < 0 ? 0 : adjustedInitiation;
            adjustedInterval = adjustedInterval < 0 ? 0 : adjustedInterval;

            if (orbTimer >= adjustedInitiation && (orbTimer % adjustedInitiation) % adjustedInterval == 0)

                return true;

            return false;
        }

        public static bool AppearsAsFast(uint level)
        {
            const int INITIAL_RATIO = 10;
            const uint B_TYPE_LEVEL = ADJUSTMENT_LEVEL - 1;

            if (Game1.bType) level = B_TYPE_LEVEL;

            level = level % ADJUSTMENT_LEVEL;

            int adjustedRatio = INITIAL_RATIO - (int)level;

            if (Game1.rand.Next(adjustedRatio) == 0)

                return true;

            return false;
        }
    }

    public class OrbAppearing : Sprite
    {
        private const uint DURATION = 60;

        public readonly Orb MaterializingOrb; //Setting to null indicates a vanishing state.

        private uint count;

        public OrbAppearing(int x, int y, Orb materializingOrb = null)
            : base(
                  new SpriteInfo(Images.ORB_APPEARING, x, y, (int)Game1.Layers.Back),
                  new SpriteExtraInfo(null, Color.DarkGray, SpriteEffects.None),
                  new CollisionInfo(null, null),
                  new AnimationInfo(6, 2, 0))
        {
            count = DURATION;
            MaterializingOrb = materializingOrb;

            if (MaterializingOrb != null)
            {
                Orb orb = MaterializingOrb;

                switch (MaterializingOrb.CurrentPhase)
                {
                    case Orb.Phases.Left:
                        orb.Reposition(
                            Orb.CalculateBoundsLeft(orb.Level) * Game1.TILE_SIZE,
                            Game1.rand.Next(
                                Orb.CalculateBoundsTop(orb.Level) * Game1.TILE_SIZE + Game1.TILE_SIZE,
                                Orb.CalculateBoundsBottom(orb.Level) * Game1.TILE_SIZE));
                        break;
                    case Orb.Phases.Right:
                        orb.Reposition(
                            Orb.CalculateBoundsRight(orb.Level) * Game1.TILE_SIZE,
                            Game1.rand.Next(
                                Orb.CalculateBoundsTop(orb.Level) * Game1.TILE_SIZE + Game1.TILE_SIZE,
                                Orb.CalculateBoundsBottom(orb.Level) * Game1.TILE_SIZE));
                        break;
                    case Orb.Phases.Top:
                        orb.Reposition(
                            Game1.rand.Next(
                                Orb.CalculateBoundsLeft(orb.Level) * Game1.TILE_SIZE + Game1.TILE_SIZE,
                                Orb.CalculateBoundsRight(orb.Level) * Game1.TILE_SIZE),
                            Orb.CalculateBoundsTop(orb.Level) * Game1.TILE_SIZE);
                        break;
                    case Orb.Phases.Bottom:
                        orb.Reposition(
                            Game1.rand.Next(
                                Orb.CalculateBoundsLeft(orb.Level) * Game1.TILE_SIZE + Game1.TILE_SIZE,
                                Orb.CalculateBoundsRight(orb.Level) * Game1.TILE_SIZE),
                            Orb.CalculateBoundsBottom(orb.Level) * Game1.TILE_SIZE);
                        break;
                }

                Reposition(orb.X, orb.Y);
            }
        }

        protected override void Animate()
        {
            if (MaterializingOrb == null)
                tileSelection.X = (int)(((count - 1) / (float)DURATION) * sheetDimensions.X);
            else tileSelection.X = (int)((1.0f - (count / (float)DURATION)) * sheetDimensions.X);

            tileSelection.Y = tileSelection.Y == 0 ? 1 : 0;

            SetFrame();
        }

        public override void Update()
        {
            if (--count == 0)
                Remove = true;

            base.Update();
        }
    }
}
