using CefSharp;
using CefSharp.DevTools.Autofill;
using CefSharp.OffScreen;
using CefSharp.SchemeHandler;
using System.Drawing;
using System.Reflection;

namespace DJMAXPlus.Browser
{
    public class BrowserController : IDisposable
    {
        private ChromiumWebBrowser? _browser;
        private string _overlayPath;

        public ControllerStates ControllerState { get; private set; }

        public BrowserController(string address = @"https://page.devr.is/DJMAXPlus/lanecover.html")
        {
            _overlayPath = address;
            ControllerState = ControllerStates.Uninitialized;
        }

        public void Init()
        {
            ControllerState = ControllerStates.Loading;

            var cefResult = Cef.Initialize(new CefSettings());
            if (cefResult == false)
            {
                ControllerState = ControllerStates.Failed;
                throw new Exception("Failed to initialize CEF");
            }

            _browser = new ChromiumWebBrowser(_overlayPath);
            var browserLoadResult = _browser.WaitForInitialLoadAsync().Result;
            if (browserLoadResult.Success == false)
            {
                ControllerState = ControllerStates.Failed;
                throw new Exception($"Failed to load target web page; Status: {browserLoadResult.HttpStatusCode}, ErrorCode = {browserLoadResult.ErrorCode}");
            }

            ControllerState = ControllerStates.Ready;
        }

        public async Task Resize(int width, int height)
        {
            ThrowIfNotReady();
            await _browser!.ResizeAsync(width, height);
            return;
        }

        public async Task<byte[]?> CaptureScreenshot()
        {
            ThrowIfNotReady();
            return await _browser!.CaptureScreenshotAsync();
        }

        public async Task ChangePage(string url)
        {
            ThrowIfNotReady();
            var browserLoadResult = await _browser!.LoadUrlAsync(url);
            if (browserLoadResult.Success == false)
            {
                ControllerState = ControllerStates.Failed;
                throw new Exception($"Failed to load target web page; Status: {browserLoadResult.HttpStatusCode}, ErrorCode = {browserLoadResult.ErrorCode}");
            }
        }

        public async Task EvaluateScript(string script)
        {
            ThrowIfNotReady();
            await _browser.EvaluateScriptAsync(script);
        }

        public void Dispose()
        {
            _browser?.Dispose();
            Cef.Shutdown();
        }

        private void ThrowIfNotReady()
        {
            if (_browser is null || ControllerState != ControllerStates.Ready)
                throw new Exception("Renderer is not ready");
        }
    }
}
