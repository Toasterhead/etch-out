using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawPauseMode()
        {
            const int NUM_TILES_HORIZONTAL = FULLFIELD_WIDTH / TILE_SIZE;
            const int NUM_TILES_VERTICAL = FULLFIELD_HEIGHT / TILE_SIZE;

            const int MENU_MARGIN_LEFT = 12 * TILE_SIZE;
            const int MENU_MARGIN_TOP = 15 * TILE_SIZE;

            int selectionIndex = MenuManager.PauseMenu.CurrentSubMenu.SelectionIndex;

            for (int i = 0; i < MenuManager.PauseMenu.CurrentSubMenuLength; i++)
            {
                MenuManager.TheTextfields[i].ChangeText(MenuManager.PauseMenu.GetCurrentAtIndex(i).ToString());
                MenuManager.TheTextfields[i].Draw();
            }

            textLives.ChangeText(player.Lives.ToString());
            textScore.ChangeText(points.ToString());
            textTime.ChangeText((gateTimer / FRAME_RATE).ToString());
            textMessage.ChangeText("@@@P A U S E D@@@");
            textLives.Draw();
            textScore.Draw();
            textTime.Draw();
            textMessage.Draw();

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            for (int i = SCREEN_DIVIDER; i < NUM_TILES_HORIZONTAL; i++)
            {
                for (int j = 0; j < NUM_TILES_VERTICAL; j++)
                {
                    const int SCORE_STARTS = 1;
                    const int SCORE_ENDS = 6;
                    const int LIVES_STARTS = 8;
                    const int LIVES_ENDS = 12;
                    const int TIME_STARTS = NUM_TILES_VERTICAL - 7;
                    const int TIME_ENDS = NUM_TILES_VERTICAL - 2;

                    Vector2 cursor = new Vector2(i * TILE_SIZE, j * TILE_SIZE);

                    if (i == SCREEN_DIVIDER && j == 1)
                        spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                    else if (i == SCREEN_DIVIDER && j == NUM_TILES_VERTICAL - 2)
                        spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);
                    else if (i == SCREEN_DIVIDER && j >= 2 && j < NUM_TILES_VERTICAL - 2)
                        spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                    else if (j >= SCORE_STARTS && j <= LIVES_ENDS)
                    {
                        //Render Score Border
                        if (i == SCREEN_DIVIDER + 1 && j == SCORE_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == SCORE_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == SCORE_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j >= SCORE_STARTS + 1 && j < SCORE_ENDS)
                            spriteBatch.Draw(Images.BORDER_LEFT, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j >= SCORE_STARTS + 1 && j < SCORE_ENDS)
                            spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j == SCORE_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == SCORE_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == SCORE_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);

                        //Render Space
                        else if (j > SCORE_ENDS && j < LIVES_STARTS)
                            spriteBatch.Draw(Images.BORDER, cursor, borderColor);

                        //Render Lives Border
                        else if (i == SCREEN_DIVIDER + 1 && j == LIVES_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == LIVES_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == LIVES_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j >= LIVES_STARTS + 1 && j < LIVES_ENDS)
                            spriteBatch.Draw(Images.BORDER_LEFT, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j >= LIVES_STARTS + 1 && j < LIVES_ENDS)
                            spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j == LIVES_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == LIVES_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == LIVES_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);

                        //Render Player Icon and Lives
                        if (i == SCREEN_DIVIDER + 3 && j == LIVES_STARTS + 2)
                            spriteBatch.Draw(Images.ICON_PLAYER, cursor, Color.Yellow);
                        else if (i == SCREEN_DIVIDER + 4 && j == LIVES_STARTS + 2)
                            spriteBatch.Draw(Images.QUANTITY, cursor, Color.White);
                        else if (i == SCREEN_DIVIDER + 5 && j == LIVES_STARTS + 2)
                            spriteBatch.Draw(textLives.Image, cursor, Color.White);

                        //Render Score and Label
                        else if (i == SCREEN_DIVIDER + 3 && j == SCORE_STARTS + 2)
                            spriteBatch.Draw(textLabelScore.Image, cursor, Color.White);
                        else if (i == SCREEN_DIVIDER + 3 && j == SCORE_STARTS + 3)
                            spriteBatch.Draw(textScore.Image, cursor, Color.White);
                    }
                    else if (j >= TIME_STARTS && j <= TIME_ENDS)
                    {
                        //Render Time Border
                        if (i == SCREEN_DIVIDER + 1 && j == TIME_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == TIME_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == TIME_STARTS)
                            spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j >= TIME_STARTS + 1 && j < TIME_ENDS)
                            spriteBatch.Draw(Images.BORDER_LEFT, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j >= TIME_STARTS + 1 && j < TIME_ENDS)
                            spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                        else if (i == SCREEN_DIVIDER + 1 && j == TIME_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, cursor, borderColor);
                        else if (i > SCREEN_DIVIDER + 1 && i < NUM_TILES_HORIZONTAL - 1 && j == TIME_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM, cursor, borderColor);
                        else if (i == NUM_TILES_HORIZONTAL - 1 && j == TIME_ENDS)
                            spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);

                        //Render Time and Label
                        if (i == SCREEN_DIVIDER + 3 && j == TIME_STARTS + 2)
                            spriteBatch.Draw(textLabelTime.Image, cursor, Color.White);
                        else if (i == SCREEN_DIVIDER + 3 && j == TIME_STARTS + 3)
                            spriteBatch.Draw(textTime.Image, cursor, Color.White);
                    }
                    else spriteBatch.Draw(Images.BORDER, cursor, borderColor);
                }
            }

            foreach (IGameObject i in spriteSet)

                if (i is Tile)
                {
                    Tile iTile = i as Tile;

                    if (iTile.GridX < 2 || iTile.GridY < 2 || iTile.GridY >= (FULLFIELD_HEIGHT / TILE_SIZE) - 2)
                        spriteBatch.Draw(i.Image, new Vector2(i.X, i.Y), i.TheColor);
                }

            spriteBatch.Draw(textMessage.Image, new Vector2(MENU_MARGIN_LEFT, MENU_MARGIN_TOP - (4 * TILE_SIZE)), Color.White);

            for (int i = 0; i < MenuManager.PauseMenu.CurrentSubMenuLength; i++)
            {
                spriteBatch.Draw(
                    MenuManager.TheTextfields[i].Image, 
                    new Vector2(
                        MENU_MARGIN_LEFT, 
                        MENU_MARGIN_TOP + (2 * i * TILE_SIZE)), 
                    Color.White);

                if (selectionIndex == i)
                    spriteBatch.Draw(
                        Images.CURSOR,
                        new Vector2(
                            MENU_MARGIN_LEFT - (2 * TILE_SIZE),
                            MENU_MARGIN_TOP + (2 * i * TILE_SIZE)),
                        Color.Yellow);
            }

            spriteBatch.End();

            SubDraw();
        }
    }
}
