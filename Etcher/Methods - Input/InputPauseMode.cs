using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckPauseInput()
        {
            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            SubMenu pauseSubMenu = MenuManager.PauseMenu.CurrentSubMenu;

            //Direction

            if ((keys.IsKeyDown(Keys.Up) && keysPrev.IsKeyUp(Keys.Up)) || DPadUpPressed(gamepad, gamepadPrev))

                pauseSubMenu.MoveCursorUp();

            else if ((keys.IsKeyDown(Keys.Down) && keysPrev.IsKeyUp(Keys.Down)) || DPadDownPressed(gamepad, gamepadPrev))

                pauseSubMenu.MoveCursorDown();

            //Button Presses

            if (KeyPressed(keys, keysPrev, Keys.Enter) ||
                ButtonPressed(gamepad, gamepadPrev) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released))

                pauseSubMenu.Select();

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
