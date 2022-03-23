using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckTutorialInput()
        {
            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            uint elapsedTime = universalTimer - universalTimeStamp;
            uint commandTime = 0;

            if (elapsedTime == 120) //
            {

            }

            for (int i = 0; i < Tutorial.CommandIndex; i++)
                commandTime += Tutorial.CommandList[i].duration;

            if (elapsedTime >= commandTime + Tutorial.CommandList[Tutorial.CommandIndex].duration &&
                Tutorial.CommandIndex < Tutorial.CommandList.Length - 1)

                Tutorial.CommandIndex++;

            Tutorial.Command currentCommand = Tutorial.CommandList[Tutorial.CommandIndex];

            //Direction (Psuedo-input from Program)

            if (currentCommand.direction == Tutorial.Directions.Up)

                player.MoveUp();

            else if (currentCommand.direction == Tutorial.Directions.Down)

                player.MoveDown();

            else if (currentCommand.direction == Tutorial.Directions.Left)

                player.MoveLeft();

            else if (currentCommand.direction == Tutorial.Directions.Right)

                player.MoveRight();

            //Button Presses (Psuedo-input from Program)

            if (currentCommand.boost)

                player.MoveMedium();

            else player.MoveSlow();

            //Real User Input

            bool tutorialFinished = elapsedTime > (Tutorial.MessageList.Length - 1) * Tutorial.MESSAGE_INTERVAL;
            bool exitTutorialA = 
                (KeyPressed(keys, keysPrev, Keys.Escape) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released));
            bool exitTutorialB = ButtonPressed(gamepad, gamepadPrev) || KeyPressed(keys, keysPrev, Keys.Enter);

            if (exitTutorialA || (tutorialFinished && exitTutorialB))
            {
                gameMode = GameModes.Title;
                StopMusic();
                PlayMusic(Sounds.Music.TITLE, repeat: true);
            }

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
