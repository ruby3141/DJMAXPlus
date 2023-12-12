using DJMAXPlus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;

namespace DJMAXPlus.Hook
{
    public class HotkeyManager
    {
        public ControllerStates ControllerState { get; private set; } = ControllerStates.Uninitialized;

        CancellationTokenSource tokenSource;
        Thread? workerThread;

        public HotkeyManager()
        {
            tokenSource = new CancellationTokenSource();
        }

        public bool RegisterHotKey(HotKey hotKey)
        {
            return true;
        }




    }
}
