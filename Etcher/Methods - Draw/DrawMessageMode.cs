using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawMessageMode()
        {
            const int MESSAGE_WIDTH = 40;
            const int BLANK_OUT_TIME = 10;

            if (message == null) return;

            int lineNumber = 0;
            int horizontalPosition = 0;

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);

            if (messageTimer > BLANK_OUT_TIME)

                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] == ' ')
                    {
                        int j;

                        for (j = i + 1; j < message.Length; j++)
                            if (message[j] == ' ') break;

                        if (horizontalPosition + (j - i) >= MESSAGE_WIDTH)
                        {
                            lineNumber++;
                            horizontalPosition = 0;
                            continue;
                        }
                    }
                    else if (horizontalPosition >= MESSAGE_WIDTH)
                    {
                        lineNumber++;
                        horizontalPosition = 0;
                        if (message[i] == ' ') continue;
                    }

                    spriteBatch.Draw(
                        characterSet.GetCharacterImage(message[i]), 
                        new Vector2(
                            (5 * TILE_SIZE) + (horizontalPosition * TILE_SIZE), 
                            (5 * TILE_SIZE) + (lineNumber * 2 * TILE_SIZE)),
                        Color.White);

                    horizontalPosition++;
                }

            spriteBatch.End();

            SubDraw();
        }
    }
}
