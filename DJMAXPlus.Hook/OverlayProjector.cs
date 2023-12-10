using GameOverlay.Drawing;
using GameOverlay.Windows;
using SharpDX.DXGI;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.X86;

namespace DJMAXPlus.Hook
{
    public class OverlayProjector
    {
        private object _lock = new object();
        private GraphicsWindow window;
        private Image? frame;

        public OverlayProjector()
        {
            window = new GraphicsWindow(0, 0, 1920, 1080, new Graphics());

            window.SetupGraphics += Window_SetupGraphics;
            window.DrawGraphics += Window_DrawGraphics;
            window.DestroyGraphics += Window_DestroyGraphics;
        }

        public void LoadFrame(byte[] bitmap)
        {
            Image? lastFrame;
            lock (_lock)
            {
                lastFrame = frame;
                frame = new Image(window.Graphics, bitmap);
            }

            lastFrame?.Dispose();
        }

        private void Window_SetupGraphics(object? sender, SetupGraphicsEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_DrawGraphics(object? sender, DrawGraphicsEventArgs e)
        {
            Image? currentFrame;

            lock(_lock)
            {
                currentFrame = frame;
                frame = null;
            }

            if (currentFrame != null)
            {
                var gfx = e.Graphics;
                gfx.ClearScene();
                gfx.DrawImage(currentFrame, 0, 0);
            }

        }

        private void Window_DestroyGraphics(object? sender, DestroyGraphicsEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
