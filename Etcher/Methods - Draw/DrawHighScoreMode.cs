using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawHighScoreMode()
        {
            const int BORDER_1_X = 12;
            const int BORDER_1_Y = 2;
            const int BORDER_1_WIDTH = 25;
            const int BORDER_1_HEIGHT = 6;

            const int BORDER_2_X = 7;
            const int BORDER_2_Y = 12;
            const int BORDER_2_WIDTH = 35;
            const int BORDER_2_HEIGHT = 6;

            const int BORDER_3_X = 3;
            const int BORDER_3_Y = 23;
            const int BORDER_3_SIZE = 4;
            const int BORDER_3_INTERVAL = 13;
            const int BORDER_3_COUNT = 4;

            const int RIBBON_L_X = BORDER_1_X + 2;
            const int RIBBON_L_Y = BORDER_1_Y + 2;
            const int RIBBON_R_X = BORDER_1_X + BORDER_1_WIDTH - 3;
            const int RIBBON_R_Y = BORDER_1_Y + 2;
            const int RIBBON_WIDTH = 2;
            const int RIBBON_HEIGHT = 3;

            const int HEADER_X = 17;
            const int HEADER_Y = 5;
            const int PROMPT_X = 9;
            const int PROMPT_Y = 14;
            const int VIRTUAL_KEYPAD_X = 9;
            const int VIRTUAL_KEYPAD_Y = 16;
            const int CURSOR_X = 9;
            const int CURSOR_Y = 17;
            const int STAR_X = 5;
            const int STAR_Y = 25;

            const int END_X = VIRTUAL_KEYPAD_X + 28;
            const int END_Y = VIRTUAL_KEYPAD_Y;

            const uint BLINK_INTERVAL = 6;

            int? listPosition = HighScore.ListPosition((int)points);
            listPosition = listPosition == null ? 0 : listPosition;

            MenuManager.TheTextfields[0].ChangeText("New High Score!");
            MenuManager.TheTextfields[1].ChangeText(HighScore.PROMPT + HighScore.NameEntry + (universalTimer % 10 < 5 ? "*" : " "));
            MenuManager.TheTextfields[2].ChangeText(HighScore.VIRTUAL_KEYPAD);

            MenuManager.TheTextfields[0].Draw();
            MenuManager.TheTextfields[1].Draw();
            MenuManager.TheTextfields[2].Draw();

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);

            //Header Section
            spriteBatch.Draw(Images.BORDER_TOP_LEFT, new Vector2(BORDER_1_X * TILE_SIZE, BORDER_1_Y * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_TOP_RIGHT, new Vector2(BORDER_1_X * TILE_SIZE + BORDER_1_WIDTH * TILE_SIZE, BORDER_1_Y * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, new Vector2(BORDER_1_X * TILE_SIZE, BORDER_1_Y * TILE_SIZE + BORDER_1_HEIGHT * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, new Vector2(BORDER_1_X * TILE_SIZE + BORDER_1_WIDTH * TILE_SIZE, BORDER_1_Y * TILE_SIZE + BORDER_1_HEIGHT * TILE_SIZE), Color.Green);
            for (int i = 1; i < BORDER_1_WIDTH; i++)
            {
                spriteBatch.Draw(Images.BORDER_TOP, new Vector2(BORDER_1_X * TILE_SIZE + i * TILE_SIZE, BORDER_1_Y * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_BOTTOM, new Vector2(BORDER_1_X * TILE_SIZE + i * TILE_SIZE, BORDER_1_Y * TILE_SIZE + BORDER_1_HEIGHT * TILE_SIZE), Color.Green);
            }
            for (int i = 1; i < BORDER_1_HEIGHT; i++)
            {
                spriteBatch.Draw(Images.BORDER_LEFT, new Vector2(BORDER_1_X * TILE_SIZE, BORDER_1_Y * TILE_SIZE + i * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_RIGHT, new Vector2(BORDER_1_X * TILE_SIZE + BORDER_1_WIDTH * TILE_SIZE, BORDER_1_Y * TILE_SIZE + i * TILE_SIZE), Color.Green);
            }
            spriteBatch.Draw(MenuManager.TheTextfields[0].Image, new Vector2(HEADER_X * TILE_SIZE, HEADER_Y * TILE_SIZE), Color.White);

            //Entry Section
            spriteBatch.Draw(Images.BORDER_TOP_LEFT, new Vector2(BORDER_2_X * TILE_SIZE, BORDER_2_Y * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_TOP_RIGHT, new Vector2(BORDER_2_X * TILE_SIZE + BORDER_2_WIDTH * TILE_SIZE, BORDER_2_Y * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, new Vector2(BORDER_2_X * TILE_SIZE, BORDER_2_Y * TILE_SIZE + BORDER_2_HEIGHT * TILE_SIZE), Color.Green);
            spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, new Vector2(BORDER_2_X * TILE_SIZE + BORDER_2_WIDTH * TILE_SIZE, BORDER_2_Y * TILE_SIZE + BORDER_2_HEIGHT * TILE_SIZE), Color.Green);
            for (int i = 1; i < BORDER_2_WIDTH; i++)
            {
                spriteBatch.Draw(Images.BORDER_TOP, new Vector2(BORDER_2_X * TILE_SIZE + i * TILE_SIZE, BORDER_2_Y * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_BOTTOM, new Vector2(BORDER_2_X * TILE_SIZE + i * TILE_SIZE, BORDER_2_Y * TILE_SIZE + BORDER_2_HEIGHT * TILE_SIZE), Color.Green);
            }
            for (int i = 1; i < BORDER_2_HEIGHT; i++)
            {
                spriteBatch.Draw(Images.BORDER_LEFT, new Vector2(BORDER_2_X * TILE_SIZE, BORDER_2_Y * TILE_SIZE + i * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_RIGHT, new Vector2(BORDER_2_X * TILE_SIZE + BORDER_2_WIDTH * TILE_SIZE, BORDER_2_Y * TILE_SIZE + i * TILE_SIZE), Color.Green);
            }
            spriteBatch.Draw(MenuManager.TheTextfields[1].Image, new Vector2(PROMPT_X * TILE_SIZE, PROMPT_Y * TILE_SIZE), Color.White);
            spriteBatch.Draw(MenuManager.TheTextfields[2].Image, new Vector2(VIRTUAL_KEYPAD_X * TILE_SIZE, VIRTUAL_KEYPAD_Y * TILE_SIZE), Color.White);
            if (HighScore.Finished == null || HighScore.Finished % BLINK_INTERVAL < (BLINK_INTERVAL / 2))
                spriteBatch.Draw(Images.END, new Vector2(END_X * TILE_SIZE, END_Y * TILE_SIZE), Color.Orange);
            spriteBatch.Draw(Images.ICON_PLAYER, new Vector2(CURSOR_X * TILE_SIZE + HighScore.VirtualKeypadIndex * TILE_SIZE, CURSOR_Y * TILE_SIZE), Color.Yellow);

            //Pattern Section
            for (int i = 0; i < BORDER_3_COUNT; i++)
            {
                int interval = i * BORDER_3_INTERVAL * TILE_SIZE;

                spriteBatch.Draw(Images.BORDER_TOP_LEFT, new Vector2(BORDER_3_X * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_TOP_RIGHT, new Vector2(BORDER_3_X * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, new Vector2(BORDER_3_X * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE), Color.Green);
                spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, new Vector2(BORDER_3_X * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE), Color.Green);
                for (int j = 1; j < BORDER_3_SIZE; j++)
                {
                    spriteBatch.Draw(Images.BORDER_TOP, new Vector2(BORDER_3_X * TILE_SIZE + j * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE), Color.Green);
                    spriteBatch.Draw(Images.BORDER_BOTTOM, new Vector2(BORDER_3_X * TILE_SIZE + j * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE), Color.Green);
                    spriteBatch.Draw(Images.BORDER_LEFT, new Vector2(BORDER_3_X * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE + j * TILE_SIZE), Color.Green);
                    spriteBatch.Draw(Images.BORDER_RIGHT, new Vector2(BORDER_3_X * TILE_SIZE + BORDER_3_SIZE * TILE_SIZE + interval, BORDER_3_Y * TILE_SIZE + j * TILE_SIZE), Color.Green);
                }
                spriteBatch.Draw(Images.STAR, new Vector2(STAR_X * TILE_SIZE + interval, STAR_Y * TILE_SIZE), Color.Orange);
            }

            //Filler
            for (int i = 0; i < FULLFIELD_WIDTH / TILE_SIZE; i++)
            {
                for (int j = 0; j < FULLFIELD_HEIGHT / TILE_SIZE; j++)
                {
                    bool inBorder1 = i >= BORDER_1_X && i <= BORDER_1_X + BORDER_1_WIDTH && j >= BORDER_1_Y && j <= BORDER_1_Y + BORDER_1_HEIGHT;
                    bool inBorder2 = i >= BORDER_2_X && i <= BORDER_2_X + BORDER_2_WIDTH && j >= BORDER_2_Y && j <= BORDER_2_Y + BORDER_2_HEIGHT;
                    bool inBorder3 = false;

                    for (int k = 0; k < BORDER_3_COUNT; k++)
                    {
                        int interval = k * BORDER_3_INTERVAL;

                        if (i >= BORDER_3_X + interval && i <= BORDER_3_X + BORDER_3_SIZE + interval && j >= BORDER_3_Y && j <= BORDER_3_Y + BORDER_3_SIZE)
                        {
                            inBorder3 = true;
                            break;
                        }
                    }

                    if (!inBorder1 && !inBorder2 && !inBorder3)
                        spriteBatch.Draw(Images.BORDER, new Vector2(i * TILE_SIZE, j * TILE_SIZE), Color.Green);
                }
            }

            //Ribbons
            spriteBatch.Draw(
                Images.RIBBON,
                new Rectangle(
                    RIBBON_L_X * TILE_SIZE, 
                    RIBBON_L_Y * TILE_SIZE, 
                    RIBBON_WIDTH * TILE_SIZE, 
                    RIBBON_HEIGHT * TILE_SIZE),
                new Rectangle(
                    (int)listPosition * RIBBON_WIDTH * TILE_SIZE, 
                    0, 
                    RIBBON_WIDTH * TILE_SIZE, 
                    RIBBON_HEIGHT * TILE_SIZE),
                Color.Red);
            spriteBatch.Draw(
                Images.RIBBON,
                new Rectangle(
                    RIBBON_R_X * TILE_SIZE,
                    RIBBON_R_Y * TILE_SIZE,
                    RIBBON_WIDTH * TILE_SIZE,
                    RIBBON_HEIGHT * TILE_SIZE),
                new Rectangle(
                    (int)listPosition * RIBBON_WIDTH * TILE_SIZE,
                    0,
                    RIBBON_WIDTH * TILE_SIZE,
                    RIBBON_HEIGHT * TILE_SIZE),
                Color.Blue);

            spriteBatch.End();

            SubDraw();
        }
    }
}
