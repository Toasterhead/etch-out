using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Etcher
{
    public partial class Game1 : Game
    {
        private void SubDraw()
        {
            int totalWidth = canvasMultiplier * canvasRaw.Width;
            int totalHeight = canvasMultiplier * canvasRaw.Height;
            int halfWidth = (int)(totalWidth * 0.5);
            int halfHeight = (int)(totalHeight * 0.5);

            int centerX = (int)(Window.ClientBounds.Width * 0.5);
            int centerY = (int)(Window.ClientBounds.Height * 0.5);
            //int centerX = (int)(Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width * 0.5);
            //int centerY = (int)(Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height * 0.5);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
            ProcessEffect();
            spriteBatch.Draw(
                canvasRaw, 
                new Rectangle(
                    centerX >= halfWidth ? centerX - halfWidth : 0,
                    centerY >= halfHeight ? centerY - halfHeight : 0, 
                    totalWidth, 
                    totalHeight), 
                Color.White);
            spriteBatch.End();
        }

        private void ProcessEffect()
        {
            switch (renderEffect)
            {
                case RenderEffects.Scanline:
                    FX.SCANLINE.Parameters["surfaceHeight"].SetValue(2.0f * canvasRaw.Height);
                    FX.SCANLINE.Parameters["brightness"].SetValue(effectBrightness);
                    FX.SCANLINE.CurrentTechnique.Passes[0].Apply();
                    break;
                case RenderEffects.PhosphorDot:
                    FX.PHOSPHOR_DOT.Parameters["surfaceDimensions"].SetValue(new Vector2(2 * canvasRaw.Width, 2 * canvasRaw.Height));
                    FX.PHOSPHOR_DOT.Parameters["brightness"].SetValue(effectBrightness);
                    FX.PHOSPHOR_DOT.CurrentTechnique.Passes[0].Apply();
                    break;
                case RenderEffects.Static:
                    FX.STATIC.Parameters["brightness"].SetValue(effectBrightness);
                    FX.STATIC.Parameters["s1"].SetValue(DetermineNoise());
                    FX.STATIC.CurrentTechnique.Passes[0].Apply();
                    break;
                case RenderEffects.PhaseShift:
                    FX.PHASE_SHIFT.CurrentTechnique.Passes[0].Apply();
                    break;
            }
        }

        private void DrawSlider(MISlider slider, int x, int y)
        {
            if (slider.ValueAsInt == 0)
                spriteBatch.Draw(Images.OFF, new Vector2(x, y), Color.White);
            else for (int i = 0; i < 10; i++)
                spriteBatch.Draw(
                    (i + 1) == slider.ValueAsInt ? Images.SLIDER_KNOB : Images.SLIDER_RAIL, 
                    new Vector2(x + (i * TILE_SIZE), y), 
                    Color.White);
        }
    }
}
