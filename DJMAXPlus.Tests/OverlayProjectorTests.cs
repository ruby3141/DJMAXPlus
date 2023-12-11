using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DJMAXPlus.Hook.Tests
{
    [TestClass()]
    public class OverlayProjectorTests
    {
        [TestMethod]
        public void GetWindowHandleTest()
        {
            IntPtr handle = OverlayProjector.GetWindowHandle("Untitled - Notepad");
            return;
        }
    }
}