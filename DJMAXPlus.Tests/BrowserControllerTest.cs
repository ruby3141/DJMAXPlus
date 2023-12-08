using CefSharp.DevTools.Browser;
using CefSharp.DevTools.HeadlessExperimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DJMAXPlus.Browser;

namespace DJMAXPlus.Tests
{
    [TestClass]
    public class BrowserControllerTest
    {
        BrowserController? browserController;

        [TestInitialize()]
        public void BeforeTest()
        {
            browserController = new BrowserController();
            browserController.Init();
        }

        [TestCleanup()]
        public void AfterTest()
        {
            browserController?.Dispose();
        }

        [TestMethod]
        public async Task RendererTakeScreenshotTest()
        {
            Assert.IsNotNull(browserController);

            await browserController.Resize(1920, 1080);
            byte[]? screenshot = await browserController.CaptureScreenshot();
            Assert.IsFalse(screenshot is null);
            return;
        }
    }
}