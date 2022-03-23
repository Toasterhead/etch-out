using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckHighScoreInput()
        {
            if (HighScore.Finished != null)
                return;

            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            //Direction

            if ((keys.IsKeyDown(Keys.Left) && keysPrev.IsKeyUp(Keys.Left)) || DPadLeftPressed(gamepad, gamepadPrev))

                HighScore.MoveCursorLeft();

            else if ((keys.IsKeyDown(Keys.Right) && keysPrev.IsKeyUp(Keys.Right)) || DPadRightPressed(gamepad, gamepadPrev))

                HighScore.MoveCursorRight();

            //Button Presses

            if (KeyPressed(keys, keysPrev, Keys.Enter) ||
                ButtonPressed(gamepad, gamepadPrev) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released))

                HighScore.ExecuteCommand();

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
