using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public class Player : Sprite
    {
        public enum Speeds { Slow = 1, Medium = 2, Fast = 3 }

        private enum AnimationIntervals { Slow = 4, Medium = 3, Fast = 2 }
        private enum Directions { Left, Right, Up, Down }

        public const int START_X = ((Game1.SCREEN_DIVIDER - 2) / 2) * Game1.TILE_SIZE + Game1.TILE_SIZE;
        public const int START_Y = Game1.FULLFIELD_HEIGHT - (5 * Game1.TILE_SIZE);
        private const int TRAIL_GAP = 20;
        private const uint HYPER_DURATION = 180;

        private bool adjustVelocity;
        private int lives;
        private uint? hyper;
        private Speeds currentSpeed;
        private Directions currentDirection;
        private Queue<Vector2> trail;

        private static readonly Rectangle[] _playerHitBox =
        {
            new Rectangle(1, 1, Game1.TILE_SIZE - 2, Game1.TILE_SIZE - 2)
        };

        public int Lives { get { return lives; } }
        public Speeds CurrentSpeed { get { return currentSpeed; } }

        public Player(bool infiniteLives = false)
            : base(
                new SpriteInfo(
                    Images.PLAYER, 
                    START_X, 
                    START_Y, 
                    (int)Game1.Layers.Front),
                new SpriteExtraInfo(null, Color.Yellow, SpriteEffects.None),
                new CollisionInfo(_playerHitBox, null),
                new AnimationInfo(4, 4, (int)AnimationIntervals.Slow))
        {
            lives = infiniteLives ? int.MaxValue : 3;
            adjustVelocity = false;
            currentSpeed = Speeds.Slow;
            trail = new Queue<Vector2>();
            trail.Enqueue(new Vector2(Center.X, Center.Y));
            currentDirection = Directions.Up;
        }

        public void IncreaseLives() { lives++; }
        public void DecreaseLives()
        {
            if (lives > 0)
                lives--;
        }

        public Vector2? ObtainTrailPoint()
        {
            if (trail.Count > TRAIL_GAP)

                return trail.Dequeue();

            else return null;
        }

        public void MoveLeft()
        {
            currentDirection = Directions.Left;
            adjustVelocity = true;
        }
        public void MoveRight()
        {
            currentDirection = Directions.Right;
            adjustVelocity = true;
        }
        public void MoveUp()
        {
            currentDirection = Directions.Up;
            adjustVelocity = true;
        }
        public void MoveDown()
        {
            currentDirection = Directions.Down;
            adjustVelocity = true;
        }
        public void MoveSlow()
        {
            if (hyper == null)
            {
                currentSpeed = Speeds.Slow;
                interval = (int)AnimationIntervals.Slow;
                adjustVelocity = true;
            }
        }
        public void MoveMedium()
        {
            if (hyper == null)
            { 
                currentSpeed = Speeds.Medium;
                interval = (int)AnimationIntervals.Medium;
                adjustVelocity = true;
            }
        }
        public void InitiateHyper()
        {
            hyper = HYPER_DURATION;
            currentSpeed = Speeds.Fast;
            interval = (int)AnimationIntervals.Fast;
            adjustVelocity = true;
        }
        public void CancelHyper() { hyper = null; }

        public void Reset()
        {
            Render = true;
            Remove = false;
            TopLeft = new Point(START_X, START_Y);
            hyper = null;
            trail.Clear();
            MoveUp();
            MoveSlow();
            tileSelection.X = 0;
            tileSelection.Y = 0;
            SetFrame();
        }

        protected override void Animate()
        {
            if (++delayCount >= interval)
            {
                if (++tileSelection.Y >= sheetDimensions.Y)
                    tileSelection.Y = 0;

                delayCount = 0;
            }

            if (Velocity.Y < 0.0f)
                tileSelection.X = 0;
            else if (Velocity.X > 0.0f)
                tileSelection.X = 1;
            else if (Velocity.Y > 0.0f)
                tileSelection.X = 2;
            else tileSelection.X = 3;

            SetFrame();
        }

        public override void Update()
        {
            trail.Enqueue(new Vector2(Center.X, Center.Y));

            if (hyper != null && --hyper == 0)
            {
                hyper = null;
                MoveSlow(); //Account for the fact that the player may be holding the shift key when hyper ends later.
            }

            if (adjustVelocity)
            {
                switch (currentDirection)
                {
                    case Directions.Left:
                        velocity = new Vector2(-(int)currentSpeed, 0.0f);
                        break;
                    case Directions.Right:
                        velocity = new Vector2((int)currentSpeed, 0.0f);
                        break;
                    case Directions.Up:
                        velocity = new Vector2(0.0f, -(int)currentSpeed);
                        break;
                    case Directions.Down:
                        velocity = new Vector2(0.0f, (int)currentSpeed);
                        break;
                }

                adjustVelocity = false;
            }

            base.Update();
        }
    }
}
