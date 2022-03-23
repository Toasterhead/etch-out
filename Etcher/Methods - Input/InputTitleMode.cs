using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Etcher
{
    public partial class Game1 : Game
    {
        public void CheckTitleInput()
        {
            if (MenuManager.Finished != null)
                return;

            keys = Keyboard.GetState();
            gamepad = GamePad.GetState(PlayerIndex.One);

            SubMenu titleSubMenu = MenuManager.TitleMenu.CurrentSubMenu;

            //Direction

            if ((keys.IsKeyDown(Keys.Up) && keysPrev.IsKeyUp(Keys.Up)) || DPadUpPressed(gamepad, gamepadPrev))

                titleSubMenu.MoveCursorUp();

            else if ((keys.IsKeyDown(Keys.Down) && keysPrev.IsKeyUp(Keys.Down)) || DPadDownPressed(gamepad, gamepadPrev))

                titleSubMenu.MoveCursorDown();

            else if ((keys.IsKeyDown(Keys.Left) && keysPrev.IsKeyUp(Keys.Left)) || DPadLeftPressed(gamepad, gamepadPrev))
            {
                if (titleSubMenu.GetSelection() is MISlider)
                    (titleSubMenu.GetSelection() as MISlider).Decrease();
                else if (titleSubMenu.GetSelection() is MISwitch)
                    (titleSubMenu.GetSelection() as MISwitch).Flip();
                else if (titleSubMenu.GetSelection() is MIDial)
                    (titleSubMenu.GetSelection() as MIDial).Backward();
            }
            else if ((keys.IsKeyDown(Keys.Right) && keysPrev.IsKeyUp(Keys.Right)) || DPadRightPressed(gamepad, gamepadPrev))
            {
                if (titleSubMenu.GetSelection() is MISlider)
                    (titleSubMenu.GetSelection() as MISlider).Increase();
                else if (titleSubMenu.GetSelection() is MISwitch)
                    (titleSubMenu.GetSelection() as MISwitch).Flip();
                else if (titleSubMenu.GetSelection() is MIDial)
                    (titleSubMenu.GetSelection() as MIDial).Forward();
            }

            //Button Presses

            if (KeyPressed(keys, keysPrev, Keys.Enter) || 
                ButtonPressed(gamepad, gamepadPrev) ||
                (gamepad.Buttons.Start == ButtonState.Pressed && gamepadPrev.Buttons.Start == ButtonState.Released))

                titleSubMenu.Select();

            while (!(titleSubMenu.GetSelection() is ISelectable))
                titleSubMenu.MoveCursorDown();

            keysPrev = keys;
            gamepadPrev = gamepad;
        }
    }
}
