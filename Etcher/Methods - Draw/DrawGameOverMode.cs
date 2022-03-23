using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void DrawGameOverMode()
        {
            int messageHorizontal = (FULLFIELD_WIDTH / 2) - ((message.Length / 2) * TILE_SIZE);
            int messageVertical = FULLFIELD_HEIGHT / 2 - TILE_SIZE;

            textMessage.ChangeText(message);
            textMessage.Draw();

            GraphicsDevice.SetRenderTarget(canvasRaw);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(
                textMessage.Image, 
                new Vector2(
                    messageHorizontal, 
                    messageVertical), 
                Color.White);

            foreach (IGameObject i in spriteSet)

                if (i is Debris && i.Render)
                {
                    Debris iDebris = i as Debris;

                    spriteBatch.Draw(
                        i.Image,
                        new Vector2(
                                (i.X - iDebris.Origin.X) / Debris.DIVISOR_FIREWORKS + iDebris.Origin.X,
                                (i.Y - iDebris.Origin.Y) / Debris.DIVISOR_FIREWORKS + iDebris.Origin.Y),
                        i.TheColor);
                }

            spriteBatch.End();

            SubDraw();
        }
    }
}
