using System.Drawing;
using CefSharp;
using CefSharp.OffScreen;

namespace DJMAXPlus.Overlay
{
    public class OverlayController : IDisposable
    {
        private ChromiumWebBrowser browser;

        // Internal lock and BitmapBuffer exposed to try using InteropServices.Marshal
        public object BitmapLock => ((DefaultRenderHandler)browser.RenderHandler).BitmapLock;
        public BitmapBuffer BitmapBuffer => ((DefaultRenderHandler)browser.RenderHandler).BitmapBuffer;

        public OverlayController()
        {
            var browserSettings = new BrowserSettings()
            {
                WindowlessFrameRate = 30,
                LocalStorage = CefState.Enabled,
            };
            browser = new ChromiumWebBrowser("about:blank", browserSettings);
            _ = browser.WaitForInitialLoadAsync().Result;
        }

        public void sendMouseClickEvent(int x, int y, bool isMouseUp)
        {
            var b = browser.GetBrowserHost();
            b.SendMouseClickEvent(new MouseEvent(x, y, CefEventFlags.LeftMouseButton), MouseButtonType.Left, isMouseUp, 1);
        }

        public void sendMouseMoveEvent(int x, int y)
        {
            var b = browser.GetBrowserHost();
            b.SendMouseMoveEvent(new MouseEvent(x, y, CefEventFlags.LeftMouseButton), false);
        }

        public void sendKeyEvent(int keyCode, bool isKeyUp)
        {
            var b = browser.GetBrowserHost();
            b.SendKeyEvent(new KeyEvent {
                WindowsKeyCode = keyCode,
                Type = isKeyUp ? KeyEventType.KeyUp : KeyEventType.KeyDown,
                FocusOnEditableField = true,
                IsSystemKey = false,
            });
        }

        public void LoadPage(string url)
        {
            browser.Load(url);
        }

        public void ShowDevTools()
        {
            browser.ShowDevTools();
        }

        public void SetViewportSize(Size size)
        {
            browser.Size = size;
        }

        public void LoadConfig(string slotName)
        {
            browser.ExecuteScriptAsync($"document.querySelector(\"#_dp_Config form\")[\"SaveSlot\"].value = \"{slotName}\"; loadConfig();");
        }

        public void SetLaneCoverVisibility(bool isVisible)
        {
            browser.ExecuteScriptAsync($"document.querySelector(\"#_dp_LaneCover\").style.maxHeight = {(isVisible ? 100 : 0)}%");
        }

        public void SetConfigFormVisibility(bool isVisible)
        {
            browser.ExecuteScriptAsync($"document.querySelector(\"#_dp_Config\").style.visibility = \"{(isVisible ? "visible" : "hidden")}\"");
        }

        public void SetLaneCoverImage(string url)
        {
            browser.ExecuteScriptAsync($"document.querySelector(\"#_dp_LaneCover\").style.backgroundImage = url({url})");
        }

        public void Dispose()
        {
            ((IDisposable)browser).Dispose();
        }
    }
}
