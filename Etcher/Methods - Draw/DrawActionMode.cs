using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawActionMode()
        {
            const int NUM_TILES_HORIZONTAL = FULLFIELD_WIDTH / TILE_SIZE;
            const int NUM_TILES_VERTICAL = FULLFIELD_HEIGHT / TILE_SIZE;
            const uint POINTS_INTERVAL = 5;

            int messageHorizontal = message == null ? 0 : (21 - (message.Length / 2)) * TILE_SIZE;
            int messageVertical = FULLFIELD_HEIGHT / 2 - TILE_SIZE;

            textLives.ChangeText(player.Lives.ToString());
            textTime.ChangeText((gateTimer / FRAME_RATE).ToString());
            textLives.Draw();
            textTime.Draw();

            if (deathTimer != null)
            {
                textScore.ChangeText(points.ToString());
                textScore.Draw();
            }
            else if (universalTimer % POINTS_INTERVAL == 0)
            {
                textScore.ChangeText((points - (points % POINTS_INTERVAL)).ToString());
                textScore.Draw();
            }

            if (message != null)
            {
                textMessage.ChangeText(message);
                textMessage.Draw();
            }

            if (Vortex.Present)
            {
                GraphicsDevice.SetRenderTarget(vortexSurface);
                GraphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
                FX.VORTEX.Parameters["radius"].SetValue(Vortex.Radius / (0.5f * vortexSurface.Width));
                FX.VORTEX.Parameters["timeStamp"].SetValue((int)universalTimer);
                FX.VORTEX.Parameters["resolutionX"].SetValue(vortexSurface.Width);
                FX.VORTEX.Parameters["resolutionY"].SetValue(vortexSurface.Height);
                FX.VORTEX.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(Images.BLOCK, new Rectangle(0, 0, vortexSurface.Width, vortexSurface.Height), Color.White);
                spriteBatch.End();
            }

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
                        else if  (i == SCREEN_DIVIDER + 1 && j == LIVES_STARTS)
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
                        if (Game1.bType)
                            spriteBatch.Draw(Images.BORDER, cursor, borderColor);
                        else
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
