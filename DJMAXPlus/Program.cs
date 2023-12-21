using System.Windows.Forms;
using DJMAXPlus.MainForm;

MainForm m = new();
ApplicationContext applicationContext = new(m);
ApplicationHelper.Run(applicationContext);
ApplicationHelper.Join();
