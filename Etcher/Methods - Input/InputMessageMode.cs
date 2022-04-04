using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckMessageInput()
        {
            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            //Button Presses

            if (KeyPressed(keys, keysPrev, Keys.Enter) || KeyPressed(keys, keysPrev, Keys.Escape) ||
                ButtonPressed(gamepad, gamepadPrev) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released))

                messageTimer = null;

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
