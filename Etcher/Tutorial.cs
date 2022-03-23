using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etcher
{
    public static class Tutorial
    {
        public enum Directions { Left, Right, Up, Down }
        
        public struct Command
        {
            public Directions direction;
            public bool boost;
            public uint duration;

            public Command(Directions direction, bool boost, uint duration)
            {
                this.direction = direction;
                this.boost = boost;
                this.duration = duration;
            }
        }

        public const uint MESSAGE_INTERVAL = 4 * Game1.FRAME_RATE;

        public static int CommandIndex { get; set; }

        public static Command[] CommandList = 
        {
            new Command(Directions.Up, false, (uint)(4.0 * Game1.FRAME_RATE)),      //Introductroy message.
            new Command(Directions.Left, false, (uint)(1.0 * Game1.FRAME_RATE)),    //Explain directional controls.
            new Command(Directions.Down, false, (uint)(1.0 * Game1.FRAME_RATE)),
            new Command(Directions.Left, false, (uint)(1.0 * Game1.FRAME_RATE)),
            new Command(Directions.Up, false, (uint)(2.0 * Game1.FRAME_RATE)),
            new Command(Directions.Right, false, (uint)(2.0 * Game1.FRAME_RATE)),
            new Command(Directions.Up, false, (uint)(0.5 * Game1.FRAME_RATE)),
            new Command(Directions.Left, false, (uint)(0.5 * Game1.FRAME_RATE)),
            new Command(Directions.Left, true, (uint)(2.0 * Game1.FRAME_RATE)),     //Explain boosting.
            new Command(Directions.Up, true, (uint)(0.5 * Game1.FRAME_RATE)),
            new Command(Directions.Right, true, (uint)(3.5 * Game1.FRAME_RATE)),
            new Command(Directions.Down, true, (uint)(0.5 * Game1.FRAME_RATE)),
            new Command(Directions.Left, true, (uint)(1.0 * Game1.FRAME_RATE)),
            new Command(Directions.Down, true, (uint)(0.5 * Game1.FRAME_RATE)),
            new Command(Directions.Right, true, (uint)(0.75 * Game1.FRAME_RATE)),
            new Command(Directions.Down, true, (uint)(0.25 * Game1.FRAME_RATE)),
            new Command(Directions.Left, true, (uint)(0.75 * Game1.FRAME_RATE)),
            new Command(Directions.Down, true, (uint)(0.25 * Game1.FRAME_RATE)),
            new Command(Directions.Right, true, (uint)(0.75 * Game1.FRAME_RATE)),
            new Command(Directions.Down, true, (uint)(0.25 * Game1.FRAME_RATE)),
            new Command(Directions.Left, true, (uint)(1.0 * Game1.FRAME_RATE)),
            new Command(Directions.Left, true, (uint)(1.0 * Game1.FRAME_RATE)),     //Explain crashing.
            new Command(Directions.Up, true, (uint)(5.0 * Game1.FRAME_RATE)),    
            new Command(Directions.Right, true, (uint)(4.0 * Game1.FRAME_RATE)),
            new Command(Directions.Up, false, (uint)(6.0 * Game1.FRAME_RATE)),      //Explain clearing the stage.
            new Command(Directions.Left, false, (uint)(12.0 * Game1.FRAME_RATE))
        };

        public static string[] MessageList = 
        {
            "Welcome to Etch-Out.",
            "Use the arrow keys or D-Pad...",
            "...to change direction.",
            "Hold the Shift key...",
            "...or a game-pad button...",
            "...to boost and gain more points.",
            "But don't crash into your trail!",
            "Also, don't crash into walls...",
            "...or blocks.",
            "After a while...",
            "...the red gates will open.",
            "Move off-screen to clear the stage.",
            "There's a bit more...",
            "...but you'll figure it out.",
            "Good luck!",
            "(Press a key or button to continue.)"
        };
    }
}
