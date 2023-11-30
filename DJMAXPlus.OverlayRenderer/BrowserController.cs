using CefSharp;
using CefSharp.OffScreen;
using CefSharp.SchemeHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DJMAXPlus.OverlayRenderer
{
    public class BrowserController
    {
        private ChromiumWebBrowser _browser;

        public RendererStates State { get; private set; } = RendererStates.Uninitialized;

        public BrowserController(BrowserSettings? browserSettings = null)
        {
            CefSettings cefSettings = new();
            cefSettings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfiles",
                DomainName = "pwd",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    hostName: "pwd"
                    )
            });

            var result = Cef.Initialize(cefSettings);
            if (result == false)
            {
                throw new Exception("Failed to initialize CEF");
            }

        }
    }
}
