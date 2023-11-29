using CefSharp;
using CefSharp.DevTools.Autofill;
using CefSharp.OffScreen;

namespace DJMAXPlus.OverlayRenderer
{
    public class OverlayRenderer: IDisposable
    {
        private ChromiumWebBrowser? _browser;
        private string _overlayPath;

        public RendererStates RendererState { get; private set; }

        public OverlayRenderer(string address = "TestPage.html")
        {
            _overlayPath = address;
            RendererState = RendererStates.Uninitialized;
        }

        public async Task Init()
        {
            RendererState = RendererStates.Loading;
            var settings = new CefSettings
            {
                BrowserSubprocessPath = Environment.ProcessPath,
                CachePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "\\Cache")),
            };

            var cefResult = await Cef.InitializeAsync(settings, false);

            if (cefResult == false)
            {
                RendererState = RendererStates.Failed;
                throw new Exception("Failed to initialize CEF");
            }

            _browser = new ChromiumWebBrowser(_overlayPath);
            var browserLoadResult = await _browser.WaitForInitialLoadAsync();
            if(browserLoadResult.Success == false)
            {
                RendererState = RendererStates.Failed;
                throw new Exception($"Failed to load target web page; Status: {browserLoadResult.HttpStatusCode}, ErrorCode = {browserLoadResult.ErrorCode}");
            }

            RendererState = RendererStates.Ready;
        }

        public async Task<byte[]?> GetScreenshot()
        {
            if (_browser is not null && RendererState == RendererStates.Ready)
                return await _browser.CaptureScreenshotAsync();

            throw new Exception("Renderer is not ready");
        }

        public void Dispose()
        {
            _browser?.Dispose();
            Cef.Shutdown();
        }
    }
}
