using System.Windows.Forms;
using DJMAXPlus.MainForm;

Console.WriteLine($"From Thread {Thread.CurrentThread.ManagedThreadId}");
var f = new Form();
ApplicationContext applicationContext = new();
ApplicationHelper.Run(applicationContext);
Thread.Sleep(3000);
ApplicationHelper.Invoke(() => Console.WriteLine($"From Thread {Thread.CurrentThread.ManagedThreadId}"));
Thread.Sleep(3000);
Console.WriteLine($"From Thread {Thread.CurrentThread.ManagedThreadId}");
ApplicationHelper.Stop();
