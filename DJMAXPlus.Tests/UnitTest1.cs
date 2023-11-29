using CefSharp.DevTools.HeadlessExperimental;

namespace DJMAXPlus.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            using (var renderer = new OverlayRenderer.OverlayRenderer())
            {
                await renderer.Init();

                var screenshot = await renderer.GetScreenshot();

                Assert.IsFalse(screenshot is null);
                return;
            }
        }
    }
}