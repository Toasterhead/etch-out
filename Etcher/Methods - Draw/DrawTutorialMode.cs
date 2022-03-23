using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawTutorialMode()
        {
            const int NUM_TILES_HORIZONTAL = FULLFIELD_WIDTH / TILE_SIZE;
            const int NUM_TILES_VERTICAL = FULLFIELD_HEIGHT / TILE_SIZE;

            int messageHorizontal = 3 * TILE_SIZE;
            int messageVertical = 4 * TILE_SIZE;

            uint elapsedTime = universalTimer - universalTimeStamp;

            message = 
                elapsedTime / Tutorial.MESSAGE_INTERVAL < Tutorial.MessageList.Length ? 
                Tutorial.MessageList[elapsedTime / Tutorial.MESSAGE_INTERVAL] :
                Tutorial.MessageList[Tutorial.MessageList.Length - 1];

            if (elapsedTime % Tutorial.MESSAGE_INTERVAL > 0.9 * Tutorial.MESSAGE_INTERVAL &&
                elapsedTime / Tutorial.MESSAGE_INTERVAL < Tutorial.MessageList.Length - 1)

                message = null;

            if (message != null)
            {
                textMessage.ChangeText(message);
                textMessage.Draw();
            }

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp);

            for (int i = SCREEN_DIVIDER; i < NUM_TILES_HORIZONTAL; i++)
            {
                for (int j = 0; j < NUM_TILES_VERTICAL; j++)
                {
                    const int TEXT_BOX_X = SCREEN_DIVIDER + 2;
                    const int TEXT_BOX_Y = 5;
                    const int TEXT_BOX_WIDTH = 4;
                    const int TEXT_BOX_HEIGHT = 18;

                    Vector2 cursor = new Vector2(i * TILE_SIZE, j * TILE_SIZE);

                    if (i == SCREEN_DIVIDER && j == 1)
                        spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                    else if (i == SCREEN_DIVIDER && j == NUM_TILES_VERTICAL - 2)
                        spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);
                    else if (i == SCREEN_DIVIDER && j >= 2 && j < NUM_TILES_VERTICAL - 2)
                        spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                    else if (i > SCREEN_DIVIDER)
                    {
                        if (i >= TEXT_BOX_X && i <= TEXT_BOX_X + TEXT_BOX_WIDTH && j >= TEXT_BOX_Y && j <= TEXT_BOX_Y + TEXT_BOX_HEIGHT)
                        {
                            if (i == TEXT_BOX_X && j == TEXT_BOX_Y)
                                spriteBatch.Draw(Images.BORDER_TOP_LEFT, cursor, borderColor);
                            else if (i == TEXT_BOX_X + TEXT_BOX_WIDTH && j == TEXT_BOX_Y)
                                spriteBatch.Draw(Images.BORDER_TOP_RIGHT, cursor, borderColor);
                            else if (i == TEXT_BOX_X && j == TEXT_BOX_Y + TEXT_BOX_HEIGHT)
                                spriteBatch.Draw(Images.BORDER_BOTTOM_LEFT, cursor, borderColor);
                            else if (i == TEXT_BOX_X + TEXT_BOX_WIDTH && j == TEXT_BOX_Y + TEXT_BOX_HEIGHT)
                                spriteBatch.Draw(Images.BORDER_BOTTOM_RIGHT, cursor, borderColor);
                            else if (j > TEXT_BOX_Y && j < TEXT_BOX_Y + TEXT_BOX_HEIGHT)
                            {
                                if (i == TEXT_BOX_X)
                                    spriteBatch.Draw(Images.BORDER_LEFT, cursor, borderColor);
                                else if (i == TEXT_BOX_X + TEXT_BOX_WIDTH)
                                    spriteBatch.Draw(Images.BORDER_RIGHT, cursor, borderColor);
                            }
                            else if (i > TEXT_BOX_X && i < TEXT_BOX_X + TEXT_BOX_WIDTH)
                            {
                                if (j == TEXT_BOX_Y)
                                    spriteBatch.Draw(Images.BORDER_TOP, cursor, borderColor);
                                else if (j == TEXT_BOX_Y + TEXT_BOX_HEIGHT)
                                    spriteBatch.Draw(Images.BORDER_BOTTOM, cursor, borderColor);
                            }
                            for (int k = 0; k < "Tutorial".Length; k++)
                                if (i == TEXT_BOX_X + 2 && j == TEXT_BOX_Y + 2 + (2 * k))
                                    switch(k)
                                    {
                                        case 0: spriteBatch.Draw(Images.Characters.T, cursor, Color.White);
                                            break;
                                        case 1: spriteBatch.Draw(Images.Characters.U, cursor, Color.White);
                                            break;
                                        case 2: spriteBatch.Draw(Images.Characters.T, cursor, Color.White);
                                            break;
                                        case 3: spriteBatch.Draw(Images.Characters.O, cursor, Color.White);
                                            break;
                                        case 4: spriteBatch.Draw(Images.Characters.R, cursor, Color.White);
                                            break;
                                        case 5: spriteBatch.Draw(Images.Characters.I, cursor, Color.White);
                                            break;
                                        case 6: spriteBatch.Draw(Images.Characters.A, cursor, Color.White);
                                            break;
                                        case 7: spriteBatch.Draw(Images.Characters.L, cursor, Color.White);
                                            break;
                                    }
                        }
                        else spriteBatch.Draw(Images.BORDER, cursor, borderColor);
                    }
                    else spriteBatch.Draw(Images.BORDER, cursor, borderColor);
                }
            }

            foreach (IGameObject i in spriteSet)
            {
                bool hideObject = clearTimer != null && !(i is Block);

                if (i.Render && !hideObject)

                    if (i is Debris)
                        spriteBatch.Draw(
                            i.Image,
                            new Vector2(
                                (i.X - (i as Debris).Origin.X) / Debris.DIVISOR_CRASH + (i as Debris).Origin.X,
                                (i.Y - (i as Debris).Origin.Y) / Debris.DIVISOR_CRASH + (i as Debris).Origin.Y),
                            i.TheColor);

                    else spriteBatch.Draw(
                        i.Image,
                        i.Rect,
                        i.SourceRect,
                        i.TheColor,
                        0.0f,
                        new Vector2(0.0f, 0.0f),
                        i.Orientation,
                        i.Layer / (float)Layers.EnumSize);
            }

            if (clearTimer == null)
                foreach (Vector2 i in trail)
                    spriteBatch.Draw(Images.DOT, i, Color.White);

            if (message != null)
                spriteBatch.Draw(textMessage.Image, new Vector2(messageHorizontal, messageVertical), Color.White);

            spriteBatch.End();

            SubDraw();
        }
    }
}
