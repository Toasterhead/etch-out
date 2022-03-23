using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawTitleMode()
        {
            const int MARGIN_TITLE_TOP = 2 * TILE_SIZE;
            const int MARGIN_MAIN_MENU_LEFT = 20 * TILE_SIZE;
            const int MARGIN_WIDE_MENU_LEFT = 12 * TILE_SIZE;
            const int MARGIN_HIGH_SCORE_MENU_LEFT = 8 * TILE_SIZE;
            const int MARGIN_CURSOR = 2 * TILE_SIZE;
            const int SLIDER_HORIZONTAL_DISPLACEMENT = 19 * TILE_SIZE;
            const uint BLINK_INTERVAL = 6;

            int selectionIndex = MenuManager.TitleMenu.CurrentSubMenu.SelectionIndex;
            int titleHeight = Images.TITLE.Height / 3;
            int titleFrame = (int)(universalTimer % 15) / 5;
            int marginTitleLeft = (FULLFIELD_WIDTH / 2) - (Images.TITLE.Width / 2);
            int marginMenuTop = MARGIN_TITLE_TOP + titleHeight + (4 * TILE_SIZE);
            int marginMenuLeft = MARGIN_MAIN_MENU_LEFT;
            bool renderTitle = true;

            string menuName = MenuManager.TitleMenu.CurrentSubMenu.Title;

            switch (menuName)
            {
                case MenuManager.MenuName.MAIN:
                    renderTitle = true;
                    marginMenuLeft = MARGIN_MAIN_MENU_LEFT;
                    marginMenuTop = MARGIN_TITLE_TOP + titleHeight + (2 * TILE_SIZE);
                    break;
                case MenuManager.MenuName.HIGH_SCORE:
                    renderTitle = false;
                    marginMenuLeft = MARGIN_HIGH_SCORE_MENU_LEFT;
                    marginMenuTop = TILE_SIZE;
                    break;
                case MenuManager.MenuName.SETTINGS:
                    renderTitle = true;
                    marginMenuLeft = MARGIN_WIDE_MENU_LEFT;
                    marginMenuTop = MARGIN_TITLE_TOP + titleHeight + (2 * TILE_SIZE);
                    break;
                case MenuManager.MenuName.ABOUT:
                    renderTitle = true;
                    marginMenuLeft = MARGIN_WIDE_MENU_LEFT;
                    marginMenuTop = MARGIN_TITLE_TOP + titleHeight + (2 * TILE_SIZE);
                    break;
            }

            for (int i = 0; i < MenuManager.TitleMenu.CurrentSubMenuLength; i++)
            {
                MenuManager.TheTextfields[i].ChangeText(MenuManager.TitleMenu.GetCurrentAtIndex(i).ToString());
                MenuManager.TheTextfields[i].Draw();
            }

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < MenuManager.TitleMenu.CurrentSubMenuLength; i++)
            {
                bool blinking =
                    i == MenuManager.TitleMenu.CurrentSubMenu.SelectionIndex && 
                    MenuManager.TitleMenu.CurrentSubMenu.Title == MenuManager.MenuName.MAIN && 
                    MenuManager.Finished % BLINK_INTERVAL < (BLINK_INTERVAL / 2);

                MenuItem menuItem = MenuManager.TitleMenu.CurrentSubMenu.GetAtIndex(i);

                Color color = 
                    menuItem is ISelectable && (menuItem as ISelectable).Muted ? 
                    Color.Gray : 
                    Color.White;
       
                if (!blinking)
                    spriteBatch.Draw(
                        MenuManager.TheTextfields[i].Image,
                        new Vector2(
                            marginMenuLeft,
                            marginMenuTop + (2 * i * TILE_SIZE)),
                        color);

                if (menuItem is MISlider)
                    DrawSlider(
                        menuItem as MISlider,
                        marginMenuLeft + SLIDER_HORIZONTAL_DISPLACEMENT,
                        marginMenuTop + (2 * i * TILE_SIZE));
            }

            spriteBatch.Draw(
                Images.CURSOR, 
                new Vector2(
                    marginMenuLeft - MARGIN_CURSOR, 
                    marginMenuTop + (2 * selectionIndex * TILE_SIZE)), 
                Color.Yellow);

             if (renderTitle)
                spriteBatch.Draw(
                    Images.TITLE,
                    new Rectangle(
                        marginTitleLeft, 
                        MARGIN_TITLE_TOP, 
                        Images.TITLE.Width, 
                        titleHeight),
                    new Rectangle(
                        0, 
                        titleFrame * titleHeight, 
                        Images.TITLE.Width, 
                        titleHeight),
                    Color.White);

            spriteBatch.End();

            SubDraw();
        }
    }
}
