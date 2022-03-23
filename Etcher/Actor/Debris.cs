using Microsoft.Xna.Framework;

namespace Etcher
{
    public class Debris : Sprite
    {
        public const int MAX_SPEED = 16;
        public const int DIVISOR_CRASH = 16;
        public const int DIVISOR_FIREWORKS = 24;

        private readonly uint _duration;

        private uint count;

        public Point Origin { get; private set; }

        public Debris()
            : base(Images.DOT, 0, 0, (int)Game1.Layers.Front)
        {
            Point sign = new Point(
                Game1.rand.Next(2) == 0 ? 1 : -1,
                Game1.rand.Next(2) == 0 ? 1 : -1);
            while(velocity.X == 0.0f && velocity.Y == 0)
                velocity = new Vector2(sign.X * Game1.rand.Next(MAX_SPEED), sign.Y * Game1.rand.Next(MAX_SPEED));
            color = Color.Yellow;

            _duration = (uint)Game1.rand.Next(10, 70);

            count = _duration;
        }

        public void SetPosition(Player player)
        {
            x = player.Center.X;
            y = player.Center.Y;
            Origin = new Point(player.Center.X, player.Center.Y);
        }

        public void SetPosition(Point position)
        {
            x = position.X;
            y = position.Y;
            Origin = position;
        }

        public void SetColor(Color color) { this.color = color; }

        public override void Update()
        {
            if (--count == 0)
                remove = true;

            if (count < _duration / 3 && count % 2 == 0)

                render = false;

            else if (count < 2 * (_duration / 3) && count % 3 == 0)

                render = false;

            else render = true;

            base.Update();
        }
    }
}
