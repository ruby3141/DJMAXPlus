using DJMAXPlus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJMAXPlus.Hook
{
    public static class InvokeHandler
    {
        public static ControllerStates ControllerState { get; private set; }

        private static object _lock = new object();
        private static Thread? mainThread = null;

        public static void Init()
        {
            var form = new Form
            {
                Visible = false,
            };

            lock (_lock)
            {

                if (mainThread == null)
                {
                    mainThread = new Thread([STAThread]() => { Application.Run(new ApplicationContext()); });
                    mainThread.Start();
                }
            }
        }
    }
}
