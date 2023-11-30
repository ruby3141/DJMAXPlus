using CefSharp;
using CefSharp.DevTools.Autofill;
using CefSharp.OffScreen;
using CefSharp.SchemeHandler;
using System.Drawing;
using System.Reflection;

namespace DJMAXPlus.OverlayRenderer
{
    public class OverlayRenderer : IDisposable
    {
        private ChromiumWebBrowser? _browser;
        private string _overlayPath;

        public RendererStates RendererState { get; private set; }

        public OverlayRenderer(string address = @"localfolder://pwd/TestPage.html")
        {
            _overlayPath = address;
            RendererState = RendererStates.Uninitialized;
        }

        public async Task InitAsync()
        {
            RendererState = RendererStates.Loading;

            CefSettings settings = new();
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "pwd",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    hostName: "pwd"
                    ),
            });

            var cefResult = await Cef.InitializeAsync(settings);
            if (cefResult == false)
            {
                RendererState = RendererStates.Failed;
                throw new Exception("Failed to initialize CEF");
            }

            _browser = new ChromiumWebBrowser(_overlayPath);
            var browserLoadResult = await _browser.WaitForInitialLoadAsync();
            if (browserLoadResult.Success == false)
            {
                RendererState = RendererStates.Failed;
                throw new Exception($"Failed to load target web page; Status: {browserLoadResult.HttpStatusCode}, ErrorCode = {browserLoadResult.ErrorCode}");
            }

            RendererState = RendererStates.Ready;
        }
        public async Task Resize(int width, int height)
        {
            ThrowIfNotReady();
            await _browser!.ResizeAsync(width, height);
            return;
        }

        public async Task<byte[]?> GetScreenshot()
        {
            ThrowIfNotReady();
            return await _browser!.CaptureScreenshotAsync();
        }

        public void Dispose()
        {
            _browser?.Dispose();
            Cef.Shutdown();
        }

        private void ThrowIfNotReady()
        {
            if (_browser is null || RendererState != RendererStates.Ready)
                throw new Exception("Renderer is not ready");
        }
    }
}
