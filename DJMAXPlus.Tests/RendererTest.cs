using CefSharp.DevTools.HeadlessExperimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DJMAXPlus.Tests
{
    [TestClass]
    public class RendererTest
    {
        [TestMethod]
        public async Task RendererTakeScreenshotTest()
        {
            using (var renderer = new OverlayRenderer.OverlayRenderer())
            {
                await renderer.InitAsync();

                await renderer.Resize(1920, 1080);

                var screenshot = await renderer.GetScreenshot();

                Assert.IsFalse(screenshot is null);
                return;
            }
        }
    }
}