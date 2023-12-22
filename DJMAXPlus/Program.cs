using System.Windows.Forms;
using CefSharp;
using CefSharp.OffScreen;
using DJMAXPlus.MainForm;
using DJMAXPlus.Overlay;

Cef.Initialize(new CefSettings() { LogSeverity = LogSeverity.Info, });

OverlayController oc = new();
MainForm m = new();
m.Resize += M_Resize;

oc.LoadPage("https://page.devr.is/DJMAXPlus/lanecover.html");
ApplicationContext applicationContext = new(m);
ApplicationHelper.Run(applicationContext);
ApplicationHelper.Join();

void M_Resize(object? sender, EventArgs e)
{
    oc.SetViewportSize(m.Size);
}