using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckActionInput()
        {
            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            //Direction

            if ((keys.IsKeyDown(Keys.Up) && keysPrev.IsKeyUp(Keys.Up)) || DPadUpPressed(gamepad, gamepadPrev))

                player.MoveUp();

            else if ((keys.IsKeyDown(Keys.Down) && keysPrev.IsKeyUp(Keys.Down)) || DPadDownPressed(gamepad, gamepadPrev))

                player.MoveDown();

            else if ((keys.IsKeyDown(Keys.Left) && keysPrev.IsKeyUp(Keys.Left)) || DPadLeftPressed(gamepad, gamepadPrev))

                player.MoveLeft();

            else if ((keys.IsKeyDown(Keys.Right) && keysPrev.IsKeyUp(Keys.Right)) || DPadRightPressed(gamepad, gamepadPrev))

                player.MoveRight();

            //Button Presses

            if ((KeyPressed(keys, keysPrev, Keys.LeftShift) || ButtonPressed(gamepad, gamepadPrev)) && clockTimer == null)

                player.MoveMedium();

            else if ((KeyReleased(keys, keysPrev, Keys.LeftShift) || ButtonReleased(gamepad, gamepadPrev)) && clockTimer == null)

                player.MoveSlow();

            else if (
                (KeyPressed(keys, keysPrev, Keys.Escape) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released)))
            {
                if (InSession())
                {
                    gameMode = GameModes.Pause;
                    PlaySound(Sounds.PAUSE);
                }
            }

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
