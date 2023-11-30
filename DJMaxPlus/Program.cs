using CefSharp;
using DJMaxPlus.Main;
using System.Diagnostics;

AsyncContext.Run(async delegate
{
    var renderer = new DJMAXPlus.OverlayRenderer.OverlayRenderer();

    await renderer.InitAsync();

    Thread.Sleep(1000);

    var result = await renderer.GetScreenshot();
    //if (result != null)
    //{
    //    // File path to save our screenshot e.g. C:\Users\{username}\Desktop\screenshot.png
    //    var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "screenshot.png");
    //    File.WriteAllBytes(screenshotPath, result);
    //    Process.Start(new ProcessStartInfo(screenshotPath)
    //    {
    //        UseShellExecute = true
    //    });
    //}

    return;
});
